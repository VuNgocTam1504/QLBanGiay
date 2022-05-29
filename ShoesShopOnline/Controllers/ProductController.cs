using PagedList;
using ShoesShopOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace ShoesShopOnline.Controllers
{
    public class ProductController : Controller
    {
        Shoes db = new Shoes();
        // GET: Product
        [HttpGet]
        public ActionResult Index(string sortOrder,string searchString,string madm, int? page)
        {

            ViewBag.searchString = sortOrder;

            ICollection<ProductDetail> sanphams = (from p in db.SanPhams
                                                   join a in db.AnhMoTas on p.MaSP equals a.MaSP
                                                   join kn in db.KhuyenMaiSPs on p.MaKM equals kn.MaKM
                                                   where p.MaDM == madm
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
            ViewBag.madm = madm;  
            if (!String.IsNullOrEmpty(sortOrder))
            {
                if (sortOrder == "Giam")
                {
                    sanphams = (from p in db.SanPhams
                                join a in db.AnhMoTas on p.MaSP equals a.MaSP
                                join kn in db.KhuyenMaiSPs on p.MaKM equals kn.MaKM
                                where p.MaDM == madm
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
                                }).OrderByDescending(s => s.GiaBan).ToList();
                }
                else if (sortOrder == "Tang")
                {
                    sanphams = (from p in db.SanPhams
                                join a in db.AnhMoTas on p.MaSP equals a.MaSP
                                join kn in db.KhuyenMaiSPs on p.MaKM equals kn.MaKM
                                where p.MaDM == madm
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
                                }).OrderBy(s => s.GiaBan).ToList();
                }
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                sanphams = (from p in db.SanPhams
                            join a in db.AnhMoTas on p.MaSP equals a.MaSP
                            where p.TenSP.Contains(searchString)
                            select new ProductDetail()
                            {
                                MaDM = p.MaDM,
                                MaSP = p.MaSP,
                                TenSP = p.TenSP,
                                GiaBan = p.GiaBan,
                                maAnh = a.MaAnh,
                                Anh = a.HinhAnh,
                                MoTa = p.MoTa
                            }).ToList();
            }
            ICollection<ProductDetail> products = Filter(sanphams,100);
            if (products.Count() == 0)
            {
                ViewBag.Error = "Không tìm được sản phẩm phù hợp!";
            }
            int pageSize = 9;
            int pageNumber = (page ?? 1);
            return View(products.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult ProductDetail(string id, int maImage, string madm)
        {
            //SanPham sp = db.SanPhams.Include("DanhMucSP").Where(s => s.MaSP.Equals(id)).FirstOrDefault();

            var sanpham = (from p in db.SanPhams
                           join a in db.AnhMoTas on p.MaSP equals a.MaSP
                           join kn in db.KhuyenMaiSPs on p.MaKM equals kn.MaKM
                           where p.MaSP.Equals(id) && a.MaAnh == maImage
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
                           }).FirstOrDefault();
            //int maImage = sanpham.maANh;
            ICollection<AnhMoTa> listAnh = (from a in db.AnhMoTas
                                            where a.MaSP.Equals(id)
                                            select a).ToList();
            ICollection<ChiTietSanPham> listSize = (from s in db.ChiTietSanPhams
                                                    where s.MaAnh.Equals(maImage)
                                                    select s).ToList();
            ICollection<ProductDetail> Products = (from p in db.SanPhams
                                                   join a in db.AnhMoTas on p.MaSP equals a.MaSP
                                                   join kn in db.KhuyenMaiSPs on p.MaKM equals kn.MaKM
                                                   where p.MaDM == madm
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
            ICollection<ProductDetail> RelateProducts = Filter(Products,4);
            ViewBag.Images = listAnh;
            ViewBag.SizeList = listSize;
            ViewBag.ListRelate = RelateProducts;
            ViewBag.check = RelateProducts.Count();
            return View(sanpham);
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

    }
}