using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;
using ShoesShopOnline.Models;

namespace ShoesShopOnline.Areas.Admin.Controllers
{
    public class SanPhamsController : BaseController
    {
        private Shoes db = new Shoes();

        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page,string madm, string makm)
        {
            /*var result = (from p in db.SanPhams
                           join a in db.AnhMoTas on p.MaSP equals a.MaSP
                           join kn in db.KhuyenMaiSPs on p.MaKM equals kn.MaKM
                           select new ProductDetail()
                           {
                               MaSP = p.MaSP,
                               TenSP = p.TenSP,
                               MaDM = p.DanhMucSP.TenDanhMuc,
                               Anh = a.HinhAnh,
                               MoTa = p.MoTa,
                               GiaBan = p.GiaBan,
                               NgaySua = p.NgaySua,
                               MaKH = p.MaKM,
                           }).ToList();*/
            var result = db.SanPhams.Join(db.AnhMoTas, p => p.MaSP, a => a.MaSP, (p, a) => new ProductDetail
            {
                MaSP = p.MaSP,
                TenSP = p.TenSP,
                MaDM = p.DanhMucSP.TenDanhMuc,
                Anh = a.HinhAnh,
                MoTa = p.MoTa,
                GiaBan = p.GiaBan,
                NgaySua = p.NgaySua,
                MaKH = p.MaKM,

            }).ToList();
            var productDetails = new List<ProductDetail>();
            foreach (var item in result)
            {
                int dem = 0;
                foreach (var t in productDetails)
                {
                    if (item.MaSP.Equals(t.MaSP))
                        dem++;
                }
                if (dem == 0)
                    productDetails.Add(item);
            }
            productDetails = productDetails.OrderByDescending(p => p.NgaySua).ToList();
            ViewBag.CurrentSort = sortOrder;
            ViewBag.SapTheoTen = String.IsNullOrEmpty(sortOrder) ? "ten_desc" : "";
            ViewBag.SapTheoGia = sortOrder == "gia" ? "gia_desc" : "gia";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                productDetails = productDetails.Where(p => p.TenSP.ToLower().Contains(searchString.ToLower())).ToList();
            }
            switch (sortOrder)
            {
                case "ten_desc":
                    productDetails = productDetails.OrderByDescending(p => p.TenSP).ToList();
                    break;
                case "gia":
                    productDetails = productDetails.OrderBy(p => p.GiaBan).ToList();
                    break;
                case "gia_desc":
                    productDetails = productDetails.OrderByDescending(p => p.GiaBan).ToList();
                    break;
                default:
                    productDetails = productDetails.OrderByDescending(p => p.NgaySua).ToList();
                    break;
            }
            ViewBag.MaDM = new SelectList(db.DanhMucSPs, "MaDM", "TenDanhmuc");
            ViewBag.MaKM = new SelectList(db.KhuyenMaiSPs, "MaKM", "MaKM");
            if (madm != null && makm!=null)
            {
                var sp = db.SanPhams.Where(x => x.MaDM == madm).ToList();
                foreach (var item in sp)
                {
                    if (item.MaKM == null)
                    {
                        item.MaKM = int.Parse(makm);
                    }
                    item.MaKM =int.Parse( makm);
                    db.SaveChanges();
                }
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(productDetails.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Details(string id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                SanPham sanPham = db.SanPhams.Find(id);
                if (sanPham == null)
                {
                    return HttpNotFound();
                }
                var images = db.AnhMoTas.Where(p => p.MaSP == id).ToList();
                var maAnh = 0;
                string filePath = "";
                if (images != null)
                {
                    foreach (var item in images)
                    {
                        filePath += item.HinhAnh + ";";
                        maAnh = item.MaAnh;
                    }
                }
                filePath = filePath.Substring(0, filePath.Length - 1);
                ViewBag.filePath = filePath;
                var sizes = db.ChiTietSanPhams.Where(p => p.MaAnh == maAnh).ToList();
                string listSize = "";
                if (sizes != null)
                {
                    foreach (var item in sizes)
                    {
                        listSize += item.KichCo + "; ";
                    }
                }
                listSize = listSize.Substring(0, listSize.Length - 2);
                ViewBag.listSize = listSize;
                return View(sanPham);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi !" + ex.Message;
                return View();
            }

        }

        public ActionResult Create()
        {
            ViewBag.MaDM = new SelectList(db.DanhMucSPs, "MaDM", "TenDanhMuc");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductDetail productDetail, List<HttpPostedFileBase> uploadFile)
        {
            try
            {
                productDetail.Anh = "";
                if (ModelState.IsValid)
                {
                    SanPham sanPham = new SanPham();
                    string maSP = "";
                    var list = db.SanPhams.ToList();
                    var sanpham = list.LastOrDefault();
                    if (sanpham == null)
                    {
                        maSP = "SP001";
                    }
                    else
                    {
                        int index = int.Parse(sanpham.MaSP.Substring(2, 3)) + 1;
                        maSP = "SP" + string.Format(CultureInfo.CreateSpecificCulture("da-DK"), "{0:000}", index);
                    }
                    sanPham.MaSP = maSP;
                    sanPham.TenSP = productDetail.TenSP;
                    sanPham.MaDM = productDetail.MaDM;
                    sanPham.MoTa = productDetail.MoTa;
                    sanPham.GiaBan = productDetail.GiaBan;
                    sanPham.NgayTao = DateTime.Now;
                    sanPham.NgaySua = DateTime.Now;
                    sanpham.MaKM = productDetail.MaKH;
                    db.SanPhams.Add(sanPham);
                    db.SaveChanges();
                    //Add Anh
                    string[] sizes = productDetail.Size.Split(';');
                    foreach (var item in uploadFile)
                    {
                        AnhMoTa anhMoTa = new AnhMoTa();
                        string FileName = item.FileName;
                        string filePath = Path.Combine(HttpContext.Server.MapPath("/wwwroot/Images"),
                                                       Path.GetFileName(item.FileName));
                        item.SaveAs(filePath);
                        anhMoTa.HinhAnh = FileName;
                        anhMoTa.MaSP = maSP;
                        db.AnhMoTas.Add(anhMoTa);
                        db.SaveChanges();
                        //Add san pham chi tiet
                        for (int j = 0; j < sizes.Length; j++)
                        {
                            ChiTietSanPham chiTietSanPham = new ChiTietSanPham();
                            chiTietSanPham.MaAnh = anhMoTa.MaAnh;
                            chiTietSanPham.KichCo = int.Parse(sizes[j]);
                            db.ChiTietSanPhams.Add(chiTietSanPham);
                            db.SaveChanges();
                        }
                    }

                    return RedirectToAction("Index");
                }
                ViewBag.MaDM = new SelectList(db.DanhMucSPs, "MaDM", "TenDanhMuc", productDetail.MaDM);
                return View(productDetail);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu !" + ex.Message;
                ViewBag.MaDM = new SelectList(db.DanhMucSPs, "MaDM", "TenDanhMuc", productDetail.MaDM);
                return View(productDetail);

            }

        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            int maAnh = 0;
            var images = db.AnhMoTas.Where(p => p.MaSP == id).ToList();
            string filePath = "";
            if (images != null)
            {
                foreach (var item in images)
                {
                    maAnh = item.MaAnh;
                    filePath += item.HinhAnh + ";";
                }
            }
            filePath = filePath.Substring(0, filePath.Length - 1);
            ViewBag.filePath = filePath;
            string listSize = "";
            var sizes = db.ChiTietSanPhams.Where(p => p.MaAnh == maAnh).ToList();
            if (sizes != null)
            {
                foreach (var item in sizes)
                {
                    listSize += item.KichCo + ";";
                }
            }
            listSize = listSize.Substring(0, listSize.Length - 1);
            ViewBag.listSize = listSize;
            ViewBag.MaDM = new SelectList(db.DanhMucSPs, "MaDM", "TenDanhMuc", sanPham.MaDM);
            ProductDetail productDetail = new ProductDetail()
            {
                MaSP = sanPham.MaSP,
                TenSP = sanPham.TenSP,
                GiaBan = sanPham.GiaBan,
                Anh = filePath,
                Size = listSize,
                MoTa = sanPham.MoTa,
                MaDM = sanPham.MaDM,
                NgayTao = sanPham.NgayTao,
                NgaySua = sanPham.NgaySua,
                DanhMucSP = sanPham.DanhMucSP,
                MaKH = sanPham.MaKM
            };
            return View(productDetail);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductDetail productDetail, List<HttpPostedFileBase> uploadFile, string id)
        {
            try
            {
                if (productDetail.Anh == null && uploadFile == null)
                {
                    ViewBag.Error = "Chọn ảnh bạn ơi !";
                    ViewBag.MaDM = new SelectList(db.DanhMucSPs, "MaDM", "TenDanhMuc", productDetail.MaDM);
                    return View(productDetail);
                }
                if (ModelState.IsValid)
                {
                    var sanPham = db.SanPhams.Where(p => p.MaSP == id).FirstOrDefault();
                    sanPham.TenSP = productDetail.TenSP;
                    sanPham.MoTa = productDetail.MoTa;
                    sanPham.GiaBan = productDetail.GiaBan;
                    sanPham.MaDM = productDetail.MaDM;
                    sanPham.NgaySua = DateTime.Now;
                    sanPham.MaKM = productDetail.MaKH;
                    db.SaveChanges();
                    string[] sizes = productDetail.Size.Split(';');
                    //Xóa ảnh cũ bị loại bỏ
                    var listimages = db.AnhMoTas.Where(p => p.MaSP == id).ToList();

                    //string[] newimages = productDetail.Anh;
                    if (productDetail.Anh != null)
                    {
                        string[] newimages = productDetail.Anh.Split(';');
                        foreach (var item in listimages)
                        {
                            int dem = 0;
                            for (int i = 0; i < newimages.Length; i++)
                                if (item.HinhAnh == newimages[i])
                                    dem++;
                            if (dem == 0)
                            {
                                db.AnhMoTas.Remove(item);
                                db.SaveChanges();
                            }
                            else
                            {
                                //Cập nhật lại size cho ảnh cũ
                                var sizecu = db.ChiTietSanPhams.Where(p => p.MaAnh == item.MaAnh);
                                foreach (var itemcu in sizecu)
                                {
                                    db.ChiTietSanPhams.Remove(itemcu);
                                }
                                for (int j = 0; j < sizes.Length; j++)
                                {
                                    ChiTietSanPham chiTietSanPham = new ChiTietSanPham();
                                    chiTietSanPham.MaAnh = item.MaAnh;
                                    chiTietSanPham.KichCo = int.Parse(sizes[j]);
                                    db.ChiTietSanPhams.Add(chiTietSanPham);
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in listimages)
                        {
                            db.AnhMoTas.Remove(item);
                            db.SaveChanges();
                        }
                    }

                    //Lưu thêm ảnh mới
                    if (uploadFile != null)
                    {
                        foreach (var item in uploadFile)
                        {
                            if (item != null)
                            {
                                AnhMoTa anhMoTa = new AnhMoTa();
                                string FileName = item.FileName;
                                string filePath = Path.Combine(HttpContext.Server.MapPath("/wwwroot/Images"),
                                                               Path.GetFileName(item.FileName));
                                item.SaveAs(filePath);
                                anhMoTa.HinhAnh = FileName;
                                anhMoTa.MaSP = id;
                                db.AnhMoTas.Add(anhMoTa);
                                db.SaveChanges();

                                //Add san pham chi tiet
                                for (int j = 0; j < sizes.Length; j++)
                                {
                                    ChiTietSanPham chiTietSanPham = new ChiTietSanPham();
                                    chiTietSanPham.MaAnh = anhMoTa.MaAnh;
                                    chiTietSanPham.KichCo = int.Parse(sizes[j]);
                                    db.ChiTietSanPhams.Add(chiTietSanPham);
                                }
                                db.SaveChanges();
                            }
                        }
                    }

                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu !" + ex.Message;
                ViewBag.MaDM = new SelectList(db.DanhMucSPs, "MaDM", "TenDanhMuc", productDetail.MaDM);
                return View(productDetail);

            }
        }


        [HttpPost]
        public async Task<JsonResult> Delete(string id)
        {
            try
            {
                SanPham sanPham = db.SanPhams.Find(id);
                db.SanPhams.Remove(sanPham);
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