using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShoesShopOnline.Models;


namespace ShoesShopOnline.Areas.Admin.Controllers
{
    public class KhuyenMaiSPsController : Controller
    {
        private Shoes db = new Shoes();

        // GET: Admin/KhuyenMaiSPs
        public ActionResult Index()
        {
            return View(db.KhuyenMaiSPs.ToList());
        }

        // GET: Admin/KhuyenMaiSPs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhuyenMaiSP khuyenMaiSP = db.KhuyenMaiSPs.Find(id);
            if (khuyenMaiSP == null)
            {
                return HttpNotFound();
            }
            return View(khuyenMaiSP);
        }

        // GET: Admin/KhuyenMaiSPs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/KhuyenMaiSPs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaKM,PhanTramKM,NgayBatDau,NgayKetThuc")] KhuyenMaiSP khuyenMaiSP)
        {
            if (ModelState.IsValid)
            {
                if (khuyenMaiSP.NgayBatDau < DateTime.Now)
                {
                    ViewBag.Error = "Nhập ngày bắt đầu lớn hơn hoặc bằng ngày hiện tại";
                }
                else if (khuyenMaiSP.NgayBatDau > khuyenMaiSP.NgayKetThuc)
                {
                    ViewBag.Error = "Nhập ngày bắt đầu nhỏ hơn ngày kết thúc";
                }
                else
                {

                    db.KhuyenMaiSPs.Add(khuyenMaiSP);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(khuyenMaiSP);
        }

        // GET: Admin/KhuyenMaiSPs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhuyenMaiSP khuyenMaiSP = db.KhuyenMaiSPs.Find(id);
            if (khuyenMaiSP == null)
            {
                return HttpNotFound();
            }
            return View(khuyenMaiSP);
        }

        // POST: Admin/KhuyenMaiSPs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaKM,PhanTramKM,NgayBatDau,NgayKetThuc")] KhuyenMaiSP khuyenMaiSP)
        {
            if (ModelState.IsValid)
            {
                db.Entry(khuyenMaiSP).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(khuyenMaiSP);
        }

        // GET: Admin/KhuyenMaiSPs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhuyenMaiSP khuyenMaiSP = db.KhuyenMaiSPs.Find(id);
            if (khuyenMaiSP == null)
            {
                return HttpNotFound();
            }
            return View(khuyenMaiSP);
        }

        // POST: Admin/KhuyenMaiSPs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KhuyenMaiSP khuyenMaiSP = db.KhuyenMaiSPs.Find(id);
            db.KhuyenMaiSPs.Remove(khuyenMaiSP);
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
