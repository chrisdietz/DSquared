using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace D_Squared.Domain.Entities
{
    public class HelpDocument
    {
        [NotMapped]
        public Dictionary<string, string> PageHeaders { get; set; }
        public HelpDocument()
        {
            PageHeaders = new Dictionary<string, string>();
            PageHeaders.Add("IdealCashView", "Ideal Cash Report - View");
            PageHeaders.Add("IdealCashSearch", "Ideal Cash Report - Search");
            PageHeaders.Add("SalesView", "Sales Report - View");
            PageHeaders.Add("SalesSearch", "Sales Report - Search");
            PageHeaders.Add("RedbookEntry", "Redbook Entry");
            PageHeaders.Add("RedbookSearch", "Redbook Search");
            PageHeaders.Add("SalesForecasts", "Sales Forecasts");
            PageHeaders.Add("SalesForecastSearch", "Sales Forecast Search");
            PageHeaders.Add("DailyDeposits", "Daily Deposits");
            PageHeaders.Add("DepositReport", "Deposit Report");
            PageHeaders.Add("TipReportView", "Tip Reporting View");
            PageHeaders.Add("TipReportSearch", "Tip Reporting Search");
            PageHeaders.Add("TipPercentage", "Tip Percentage");
            PageHeaders.Add("SpreadHoursView", "Spread Hours View");
            PageHeaders.Add("SpreadHoursSearch", "Spread Hours Search");
            PageHeaders.Add("MandatedHoursView", "Mandated Hours View");
            PageHeaders.Add("MandatedHoursSearch", "Mandated Hours Search");
            PageHeaders.Add("OvertimeView", "Overtime Report - View");
            PageHeaders.Add("OvertimeSearch", "Overtime Report - Search");
            PageHeaders.Add("LaborSummaryView", "Labor Summary Report - View");
            PageHeaders.Add("LaborSummarySearch", "Labor Summary Report - Search");
            PageHeaders.Add("Labor8020View", "80/20 Report - View");
            PageHeaders.Add("Labor8020Search", "80/20 Report - Search");
            PageHeaders.Add("PaidInOutView", "Paid In/Out Report - View");
            PageHeaders.Add("PaidInOutSearch", "Paid In/Out Report - Search");
            PageHeaders.Add("ServerSalesView", "Server Sales Report - View");
            PageHeaders.Add("ServerSalesSearch", "Server Sales Report - Search");
            PageHeaders.Add("HourlySalesSearch", "Hourly Sales Report - Search");
            PageHeaders.Add("HourlySalesView", "Hourly Sales Report - View");
            PageHeaders.Add("TimeClockDetailSearch", "Time Clock Detail Report - Search");
            PageHeaders.Add("TimeClockDetailView", "Time Clock Detail Report - View");
            PageHeaders.Add("ForcedOutEmployeesSearch", "Forced Clock Out Employee Report - Search");
            PageHeaders.Add("ForcedOutEmployeesView", "Forced Clock Out Employee Report - View");
            PageHeaders.Add("MenuMixSearch", "Menu Mix Report - Search");
            PageHeaders.Add("MenuMixView", "Menu Mix Report - View");
            PageHeaders.Add("HuddleNotesView", "Meeting Notes - Bartender/Server View");
            PageHeaders.Add("NotesView", "Meeting Notes - View");
            PageHeaders.Add("NotesEntry", "Meeting Notes - Entry");
        }

        public int Id { get; set; }

        public string ControllerName { get; set; }
        
        public string ActionName { get; set; }

        [StringLength(5000)]
        public string HelpHtml { get; set; }
        [NotMapped]
        public string PageHeader { get; set; }

        #region AuditFields
        [ScaffoldColumn(false)]
        [DataType(DataType.DateTime)]
        [Display(Name = "Updated Date")]
        public DateTime? UpdatedDate { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Updated By")]
        [StringLength(50)]
        public string UpdatedBy { get; set; }

        [ScaffoldColumn(false)]
        [DataType(DataType.DateTime)]
        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Created By")]
        [StringLength(50)]
        public string CreatedBy { get; set; }
        #endregion
    }
}
