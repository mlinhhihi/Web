using System;
using System.Linq;
using System.Web.Mvc;
using QLBanHangGauBong_65131773.Models;

public class TaiKhoans_65131773Controller : Controller
{
    private QuanLyBanHangGauBongEntities4 db = new QuanLyBanHangGauBongEntities4();

    // GET: Đăng ký
    public ActionResult DangKy()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult DangKy(string HoTen, string TenTK, string MatKhau, string Email, string SDT, string GioiTinh, string DiaChi)
    {
        try
        {
            db.sp_DangKyKhachHang(HoTen, TenTK, MatKhau, Email, SDT, GioiTinh, DiaChi);
            TempData["ThongBao"] = "Đăng ký thành công!";
            return RedirectToAction("DangNhap");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();
        }
    }

    // GET: DangNhap
    public ActionResult DangNhap()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult DangNhap(string TenTK, string MatKhau)
    {
        
        // Kiểm tra khách hàng
        var kh = db.sp_DangNhapKhachHang(TenTK, MatKhau).FirstOrDefault();
        if (kh != null)
        {
            Session["MaKH"] = kh.MaKH;
            Session["HoTenKH"] = kh.HoTen;
            Session["Role"] = "KhachHang";

            return RedirectToAction("Index", "SANPHAMs_65131773");
        }

        ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng!");
        return View();
    }

    // Đăng xuất
    public ActionResult DangXuat()
    {
        Session.Clear();
        return RedirectToAction("DangNhap");
    }
}