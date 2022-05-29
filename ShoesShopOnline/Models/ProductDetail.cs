using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoesShopOnline.Models
{
    public class ProductDetail
    {
        [StringLength(20)]
        [DisplayName("Mã sản phẩm")]
        public string MaSP { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống!")]
        [StringLength(200)]
        [DisplayName("Tên sản phẩm")]
        public string TenSP { get; set; }

        [StringLength(20)]
        [DisplayName("Mã danh mục")]
        public string MaDM { get; set; }

        [DisplayName("Ngày tạo")]
        public DateTime NgayTao { get; set; }

        [DisplayName("Ngày sửa")]
        public DateTime NgaySua { get; set; }

        [Required(ErrorMessage = "Mô tả sản phẩm không được để trống!")]
        [StringLength(500)]
        [DisplayName("Mô tả")]
        public string MoTa { get; set; }

        [Required(ErrorMessage = "Giá bán không được để trống!")]
        [Column(TypeName = "money")]
        [DisplayName("Giá bán")]
        public decimal GiaBan { get; set; }

        [StringLength(300)]
        [DisplayName("Ảnh sản phẩm")]
        public string Anh { get; set; }

        [Required(ErrorMessage = "Size sản phẩm không được để trống!")]
        [StringLength(300)]
        [DisplayName("Các size sẵn có")]
        public string Size { get; set; }
        public int maAnh { get; set; }
        public int? MaKH { get; set; }
        public decimal PhanTramKM { get; set; }

        public virtual DanhMucSP DanhMucSP { get; set; }

    }
}