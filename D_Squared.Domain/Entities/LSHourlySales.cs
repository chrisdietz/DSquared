using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.Entities
{
    public class LSHourlySales
    {
        public long ID { get; set; }
        public DateTime BusinessDate { get; set; }
        public string Store { get; set; }
        public int Hour { get; set; }
        public decimal TotalSales { get; set; }
        public decimal FoodSales { get; set; }
        public decimal LiquorSales { get; set; }
        public decimal BeerDraftSales { get; set; }
        public decimal BeerBottleSales { get; set; }
        public decimal NonAlcBevSales { get; set; }
        public decimal WineSales { get; set; }
        public decimal RetailBeerSales { get; set; }
        public decimal RetailSales { get; set; }
    }
}
