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
    public class AnhMoTasController : BaseController
    {
        private Shoes db = new Shoes();

        // GET: Admin/AnhMoTas
        public ActionResult Index()
        {
            var anhMoTas = db.AnhMoTas.Include(a => a.SanPham);
            return View(anhMoTas.ToList());
        }

        // GET: Admin/AnhMoTas/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnhMoTa anhMoTa = db.AnhMoTas.Find(id);
            if (anhMoTa == null)
            {
                return HttpNotFound();
            }
            return View(anhMoTa);
        }

        // GET: Admin/AnhMoTas/Create
        public ActionResult Create()
        {
            ViewBag.MaSP = new SelectList(db.SanPhams, "MaSP", "TenSP");
            return View();
        }

        // POST: Admin/AnhMoTas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaAnh,HinhAnh,MaSP")] AnhMoTa anhMoTa)
        {
            if (ModelState.IsValid)
            {
                db.AnhMoTas.Add(anhMoTa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaSP = new SelectList(db.SanPhams, "MaSP", "TenSP", anhMoTa.MaSP);
            return View(anhMoTa);
        }

        // GET: Admin/AnhMoTas/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnhMoTa anhMoTa = db.AnhMoTas.Find(id);
            if (anhMoTa == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaSP = new SelectList(db.SanPhams, "MaSP", "TenSP", anhMoTa.MaSP);
            return View(anhMoTa);
        }

        // POST: Admin/AnhMoTas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaAnh,HinhAnh,MaSP")] AnhMoTa anhMoTa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(anhMoTa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaSP = new SelectList(db.SanPhams, "MaSP", "TenSP", anhMoTa.MaSP);
            return View(anhMoTa);
        }

        // GET: Admin/AnhMoTas/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnhMoTa anhMoTa = db.AnhMoTas.Find(id);
            if (anhMoTa == null)
            {
                return HttpNotFound();
            }
            return View(anhMoTa);
        }

        // POST: Admin/AnhMoTas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AnhMoTa anhMoTa = db.AnhMoTas.Find(id);
            db.AnhMoTas.Remove(anhMoTa);
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