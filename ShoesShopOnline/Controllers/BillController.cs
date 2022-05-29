using PagedList;
using ShoesShopOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ShoesShopOnline.Controllers
{
    public class BillController : Controller
    {
        Shoes db = new Shoes();

        // GET: Bill
        [HttpGet]
        public ActionResult Index(int? page)
        {
            if (Session[ShoesShopOnline.Session.ConstaintUser.USER_SESSION] == null || Session[ShoesShopOnline.Session.ConstaintUser.USER_SESSION].ToString() == "")
            {
                return RedirectToAction("Login", "Home");
            }
            List<HoaDon> list = new List<HoaDon>();
            TaiKhoan tk = (TaiKhoan)Session[ShoesShopOnline.Session.ConstaintUser.USER_SESSION];
            list = db.HoaDons.Where(p => p.MaTK == tk.MaTK).OrderByDescending(x => x.NgayLap).ToList();
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            TaiKhoan tk = (TaiKhoan)Session[ShoesShopOnline.Session.ConstaintUser.USER_SESSION];
            if (tk == null)
            {
                return RedirectToAction("PageNotFound", "Error");
            }
            else
            {
                if (db.HoaDons.FirstOrDefault(x => x.MaTK == tk.MaTK) == null)
                {
                    return RedirectToAction("PageNotFound", "Error");
                }
            }
            var hoadons = db.ChiTietHoaDons.Where(x => x.MaHD == id).ToList();
            var sanphams = new List<CartItem>();
            foreach (var item in hoadons)
            {
                var sanpham = new CartItem();
                sanpham.ChiTietSanPham = item.ChiTietSanPham;
                if (item.ChiTietSanPham.AnhMoTa.SanPham.KhuyenMaiSP.MaKM != null)
                {
                    sanpham.Gia = item.ChiTietSanPham.AnhMoTa.SanPham.GiaBan - (item.ChiTietSanPham.AnhMoTa.SanPham.GiaBan * item.ChiTietSanPham.AnhMoTa.SanPham.KhuyenMaiSP.PhanTramKM);
                }
                else
                {
                    sanpham.Gia = item.ChiTietSanPham.AnhMoTa.SanPham.GiaBan;
                }
                sanpham.MaAnh = item.MaAnh;
                sanpham.SoLuong = item.SoluongMua;
                sanpham.KichCo = item.KichCo;
                sanphams.Add(sanpham);
            }
            var hoadon = db.HoaDons.Where(x => x.MaHD == id).FirstOrDefault();
            ViewBag.HoaDon = hoadon;
            return View(sanphams);
        }


        [HttpPost]
        public ActionResult Details(int id, FormCollection collection)
        {

            TaiKhoan tk = (TaiKhoan)Session[ShoesShopOnline.Session.ConstaintUser.USER_SESSION];
            var hoaDon = db.HoaDons.Where(x => x.MaHD == id).FirstOrDefault();
            hoaDon.MaTK = tk.MaTK;
            hoaDon.HoTenNguoiNhan = collection["name"];
            hoaDon.SDTNguoiNhan = collection["phone"];
            hoaDon.EmailNguoiNhan = collection["email"];
            hoaDon.DiaChiNhan = collection["address"];
            hoaDon.GhiChu = collection["note"];
            db.SaveChanges();
            var hoadons = db.ChiTietHoaDons.Where(x => x.MaHD == id).ToList();
            var sanphams = new List<CartItem>();
            foreach (var item in hoadons)
            {
                var sanpham = new CartItem();
                sanpham.ChiTietSanPham = item.ChiTietSanPham;
                sanpham.Gia = item.ChiTietSanPham.AnhMoTa.SanPham.GiaBan;
                sanpham.MaAnh = item.MaAnh;
                sanpham.SoLuong = item.SoluongMua;
                sanpham.KichCo = item.KichCo;
                sanphams.Add(sanpham);
            }
            var hoadon = db.HoaDons.Where(x => x.MaHD == id).FirstOrDefault();
            ViewBag.HoaDon = hoadon;
            return View(sanphams);
        }
        [HttpPost]
        public JsonResult ChangeStatus(int mahd, string stt)
        {
            try
            {
                TaiKhoan tk = (TaiKhoan)Session[ShoesShopOnline.Session.ConstaintUser.USER_SESSION];
                HoaDon hd = db.HoaDons.Where(x => x.MaHD == mahd).FirstOrDefault();
                if (hd.TrangThai == "Đã thanh toán" || hd.TrangThai == "Đang giao hàng")
                {
                    return Json(new { status = false }, JsonRequestBehavior.AllowGet);
                }
                hd.TrangThai = stt;
                db.SaveChanges();
                return Json(new { status = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = false }, JsonRequestBehavior.AllowGet);
            }
        }



    }
}