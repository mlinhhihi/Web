using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLBanHangGauBong_65131773.Models;

namespace QLBanHangGauBong_65131773.Controllers
{
    public class SANPHAMs_65131773Controller : Controller
    {
        private QuanLyBanHangGauBongEntities4 db = new QuanLyBanHangGauBongEntities4();

        // Kiểm tra admin
        private bool IsAdmin()
        {
            return Session["UserRole"] != null && Session["UserRole"].ToString() == "Admin";
        }

        // GET: SANPHAMs
        public ActionResult Index()
        {
            var sanphams = db.SANPHAMs.ToList();
            string role = Session["UserRole"] as string; // Admin hoặc NhanVien
            ViewBag.UserRole = role;

            var kh = Session["KhachHang"] as KHACHHANG;
            ViewBag.KhachHang = kh; // nếu khách đang đăng nhập

            return View(sanphams);
        }

        // GET: SANPHAMs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            SANPHAM sp = db.SANPHAMs.Find(id);
            if (sp == null) return HttpNotFound();
            return View(sp);
        }
    }
}
