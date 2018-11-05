using D_Squared.Data.Context;
using D_Squared.Domain.Entities;
using D_Squared.Domain.TransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Data.Queries
{
    public class BudgetQueries
    {
        private readonly D_SquaredDbContext db;

        public BudgetQueries(D_SquaredDbContext db)
        {
            this.db = db;
        }

        private List<Budget> GetBudgetsByYear(int year, string storeNumber)
        {
            return db.Budgets.Where(b => b.FiscalYear == year && b.StoreNumber == storeNumber).OrderBy(b => b.BudgetDate).ToList();
        }

        private List<BudgetDTO> GetBudgetDTOListByYear(int year, string storeNumber)
        {
            List<BudgetDTO> dtoList = new List<BudgetDTO>();
            List<Budget> budgetList = GetBudgetsByYear(year, storeNumber);

            for (int i = 0; i < budgetList.Count(); i++)
            {
                DateTime startDate = new DateTime();
                DateTime endDate = budgetList[i].BudgetDate;

                if (budgetList[i].FiscalPeriod == 1)
                    startDate = new DateTime(budgetList[i].BudgetDate.Year, budgetList[i].BudgetDate.Month, 1);
                else if (i > 0)
                    startDate = budgetList[i - 1].BudgetDate.AddDays(1);
                else
                    startDate = budgetList[i].BudgetDate.AddDays(-28);

                List<DateTime> dateRange = Enumerable.Range(0, 1 + endDate.Subtract(startDate).Days)
                                                        .Select(offset => startDate.AddDays(offset))
                                                        .ToList();

                BudgetDTO dto = new BudgetDTO
                {
                    Budget = budgetList[i],
                    NumberOfWeeks = (int)Math.Round((decimal)dateRange.Count / 7, 0),
                    BudgetDateRange = dateRange
                };

                dtoList.Add(dto);
            }

            return dtoList;
        }

        /// <summary>
        ///     Returns the appropriate Budget for the given date within a fiscal week
        ///     (passing a day in the middle of the fiscal week eliminates cases where Budget end/start dates overlap)
        /// </summary>
        /// <param name="thursdayOfFiscalWeek"></param>
        /// <returns></returns>
        public BudgetDTO GetBudgetByDate(DateTime thursdayOfFiscalWeek, string storeNumber)
        {
            List<BudgetDTO> dtoList = GetBudgetDTOListByYear(thursdayOfFiscalWeek.Year, storeNumber);

            return dtoList.Where(d => d.BudgetDateRange.Contains(thursdayOfFiscalWeek)).FirstOrDefault();
        }

        public List<FY18Budget> GetFY18Budgets(string storeLocation)
        {
            if(storeLocation != "OSRC")
            {
                int location;
                Int32.TryParse(storeLocation, out location);

                return db.FY18Budgets.Where(b => b.AX_CC == location).ToList();
            }
            else
                return new List<FY18Budget>();
        }
    }
}
