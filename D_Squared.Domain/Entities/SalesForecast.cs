using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.Entities
{
    public class SalesForecast
    {
        public int Id { get; set; }

        [Display(Name = "Date")]
        public DateTime BusinessDate { get; set; }

        [StringLength(3)]
        public string StoreNumber { get; set; }

        [Display(Name = "AM Forecast")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal ForecastAM { get; set; }

        [Display(Name = "PM Forecast")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal ForecastPM { get; set; }

        [Display(Name = "Total Sales Forecast")]
        public decimal ForecastAmount { get; set; }

        [Display(Name = "FY18 Sales")]
        public decimal ActualPriorYear { get; set; }

        [Display(Name = "FY17 Sales")]
        public decimal ActualPrior2Years { get; set; }

        [Display(Name = "FY16 Sales")]
        public decimal ActualPrior3Years { get; set; }

        [Display(Name = "6 Week Average")]
        public decimal AvgPrior4Weeks { get; set; }

        [Display(Name = "Labor Forecast")]
        public decimal LaborForecast { get; set; }

        public decimal LaborFOH { get; set; }

        public decimal LaborBOH { get; set; }

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
