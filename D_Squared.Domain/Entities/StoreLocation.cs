using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace D_Squared.Domain.Entities
{
    public class StoreLocation
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string StoreNumber { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string AKA { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(40)]
        public string LocationName { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string StoreAddress { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(50)]
        public string StoreCity { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(50)]
        public string State { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(50)]
        public string Zip { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(50)]
        public string County { get; set; }

        [Key]
        [Column(Order = 8)]
        [StringLength(50)]
        public string CityState { get; set; }

        [Key]
        [Column(Order = 9)]
        [StringLength(50)]
        public string Phone { get; set; }

        [Key]
        [Column(Order = 10)]
        [StringLength(50)]
        public string Division { get; set; }

        [Key]
        [Column(Order = 11)]
        [StringLength(50)]
        public string Region { get; set; }

        [Key]
        [Column(Order = 12)]
        [StringLength(50)]
        public string Company { get; set; }

        public string FullAddress { get; set; }

        public int? isClose { get; set; }

        public int? isComp { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CompDate { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CloseDate { get; set; }

        [StringLength(50)]
        public string Market { get; set; }

        [StringLength(50)]
        public string DMAName { get; set; }
    }
}
