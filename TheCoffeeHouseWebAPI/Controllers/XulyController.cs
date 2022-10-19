using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TheCoffeeHouseWebAPI.Models;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace TheCoffeeHouseWebAPI.Controllers
{
    public class XulyController : ApiController
    {
        [Route("api/XulyController/Get_SanPhams")]
        [HttpGet]
        public IHttpActionResult Get_SanPhams()
        {
            Database database = new Database();
            List<SanPham> listSanPham = database.Get_SanPhams();
            return Ok(listSanPham);
        }
        [Route("api/XulyController/Get_LoaiSanPhams")]
        [HttpGet]
        public IHttpActionResult Get_LoaiSanPhams()
        {
            Database database = new Database();
            List<LoaiSanPham> listLoaiSanPham = database.Get_LoaiSanPhams();
            return Ok(listLoaiSanPham);
        }
        [Route("api/XulyController/GetDotKhuyenMai")]
        [HttpGet]
        public IHttpActionResult GetDotKhuyenMai()
        {
            Database database = new Database();
            List<DotKhuyenMai> listDotKhuyenMai = database.GetDotKhuyenMai();
            return Ok(listDotKhuyenMai);
        }
        [Route("api/XulyController/GetSize")]
        [HttpGet]
        public IHttpActionResult GetSize()
        {
            Database database = new Database();
            List<SP_SIZE> listSize = database.GetSize();
            return Ok(listSize);
        }
        [Route("api/XulyController/GetOTP")]
        [HttpGet]
        public IHttpActionResult GetOTP(string eMail)
        {
            Database database = new Database();
            bool result = database.GetOTP(eMail);
            return Ok(result);
        }
        [Route("api/XulyController/CheckOTP")]
        [HttpGet]
        public IHttpActionResult CheckOTP(string eMail, string OTP)
        {
            Database database = new Database();
            KhachHang result = database.CheckOTP(eMail, OTP);
            return Ok(result);
        }

        [Route("api/XulyController/GetLoaiKhachHang")]
        [HttpGet]
        public IHttpActionResult GetLoaiKhachHang()
        {
            Database database = new Database();
            List<LoaiKhachHang> listLoaiKhachHang = database.GetLoaiKhachHang();
            return Ok(listLoaiKhachHang);
        }


        //Code moi
        [Route("api/XulyController/Get_CuaHangs")]
        [HttpGet]
        public IHttpActionResult Get_CuaHangs()
        {
            Database database = new Database();
            List<CuaHangChiTiet> listCuaHang = database.Get_CuaHangs();
            return Ok(listCuaHang);
        }


        //Code moi
        [Route("api/XulyController/Update_KhachHang")]
        [HttpGet]
        public IHttpActionResult Update_KhachHang(string makh, string hoten, string sdt, string ngaysinh, byte gioitinh)
        {
            Database database = new Database();
            hoten = hoten.Replace('+', ' ');
            KhachHang kh = new KhachHang
            {
                MaKH = makh,
                HoTen = hoten,
                SDT = sdt,
                NgaySinh = Convert.ToDateTime(ngaysinh),
                GioiTinh = gioitinh
            };
            bool check = database.Update_KhachHang(kh);
            return Ok(check);
        }
        [Route("api/XulyController/GuiGopY")]
        [HttpGet]
        public IHttpActionResult GuiGopY(string makh, string noidung)
        {
            Database database = new Database();
            string noiDungDecode = HttpUtility.UrlDecode(noidung);
            GopY gopy = new GopY
            {
                MaKH = makh,
                NoiDung = noiDungDecode
            };
            bool check = database.Them_GopY(gopy);
            return Ok(check);
        }
        //[Route("api/XulyController/Upload_Avatar_User")]
        //[HttpPost]
        //public async Task<string> Upload_Avatar_User()
        //{
        //    try
        //    {
        //        var httpRequest = HttpContext.Current.Request;
        //        if (httpRequest.Files.Count > 0)
        //        {
        //            foreach (string file in httpRequest.Files)
        //            {
        //                var postedFile = httpRequest.Files[file];
        //                //var fileName = postedFile.FileName.Split('\\').LastOrDefault().Split('/').LastOrDefault();
        //                var fileName = postedFile.FileName;
        //                var filePath = HttpContext.Current.Server.MapPath("~/Image_Avatar/" + fileName);
        //                postedFile.SaveAs(filePath);
        //                return "/Image_Avatar/" + fileName;
        //            }
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        return exception.Message;
        //    }
        //    return "no files";
        //}

        [Route("api/XulyController/Get_TinTucs")]
        [HttpGet]
        public IHttpActionResult Get_TinTucs()
        {
            Database database = new Database();
            List<TinTuc> listTinTuc = database.Get_TinTucs();
            return Ok(listTinTuc);
        }


        // Code lan 2

        [Route("api/XulyController/GetTinhThanh")]
        [HttpGet]
        public IHttpActionResult GetTinhThanh()
        {
            List<province> listProvince = new List<province>();
            string connectionString = "server = 127.0.0.1; user id = root; password =; port = 3307; database = tinhthanhvietnam;";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string str = "SELECT * FROM province;";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listProvince.Add(new province
                        {
                            provinceid = reader["provinceid"].ToString(),
                            name = reader["name"].ToString()
                        });
                    }
                    return Ok(listProvince);
                }
            }
        }
        [Route("api/XulyController/GetQuanOfTinh")]
        [HttpGet]
        public IHttpActionResult GetQuanOfTinh(string provinceid)
        {
            List<district> listDistrict = new List<district>();
            string connectionString = "server = 127.0.0.1; user id = root; password =; port = 3307; database = tinhthanhvietnam;";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string str = "SELECT * FROM district WHERE provinceid=@provinceid;";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("provinceid", provinceid);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listDistrict.Add(new district
                        {
                            provinceid = reader["provinceid"].ToString(),
                            name = reader["name"].ToString(),
                            districtid = reader["districtid"].ToString()
                        });
                    }
                    return Ok(listDistrict);
                }
            }
        }
        [Route("api/XulyController/GetPhuongOfQuan")]
        [HttpGet]
        public IHttpActionResult GetPhuongOfQuan(string districtid)
        {
            List<ward> listWard = new List<ward>();
            string connectionString = "server = 127.0.0.1; user id = root; password =; port = 3307; database = tinhthanhvietnam;";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string str = "SELECT * FROM ward WHERE districtid=@districtid;";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("districtid", districtid);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listWard.Add(new ward
                        {
                            wardid = reader["wardid"].ToString(),
                            name = reader["name"].ToString(),
                            districtid = reader["districtid"].ToString()
                        });
                    }
                    return Ok(listWard);
                }
            }
        }
        [Route("api/XulyController/DatHang")]
        [HttpPost]
        public IHttpActionResult DatHang(FormDatHang form)
        {
            var donHangString = form.donHangString;
            var listCTDHString = form.listCTDHString;
            DonHang donHang = JsonConvert.DeserializeObject<DonHang>(donHangString);
            List<CTDH> listCTDH = JsonConvert.DeserializeObject<List<CTDH>>(listCTDHString);

            Database db = new Database();
            var diaChiGiaoString = form.diaChiGiaoString;
            DiaChiDonHang diaChiGiao = JsonConvert.DeserializeObject<DiaChiDonHang>(diaChiGiaoString);

            if (diaChiGiao == null)
            {
                if (db.DatHangCuaHang(listCTDH, donHang))
                {
                    return Ok(1);
                }
                else
                {
                    return Ok(0);
                }
            }
            else
            {

                if (db.DatHangGiao(listCTDH, donHang, diaChiGiao))
                {
                    return Ok(1);
                }
                else
                {
                    return Ok(0);
                }
            }
        }
        //------------------
        [Route("api/XulyController/GetListDonHang")]
        [HttpGet]
        public IHttpActionResult GetListDonHang(string MaKH)
        {
            Database db = new Database();
            List<DonHang> donHangs = db.GetListDonHang(MaKH);
            return Ok(donHangs);
        }
        [Route("api/XulyController/GetCTDH")]
        [HttpGet]
        public IHttpActionResult GetCTDH(string MaDH)
        {
            Database db = new Database();
            List<CTDH> CTDHs = db.GetListCTDH(MaDH);

            return Ok(CTDHs);
        }
        [Route("api/XulyController/GetDiaChiDonHang")]
        [HttpGet]
        public IHttpActionResult GetDiaChiDonHang(int MaDC)
        {
            Database db = new Database();
            DiaChiDonHang DCDH = db.GetDiaChiDonHang(MaDC);

            return Ok(DCDH);
        }
    }
}
