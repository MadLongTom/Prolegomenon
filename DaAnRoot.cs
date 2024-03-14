using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolegomenon
{
    internal class DaAnRoot
    {
        //如果好用，请收藏地址，帮忙分享。
        public class XuanXiangListItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string XuanXiangXuHao { get; set; }
            /// <summary>
            /// 正确
            /// </summary>
            public string XuanXiangNeiRong { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int IsDaAn { get; set; }
        }

        
            /// <summary>
            /// 
            /// </summary>
            public string ID { get; set; }
            /// <summary>
            /// 在操场锻炼时，可以直接将包放在一旁去运动。
            /// </summary>
            public string TiMu { get; set; }
            /// <summary>
            /// 判断题
            /// </summary>
            public string TiXing { get; set; }
            /// <summary>
            /// 通用安全类
            /// </summary>
            public string FenLei { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string TuPian { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<XuanXiangListItem> XuanXiangList { get; set; }

    }
}
