using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolegomenon
{
    internal class ShiJuanRoot
    {
        //如果好用，请收藏地址，帮忙分享。
        public class OptionsItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string XuanXiangXuHao { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string XuanXiangNeiRong { get; set; }
        }

        public class RowsItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string ShiJuanID { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string ShiTiID { get; set; }
            /// <summary>
            /// </summary>
            public string ShiTiTiMu { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string TiXing { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string ShiTiTuPian { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string MiaoShu { get; set; }
            /// <summary>
            /// 安全考试
            /// </summary>
            public string ShiJuanMingCheng { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string ZongFen { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string HeGeFen { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string DanXuanTiFenZhi { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string DuoXuanTiFenZhi { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string PanDuanTiFenZhi { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string ShiJuanFenLei { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<OptionsItem> Options { get; set; }
        }


        /// <summary>
        /// 
        /// </summary>
        public string ErrorMsg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string KaoShiShiChang { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<RowsItem> rows { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string KaiShiShiJian { get; set; }

    }
}
