using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace D_Squared.Domain.Entities
{
    [Table("TipCredit")]
    public class TipCredit
    {
        [Key]
        [StringLength(20)]
        public string State { get; set; }

        [Required]
        [StringLength(40)]
        public string StateName { get; set; }

        public decimal Tipped { get; set; }

        public decimal Full { get; set; }

        [Column("TipCredit")]
        public decimal? TipCredit1 { get; set; }
    }
}
