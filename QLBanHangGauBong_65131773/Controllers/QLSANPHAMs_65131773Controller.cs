using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLBanHangGauBong_65131773.Models;

namespace QLBanHangGauBong_65131773.Controllers
{
    public class QLSANPHAMs_65131773Controller : Controller
    {
        private QuanLyBanHangGauBongEntities4 db = new QuanLyBanHangGauBongEntities4();

        // GET: QLSANPHAMs_65131773
        public ActionResult Index()
        {
            var sANPHAMs = db.SANPHAMs.Include(s => s.LOAISP);
            return View(sANPHAMs.ToList());
        }

        // GET: QLSANPHAMs_65131773/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SANPHAM sANPHAM = db.SANPHAMs.Find(id);
            if (sANPHAM == null)
            {
                return HttpNotFound();
            }
            return View(sANPHAM);
        }

        // GET: QLSANPHAMs_65131773/Create
        public ActionResult Create()
        {
            ViewBag.MaLoaiSP = new SelectList(db.LOAISPs, "MaLoaiSP", "TenLoaiSP");
            return View();
        }

        // POST: QLSANPHAMs_65131773/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaSP,MaLoaiSP,TenSP,AnhSP,DonGia,GiaNhap,SoLuongTon")] SANPHAM sANPHAM)
        {
            if (ModelState.IsValid)
            {
                db.SANPHAMs.Add(sANPHAM);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaLoaiSP = new SelectList(db.LOAISPs, "MaLoaiSP", "TenLoaiSP", sANPHAM.MaLoaiSP);
            return View(sANPHAM);
        }

        // GET: QLSANPHAMs_65131773/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SANPHAM sANPHAM = db.SANPHAMs.Find(id);
            if (sANPHAM == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaLoaiSP = new SelectList(db.LOAISPs, "MaLoaiSP", "TenLoaiSP", sANPHAM.MaLoaiSP);
            return View(sANPHAM);
        }

        // POST: QLSANPHAMs_65131773/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaSP,MaLoaiSP,TenSP,AnhSP,DonGia,GiaNhap,SoLuongTon")] SANPHAM sANPHAM)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sANPHAM).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaLoaiSP = new SelectList(db.LOAISPs, "MaLoaiSP", "TenLoaiSP", sANPHAM.MaLoaiSP);
            return View(sANPHAM);
        }

        // GET: QLSANPHAMs_65131773/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SANPHAM sANPHAM = db.SANPHAMs.Find(id);
            if (sANPHAM == null)
            {
                return HttpNotFound();
            }
            return View(sANPHAM);
        }

        // POST: QLSANPHAMs_65131773/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SANPHAM sANPHAM = db.SANPHAMs.Find(id);
            db.SANPHAMs.Remove(sANPHAM);
            db.SaveChanges();
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
