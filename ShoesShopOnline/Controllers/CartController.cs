using ShoesShopOnline.Models;
using ShoesShopOnline.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ShoesShopOnline.Controllers
{
    public class CartController : Controller
    {
        Shoes db = new Shoes();

        // GET: Cart
        public ActionResult Index()
        {
            var cart = Session[ConstainCart.CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }

        //C1
        //public ActionResult AddItem(int maSP, int quality)
        //{
        //    var SanPham = db.ChiTietSanPhams.Where(p => p.MaAnh == maSP).FirstOrDefault();

        //    var cart = Session[CartSession];
        //    if (cart != null)
        //    {
        //        var list = (List<CartItem>)cart;
        //        if (list.Exists(x => x.ChiTietSanPham.MaAnh == maSP))
        //        {
        //            foreach (var item in list)
        //            {
        //                if (item.ChiTietSanPham.MaAnh == maSP)
        //                    item.SoLuong += quality;
        //            }
        //        }
        //        else
        //        {
        //            var item = new CartItem();
        //            item.ChiTietSanPham = SanPham;
        //            item.MaAnh = SanPham.MaAnh;
        //            item.SoLuong = quality;
        //            item.Gia = SanPham.AnhMoTa.SanPham.GiaBan;
        //            list.Add(item);
        //        }
        //        Session[CartSession] = list;
        //    }
        //    else
        //    {
        //        var item = new CartItem();
        //        item.ChiTietSanPham = SanPham;
        //        item.MaAnh = SanPham.MaAnh;
        //        item.SoLuong = quality;
        //        item.Gia = SanPham.AnhMoTa.SanPham.GiaBan;
        //        var list = new List<CartItem>();
        //        list.Add(item);
        //        Session[CartSession] = list;
        //    }
        //    return RedirectToAction("Index");
        //}

        //C2
        public JsonResult AddCart(int maSP, int kichCo)
        {
            var SanPham = db.ChiTietSanPhams.Where(p => p.MaAnh == maSP).FirstOrDefault();
            var cart = Session[ConstainCart.CartSession];
            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                if (list.Exists(x => x.ChiTietSanPham.MaAnh == maSP && x.KichCo == kichCo))
                {
                    foreach (var item in list)
                    {
                        if (item.ChiTietSanPham.MaAnh == maSP && item.KichCo == kichCo)
                            item.SoLuong += 1;
                    }
                }
                else
                {
                    var item = new CartItem();
                    item.ChiTietSanPham = SanPham;
                    item.MaAnh = SanPham.MaAnh;
                    item.KichCo = kichCo;
                    item.SoLuong = 1;
                    if (item.ChiTietSanPham.AnhMoTa.SanPham.KhuyenMaiSP.MaKM !=null)
                    {
                        item.Gia = SanPham.AnhMoTa.SanPham.GiaBan - (item.ChiTietSanPham.AnhMoTa.SanPham.GiaBan * item.ChiTietSanPham.AnhMoTa.SanPham.KhuyenMaiSP.PhanTramKM);
                    }
                    else
                    {
                        item.Gia = SanPham.AnhMoTa.SanPham.GiaBan;
                    }
                   
                    list.Add(item);
                }
                Session[ConstainCart.CartSession] = list;
            }
            else
            {
                var item = new CartItem();
                item.ChiTietSanPham = SanPham;
                item.MaAnh = SanPham.MaAnh;
                item.KichCo = kichCo;
                item.SoLuong = 1;
                if (item.ChiTietSanPham.AnhMoTa.SanPham.KhuyenMaiSP.MaKM != null)
                {
                    item.Gia = SanPham.AnhMoTa.SanPham.GiaBan - (item.ChiTietSanPham.AnhMoTa.SanPham.GiaBan * item.ChiTietSanPham.AnhMoTa.SanPham.KhuyenMaiSP.PhanTramKM);
                }
                else
                {
                    item.Gia = SanPham.AnhMoTa.SanPham.GiaBan;
                }
                var list = new List<CartItem>();
                list.Add(item);
                Session[ConstainCart.CartSession] = list;
            }
            return Json(new
            {
                status = true
            });
        }

        public JsonResult Update(string cartModel)
        {
            var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel);
            var sessionCart = (List<CartItem>)Session[ConstainCart.CartSession];
            foreach (var item in sessionCart)
            {
                var jsonItem = jsonCart.SingleOrDefault(x => x.ChiTietSanPham.MaAnh == item.MaAnh && x.KichCo == item.KichCo);
                if (jsonItem != null)
                {
                    item.SoLuong = jsonItem.SoLuong;
                }
            }
            Session[ConstainCart.CartSession] = sessionCart;
            return Json(sessionCart, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(int MaAnh, int KichCo)
        {
            var sessionCart = (List<CartItem>)Session[ConstainCart.CartSession];
            sessionCart.RemoveAll(x => x.MaAnh == MaAnh && x.KichCo == KichCo);
            Session[ConstainCart.CartSession] = sessionCart;
            return Json(sessionCart, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Payment()
        {

            if (Session[ShoesShopOnline.Session.ConstaintUser.USER_SESSION] == null || Session[ShoesShopOnline.Session.ConstaintUser.USER_SESSION].ToString() == "")
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                var cart = Session[ConstainCart.CartSession];
                var list = new List<CartItem>();
                if (cart != null)
                {
                    list = (List<CartItem>)cart;
                }
                return View(list);
            }
        }

        public ActionResult Payment(FormCollection collection)
        {
            //Thêm đơn hàng
            HoaDon hoaDon = new HoaDon();
            TaiKhoan tk = (TaiKhoan)Session[ShoesShopOnline.Session.ConstaintUser.USER_SESSION];
            hoaDon.MaTK = tk.MaTK;
            hoaDon.HoTenNguoiNhan = collection["name"];
            hoaDon.SDTNguoiNhan = collection["phone"];
            hoaDon.EmailNguoiNhan = collection["email"];
            hoaDon.DiaChiNhan = collection["address"];
            hoaDon.GhiChu = collection["note"];
            hoaDon.NgayLap = DateTime.Now;
            hoaDon.TrangThai = "Chờ xác nhận";
            db.HoaDons.Add(hoaDon);
            db.SaveChanges();
            //Thêm chi tiết đơn hàng
            var cart = Session[ConstainCart.CartSession];
            var list = new List<CartItem>();
            list = (List<CartItem>)cart;
            foreach (var item in list)
            {
                ChiTietHoaDon cthd = new ChiTietHoaDon();
                cthd.MaHD = hoaDon.MaHD;
                cthd.MaAnh = item.MaAnh;
                cthd.KichCo = item.KichCo;
                cthd.SoluongMua = item.SoLuong;
                db.ChiTietHoaDons.Add(cthd);
            }
            db.SaveChanges();
            Session[ConstainCart.CartSession] = null;
            return RedirectToAction("SubmitOrder", "Cart");
        }

        public ActionResult SubmitOrder()
        {
            return View();
        }
    }

}