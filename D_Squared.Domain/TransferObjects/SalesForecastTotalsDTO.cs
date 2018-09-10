using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.TransferObjects
{
    public class SalesForecastColumnTotalsDTO
    {
        public SalesForecastColumnTotalsDTO(List<SalesForecastDTO> weekdays)
        {
            PriorYearSalesTotal = weekdays.Sum(w => w.PriorYearSales);
            Prior2YearSalesTotal = weekdays.Sum(w => w.Prior2YearSales);
            AverageSalesPerMonthTotal = weekdays.Sum(w => w.AverageSalesPerMonth);
            ForecastAMTotal = weekdays.Sum(w => w.ForecastAM);
            ForecastPMTotal = weekdays.Sum(w => w.ForecastPM);
            ForecastAmountTotal = weekdays.Sum(w => w.ForecastAmount);
            LaborForecastTotal = weekdays.Sum(w => w.LaborForecast);
        }

        public decimal PriorYearSalesTotal { get; set; }

        public decimal Prior2YearSalesTotal { get; set; }

        public decimal AverageSalesPerMonthTotal { get; set; }

        public decimal ForecastAMTotal { get; set; }

        public decimal ForecastPMTotal { get; set; }

        public decimal ForecastAmountTotal { get; set; }

        public decimal LaborForecastTotal { get; set; }
    }
}
