using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using QLBanHangGauBong_65131773.Models;

namespace QLBanHangGauBong_65131773.Controllers
{
    public class NHANVIENs_65131773Controller : Controller
    {
        private QuanLyBanHangGauBongEntities4 db = new QuanLyBanHangGauBongEntities4();

        #region Login / Logout Admin & Nhân viên
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string TenTK, string MatKhau)
        {
            if (string.IsNullOrEmpty(TenTK) || string.IsNullOrEmpty(MatKhau))
            {
                ViewBag.Error = "Vui lòng điền đầy đủ thông tin.";
                return View();
            }

            // Lấy user theo TenTK trước
            var nv = db.NHANVIENs.FirstOrDefault(n => n.TenTK == TenTK);

            if (nv != null)
            {
                // Hash mật khẩu nhập
                using (SHA256 sha = SHA256.Create())
                {
                    byte[] hashedInput = sha.ComputeHash(Encoding.UTF8.GetBytes(MatKhau));

                    // So sánh byte[] trong C# (in-memory)
                    if (nv.MatKhau != null && nv.MatKhau.Length == hashedInput.Length)
                    {
                        bool isMatch = true;
                        for (int i = 0; i < hashedInput.Length; i++)
                        {
                            if (nv.MatKhau[i] != hashedInput[i])
                            {
                                isMatch = false;
                                break;
                            }
                        }

                        if (isMatch)
                        {
                            // Login thành công
                            Session["UserName"] = nv.HoTenNV;
                            Session["UserRole"] = nv.LoaiNV; // Admin hoặc NhanVien
                            return RedirectToAction("IndexAdmin", "Home"); // Trang admin chung
                        }
                    }
                }
            }

            ViewBag.Error = "Tên tài khoản hoặc mật khẩu không đúng";
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
        #endregion

        #region CRUD Nhân viên (chỉ Admin)
        public ActionResult Index()
        {
            if (Session["UserRole"] == null || Session["UserRole"].ToString() != "Admin")
                return RedirectToAction("Login");

            var nHANVIENs = db.NHANVIENs.Include(n => n.QUYEN);
            return View(nHANVIENs.ToList());
        }

        public ActionResult Create()
        {
            if (Session["UserRole"] == null || Session["UserRole"].ToString() != "Admin")
                return RedirectToAction("Login");

            ViewBag.MaQuyen = new SelectList(db.QUYENs, "MaQuyen", "TenQuyen");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection form)
        {
            if (Session["UserRole"] == null || Session["UserRole"].ToString() != "Admin")
                return RedirectToAction("Login");

            string HoTenNV = form["HoTenNV"];
            string SDT = form["SDT"];
            string TenTK = form["TenTK"];
            string MatKhauInput = form["MatKhau"];
            int MaQuyen = int.Parse(form["MaQuyen"]);
            string LoaiNV = form["LoaiNV"];

            if (string.IsNullOrEmpty(HoTenNV) || string.IsNullOrEmpty(TenTK) || string.IsNullOrEmpty(MatKhauInput))
            {
                ViewBag.Error = "Vui lòng điền đầy đủ thông tin.";
                ViewBag.MaQuyen = new SelectList(db.QUYENs, "MaQuyen", "TenQuyen", MaQuyen);
                return View();
            }

            byte[] hashed;
            using (SHA256 sha = SHA256.Create())
            {
                hashed = sha.ComputeHash(Encoding.UTF8.GetBytes(MatKhauInput));
            }

            NHANVIEN nv = new NHANVIEN
            {
                HoTenNV = HoTenNV,
                SDT = SDT,
                TenTK = TenTK,
                MatKhau = hashed,
                MaQuyen = MaQuyen,
                LoaiNV = LoaiNV
            };

            db.NHANVIENs.Add(nv);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
