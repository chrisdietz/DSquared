using D_Squared.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.TransferObjects
{
    public class SalesDataDTO
    {
        public SalesDataDTO()
        {

        }

        public SalesDataDTO(LSSales salesData)
        {
            if (salesData != null)
            {
                DayOfWeek = salesData.BusinessDate.DayOfWeek.ToString();
                DateOfEntry = salesData.BusinessDate;
                Sales = salesData.Sales;
                Discounts = salesData.Discounts;
                Checks = salesData.CheckID != long.MinValue ? salesData.CheckID.ToString() : "0";
            }
            else
            {
                DateOfEntry = DateTime.Now;
            }
        }

        public SalesDataDTO (RedbookSalesData rbSalesData)
        {
            if (rbSalesData != null)
            {
                DateOfEntry = rbSalesData.CreatedDate;
                Sales = rbSalesData.Sales.HasValue ? rbSalesData.Sales.Value : 0;
                Discounts = rbSalesData.Discounts.HasValue ? rbSalesData.Discounts.Value : 0;
                Checks = rbSalesData.Checks;
                Manager = rbSalesData.CreatedBy;
            }
        }

        [Display(Name = "Day of Week")]
        public string DayOfWeek { get; set; }

        [Display(Name = "Date")]
        public DateTime DateOfEntry { get; set; }

        [Display(Name = "Sales")]
        public decimal Sales { get; set; }

        [Display(Name = "Discounts")]
        public decimal Discounts { get; set; }

        [Display(Name = "Checks")]
        public string Checks { get; set; }

        public string Manager { get; set; }
    }
}
