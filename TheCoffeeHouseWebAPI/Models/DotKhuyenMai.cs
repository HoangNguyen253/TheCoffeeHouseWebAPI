using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCoffeeHouseWebAPI.Models
{
    public class DotKhuyenMai
    {
        public int MaDKM { get; set; }
        public string TenDKM { get; set; }
        public DateTime ThoiGianBatDau { get; set; }
        public DateTime ThoiGianKetThuc { get; set; }
        public int MaSP { get; set; }
        public int GiamGia { get; set; }

    }
}