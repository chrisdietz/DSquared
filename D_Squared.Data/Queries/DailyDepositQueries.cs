using D_Squared.Data.Context;
using D_Squared.Domain.Entities;
using D_Squared.Domain.TransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using D_Squared.Domain;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Data.Queries
{
    public class DailyDepositQueries
    {
        private readonly D_SquaredDbContext db;

        public DailyDepositQueries(D_SquaredDbContext db)
        {
            this.db = db;
        }

        public bool CheckForExistingDepositRecordByDate(DateTime date, string storeNumber)
        {
            return db.DailyDeposits.Any(dd => dd.BusinessDate == date && dd.StoreNumber == storeNumber);
        }

        public bool CheckForExistingDepositRecordByDateAndType(DateTime date, int GlAccountType, string storeNumber)
        {
            return db.DailyDeposits.Any(dd => dd.BusinessDate == date && dd.GlAccount == GlAccountType && dd.StoreNumber == storeNumber);
        }

        public DailyDeposit GetDepositRecordByDateAndType(DateTime date, int GlAccount, string storeNumber)
        {
            return db.DailyDeposits.Where(dd => dd.BusinessDate == date && dd.GlAccount == GlAccount && dd.StoreNumber == storeNumber).FirstOrDefault();
        }

        public List<DailyDeposit> GetDepositRecordsByDate(DateTime date, string storeNumber)
        {
            if (CheckForExistingDepositRecordByDate(date, storeNumber))
                return db.DailyDeposits.Where(dd => dd.BusinessDate == date && dd.StoreNumber == storeNumber).ToList();
            else
                return new List<DailyDeposit>();
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

        public List<DepositEntryDTO> GetSpecificWeekAsDepositEntryDTOList(DateTime selectedDay, string storeNumber)
        {
            List<DepositEntryDTO> theList = new List<DepositEntryDTO>();

            var dates = GetCurrentWeek(selectedDay);

            foreach(var day in dates)
            {
                if (!CheckForExistingDepositRecordByDate(day, storeNumber))
                    theList.Add(new DepositEntryDTO(day));
                else
                    theList.Add(new DepositEntryDTO(GetDepositRecordsByDate(day, storeNumber)));
            }

            return theList;
        }

        public void AddOrUpdateDeposits(List<DepositEntryDTO> deposits, string storeNumber, string userName)
        {
            foreach (var deposit in deposits)
            {
                if(CheckForExistingDepositRecordByDateAndType(deposit.DateOfEntry, DomainConstants.GL_ACCOUNT_CONSTANTS.CASH_DEPOSIT, storeNumber))
                {
                    DailyDeposit entry = GetDepositRecordByDateAndType(deposit.DateOfEntry, DomainConstants.GL_ACCOUNT_CONSTANTS.CASH_DEPOSIT, storeNumber);
                    entry.Amount = deposit.CashDeposit;
                    entry.UpdatedBy = userName;
                    entry.UpdatedDate = DateTime.Now;
                }
                else
                {
                    DailyDeposit entry = new DailyDeposit()
                    {
                        BusinessDate = deposit.DateOfEntry,
                        StoreNumber = storeNumber,
                        GlAccount = DomainConstants.GL_ACCOUNT_CONSTANTS.CASH_DEPOSIT,
                        Amount = deposit.CashDeposit,
                        CreatedBy = userName,
                        CreatedDate = DateTime.Now,
                        UpdatedBy = userName,
                        UpdatedDate = DateTime.Now
                    };

                    db.DailyDeposits.Add(entry);
                }

                if (CheckForExistingDepositRecordByDateAndType(deposit.DateOfEntry, DomainConstants.GL_ACCOUNT_CONSTANTS.MISC_DEPOSIT, storeNumber))
                {
                    DailyDeposit entry = GetDepositRecordByDateAndType(deposit.DateOfEntry, DomainConstants.GL_ACCOUNT_CONSTANTS.MISC_DEPOSIT, storeNumber);
                    entry.Amount = deposit.MiscDeposit;
                    entry.UpdatedBy = userName;
                    entry.UpdatedDate = DateTime.Now;
                }
                else
                {
                    DailyDeposit entry = new DailyDeposit()
                    {
                        BusinessDate = deposit.DateOfEntry,
                        StoreNumber = storeNumber,
                        GlAccount = DomainConstants.GL_ACCOUNT_CONSTANTS.MISC_DEPOSIT,
                        Amount = deposit.MiscDeposit,
                        CreatedBy = userName,
                        CreatedDate = DateTime.Now,
                        UpdatedBy = userName,
                        UpdatedDate = DateTime.Now
                    };

                    db.DailyDeposits.Add(entry);
                }
            }

            db.SaveChanges();
        }

        public List<DepositSummaryDTO> GetDepositSummaryList (DateTime selectedDate, List<string> locationList)
        {
            List<DepositSummaryDTO> summaryList = new List<DepositSummaryDTO>();

            foreach (string location in locationList)
            {
                summaryList.Add(new DepositSummaryDTO(location, GetSpecificWeekAsDepositEntryDTOList(selectedDate, location)));
            }

            return summaryList;
        }

        public List<DepositSummaryColumnSumDTO> GetWeeklyReportColumnTotals(DateTime selectedDay)
        {
            List<DateTime> dates = GetCurrentWeek(selectedDay);

            List<DailyDeposit> theList = db.DailyDeposits.Where(dd => dates.Contains(dd.BusinessDate)).ToList();

            List<DepositSummaryColumnSumDTO> columnSums = new List<DepositSummaryColumnSumDTO>();

            foreach(var day in dates)
            {
                columnSums.Add(new DepositSummaryColumnSumDTO(day, theList.Where(tl => tl.BusinessDate == day).ToList()));
            }

            return columnSums;
        }
    }
}
