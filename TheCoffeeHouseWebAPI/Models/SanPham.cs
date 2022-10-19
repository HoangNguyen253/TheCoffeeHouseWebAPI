using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCoffeeHouseWebAPI.Models
{
    public class SanPham
    {
        public int MaSP { get; set; }
        public string TenSP { get; set; }
        public string MoTa { get; set; }
        public string Img { get; set; }
        public int Gia { get; set; }
        public string LoaiSP { get; set; }
    }
}