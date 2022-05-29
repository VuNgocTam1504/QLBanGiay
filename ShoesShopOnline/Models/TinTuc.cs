namespace ShoesShopOnline.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TinTuc")]
    public partial class TinTuc
    {
        [Key]
        public int MaTin { get; set; }

        [StringLength(100)]
        public string TenTin { get; set; }

        public DateTime NgayDang { get; set; }

        [Column(TypeName = "ntext")]
        public string NoiDung { get; set; }
    }
}
