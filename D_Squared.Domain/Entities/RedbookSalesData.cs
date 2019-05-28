using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace D_Squared.Domain.Entities
{
    public class RedbookSalesData
    {
        public RedbookSalesData()
        {

        }

        public RedbookSalesData(int redbookId, decimal sales, decimal discounts, string checks, string username)
        {
            RedbookEntryId = redbookId;
            Sales = sales;
            Discounts = discounts;
            Checks = checks;
            CreatedDate = DateTime.Now;
            CreatedBy = username;
        }
        public int Id { get; set; }
        [Required]
        [ForeignKey("RedbookEntry")]
        public int RedbookEntryId { get; set; }
        public decimal? Sales { get; set; }
        public decimal? Discounts { get; set; }
        public string Checks { get; set; }
        [ScaffoldColumn(false)]
        [DataType(DataType.DateTime)]
        [Display(Name = "Entry Time")]
        public DateTime CreatedDate { get; set; }
        [Display(Name = "Manager")]
        public string CreatedBy { get; set; }

        public virtual RedbookEntry RedbookEntry { get; set; }
    }
}
