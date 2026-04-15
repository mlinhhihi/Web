using System;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Helpers;
using System.Web.Mvc;
using QLBanHangGauBong_65131773.Models;

namespace QLBanHangGauBong_65131773.Controllers
{
    public class KHACHHANGs_65131773Controller : Controller
    {
        private QuanLyBanHangGauBongEntities4 db = new QuanLyBanHangGauBongEntities4();

        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangKy(DangKyKH model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                db.sp_DangKyKhachHang(
                    model.HoTen,
                    model.TenTK,
                    model.MatKhau,  
                    model.Email,
                    model.Sdt,
                    model.GioiTinh,
                    model.DiaChi
                );

                TempData["ThongBao"] = "Đăng ký thành công! Vui lòng đăng nhập.";
                return RedirectToAction("DangNhap");
            }
            catch (SqlException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangNhap(DangNhapKH model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var khach = db.sp_DangNhapKhachHang(
                            model.TenTK,
                            model.MatKhau   
                        ).FirstOrDefault();

            if (khach == null)
            {
                ModelState.AddModelError("", "Sai tên tài khoản hoặc mật khẩu");
                return View(model);
            }
            var khFull = db.KHACHHANGs.FirstOrDefault(k => k.MaKH == khach.MaKH);
            Session["KhachHang"] = khFull;

            return RedirectToAction("Index", "SANPHAMs_65131773");
        }
        public ActionResult ThongTinKH()
        {
            if (Session["KhachHang"] == null)
                return RedirectToAction("DangNhap");

            var kh = Session["KhachHang"] as KHACHHANG;
            return View(kh);
        }

        public ActionResult GioHang()
        {
            if (Session["KhachHang"] == null)
                return RedirectToAction("DangNhap");

            dynamic kh = Session["KhachHang"];
            int maKH = kh.MaKH;

            var gioHang = db.sp_XemGioHang(maKH).ToList();
            return View(gioHang);
        }


        [HttpPost]
        [Authorize]
        public ActionResult ThemGioHang(int maSP, int soLuong)
        {
            if (Session["KhachHang"] == null) return RedirectToAction("DangNhap");

            var kh = (dynamic)Session["KhachHang"];
            int maKH = kh.MaKH;

            try
            {
                db.sp_ThemChiTietDonHang(0, maSP, soLuong); 
                TempData["ThongBao"] = "Đã thêm sản phẩm vào giỏ hàng!";
            }
            catch (Exception ex)
            {
                TempData["Loi"] = ex.Message;
            }

            return RedirectToAction("GioHang");
        }

        [Authorize]
        public ActionResult TaoDonHang(string diaChiGiao, string hinhThucTT)
        {
            if (Session["KhachHang"] == null) return RedirectToAction("DangNhap");

            var kh = (dynamic)Session["KhachHang"];
            int maKH = kh.MaKH;

            try
            {
                var soHD = db.sp_TaoDonHang(maKH, diaChiGiao, hinhThucTT).FirstOrDefault();
                TempData["ThongBao"] = "Tạo đơn hàng thành công! Mã hóa đơn: " + soHD;
            }
            catch (Exception ex)
            {
                TempData["Loi"] = "Lỗi khi tạo đơn: " + ex.Message;
            }

            return RedirectToAction("GioHang");
        }
        public ActionResult LSDonMua()
        {
            if (Session["KhachHang"] == null)
                return RedirectToAction("DangNhap");

            var kh = Session["KhachHang"] as KHACHHANG;

            var donHang = db.DONHANGs
                            .Where(d => d.MaKH == kh.MaKH)
                            .OrderByDescending(d => d.NgayDatHang)
                            .ToList();

            return View(donHang);
        }

        public ActionResult DangXuat()
        {
            Session.Clear();
            return RedirectToAction("DangNhap");
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.KHACHHANGs.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
