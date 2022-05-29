using ShoesShopOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoesShopOnline.Controllers
{
    public class AccountUserController : Controller
    {
        Shoes db = new Shoes();
        // GET: AccountUser
        [HttpGet]
        public ActionResult ChangePassWord()
        {
            if (Session[ShoesShopOnline.Session.ConstaintUser.USER_SESSION] == null || Session[ShoesShopOnline.Session.ConstaintUser.USER_SESSION].ToString() == "")
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassWord(string oldpassword, string password)
        {
            TaiKhoan tk = (TaiKhoan)Session[ShoesShopOnline.Session.ConstaintUser.USER_SESSION];
            if (tk.MatKhau != oldpassword)
            {
                ModelState.AddModelError("ErrorUpdate", "Mật khẩu cũ không đúng");
            }
            else
            {
                TaiKhoan edit = db.TaiKhoans.Where(a => a.MaTK.Equals(tk.MaTK)).FirstOrDefault();
                edit.MatKhau = password;
                db.SaveChanges();
                Session[ShoesShopOnline.Session.ConstaintUser.USER_SESSION] = edit;
                ModelState.AddModelError("ErrorUpdate", "Đổi mật khẩu thành công!");
            }
            return View();
        }


        [HttpGet]
        public ActionResult UserInfor(int id)
        {
            TaiKhoan session = (TaiKhoan)Session[ShoesShopOnline.Session.ConstaintUser.USER_SESSION];
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
        public ActionResult UserInfor([Bind(Include = "MaTK,HoTen,SDT,DiaChi,Email")] TaiKhoan tk)
        {
            TaiKhoan edit = db.TaiKhoans.Where(a => a.MaTK.Equals(tk.MaTK)).FirstOrDefault();
            try
            {
                edit.HoTen = tk.HoTen;
                edit.DiaChi = tk.DiaChi;
                edit.Email = tk.Email;
                edit.SDT = tk.SDT;
                db.SaveChanges();
                Session[ShoesShopOnline.Session.ConstaintUser.USER_SESSION] = edit;
            }
            catch (Exception)
            {
                ModelState.AddModelError("ErrorUpdate", "Cập nhật thông tin không thành công ! Thử lại sau !");
            }
            return View(edit);
        }
    }
}