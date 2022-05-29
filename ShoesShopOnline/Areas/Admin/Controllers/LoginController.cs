﻿using ShoesShopOnline.Areas.Admin.Data;
using ShoesShopOnline.Models;
using ShoesShopOnline.Session;
using System.Linq;
using System.Web.Mvc;

namespace ShoesShopOnline.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        Shoes db = new Shoes();
        // GET: Admin/Login
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginAccount loginAccount)
        {
            if (ModelState.IsValid)
            {
                TaiKhoan tk = db.TaiKhoans.Where(a => a.TenDangNhap.Equals(loginAccount.username) && a.RolesID != '3'

                && a.MatKhau.Equals(loginAccount.password)).SingleOrDefault();
               
                if (tk != null)
                {
                    if (tk.TrangThai == false)
                    {
                        ModelState.AddModelError("ErrorLogin", "Tài khoản đã bị vô hiệu hóa! Liên hệ quản trị viên!");
                    }
                    else
                    {
                        Session.Add(ConstaintUser.ADMIN_SESSION, tk);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("ErrorLogin", "Tài khoản hoặc mật khẩu không đúng!");
                }
            }
            return View(loginAccount);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Remove(ConstaintUser.ADMIN_SESSION);
            return RedirectToAction("Index");
        }
    }
}