using ShoesShopOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace ShoesShopOnline.Models
{
    public class CartItem
    {
        [ScriptIgnore]
        public virtual ChiTietSanPham ChiTietSanPham { set; get; }
        public int MaAnh { set; get; }
        public int KichCo { set; get; }
        public int SoLuong { set; get; }
        public decimal Gia { set; get; }
    }
}