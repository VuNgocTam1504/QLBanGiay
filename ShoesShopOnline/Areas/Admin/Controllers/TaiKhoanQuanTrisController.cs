using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using ShoesShopOnline.Models;

namespace ShoesShopOnline.Areas.Admin.Controllers
{
    public class TaiKhoanQuanTrisController : BaseController
    {
        private Shoes db = new Shoes();

        // GET: Admin/TaiKhoanQuanTris
        public ActionResult Index(int? page)
        {
            ViewBag.Error = TempData["Error"];
            ViewBag.Success = TempData["Success"];
            var taikhoans = db.TaiKhoans.Include(tk => tk.Role).Where(a => a.RolesID != 3); ;
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(taikhoans.OrderBy(tk => tk.MaTK).ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/TaiKhoanQuanTris/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoanQuanTri = db.TaiKhoans.Find(id);
            if (taiKhoanQuanTri == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoanQuanTri);
        }

        // GET: Admin/TaiKhoanQuanTris/Create
        public ActionResult Create()
        {
            ViewBag.RolesID = new SelectList(db.Roles, "RolesID", "RoleName");
            return View();
        }

        // POST: Admin/TaiKhoanQuanTris/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaTK,TenDangNhap,MatKhau,HoTen,SDT,DiaChi,Email,TrangThai,RolesID")] TaiKhoan taiKhoanQuanTri)
        {
            if (ModelState.IsValid)
            {
                db.TaiKhoans.Add(taiKhoanQuanTri);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RolesID = new SelectList(db.Roles, "RolesID", "RoleName", taiKhoanQuanTri.RolesID);
            return View(taiKhoanQuanTri);
        }

        // GET: Admin/TaiKhoanQuanTris/Edit/5
        public ActionResult Edit(int? id)
        {        
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoanQuanTri = db.TaiKhoans.Find(id);
            if (taiKhoanQuanTri == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoanQuanTri);
        }

        // POST: Admin/TaiKhoanQuanTris/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, bool TrangThai)
        {
            string Error = null;
            string Success = null;
            try
            {
                TaiKhoan login = (TaiKhoan)Session[ShoesShopOnline.Session.ConstaintUser.ADMIN_SESSION];
                if (ModelState.IsValid)
                {
                    TaiKhoan acc = (from tk in db.TaiKhoans where tk.MaTK.Equals(id) select tk).FirstOrDefault();
                    if (login.MaTK == acc.MaTK)
                    {
                        Error = "Bạn không thể sửa tài khoản này!";
                    }
                    else
                    {
                        var taiKhoanQuanTri = db.TaiKhoans.Find(id);
                        taiKhoanQuanTri.TrangThai = TrangThai;
                        db.Entry(taiKhoanQuanTri).State = EntityState.Modified;
                        db.SaveChanges();
                        Success = "Update thành công!";
                    }
                }
                TempData["Error"] = Error;
                TempData["Success"] = Success;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi edit dữ liệu! " + ex.Message;
                return View();
            }
        }

        // GET: Admin/TaiKhoanQuanTris/Delete/5
        public ActionResult Delete(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoanQuanTri = db.TaiKhoans.Find(id);
            if (taiKhoanQuanTri == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoanQuanTri);
        }

        // POST: Admin/TaiKhoanQuanTris/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TaiKhoan taiKhoanQuanTri = db.TaiKhoans.Find(id);
            db.TaiKhoans.Remove(taiKhoanQuanTri);
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