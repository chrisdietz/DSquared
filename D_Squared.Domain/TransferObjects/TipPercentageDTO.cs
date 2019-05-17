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
        public DateTime BusinessDate { get; set; }

        public String EmployeeName { get; set; }

        public string StoreNumber { get; set; }

        public string Job { get; set; }

        public decimal? Sales { get; set; }

        public decimal? TotalTips { get; set; }

        public TipPercentageDTO() { }

        public TipPercentageDTO(TipPercentage tipPercentage)
        {
            BusinessDate = tipPercentage.BusinessDate;
            EmployeeName = tipPercentage.EmployeeName;
            StoreNumber = tipPercentage.StoreNumber;
            Job = tipPercentage.Job;
            Sales = tipPercentage.Sales;
            TotalTips = tipPercentage.TotalTips;
        }
    }
    public class TipPercentageSearchDTO
    {
        public TipPercentageSearchDTO()
        {
            StartDate = DateTime.Today;
            EndDate = DateTime.Today;

            SelectedEmployee = string.Empty;
        }

        public TipPercentageSearchDTO(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        [Display(Name = "Fiscal Week")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yy}")]
        public DateTime StartDate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yy}")]
        public DateTime EndDate { get; set; }

        public string SelectedEmployee { get; set; }
    }
}
