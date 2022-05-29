namespace ShoesShopOnline.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HoaDon")]
    public partial class HoaDon
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HoaDon()
        {
            ChiTietHoaDons = new HashSet<ChiTietHoaDon>();
        }

        [Key]
        public int MaHD { get; set; }

        public int? MaTK { get; set; }

        [Required]
        [StringLength(50)]
        public string HoTenNguoiNhan { get; set; }

        [Required]
        [StringLength(20)]
        public string SDTNguoiNhan { get; set; }

        [Required]
        [StringLength(100)]
        public string DiaChiNhan { get; set; }

        [StringLength(50)]
        public string EmailNguoiNhan { get; set; }

        public DateTime NgayLap { get; set; }

        [StringLength(50)]
        public string TrangThai { get; set; }

        [StringLength(50)]
        public string GhiChu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; }

        public virtual TaiKhoan TaiKhoan { get; set; }
    }
}
