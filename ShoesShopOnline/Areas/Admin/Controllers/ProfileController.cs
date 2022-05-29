using ShoesShopOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoesShopOnline.Areas.Admin.Controllers
{
    public class ProfileController : Controller
    {
        Shoes db = new Shoes();
        // GET: Admin/Profile
        [HttpGet]
        public ActionResult AdminInfor(int id)
        {
            TaiKhoan session = (TaiKhoan)Session[ShoesShopOnline.Session.ConstaintUser.ADMIN_SESSION];
            if (session == null)
            {
                return RedirectToAction("PageNotFound", "Error");
            }
            else
            {
                TaiKhoan tk = db.TaiKhoans.Where(a => a.MaTK.Equals(id)).FirstOrDefault();
                return View(tk);
            }
        }

        [HttpPost]
        public ActionResult AdminInfor([Bind(Include = "MaTK,HoTen,TenDangNhap,MatKhau,DiaChi,Email,SDT")] TaiKhoan tk)
        {
            TaiKhoan edit = db.TaiKhoans.Where(a => a.MaTK.Equals(tk.MaTK)).FirstOrDefault();
            try
            {
                edit.HoTen = tk.HoTen;
                edit.TenDangNhap = tk.TenDangNhap;
                edit.MatKhau = tk.MatKhau;
                edit.DiaChi = tk.DiaChi;
                edit.Email = tk.Email;
                edit.SDT = tk.SDT;
                db.SaveChanges();
                Session[ShoesShopOnline.Session.ConstaintUser.ADMIN_SESSION] = edit;
            }
            catch (Exception)
            {
                ModelState.AddModelError("ErrorUpdate", "Cập nhật thông tin không thành công ! Thử lại sau !");
            }
            return View(edit);
        }
    }
}