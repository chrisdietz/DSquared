using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.TransferObjects
{
    public class TipReportingSearchDTO
    {
        public TipReportingSearchDTO()
        {
            StartDate = DateTime.Today;
            EndDate = DateTime.Today;
            MinimumMakeUpPay = 0;
            SelectedLocation = string.Empty;
        }

        public TipReportingSearchDTO(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        [Display(Name = "Fiscal Week")]
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string SelectedLocation { get; set; }
        
        [Display(Name = "Min. Make Up Pay (Total) ")]
        public decimal MinimumMakeUpPay { get; set; }
    }
}
