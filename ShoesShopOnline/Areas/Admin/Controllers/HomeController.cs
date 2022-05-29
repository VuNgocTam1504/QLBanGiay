using ShoesShopOnline.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ShoesShopOnline.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        Shoes db = new Shoes();
        // GET: Admin/Home
        public ActionResult Index()
        {
            /*int? dem = 0;
            foreach (var item in db.HoaDons)
            {
                if(item.TrangThai.Equals("Chờ xác nhận"))
                {
                    dem++;
                }
            }
            ViewBag.tong = dem;
            return View();*/
            int? dem = 0, demsp = 0;
            decimal dt = 0;
            int thang, nam;
            foreach (var item in db.HoaDons)
            {
                if (item.TrangThai.Equals("Chờ xác nhận"))
                {
                    dem++;
                }
            }
            ViewBag.tong = dem;
            demsp = db.SanPhams.Count();
            ViewBag.tongsp = demsp;

            if (DateTime.Now.Month == 1)
            {
                thang = 12;
                nam = DateTime.Now.Year - 1;
            }
            else
            {
                thang = DateTime.Now.Month - 1;
                nam = DateTime.Now.Year;
            }
            var list = db.HoaDons.Where(p => p.NgayLap.Month == thang && p.NgayLap.Year == nam && p.TrangThai == "Đã thanh toán");
            foreach (var item in list)
            {
                dt += decimal.Parse(item.ChiTietHoaDons.Sum(p => p.SoluongMua * p.ChiTietSanPham.AnhMoTa.SanPham.GiaBan).ToString());
            }
            ViewBag.doanhthu = dt;
            int[] dataOfYear = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            for (int i = 0; i < 12; i++)
            {
                int month = i + 1;
                decimal data = 0;
                try
                {
                    var listdt = db.HoaDons.Where(b => b.NgayLap.Month == month && b.NgayLap.Year == DateTime.Now.Year && b.TrangThai == "Đã thanh toán");
                    foreach (var item in listdt)
                    {
                        data += decimal.Parse(item.ChiTietHoaDons.Sum(p => p.SoluongMua * p.ChiTietSanPham.AnhMoTa.SanPham.GiaBan).ToString());
                    }
                }
                catch (Exception ex)
                {

                }
                dataOfYear[i] = (int)data;
            }
            ViewBag.dataOfYear = dataOfYear;
            int countlh = 0;
            countlh = db.Contacts.Count();
            ViewBag.LienHe = countlh;
            return View();
        }
    }
}