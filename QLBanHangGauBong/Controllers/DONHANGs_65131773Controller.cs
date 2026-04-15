using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLBanHangGauBong_65131773.Models;

namespace QLBanHangGauBong_65131773.Controllers
{
    [Authorize] 
    public class DONHANGs_65131773Controller : Controller
    {
        private QuanLyBanHangGauBongEntities4 db = new QuanLyBanHangGauBongEntities4();


        [Authorize(Roles = "KhachHang")]
        public ActionResult GioHang(int maKH)
        {
            var gioHang = db.sp_XemGioHang(maKH).ToList();
            return View(gioHang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "KhachHang")]
        public ActionResult DatHang(int maKH, string diaChiGiao, string hinhThucTT)
        {

            var soHD = db.sp_TaoDonHang(maKH, diaChiGiao, hinhThucTT).FirstOrDefault();

            if (soHD == 0)
            {
                TempData["Loi"] = "Tạo đơn hàng thất bại!";
                return RedirectToAction("GioHang", new { maKH });
            }


            var gioHang = db.GIOHANGs.Where(g => g.MaKH == maKH).ToList();


            foreach (var item in gioHang)
            {
                db.sp_ThemChiTietDonHang(soHD, item.MaSP, item.SoLuong);
            }
            db.GIOHANGs.RemoveRange(gioHang);
            db.SaveChanges();

            TempData["ThanhCong"] = "Đặt hàng thành công!";
            return RedirectToAction("Index", "SANPHAMs_65131773");
        }

        [Authorize(Roles = "Admin,NhanVien")]
        public ActionResult Index()
        {
            var dONHANGs = db.DONHANGs.Include("KHACHHANG").Include("NHANVIEN").Include("NHANVIEN1").ToList();
            return View(dONHANGs);
        }

        [Authorize(Roles = "Admin,NhanVien,KhachHang")]
        public ActionResult Details(int soHD)
        {
            var chiTiet = db.v_ChiTietDonHang.Where(ct => ct.SoHD == soHD).ToList();
            if (!chiTiet.Any())
            {
                return HttpNotFound();
            }
            return View(chiTiet);
        }


        [Authorize(Roles = "Admin,NhanVien")]
        public ActionResult CapNhatTinhTrang(int soHD, string tinhTrang)
        {
            var dh = db.DONHANGs.Find(soHD);
            if (dh == null)
            {
                return HttpNotFound();
            }

            dh.TinhTrangDonHang = tinhTrang;
            db.SaveChanges();

            TempData["ThanhCong"] = "Cập nhật trạng thái thành công!";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
