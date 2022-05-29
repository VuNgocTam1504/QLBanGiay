using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ShoesShopOnline.Models;

namespace ShoesShopOnline.Areas.Admin.Controllers
{
    public class DanhMucSPsController : BaseController
    {
        private Shoes db = new Shoes();

        public ActionResult Index()
        {
            return View(db.DanhMucSPs.ToList());
        }

        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanhMucSP danhMucSP = db.DanhMucSPs.Find(id);
            if (danhMucSP == null)
            {
                return HttpNotFound();
            }
            return View(danhMucSP);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaDM,TenDanhMuc")] DanhMucSP danhMucSP)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Tạo mã danh mục
                    string madm = "";
                    var list = db.DanhMucSPs.ToList();
                    var danhmuc = list.LastOrDefault();
                    if (danhmuc == null)
                    {
                        madm = "DM01";
                    }
                    else
                    {
                        int index = int.Parse(danhmuc.MaDM.Substring(2, 2)) + 1;
                        madm = "DM" + string.Format(CultureInfo.CreateSpecificCulture("da-DK"), "{0:00}", index);
                    }
                    danhMucSP.MaDM = madm;
                    db.DanhMucSPs.Add(danhMucSP);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(danhMucSP);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu !" + ex.Message;
                return View(danhMucSP);
            }
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanhMucSP danhMucSP = db.DanhMucSPs.Find(id);
            if (danhMucSP == null)
            {
                return HttpNotFound();
            }
            return View(danhMucSP);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaDM,TenDanhMuc")] DanhMucSP danhMucSP)
        {
            if (ModelState.IsValid)
            {
                db.Entry(danhMucSP).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(danhMucSP);
        }

        // GET: Admin/DanhMucSPs/Delete/5
        //public ActionResult Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    DanhMucSP danhMucSP = db.DanhMucSPs.Find(id);
        //    if (danhMucSP == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(danhMucSP);
        //}

        // POST: Admin/DanhMucSPs/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(string id)
        //{
        //    DanhMucSP danhMucSP = db.DanhMucSPs.Find(id);
        //    db.DanhMucSPs.Remove(danhMucSP);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        public async Task<JsonResult> Delete(string id)
        {
            try
            {
                DanhMucSP danhMucSP = db.DanhMucSPs.Find(id);
                db.DanhMucSPs.Remove(danhMucSP);
                db.SaveChanges();
                return Json(new { status = true });
            }
            catch (Exception)
            {
                return Json(new { status = false });
            }
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