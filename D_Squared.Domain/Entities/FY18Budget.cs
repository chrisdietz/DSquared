using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace D_Squared.Domain.Entities
{
    public class FY18Budget
    {
        [Key]
        [Column("Line Item", Order = 0)]
        [StringLength(50)]
        public string Line_Item { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Account { get; set; }

        [Key]
        [Column("AX CC", Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AX_CC { get; set; }

        public decimal Jan { get; set; }

        public decimal Feb { get; set; }

        public decimal Mar { get; set; }

        public decimal Apr { get; set; }

        public decimal May { get; set; }

        public decimal Jun { get; set; }

        public decimal Jul { get; set; }

        public decimal Aug { get; set; }

        public decimal Sep { get; set; }

        public decimal Oct { get; set; }

        public decimal Nov { get; set; }

        public decimal Dec { get; set; }
    }
}
