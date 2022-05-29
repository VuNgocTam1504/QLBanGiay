namespace ShoesShopOnline.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SanPham")]
    public partial class SanPham
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SanPham()
        {
            AnhMoTas = new HashSet<AnhMoTa>();
        }

        [Key]
        [StringLength(10)]
        public string MaSP { get; set; }

        [Required]
        [StringLength(200)]
        public string TenSP { get; set; }

        [StringLength(20)]
        public string MaDM { get; set; }

        public int? MaKM { get; set; }

        [StringLength(500)]
        public string MoTa { get; set; }

        [Column(TypeName = "money")]
        public decimal GiaBan { get; set; }

        public DateTime NgayTao { get; set; }

        public DateTime NgaySua { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AnhMoTa> AnhMoTas { get; set; }

        public virtual DanhMucSP DanhMucSP { get; set; }

        public virtual KhuyenMaiSP KhuyenMaiSP { get; set; }
    }
}
