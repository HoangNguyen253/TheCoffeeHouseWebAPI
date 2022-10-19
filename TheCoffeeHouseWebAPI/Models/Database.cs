using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;

namespace TheCoffeeHouseWebAPI.Models
{
    public class Database
    {
        public List<SanPham> Get_SanPhams()
        {
            List<SanPham> listSanPham = new List<SanPham>();
            string connstring = ConfigurationManager.ConnectionStrings["TheCoffeeHouseConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                conn.Open();
                string str = "SELECT * FROM SANPHAM";
                SqlCommand cmd = new SqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            listSanPham.Add(new SanPham
                            {
                                MaSP = Convert.ToInt32(reader["MaSP"]),
                                TenSP = reader["TenSP"].ToString(),
                                MoTa = reader["MoTa"].ToString(),
                                Img = reader["Img"].ToString(),
                                Gia = Convert.ToInt32(reader["Gia"]),
                                LoaiSP = reader["LoaiSP"].ToString()
                            });
                        }
                    }
                }
                return listSanPham;
            }
        }
        public List<LoaiSanPham> Get_LoaiSanPhams()
        {
            List<LoaiSanPham> listLoaiSanPham = new List<LoaiSanPham>();
            string connstring = ConfigurationManager.ConnectionStrings["TheCoffeeHouseConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                conn.Open();
                string str = "SELECT * FROM LOAISANPHAM";
                SqlCommand cmd = new SqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            listLoaiSanPham.Add(new LoaiSanPham
                            {
                                LoaiSP = Convert.ToInt32(reader["LoaiSP"]),
                                TenLoaiSP = reader["TenLoaiSP"].ToString()
                            });
                        }
                    }
                }
                return listLoaiSanPham;
            }
        }
        public List<DotKhuyenMai> GetDotKhuyenMai()
        {
            List<DotKhuyenMai> listDotKhuyenMai = new List<DotKhuyenMai>();
            string connstring = ConfigurationManager.ConnectionStrings["TheCoffeeHouseConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                conn.Open();
                string str = "SELECT * FROM DOTKHUYENMAI WHERE ThoiGianKetThuc >= CAST( GETDATE() AS Date ) AND ThoiGianBatDau <= CAST( GETDATE() AS Date )";
                SqlCommand cmd = new SqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            listDotKhuyenMai.Add(new DotKhuyenMai
                            {
                                MaDKM = Convert.ToInt32(reader["MaDKM"]),
                                TenDKM = reader["TenDKM"].ToString(),
                                ThoiGianBatDau = Convert.ToDateTime(reader["ThoiGianBatDau"]),
                                ThoiGianKetThuc = Convert.ToDateTime(reader["ThoiGianKetThuc"]),
                                GiamGia = Convert.ToInt32(reader["GiamGia"]),
                                MaSP = Convert.ToInt32(reader["MaSP"]),
                            });
                        }
                    }
                }
                return listDotKhuyenMai;
            }
        }
        public List<SP_SIZE> GetSize()
        {
            List<SP_SIZE> listSize = new List<SP_SIZE>();
            string connstring = ConfigurationManager.ConnectionStrings["TheCoffeeHouseConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                conn.Open();
                string str = "SELECT * FROM SP_SIZE";
                SqlCommand cmd = new SqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            listSize.Add(new SP_SIZE
                            {
                                TienThem = Convert.ToInt32(reader["TienThem"]),
                                Size = Convert.ToChar(reader["Size"]),
                                MaSP = Convert.ToInt32(reader["MaSP"])
                            });
                        }
                    }
                }
                return listSize;
            }
        }
        private string GenerateNewOTP()
        {
            int length = 6;
            const string valid = "1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
        public bool GetOTP(string eMail)
        {
            bool kt;
            string OTP = GenerateNewOTP();
            DateTime _now = DateTime.Now;
            string connstring = ConfigurationManager.ConnectionStrings["TheCoffeeHouseConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                conn.Open();
                string str = "SELECT * FROM KHACHHANG WHERE MaKH=@MaKH";
                SqlCommand cmd = new SqlCommand(str, conn);
                cmd.Parameters.AddWithValue("MaKH", eMail);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        kt = true;

                    }
                    else
                    {
                        kt = false;

                    }
                }
                if (kt)
                {
                    string updateSTR = "UPDATE KHACHHANG SET OTP=@OTP, OTPTime=@OTPTime WHERE MaKH=@MaKH";
                    SqlCommand cmd1 = new SqlCommand(updateSTR, conn);
                    cmd1.Parameters.AddWithValue("MaKH", eMail);
                    cmd1.Parameters.AddWithValue("OTP", OTP);
                    cmd1.Parameters.AddWithValue("OTPTime", _now.ToString("yyyy-MM-dd HH:mm:ss"));
                    try
                    {
                        cmd1.ExecuteNonQuery();
                        clsMail mail = new clsMail();
                        string bodyMail = "<h2>Chào bạn</h2><p>Mã OTP của bạn là:" + OTP + "</p>";
                        mail.Send(eMail, "Mã OTP", bodyMail);
                        return true;
                    }
                    catch (SqlException e)
                    {
                        return false;
                        throw e;
                    }
                }
                else
                {
                    string insertSTR = "INSERT INTO KHACHHANG(MaKH, HoTen, NgaySinh, SDT, ImgAvt, ImgQR, GioiTinh, OTP, OTPTime, TongTien, LoaiKH, TongDiemBean, DiemBeanConLai) " +
                            "VALUES (@MaKH, '', '', '', '', '', 1, @OTP, @OTPTime, 0, 'NE', 0, 0)";
                    SqlCommand cmd1 = new SqlCommand(insertSTR, conn);
                    cmd1.Parameters.AddWithValue("MaKH", eMail);
                    cmd1.Parameters.AddWithValue("OTP", OTP);
                    cmd1.Parameters.AddWithValue("OTPTime", _now.ToString("yyyy-MM-dd HH:mm:ss"));
                    try
                    {
                        cmd1.ExecuteNonQuery();
                        clsMail mail = new clsMail();
                        string bodyMail = "<h2>Chào bạn</h2><p>Mã OTP của bạn là:" + OTP + "</p>";
                        mail.Send(eMail, "Mã OTP", bodyMail);
                        return true;
                    }
                    catch (SqlException e)
                    {
                        return false;
                        throw e;
                    }
                }
            }
        }
        public KhachHang CheckOTP(string eMail, string OTP)
        {
            KhachHang kh = new KhachHang();
            string connstring = ConfigurationManager.ConnectionStrings["TheCoffeeHouseConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                conn.Open();
                string str = "SELECT * FROM KHACHHANG WHERE MaKH=@MaKH";
                SqlCommand cmd = new SqlCommand(str, conn);
                cmd.Parameters.AddWithValue("MaKH", eMail);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        DateTime OTPTime = Convert.ToDateTime(reader["OTPTime"]);
                        DateTime _now = DateTime.Now;
                        if ((int)(_now - OTPTime).TotalSeconds > 180)
                        {
                            return null;
                        }
                        else
                        {
                            string OTPReal = reader["OTP"].ToString();
                            if (OTPReal == OTP)
                            {
                                kh.MaKH = reader["MaKH"].ToString();
                                kh.HoTen = reader["HoTen"].ToString();
                                kh.GioiTinh = Convert.ToByte(reader["GioiTinh"]);
                                kh.ImgAvt = reader["ImgAvt"].ToString();
                                kh.ImgQR = reader["ImgQR"].ToString();
                                kh.LoaiKH = reader["LoaiKH"].ToString();
                                kh.NgaySinh = Convert.ToDateTime(reader["NgaySinh"].ToString());
                                kh.DiemBeanConLai = Convert.ToInt32(reader["DiemBeanConLai"].ToString());
                                kh.TongDiemBean = Convert.ToInt32(reader["TongDiemBean"].ToString());
                                kh.SDT = reader["SDT"].ToString();
                                return kh;
                            }
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
        public List<LoaiKhachHang> GetLoaiKhachHang()
        {
            List<LoaiKhachHang> listLoaiKhachHang = new List<LoaiKhachHang>();
            string connstring = ConfigurationManager.ConnectionStrings["TheCoffeeHouseConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                conn.Open();
                string str = "SELECT * FROM LOAIKHACHHANG";
                SqlCommand cmd = new SqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            listLoaiKhachHang.Add(new LoaiKhachHang
                            {
                                LoaiKH = reader["LoaiKH"].ToString(),
                                TenLoaiKH = reader["TenLoaiKH"].ToString(),
                                ChietKhauHoaDon = Convert.ToInt32(reader["ChietKhauHoaDon"])
                            });
                        }
                    }
                }
                return listLoaiKhachHang;
            }
        }

        //Code moi
        public List<CuaHangChiTiet> Get_CuaHangs()
        {
            List<CuaHangChiTiet> listCuaHang = new List<CuaHangChiTiet>();
            string connstring = ConfigurationManager.ConnectionStrings["TheCoffeeHouseConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connstring);
            conn.Open();
            string str = "SELECT * FROM CUAHANG ch inner join DIACHI dc on ch.MaDC = dc.MaDC";
            SqlCommand cmd = new SqlCommand(str, conn);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        listCuaHang.Add(new CuaHangChiTiet
                        {
                            MaCH = Convert.ToInt32(reader["MaCH"]),
                            TenCH = reader["TenCH"].ToString(),
                            Img = reader["Img"].ToString(),
                            MaDC = Convert.ToInt32(reader["MaDC"]),
                            KinhDo = reader["KinhDo"].ToString(),
                            ViDo = reader["ViDo"].ToString(),
                            Tinh = reader["Tinh"].ToString(),
                            Quan = reader["Quan"].ToString(),
                            Phuong = reader["Phuong"].ToString(),
                            SoNhaDuong = reader["SoNhaDuong"].ToString(),
                        });
                    }
                }
            }
            return listCuaHang;
        }


        // Code moi nhat
        public bool Update_KhachHang(KhachHang kh)
        {
            string connstring = ConfigurationManager.ConnectionStrings["TheCoffeeHouseConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                conn.Open();
                string str = "UPDATE KHACHHANG " +
                    "SET HoTen = @hoten, SDT = @SDT, NgaySinh = @ngaysinh, GioiTinh = @gioitinh " +
                    "WHERE MaKH=@MaKH";
                SqlCommand cmd = new SqlCommand(str, conn);
                cmd.Parameters.AddWithValue("MaKH", kh.MaKH);
                cmd.Parameters.AddWithValue("hoten", kh.HoTen);
                cmd.Parameters.AddWithValue("SDT", kh.SDT);
                cmd.Parameters.AddWithValue("ngaysinh", kh.NgaySinh);
                cmd.Parameters.AddWithValue("gioitinh", kh.GioiTinh);
                int check = cmd.ExecuteNonQuery();
                if (check > 0)
                {
                    return true;
                }
                else return false;
            }
        }
        public bool Them_GopY(GopY gopy)
        {
            string connstring = ConfigurationManager.ConnectionStrings["TheCoffeeHouseConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                conn.Open();
                string str = "INSERT INTO GOPY(MaKH, NoiDung, TrangThai) VALUES(@makh, @noidung, 0) ";
                SqlCommand cmd = new SqlCommand(str, conn);
                cmd.Parameters.AddWithValue("makh", gopy.MaKH);
                cmd.Parameters.AddWithValue("noidung", gopy.NoiDung);
                int check = cmd.ExecuteNonQuery();
                if (check > 0)
                {
                    return true;
                }
                else return false;
            }
        }

        public List<TinTuc> Get_TinTucs()
        {
            List<TinTuc> listTinTuc = new List<TinTuc>();
            string connstring = ConfigurationManager.ConnectionStrings["TheCoffeeHouseConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connstring);
            conn.Open();
            string str = "SELECT * FROM TINTUC ORDER BY NgayTinTuc DESC";
            SqlCommand cmd = new SqlCommand(str, conn);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        listTinTuc.Add(new TinTuc
                        {
                            MaTT = Convert.ToInt32(reader["MaTT"]),
                            Img = reader["Img"].ToString(),
                            TieuDe = reader["TieuDe"].ToString(),
                            NoiDung = reader["NoiDung"].ToString(),
                            NgayTinTuc = Convert.ToDateTime(reader["NgayTinTuc"])
                        });
                    }
                }
            }
            return listTinTuc;
        }

        //Code lan 2
        public bool DatHangCuaHang(List<CTDH> listCTDH, DonHang donHang)
        {

            string connstring = ConfigurationManager.ConnectionStrings["TheCoffeeHouseConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connstring))
            {
                conn.Open();
                DateTime _now = DateTime.Now;
                string MaDH = donHang.MaKH + _now.ToString("yyyy") + _now.ToString("MM")+ _now.ToString("dd")+ _now.ToString("HH")+ _now.ToString("mm")+ _now.ToString("ss");
                string str = "INSERT INTO DONHANG(MaDH, MaKH, PhuongThucNhan, PhuongThucThanhToan, ThoiGianDat, GiamGiaLoaiKhachHang, TongTien, TrangThai, MaCH, MaDC) VALUES " +
                    "(@MaDH, @MaKH, @PhuongThucNhan, @PhuongThucThanhToan, @ThoiGianDat, @GiamGiaLoaiKhachHang, @TongTien, @TrangThai, @MaCH, @MaDC);";
                SqlCommand cmd = new SqlCommand(str, conn);
                cmd.Parameters.AddWithValue("MaDH", MaDH);
                cmd.Parameters.AddWithValue("MaKH", donHang.MaKH);
                cmd.Parameters.AddWithValue("PhuongThucNhan", donHang.PhuongThucNhan);
                cmd.Parameters.AddWithValue("PhuongThucThanhToan", donHang.PhuongThucThanhToan);
                cmd.Parameters.AddWithValue("ThoiGianDat", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("GiamGiaLoaiKhachHang", donHang.GiamGiaLoaiKhachHang);
                cmd.Parameters.AddWithValue("TongTien", donHang.TongTien);
                cmd.Parameters.AddWithValue("TrangThai", donHang.TrangThai);
                cmd.Parameters.AddWithValue("MaCH", donHang.MaCH);
                cmd.Parameters.AddWithValue("MaDC", donHang.MaDC);
                int count = cmd.ExecuteNonQuery();
                if (count == 0)
                {
                    return false;
                }
                else
                {
                    string queryCTDH = "INSERT INTO CTDH(MaDH, MaSP, Size, SoLuong, ThanhTien) VALUES ";
                    for (int i = 0; i < listCTDH.Count; i++)
                    {
                        queryCTDH += "('" + MaDH + "', " + listCTDH[i].MaSP + ", '" + listCTDH[i].Size + "', " + listCTDH[i].SoLuong + ", " + listCTDH[i].ThanhTien + "),";
                    }
                    queryCTDH = queryCTDH.Remove(queryCTDH.Length - 1, 1);
                    SqlCommand cmd1 = new SqlCommand(queryCTDH, conn);
                    int countCTDH = cmd1.ExecuteNonQuery();
                    if (countCTDH == 0)
                    {
                        return false;
                    }
                    return true;
                }
            }
        }
        public bool DatHangGiao(List<CTDH> listCTDH, DonHang donHang, DiaChiDonHang diaChiDonHang)
        {
            string connstring = ConfigurationManager.ConnectionStrings["TheCoffeeHouseConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connstring))
            {
                conn.Open();
                string strDiaChi = "INSERT INTO DIACHI(Tinh, Quan, Phuong, SoNhaDuong) VALUES " +
                    "(@Tinh, @Quan, @Phuong, @SoNhaDuong);";
                SqlCommand cmdDiaChi = new SqlCommand(strDiaChi, conn);
                cmdDiaChi.Parameters.AddWithValue("Tinh", diaChiDonHang.Tinh);
                cmdDiaChi.Parameters.AddWithValue("Quan", diaChiDonHang.Quan);
                cmdDiaChi.Parameters.AddWithValue("Phuong", diaChiDonHang.Phuong);
                cmdDiaChi.Parameters.AddWithValue("SoNhaDuong", diaChiDonHang.SoNhaDuong);
                int countDiaChi = cmdDiaChi.ExecuteNonQuery();

                if (countDiaChi == 0)
                {
                    return false;
                }
                else
                {
                    string getMaDC = "SELECT MAX(MaDC) FROM DIACHI;";
                    SqlCommand cmdGetDiaChi = new SqlCommand(getMaDC, conn);
                    int MaDC = Convert.ToInt32(cmdGetDiaChi.ExecuteScalar());
                    DateTime _now = DateTime.Now;
                    string MaDH = donHang.MaKH + _now.ToString("yyyy") + _now.ToString("MM") + _now.ToString("dd") + _now.ToString("HH") + _now.ToString("mm") + _now.ToString("ss");
                    string str = "INSERT INTO DONHANG(MaDH, MaKH, PhuongThucNhan, PhuongThucThanhToan, ThoiGianDat, GiamGiaLoaiKhachHang, TongTien, TrangThai, MaDC) VALUES " +
                        "(@MaDH, @MaKH, @PhuongThucNhan, @PhuongThucThanhToan, @ThoiGianDat, @GiamGiaLoaiKhachHang, @TongTien, @TrangThai, @MaDC);";
                    SqlCommand cmd = new SqlCommand(str, conn);
                    cmd.Parameters.AddWithValue("MaDH", MaDH);
                    cmd.Parameters.AddWithValue("MaKH", donHang.MaKH);
                    cmd.Parameters.AddWithValue("PhuongThucNhan", donHang.PhuongThucNhan);
                    cmd.Parameters.AddWithValue("PhuongThucThanhToan", donHang.PhuongThucThanhToan);
                    cmd.Parameters.AddWithValue("ThoiGianDat", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("GiamGiaLoaiKhachHang", donHang.GiamGiaLoaiKhachHang);
                    cmd.Parameters.AddWithValue("TongTien", donHang.TongTien);
                    cmd.Parameters.AddWithValue("TrangThai", donHang.TrangThai);
                    cmd.Parameters.AddWithValue("MaDC", MaDC);
                    int count = cmd.ExecuteNonQuery();
                    if (count == 0)
                    {
                        return false;
                    }
                    else
                    {
                        string queryCTDH = "INSERT INTO CTDH(MaDH, MaSP, Size, SoLuong, ThanhTien) VALUES ";
                        for (int i = 0; i < listCTDH.Count; i++)
                        {
                            queryCTDH += "('" + MaDH + "', " + listCTDH[i].MaSP + ", '" + listCTDH[i].Size + "', " + listCTDH[i].SoLuong + ", " + listCTDH[i].ThanhTien + "),";
                        }
                        queryCTDH = queryCTDH.Remove(queryCTDH.Length - 1, 1);
                        SqlCommand cmd1 = new SqlCommand(queryCTDH, conn);
                        int countCTDH = cmd1.ExecuteNonQuery();
                        if (countCTDH == 0)
                        {
                            return false;
                        }
                        return true;
                    }
                }
            }
        }

        public List<DonHang> GetListDonHang(string MaKH)
        {
            List<DonHang> listDonHang = new List<DonHang>();
            string connstring = ConfigurationManager.ConnectionStrings["TheCoffeeHouseConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                conn.Open();
                string str = "SELECT * FROM DONHANG WHERE MaKH=@MaKH";
                SqlCommand cmd = new SqlCommand(str, conn);
                cmd.Parameters.AddWithValue("MaKH", MaKH);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            DonHang dh = new DonHang();
                            dh.MaDH = reader["MaDH"].ToString();
                            dh.TongTien = Convert.ToInt32(reader["TongTien"]);
                            dh.PhuongThucNhan = Convert.ToInt32(reader["PhuongThucNhan"]);
                            dh.PhuongThucThanhToan = Convert.ToInt32(reader["PhuongThucThanhToan"]);
                            dh.ThoiGianDat = Convert.ToDateTime(reader["ThoiGianDat"]);
                            dh.TrangThai = Convert.ToInt32(reader["TrangThai"]);
                            dh.GiamGiaLoaiKhachHang = Convert.ToInt32(reader["GiamGiaLoaiKhachHang"]);
                            dh.MaDC = Convert.ToInt32(reader["MaDC"]);
                            if (dh.PhuongThucNhan == 1)
                            {
                                dh.MaCH = Convert.ToInt32(reader["MaCH"]);
                            }
                            listDonHang.Add(dh);
                        }
                    }
                }
                return listDonHang;
            }
        }

        public List<CTDH> GetListCTDH(string MaDH)
        {
            List<CTDH> listCTDH = new List<CTDH>();
            string connstring = ConfigurationManager.ConnectionStrings["TheCoffeeHouseConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                conn.Open();
                string str = "SELECT * FROM CTDH WHERE MaDH=@MaDH";
                SqlCommand cmd = new SqlCommand(str, conn);
                cmd.Parameters.AddWithValue("MaDH", MaDH);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            CTDH ctdh = new CTDH();
                            ctdh.MaSP = Convert.ToInt32(reader["MaSP"]);
                            ctdh.SoLuong = Convert.ToInt32(reader["SoLuong"]);
                            ctdh.ThanhTien = Convert.ToInt32(reader["ThanhTien"]);
                            if (reader["Size"] != null)
                            {
                                ctdh.Size = reader["Size"].ToString();
                            }
                            listCTDH.Add(ctdh);
                        }
                    }
                }
                return listCTDH;
            }
        }
        public DiaChiDonHang GetDiaChiDonHang(int MaDC)
        {
            DiaChiDonHang diaChi = new DiaChiDonHang();
            string connstring = ConfigurationManager.ConnectionStrings["TheCoffeeHouseConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                conn.Open();
                string str = "SELECT * FROM DIACHI WHERE MaDC=@MaDC";
                SqlCommand cmd = new SqlCommand(str, conn);
                cmd.Parameters.AddWithValue("MaDC", MaDC);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            diaChi.Tinh = reader["Tinh"].ToString();
                            diaChi.Quan = reader["Quan"].ToString();
                            diaChi.Phuong = reader["Phuong"].ToString();
                            diaChi.SoNhaDuong = reader["SoNhaDuong"].ToString();
                        }
                    }
                }
                return diaChi;
            }
        }
    }
}