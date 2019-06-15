using D_Squared.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.TransferObjects
{
    public class TipPercentageDTO
    {
        [Display(Name = "Business Date")]
        public DateTime BusinessDate { get; set; }

        public string EmployeeNumber { get; set; }

        [Display(Name = "Bartender/Server")]
        public String EmployeeName { get; set; }

        public string StoreNumber { get; set; }

        public string Job { get; set; }

        public decimal? Sales { get; set; }

        public decimal? Tips { get; set; }

        [Display(Name = "Total Sales")]
        public decimal TotalSalesForTheWeek { get; set; }

        [Display(Name = "Total Tips")]
        public decimal TotalTipsForTheWeek { get; set; }

        [Display(Name = "Tip %")]
        public decimal TipPercentageForTheWeek { get; set; }

        public TipPercentageDTO() { }

        public TipPercentageDTO(TipPercentage tipPercentage)
        {
            BusinessDate = tipPercentage.BusinessDate;
            EmployeeName = tipPercentage.EmployeeName;
            StoreNumber = tipPercentage.StoreNumber;
            Job = tipPercentage.Job;
            Sales = tipPercentage.Sales;
            Tips = tipPercentage.Tips;
        }
    }
    public class TipPercentageSearchDTO
    {
        [Display(Name = "Business Week by Date")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime SelectedDate { get; set; }

        public string SelectedLocation { get; set; }
        public string SelectedEmployee { get; set; }

        public TipPercentageSearchDTO()
        {
            SelectedDate = DateTime.Today;
            SelectedLocation = string.Empty;
            SelectedEmployee = string.Empty;
        }

        public TipPercentageSearchDTO(DateTime selectedDate)
        {
            SelectedDate = selectedDate;
        }

    }
}
