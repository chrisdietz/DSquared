using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.Entities
{
    public class HelpDocument
    {
        [NotMapped]
        public Dictionary<string, string> PageHeaders { get; set; }
        public HelpDocument()
        {
            PageHeaders = new Dictionary<string, string>();
            PageHeaders.Add("IdealCashReport", "Ideal Cash Report");
            PageHeaders.Add("SalesReport", "Sales Report");
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
            PageHeaders.Add("OvertimeReport", "Overtime Report");
            PageHeaders.Add("LaborSummary", "Labor Summary Report");
            PageHeaders.Add("Labor8020", "80/20 Report");
            PageHeaders.Add("PaidInOut", "Paid In/Out Report");
            PageHeaders.Add("ServerSales", "Server Sales Report");
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
