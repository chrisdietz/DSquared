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

        public DateTime BusinessDate { get; set; }

        [StringLength(3)]
        public string StoreNumber { get; set; }

        public decimal ForecastAM { get; set; }

        public decimal ForecastPM { get; set; }

        public decimal ForecastAmount { get; set; }

        public decimal ActualPriorYear { get; set; }

        public decimal ActualPrior2Years { get; set; }

        public decimal AvgPrior4Weeks { get; set; }

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
