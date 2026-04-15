using QLBanHangGauBong_65131773.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLBanHangGauBong_65131773.Controllers
{
    public class HomeController : Controller
    {
        private QuanLyBanHangGauBongEntities4 db = new QuanLyBanHangGauBongEntities4();
        public ActionResult Index()
        {
            var spNoiBat = db.SANPHAMs
                .Select(sp => new SanPhamNoiBat
                {
                    MaSP = sp.MaSP,
                    TenSP = sp.TenSP,
                    AnhSP = sp.AnhSP,
                    DonGia = sp.DonGia ?? 0,
                    TongBan = db.CHITIET_DONHANG
                                .Where(ct => ct.MaSP == sp.MaSP)
                                .Sum(ct => (int?)ct.SoLuong) ?? 0
                })
                .OrderByDescending(x => x.TongBan)
                .ThenByDescending(x => x.MaSP)
                .Take(8)
                .ToList();

            return View(spNoiBat);
        }
        public ActionResult IndexAdmin()
        {
            // Nếu chưa login, redirect về login
            if (Session["UserRole"] == null)
                return RedirectToAction("Login", "NHANVIENs_65131773");

            ViewBag.UserName = Session["UserName"];
            ViewBag.UserRole = Session["UserRole"];

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}