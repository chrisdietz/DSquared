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
            }
            else
            {
                DateOfEntry = DateTime.Now;
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
    }
}
