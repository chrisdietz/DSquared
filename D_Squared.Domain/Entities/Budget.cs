using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace D_Squared.Domain.Entities
{
    [Table("Budget")]
    public class Budget
    {
        [Key]
        [Column(Order = 0)]
        public DateTime BudgetDate { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FiscalPeriod { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FiscalYear { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(3)]
        public string StoreNumber { get; set; }

        public decimal? LaborBudgetAmount { get; set; }

        public decimal? SalesBudgetAmount { get; set; }
    }
}
