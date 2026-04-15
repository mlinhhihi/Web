using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using QLBanHangGauBong_65131773.Models;

namespace QLBanHangGauBong_65131773.Controllers
{
    public class GIOHANGs_65131773Controller : Controller
    {
        private QuanLyBanHangGauBongEntities4 db = new QuanLyBanHangGauBongEntities4();

        // Xem giỏ hàng
        public ActionResult Index()
        {
            if (Session["MaKH"] == null)
                return RedirectToAction("DangNhap", "KHACHHANGs_65131773");

            int MaKH = (int)Session["MaKH"];
            List<sp_XemGioHang_Result> spResults = new List<sp_XemGioHang_Result>();

            try
            {
                var result = db.sp_XemGioHang(MaKH);
                if (result != null)
                    spResults = result.ToList();
            }
            catch
            {
                spResults = new List<sp_XemGioHang_Result>();
            }

            return View(spResults); // Trả trực tiếp SP result cho view
        }

        // Thêm sản phẩm vào giỏ
        [HttpPost]
        public ActionResult ThemVaoGio(int MaSP, int SoLuong)
        {
            if (Session["MaKH"] == null)
                return RedirectToAction("DangNhap", "KHACHHANGs_65131773");

            int MaKH = (int)Session["MaKH"];

            try
            {
                db.sp_ThemChiTietDonHang(MaKH, MaSP, SoLuong); // lưu MaKH
                TempData["ThongBao"] = "Đã thêm vào giỏ hàng!";
            }
            catch
            {
                TempData["ThongBao"] = "Lỗi: Không thể thêm vào giỏ hàng!";
            }

            return RedirectToAction("Index", "SANPHAMs_65131773");
        }

        // Xóa sản phẩm khỏi giỏ
        [HttpPost]
        public ActionResult XoaKhoiGio(int MaSP)
        {
            if (Session["MaKH"] == null)
                return RedirectToAction("DangNhap", "KHACHHANGs_65131773");

            int MaKH = (int)Session["MaKH"];
            var item = db.GIOHANGs.FirstOrDefault(g => g.MaKH == MaKH && g.MaSP == MaSP);
            if (item != null)
            {
                db.GIOHANGs.Remove(item);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // Đặt hàng
        [HttpPost]
        public ActionResult DatHang(string DiaChiGiao, string HinhThucTT)
        {
            if (Session["MaKH"] == null)
                return RedirectToAction("DangNhap", "KHACHHANGs_65131773");

            int MaKH = (int)Session["MaKH"];
            var soHD = db.sp_TaoDonHang(MaKH, DiaChiGiao, HinhThucTT).FirstOrDefault();

            if (soHD != null)
            {
                var gioHang = db.GIOHANGs.Where(g => g.MaKH == MaKH).ToList();
                foreach (var item in gioHang)
                {
                    db.sp_ThemChiTietDonHang(soHD, item.MaSP, item.SoLuong ?? 0);
                }

                db.GIOHANGs.RemoveRange(gioHang);
                db.SaveChanges();
                TempData["ThongBao"] = "Đặt hàng thành công!";
            }

            return RedirectToAction("Index", "DONHANGs_65131773");
        }
    }
}
