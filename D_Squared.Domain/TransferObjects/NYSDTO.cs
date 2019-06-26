using D_Squared.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.TransferObjects
{
    public class NYSDTO
    {
        public NYSDTO()
        {

        }

        public NYSDTO(NYS nys, MinimumWage mw)
        {
            NYS = nys;
            MinimumWage = mw;

            NYSPay = nys.NYSHours * mw.MinWage;
        }

        public NYS NYS { get; set; }

        public MinimumWage MinimumWage { get; set; }

        public decimal NYSPay { get; set; }
    }

    public class NYSSearchDTO
    {
        public NYSSearchDTO()
        {
            StartDate = DateTime.Today;
            EndDate = DateTime.Today;

            SelectedLocation = string.Empty;
        }

        public NYSSearchDTO(DateTime startDate, DateTime endDate)
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
