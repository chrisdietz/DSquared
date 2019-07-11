using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.TransferObjects
{
    public class IdealCashDTO
    {
        public long ID { get; set; }
        [Display(Name = "Business Date")]
        public DateTime BusinessDate { get; set; }
        [Display(Name = "Location")]
        public string Store { get; set; }
        [Display(Name = "Cash")]
        public double Cash { get; set; }
        [Display(Name = "CC Tips")]
        public double CCTips { get; set; }
        [Display(Name = "Paid In")]
        public double PaidIn { get; set; }
        [Display(Name = "Paid Out")]
        public double PaidOut { get; set; }
        [Display(Name = "Ideal Cash")]
        public double IdealCash { get; set; }
        [Display(Name = "Total Sales")]
        public double TotalSales { get; set; }
    }

    public class IdealCashSearchDTO
    {
        [Display(Name = "Business Date")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime SelectedDate { get; set; }
        public string SelectedLocation { get; set; }

        public IdealCashSearchDTO()
        {
            SelectedDate = DateTime.Today;
            SelectedLocation = string.Empty;
        }
    }
}
