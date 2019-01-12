using D_Squared.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.TransferObjects
{
    public class SpreadHourDTO
    {
        public SpreadHourDTO()
        {

        }

        public SpreadHourDTO(SpreadHour sh, MinimumWage mw)
        {
            SpreadHour = sh;
            MinimumWage = mw;

            SpreadHourPay = sh.SpreadHours * mw.MinWage;
        }

        public SpreadHour SpreadHour { get; set; }

        public MinimumWage MinimumWage { get; set; }

        public decimal SpreadHourPay { get; set; }
    }

    public class SpreadHourSearchDTO
    {
        public SpreadHourSearchDTO()
        {
            StartDate = DateTime.Today;
            EndDate = DateTime.Today;

            SelectedLocation = string.Empty;
        }

        public SpreadHourSearchDTO(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        [Display(Name = "Fiscal Week")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yy}")]
        public DateTime StartDate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yy}")]
        public DateTime EndDate { get; set; }

        public string SelectedLocation { get; set; }
    }
}
