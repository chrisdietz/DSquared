using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.Entities
{
    public class LSServerSales
    {
        [Key]
        public long ID { get; set; }
        public DateTime BusinessDate { get; set; }
        public string Store { get; set; }
        public decimal TotalSales { get; set; }
        public decimal FoodSales { get; set; }
        [Column("FoodSales%")]
        public decimal FoodSalesPercent { get; set; }
        public decimal LBWSales { get; set; }
        [Column("LBWSales%")]
        public decimal LBWSalesPercent { get; set; }
        public decimal NonAlcBevSales { get; set; }
        [Column("NonAlcBevSales%")]
        public decimal NonAlcBevSalesPercent { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
    }
}
