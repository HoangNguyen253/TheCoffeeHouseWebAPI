using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCoffeeHouseWebAPI.Models
{
    public class TinTuc
    {
        public int MaTT { get; set; }
        public string TieuDe { get; set; }
        public string Img { get; set; }
        public string NoiDung { get; set; }
        public DateTime NgayTinTuc { get; set; }
    }
}