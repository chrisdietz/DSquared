using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.Entities
{
    public class LSMenuMix
    {
        public long ID { get; set; }
        public DateTime BusinessDate { get; set; }
        public string Store { get; set; }
        public string Department { get; set; }
        public string Category { get; set; }
        public string REPORTINGCATEGORY { get; set; }
        public string PLU { get; set; }
        public string ItemName { get; set; }
        public decimal Price { get; set; }
        public string BasicUnit { get; set; }
        public decimal BasicQty { get; set; }
        public string SellingUnit { get; set; }
        public decimal SellingQty { get; set; }
        public double Amount { get; set; }
    }
}
