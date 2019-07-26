using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.TransferObjects
{
    public class ServerSalesDTO
    {
        public long ID { get; set; }
        [Display(Name = "Business Date")]
        public DateTime BusinessDate { get; set; }
        [Display(Name = "Location Name")]
        public string Store { get; set; }
        [Display(Name = "Total Sales")]
        public decimal TotalSales { get; set; }
        [Display(Name = "Food Sales")]
        public decimal FoodSales { get; set; }
        [Display(Name = "Food Sales%")]
        public decimal FoodSalesPercent { get; set; }
        [Display(Name = "LBW Sales")]
        public decimal LBWSales { get; set; }
        [Display(Name = "LBW Sales%")]
        public decimal LBWSalesPercent { get; set; }
        [Display(Name = "Non Alc Bev Sales")]
        public decimal NonAlcBevSales { get; set; }
        [Display(Name = "Non Alc Bev Sales%")]
        public decimal NonAlcBevSalesPercent { get; set; }
        public int EmployeeID { get; set; }
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }
    }

    public class ServerSalesSearchDTO
    {
        public const string ReportByDay = "Date";
        public const string ReportByWeek = "Week";
        public const string ReportByBiWeekly = "Bi-Weekly";

        [Display(Name = "Business Date")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime SelectedDate { get; set; }
        public string SelectedDWBWFilter { get; set; }
        public string SelectedEmployee { get; set; }
        public string SelectedLocation { get; set; }

        public ServerSalesSearchDTO()
        {
            SelectedDate = DateTime.Today;
            SelectedLocation = string.Empty;
            SelectedDWBWFilter = ReportByDay;
            SelectedEmployee = "-1";
        }
    }
}
