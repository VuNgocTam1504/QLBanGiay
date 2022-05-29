using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PagedList;
using ShoesShopOnline.Models;

namespace ShoesShopOnline.Areas.Admin.Controllers
{
    public class TaiKhoanNguoiDungsController : BaseController
    {
        private Shoes db = new Shoes();

        // GET: Admin/TaiKhoanNguoiDungs
        public ActionResult Index(string searchString, int? page)
        {
            ViewBag.searchString = searchString;
            var taikhoans = db.TaiKhoans.Include(tk => tk.Role).Where(a =>a.RolesID == 3);
            if (!String.IsNullOrEmpty(searchString))
            {
                taikhoans = taikhoans.Where(tk => tk.TenDangNhap.Contains(searchString));
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(taikhoans.OrderBy(tk => tk.MaTK).ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/TaiKhoanNguoiDungs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoanNguoiDung = db.TaiKhoans.Find(id);
            if (taiKhoanNguoiDung == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoanNguoiDung);
        }

        // GET: Admin/TaiKhoanNguoiDungs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/TaiKhoanNguoiDungs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaTK,TenDangNhap,MatKhau,HoTen,SDT,DiaChi,Email,TrangThai")] TaiKhoan taiKhoanNguoiDung)
        {
            if (ModelState.IsValid)
            {
                db.TaiKhoans.Add(taiKhoanNguoiDung);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(taiKhoanNguoiDung);
        }

        // GET: Admin/TaiKhoanNguoiDungs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoanNguoiDung = db.TaiKhoans.Find(id);
            if (taiKhoanNguoiDung == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoanNguoiDung);
        }

        // POST: Admin/TaiKhoanNguoiDungs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, bool TrangThai)
        {
            try
            {
                if (ModelState.IsValid)
                {
                        var tk = db.TaiKhoans.Find(id);
                        tk.TrangThai = TrangThai;
                        db.Entry(tk).State = EntityState.Modified;
                        db.SaveChanges();
                    
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi edit dữ liệu! " + ex.Message;
                return View();
            }
        }

        // GET: Admin/TaiKhoanNguoiDungs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoanNguoiDung = db.TaiKhoans.Find(id);
            if (taiKhoanNguoiDung == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoanNguoiDung);
        }

        // POST: Admin/TaiKhoanNguoiDungs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TaiKhoan taiKhoanNguoiDung = db.TaiKhoans.Find(id);
            db.TaiKhoans.Remove(taiKhoanNguoiDung);
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