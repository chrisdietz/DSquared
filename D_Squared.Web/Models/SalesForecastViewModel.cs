using D_Squared.Domain.Entities;
using D_Squared.Domain.TransferObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;

namespace D_Squared.Web.Models
{
    public class SalesForecastViewModel
    {
        public SalesForecastViewModel()
        {
            Weekdays = new List<SalesForecastDTO>();
        }

        public SalesForecastViewModel(List<SalesForecastDTO> weekdays, DateTime accessTime, EmployeeDTO employeeDTO, string selectedDate, BudgetDTO budgetDTO, List<string> validLocations)
        {
            Weekdays = weekdays;
            AccessTime = accessTime;
            EndingPeriod = weekdays.Last().DateOfEntry;
            EmployeeInfo = employeeDTO;
            TicketURL = ConfigurationManager.AppSettings["SalesForecastTicketURL"];
            SelectedDateString = selectedDate;
            StartDate = GetCurrentWeek(accessTime).First();
            EndDate = StartDate.AddDays(42);

            BudgetDTO = budgetDTO;
            Totals = new SalesForecastColumnTotalsDTO(weekdays);

            RecommendedLabor = CalculateRecommendedLabor(validLocations);
            Variance = Totals.LaborForecastTotal - RecommendedLabor;
        }

        public List<DateTime> GetCurrentWeek(DateTime selectedDay)
        {
            int currentDayOfWeek = (int)selectedDay.DayOfWeek;
            DateTime sunday = selectedDay.AddDays(-currentDayOfWeek);
            DateTime monday = sunday.AddDays(1);

            if (currentDayOfWeek == 0)
            {
                monday = monday.AddDays(-7);
            }
            var dates = Enumerable.Range(0, 7).Select(days => monday.AddDays(days)).ToList();

            return dates;
        }

        //assign in controller
        private decimal CalculateRecommendedLabor(List<string> validLocations)
        {
            if (validLocations.Contains(EmployeeInfo.StoreNumber))
            {
                return ((decimal)BudgetDTO.Budget.LaborBudgetAmount / BudgetDTO.NumberOfWeeks) +
                                                ((Totals.ForecastAmountTotal - ((decimal)BudgetDTO.Budget.SalesBudgetAmount / BudgetDTO.NumberOfWeeks))
                                                *
                                                (((decimal)BudgetDTO.Budget.LaborBudgetAmount / (decimal)BudgetDTO.Budget.SalesBudgetAmount) / 2));
            }
            else
            {
                return new decimal(-1);
            }
        }

        public decimal RecommendedLabor { get; set; }

        public decimal Variance { get; set; }

        public DateTime EndingPeriod { get; set; }

        public DateTime AccessTime { get; set; }

        public List<SalesForecastDTO> Weekdays { get; set; }

        public SalesForecastColumnTotalsDTO Totals { get; set; }

        public BudgetDTO BudgetDTO { get; set; }

        public EmployeeDTO EmployeeInfo { get; set; }

        public string TicketURL { get; set; }

        [Display(Name = "Date Search:")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public string SelectedDateString { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}