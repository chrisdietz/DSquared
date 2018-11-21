using D_Squared.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.TransferObjects
{
    public class SalesForecastDTO
    {
        public SalesForecastDTO()
        {

        }

        public SalesForecastDTO(SalesForecast salesForecast)
        {
            DayOfWeek = salesForecast.BusinessDate.DayOfWeek.ToString();
            DateOfEntry = salesForecast.BusinessDate;
            ForecastAmount = salesForecast.ForecastAmount;
            ForecastAM = salesForecast.ForecastAM;
            ForecastPM = salesForecast.ForecastPM;
            PriorYearSales = salesForecast.ActualPriorYear;
            Prior2YearSales = salesForecast.ActualPrior2Years;
            AverageSalesPerMonth = salesForecast.AvgPrior4Weeks;
            LaborForecast = salesForecast.LaborForecast;
            LaborFOH = salesForecast.LaborFOH;
            LaborBOH = salesForecast.LaborBOH;
        }

        public SalesForecastDTO(DateTime newSalesForecastDate, decimal priorSales, decimal prior2Sales, decimal averageSales, decimal laborForecast)
        {
            DayOfWeek = newSalesForecastDate.DayOfWeek.ToString();
            DateOfEntry = newSalesForecastDate;
            PriorYearSales = priorSales;
            Prior2YearSales = prior2Sales;
            AverageSalesPerMonth = averageSales;
            LaborForecast = laborForecast;
        }

        public SalesForecastDTO(List<SalesForecast> preexistingForecasts)
        {
            foreach (var forecast in preexistingForecasts)
            {
                DayOfWeek = forecast.BusinessDate.DayOfWeek.ToString();
                DateOfEntry = forecast.BusinessDate;
                ForecastAmount = forecast.ForecastAmount;
                ForecastAM = forecast.ForecastAM;
                ForecastPM = forecast.ForecastPM;
                PriorYearSales = forecast.ActualPriorYear;
                Prior2YearSales = forecast.ActualPrior2Years;
                AverageSalesPerMonth = forecast.AvgPrior4Weeks;
                LaborForecast = forecast.LaborForecast;
                LaborFOH = forecast.LaborFOH;
                LaborBOH = forecast.LaborBOH;
            }
        }

        [Display(Name = "Day of Week")]
        public string DayOfWeek { get; set; }

        [Display(Name = "Date")]
        public DateTime DateOfEntry { get; set; }

        [Display(Name = "Total Sales Forecast")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C}")]
        public decimal ForecastAmount { get; set; }

        [Display(Name = "AM Forecast")]
        [DisplayFormat(DataFormatString = "{0:F0}", ApplyFormatInEditMode = true)]
        public decimal ForecastAM { get; set; }

        [Display(Name = "PM Forecast")]
        [DisplayFormat(DataFormatString = "{0:F0}", ApplyFormatInEditMode = true)]
        public decimal ForecastPM { get; set; }

        [Display(Name = "FY17 Sales")]
        public decimal PriorYearSales { get; set; }

        [Display(Name = "FY16 Sales")]
        public decimal Prior2YearSales { get; set; }

        [Display(Name = "6 Week Average")]
        public decimal AverageSalesPerMonth { get; set; }

        [Display(Name = "Labor Forecast")]
        public decimal LaborForecast { get; set; }

        public decimal LaborFOH { get; set; }

        public decimal LaborBOH { get; set; }
    }

    public class SalesForecastCalculationDTO
    {
        public SalesForecastCalculationDTO()
        {
            SalesForecastColumnTotalsDTO = new SalesForecastColumnTotalsDTO();
        }

        public SalesForecastCalculationDTO(List<SalesForecastDTO> weekdays)
        {
            SalesForecastColumnTotalsDTO = new SalesForecastColumnTotalsDTO(weekdays);
        }

        public SalesForecastCalculationDTO(List<SalesForecast> weekdays)
        {
            SalesForecastColumnTotalsDTO = new SalesForecastColumnTotalsDTO(weekdays);
        }

        [Display(Name = "Rec. Labor")]
        public decimal RecommendedLabor { get; set; }

        [Display(Name = "Rec. FOH Labor")]
        public decimal RecommendedFOHLabor { get; set; }

        [Display(Name = "Rec. BOH Labor")]
        public decimal RecommendedBOHLabor { get; set; }

        public decimal Variance { get; set; }

        [Display(Name = "FOH Variance")]
        public decimal VarianceFOH { get; set; }

        [Display(Name = "BOH Variance")]
        public decimal VarianceBOH { get; set; }

        public SalesForecastColumnTotalsDTO SalesForecastColumnTotalsDTO { get; set; }
    }

    public class SalesForecastExportDTO
    {
        public SalesForecastDTO Record { get; set; }

        public List<SalesForecastDTO> Weekdays { get; set; }

        public SalesForecastCalculationDTO Calculations { get; set; }
    }

    public class SalesForecastSearchDTO
    {
        public SalesForecastSearchDTO()
        {
            LocationId = string.Empty;
            EndDate = DateTime.Today.ToLocalTime();
            StartDate = DateTime.Today.ToLocalTime();
        }

        public SalesForecastSearchDTO(SalesForecast salesForecast)
        {
            LocationId = salesForecast.StoreNumber;
            EndDate = salesForecast.BusinessDate;
            StartDate = salesForecast.BusinessDate;
        }

        public SalesForecastSearchDTO(SalesForecast salesForecast, DateTime start, DateTime end)
        {
            LocationId = salesForecast.StoreNumber;
            EndDate = end;
            StartDate = start;

            //ForecastAmountMin = salesForecast.ForecastAmount;
            //ForecastAmountMax = salesForecast.ForecastAmount;

            //ActualPriorYearMin = salesForecast.ActualPriorYear;
            //ActualPriorYearMax = salesForecast.ActualPriorYear;

            //ActualPrior2YearsMin = salesForecast.ActualPrior2Years;
            //ActualPrior2YearsMax = salesForecast.ActualPrior2Years;

            //AvgPrior4WeeksMin = salesForecast.AvgPrior4Weeks;
            //AvgPrior4WeeksMax = salesForecast.AvgPrior4Weeks;

            //LaborForecastMin = salesForecast.LaborForecast;
            //LaborForecastMax = salesForecast.LaborForecast;
        }

        [Display(Name = "Location")]
        public string LocationId { get; set; }

        //[Display(Name = "Day of Week")]
        //public string DayOfWeek { get; set; }

        //[Display(Name = "Forecast Amount")]
        //[DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        //public decimal ForecastAmountMin { get; set; }

        //[DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        //public decimal ForecastAmountMax { get; set; }

        //[Display(Name = "FY 2017 Amount")]
        //[DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        //public decimal ActualPriorYearMin { get; set; }

        //[DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        //public decimal ActualPriorYearMax { get; set; }

        //[Display(Name = "FY 2016 Amount")]
        //[DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        //public decimal ActualPrior2YearsMin { get; set; }

        //[DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        //public decimal ActualPrior2YearsMax { get; set; }

        //[Display(Name = "6 Week Average Amount")]
        //[DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        //public decimal AvgPrior4WeeksMin { get; set; }

        //[DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        //public decimal AvgPrior4WeeksMax { get; set; }

        //[Display(Name = "Labor Forecast Amount")]
        //[DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        //public decimal LaborForecastMin { get; set; }

        //[DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        //public decimal LaborForecastMax { get; set; }

        [Display(Name = "Business Date Range")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
    }

    public class SalesForecastColumnTotalsDTO
    {
        public SalesForecastColumnTotalsDTO()
        {

        }

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

        public SalesForecastColumnTotalsDTO(List<SalesForecast> weekdays)
        {
            PriorYearSalesTotal = weekdays.Sum(w => w.ActualPriorYear);
            Prior2YearSalesTotal = weekdays.Sum(w => w.ActualPrior2Years);
            AverageSalesPerMonthTotal = weekdays.Sum(w => w.AvgPrior4Weeks);
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


    public class SalesForecastSummaryDTO
    {
        public SalesForecastSummaryDTO(string location, List<SalesForecastDTO> forecastList)
        {
            LocationNumber = location;
            WeeklyForecastRecords = forecastList;
        }

        public string LocationNumber { get; set; }

        public List<SalesForecastDTO> WeeklyForecastRecords { get; set; }
    }

    public class SalesForecastSummarySearchDTO
    {
        public SalesForecastSummarySearchDTO()
        {
            DesiredDate = DateTime.MinValue;
        }

        public SalesForecastSummarySearchDTO(DateTime currentDate)
        {
            DesiredDate = currentDate;
        }

        [DisplayFormat(DataFormatString = "{0:MM-dd-yy}", ApplyFormatInEditMode = true)]
        public DateTime DesiredDate { get; set; }
    }

    public class SalesForecastSummaryColumnDTO
    {
        public SalesForecastSummaryColumnDTO(DateTime day, List<SalesForecast> salesForecastListByDay)
        {
            DayOfWeek = day;
            TotalSalesForecast = salesForecastListByDay.AsQueryable().Sum(sf => sf.ForecastAmount);
        }

        public DateTime DayOfWeek { get; set; }

        public decimal TotalSalesForecast { get; set; }
    }

    public class SalesForecastSearchResultDTO
    {
        public SalesForecastSearchResultDTO()
        {
            Calculations = new SalesForecastCalculationDTO();
        }

        public SalesForecastSearchResultDTO(List<SalesForecast> salesForecasts)
        {
            FiscalWeekEnding = salesForecasts.LastOrDefault().BusinessDate;
            StoreNumber = salesForecasts.FirstOrDefault().StoreNumber;
            Calculations = new SalesForecastCalculationDTO(salesForecasts);
        }

        public SalesForecastCalculationDTO Calculations { get; set; }
        
        public DateTime FiscalWeekEnding { get; set; }

        public string StoreNumber { get; set; }
    }
}
