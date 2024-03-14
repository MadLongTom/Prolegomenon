// See https://aka.ms/new-console-template for more information
using ddddocrsharp;
using Prolegomenon.Extensions;
using Prolegomenon.Models;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

DdddOcr ocr = new(show_ad: false, use_gpu: false);
List<DaAnRoot> DaAn = JsonSerializer.Deserialize<List<DaAnRoot>>(File.ReadAllText("AllShiTi.json"));
List<Task> taskPool = [];
foreach (var kvp in File.ReadAllLines("queryList.txt"))
{
    if (kvp.Trim() == string.Empty) continue;
    var kv = kvp.Split(' ');
    taskPool.Add(Run(ocr, DaAn, kv[0], kv.Length > 1 ? kv[1] : "123456", taskPool));
    await Task.WhenAll(taskPool);
    File.WriteAllText("queryList.txt", string.Empty);
}

static async Task Run(DdddOcr ocr, List<DaAnRoot> DaAn, string username, string password, List<Task> taskPool)
{
    var handler = new HttpClientHandler()
    {
        AutomaticDecompression = DecompressionMethods.GZip
    };
    using HttpClient client = new(handler);
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add("Accept", "text/plain, */*");
    client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
    client.DefaultRequestHeaders.Add("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
    client.DefaultRequestHeaders.Add("Host", "47.110.11.26:10003");
    client.DefaultRequestHeaders.Add("Origin", "http://47.110.11.26:10003");
    client.DefaultRequestHeaders.Add("Proxy-Connection", "keep-alive");
    client.DefaultRequestHeaders.Add("Referer", "http://47.110.11.26:10003/Login.aspx");
    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/122.0.0.0 Safari/537.36 Edg/122.0.0.0");
    client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");

    var res = await Login(client, ocr, username, password);
    if (await res.Content.ReadAsStringAsync() != "{\"Flag\":[{\"Status\":\"1\"}]}")
    {
        if (password != "123456")
        {
            taskPool.Add(Run(ocr, DaAn, username, "123456", taskPool));
            return;
        }
        else
        {
            Console.WriteLine("密码错误:" + username);
            await WriteHelper.WriteFileAsync("out.txt", "密码错误:" + username);
            return;
        }

    }
    res = await GetAnPaiList(client);
    var apList = await res.Content.ReadFromJsonAsync<AnPaiRoot[]>();
    foreach (var ap in apList!)
    {
        res = await GetZuJuan(client, ap.KaoShiAnPaiID);
        var zujuan = await res.Content.ReadFromJsonAsync<ZuJuanRoot>();
        res = await ShengChengShiJuan(client, zujuan.ShiJuanID, zujuan.ShiJuanFenLei);
        var shijuan = await res.Content.ReadFromJsonAsync<ShiJuanRoot>();
        List<ShiTiRoot> answer = [];
        foreach (var shiti in shijuan.rows)
        {
            answer.Add(new()
            {
                ShiTiID = shiti.ShiTiID,
                DaAn = string.Join(',', DaAn.First(d => d.ID == shiti.ShiTiID).XuanXiangList.Where(x => x.IsDaAn == 0).Select(x => Convert.ToChar(x.XuanXiangXuHao) - 'A' + 1)),
                TiXing = shiti.TiXing
            });
        }
        res = await TiJiaoShiJuan(client, ap.KaoShiAnPaiID, shijuan.KaiShiShiJian, zujuan.ShiJuanID, JsonSerializer.Serialize(answer));
        Console.WriteLine(username + ":" + await res.Content.ReadAsStringAsync());
    }
    await WriteHelper.WriteFileAsync("out.txt", "考试完成:" + username);
}
static async Task<HttpResponseMessage> GetZuJuan(HttpClient client, string AnPaiID)
{
    return await client.GetAsync(@"http://47.110.11.26:10003/LMWeb/LM42/HLM420501.ashx?Method=GetZuJuan&t=" + Random.Shared.NextDouble() + @$"&AnPaiID={AnPaiID}".ToString());
}
static async Task<HttpResponseMessage> GetAnPaiList(HttpClient client)
{
    return await client.GetAsync(@"http://47.110.11.26:10003/LMWeb/LM42/HLM420500.ashx?Method=GetAnPaiList&t=" + Random.Shared.NextDouble().ToString());
}
static async Task<HttpResponseMessage> TiJiaoShiJuan(HttpClient client, string AnPaiID, string KaiShiShiJian, string KaoShiID, string Answer)
{
    var ctx = new FormUrlEncodedContent(new Dictionary<string, string>
    {
        ["AnPaiID"] = AnPaiID,
        ["KaiShiShiJian"] = KaiShiShiJian,
        ["KaoShiID"] = KaoShiID,
        ["Answer"] = Answer
    });
    return await client.PostAsync(@"http://47.110.11.26:10003/LMWeb/LM42/HLM420501.ashx?Method=TiJiaoShiJuan&t=" + Random.Shared.NextDouble().ToString(), ctx);
}
static async Task<HttpResponseMessage> ShengChengShiJuan(HttpClient client, string KaoShiID, string ShiJuanFenLei)
{
    var ctx = new FormUrlEncodedContent(new Dictionary<string, string>
    {
        ["KaoShiID"] = KaoShiID,
        ["ShiJuanFenLei"] = ShiJuanFenLei
    });
    return await client.PostAsync(@"http://47.110.11.26:10003/LMWeb/LM42/HLM420501.ashx?Method=ShengChengShiJuan&t=" + Random.Shared.NextDouble().ToString(), ctx);
}
static async Task<HttpResponseMessage> Login(HttpClient client, DdddOcr ocr, string username, string password)
{
    var ctx = new FormUrlEncodedContent(new Dictionary<string, string>
    {
        ["userName"] = username,
        ["passWord"] = MD5_32(password),
        ["checkCode"] = await GetAuthCode(client, ocr),
        ["flag"] = "0"
    });
    var res = await client.PostAsync(@"http://47.110.11.26:10003/LMWeb/LM00/index.ashx?Method=Login&t=" + Random.Shared.NextDouble().ToString(), ctx);
    client.DefaultRequestHeaders.Add("Cookie", "login=" + username);
    return res;
}
static async Task<string> GetAuthCode(HttpClient client, DdddOcr ocr)
{
    var res = await client.GetAsync(@"http://47.110.11.26:10003/LMWeb/LM00/CheckCode.ashx?Flag=DengLu");
    var gif = await res.Content.ReadAsStreamAsync();
    Image img = Image.FromStream(gif);
    FrameDimension frameDimension = new(img.FrameDimensionsList[0]);
    img.SelectActiveFrame(frameDimension, 0);
    using MemoryStream ms = new();
    img.Save(ms, ImageFormat.Png);
    byte[] BPicture = new byte[ms.Length];
    BPicture = ms.GetBuffer();
    var code = ocr.classification(BPicture);
    return code;
}
static string MD5_32(string cipher)
{
    MD5 md5 = MD5.Create();
    byte[] t = md5.ComputeHash(Encoding.UTF8.GetBytes(cipher));
    StringBuilder stringBuilder = new();
    for (int i = 0; i < t.Length; i++)
    {
        stringBuilder.Append(t[i].ToString("x").PadLeft(2, '0'));
    }
    return stringBuilder.ToString();
}