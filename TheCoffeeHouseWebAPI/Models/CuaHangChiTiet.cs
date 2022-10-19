using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCoffeeHouseWebAPI.Models
{
    public class CuaHangChiTiet
    {
        public int MaCH { get; set; }
        public string TenCH { get; set; }
        public int MaDC { get; set; }
        public string Img { get; set; }
        public string KinhDo { get; set; }
        public string ViDo { get; set; }
        public string Tinh { get; set; }
        public string Quan { get; set; }
        public string Phuong { get; set; }
        public string SoNhaDuong { get; set; }
    }
}