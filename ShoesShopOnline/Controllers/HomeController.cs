using ShoesShopOnline.Areas.Admin.Data;
using ShoesShopOnline.Models;
using ShoesShopOnline.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoesShopOnline.Controllers
{
    public class HomeController : Controller
    {
        Shoes db = new Shoes();
        // GET: About
        public ActionResult Index()
        {
            ICollection<ProductDetail> sanphamnew = (from s in db.SanPhams select new ProductDetail { }).ToList();
            sanphamnew = (from p in db.SanPhams
                          join a in db.AnhMoTas on p.MaSP equals a.MaSP
                          join kn in db.KhuyenMaiSPs on p.MaKM equals kn.MaKM
                          orderby p.NgayTao descending
                          select new ProductDetail()
                          {
                              MaDM = p.MaDM,
                              MaSP = p.MaSP,
                              TenSP = p.TenSP,
                              GiaBan = p.GiaBan,
                              maAnh = a.MaAnh,
                              Anh = a.HinhAnh,
                              MoTa = p.MoTa,
                              MaKH = p.MaKM,
                              PhanTramKM = kn.PhanTramKM


                          }).ToList();
            //get products hot
            ICollection<ProductDetail> sanphamhot = (from hot in db.ChiTietHoaDons
                                                     join chsp in db.ChiTietSanPhams on hot.MaAnh equals chsp.MaAnh
                                                     join a in db.AnhMoTas on chsp.MaAnh equals a.MaAnh
                                                     join p in db.SanPhams on a.MaSP equals p.MaSP
                                                     select new
                                                     {
                                                         hot,
                                                         chsp,
                                                         a,
                                                         p
                                                     } into t1
                                                     group t1 by t1.hot.MaAnh into hotsp
                                                     orderby hotsp.Count()
                                                     select new ProductDetail
                                                     {
                                                         MaDM = hotsp.FirstOrDefault().p.MaDM,
                                                         MaSP = hotsp.FirstOrDefault().p.MaSP,
                                                         TenSP = hotsp.FirstOrDefault().p.TenSP,
                                                         GiaBan = hotsp.FirstOrDefault().p.GiaBan,
                                                         maAnh = hotsp.FirstOrDefault().chsp.MaAnh,
                                                         Anh = hotsp.FirstOrDefault().a.HinhAnh,
                                                         MoTa = hotsp.FirstOrDefault().p.MoTa,
                                                         MaKH = hotsp.FirstOrDefault().p.MaKM,
                                                         PhanTramKM = hotsp.FirstOrDefault().p.KhuyenMaiSP.PhanTramKM
                                                     }).Take(8).ToList();
            ICollection<ProductDetail> products = new List<ProductDetail>();
            products = Filter(sanphamnew, 8);
            //Get newFeed
            ICollection<TinTuc> newfeed = (from tt in db.TinTucs orderby tt.NgayDang descending select tt).Take(3).ToList();
            ViewBag.ListNewFeed = newfeed;
            //ICollection<ProductDetail> Producthot = Filter(sanphamhot,8);
            ViewBag.ListHot = sanphamhot;
            return View(products.ToList());

        }
        private ICollection<ProductDetail> Filter(ICollection<ProductDetail> products, int count)
        {
            List<ProductDetail> list = new List<ProductDetail>();
            foreach (var item in products)
            {
                int dem = 0;
                foreach (var t in list)
                {
                    if (item.MaSP == t.MaSP)
                        dem++;
                }
                if (dem == 0 && list.Count() < count)
                    list.Add(item);
            }
            return list;
        }
        [ChildActionOnly]
        public ActionResult SearchBox()
        {

            return PartialView();
        }
        [HttpGet]
        public ActionResult Login()
        {
            /*TaiKhoanNguoiDung session = (TaiKhoanNguoiDung)Session[ShoesShopOnline.Session.ConstaintUser.USER_SESSION];
            if (session != null)
            {
                return RedirectToAction("PageNotFound", "Error");
            }*/
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginAccount loginAccount)
        {
            if (ModelState.IsValid)
            {
                TaiKhoan tk = db.TaiKhoans.Where
                (a => a.TenDangNhap.Equals(loginAccount.username) && a.MatKhau.Equals(loginAccount.password)).FirstOrDefault();
                if (tk != null)
                {
                    if (tk.TrangThai == false)
                    {
                        ModelState.AddModelError("ErrorLogin", "Tài khoản của bạn đã bị vô hiệu hóa !");
                    }
                    else
                    {
                        Session.Add(ConstaintUser.USER_SESSION, tk);
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
            Session.Remove(ConstaintUser.USER_SESSION);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            TaiKhoan session = (TaiKhoan)Session[ShoesShopOnline.Session.ConstaintUser.USER_SESSION];
            if (session != null)
            {
                return RedirectToAction("PageNotFound", "Error");
            }
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(TaiKhoan tk)
        {
            TaiKhoan check = db.TaiKhoans.Where
                (a => a.TenDangNhap.Equals(tk.TenDangNhap)).FirstOrDefault();
            if (check != null)
            {
                ModelState.AddModelError("ErrorSignUp", "Tên đăng nhập đã tồn tại");
            }
            else
            {
                try
                {
                    tk.TrangThai = true;
                    tk.RolesID = 3;
                    db.TaiKhoans.Add(tk);
                    db.SaveChanges();
                    TaiKhoan session = db.TaiKhoans.Where(a => a.TenDangNhap.Equals(tk.TenDangNhap)).FirstOrDefault();
                    Session[ShoesShopOnline.Session.ConstaintUser.USER_SESSION] = session;
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("ErrorSignUp", "Đăng ký không thành công. Thử lại sau !");
                }
            }

            return View(tk);
        }
        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Blog()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult DanhMuc()
        {
            IEnumerable<DanhMucSP> danhmucs = db.DanhMucSPs.Select(p => p);
            return PartialView(danhmucs);
        }

        [ChildActionOnly]
        public PartialViewResult HeaderCart()
        {
            var cart = Session[ConstainCart.CartSession];
            var list = new List<CartItem>();

            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return PartialView(list);
        }
        public ActionResult HeThongCuaHang() {
            return View();
            }
    }
}