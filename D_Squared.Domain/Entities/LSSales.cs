using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.Entities
{
    public class LSSales
    {
        [Key]
        public long CheckID { get; set; }

        [Display(Name = "Date")]
        public DateTime BusinessDate { get; set; }

        [StringLength(3)]
        public string Store { get; set; }

        [Display(Name = "Sales")]
        public decimal Sales { get; set; }

        [Display(Name = "Discounts")]
        public decimal Discounts { get; set; }

        public decimal FoodSales { get; set; }
        public decimal LiquorSales { get; set; }
        public decimal BeerDraftSales { get; set; }
        public decimal BeerBottleSales { get; set; }
        public decimal NonAlcBevSales { get; set; }
        public decimal WineSales { get; set; }
        public decimal RetailBeerSales { get; set; }
        public decimal RetailSales { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal PaymentAmount { get; set; }
    }
}
