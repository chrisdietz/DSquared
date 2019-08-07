using D_Squared.Domain.TransferObjects.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.TransferObjects
{
    public class PaidInOutDTO
    {
        public long ID { get; set; }
        [Display(Name = "Location Name")]
        public string Store { get; set; }
        [Display(Name = "Business Date")]
        [Exportable("Business Date", DataFormatType.Date, false, DisplayFor.Condition_2)]
        public DateTime BusinessDate { get; set; }
        [Display(Name = "Receipt")]
        [Exportable("Receipt", DataFormatType.BigNumber, false)]
        public string Receipt { get; set; }
        public string CheckID { get; set; }
        [Display(Name = "Account Type")]
        [Exportable("Account Type", DataFormatType.String, false)]
        public string AccountType { get; set; }
        [Display(Name = "Account Name")]
        [Exportable("Account Name", DataFormatType.String, false)]
        public string AccountName { get; set; }
        [Display(Name = "Expense Amount")]
        [Exportable("Expense Amount", DataFormatType.Currency, true)]
        public decimal ExpenseAmount { get; set; }
        [Display(Name = "Personnel Number")]
        public int PersonnelNumber { get; set; }
        [Display(Name = "Employee Name")]
        [Exportable("Employee Name", DataFormatType.String, false)]
        public string EmployeeName { get; set; }

    }
    public class PaidInOutSearchDTO
    {
        public const string ReportByDay = "ByDay";
        public const string ReportByWeek = "ByWeek";
        public const string ReportByPaidIn = "Paid In";
        public const string ReportByPaidOut = "Paid Out";
        public const string ReportByPaidInNOut = "Both";

        [Display(Name = "Business Date")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime SelectedDate { get; set; }
        public string SelectedDayOrWeekFilter { get; set; }
        public string SelectedAccountTypeFilter { get; set; }

        public string SelectedLocation { get; set; }

        public PaidInOutSearchDTO()
        {
            SelectedDate = DateTime.Today;
            SelectedLocation = string.Empty;
            SelectedDayOrWeekFilter = ReportByDay;
            SelectedAccountTypeFilter = ReportByPaidInNOut;
        }
    }
}
