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

        public bool CheckForExistingDepositRecordByDate(DateTime date)
        {
            if (db.DailyDeposits.Any(dd => dd.BusinessDate == date))
                return true;
            else
                return false;
        }

        public bool CheckForExistingDepositRecordByDateAndType(DateTime date, int GlAccountType)
        {
            if (db.DailyDeposits.Any(dd => dd.BusinessDate == date && dd.GlAccount == GlAccountType))
                return true;
            else
                return false;
        }

        public DailyDeposit GetDepositRecordByDateAndType(DateTime date, int GlAccount)
        {
            return db.DailyDeposits.Where(dd => dd.BusinessDate == date && dd.GlAccount == GlAccount).FirstOrDefault();
        }

        public List<DailyDeposit> GetDepositRecordsByDate(DateTime date)
        {
            if (db.DailyDeposits.Any(dd => dd.BusinessDate == date))
                return db.DailyDeposits.Where(dd => dd.BusinessDate == date).ToList();
            else
                return new List<DailyDeposit>();
        }

        public List<DepositEntryDTO> GetCurrentWeekAsDepositEntryDTOList(DateTime today)
        {
            List<DepositEntryDTO> theList = new List<DepositEntryDTO>();

            int currentDayOfWeek = (int)today.DayOfWeek;
            DateTime sunday = today.AddDays(-currentDayOfWeek);
            DateTime monday = sunday.AddDays(1);

            if (currentDayOfWeek == 0)
            {
                monday = monday.AddDays(-7);
            }
            var dates = Enumerable.Range(0, 7).Select(days => monday.AddDays(days)).ToList();

            foreach(var day in dates)
            {
                if (!CheckForExistingDepositRecordByDate(day))
                    theList.Add(new DepositEntryDTO(day));
                else
                    theList.Add(new DepositEntryDTO(GetDepositRecordsByDate(day)));
            }

            return theList;
        }

        public void AddOrUpdateDeposits(List<DepositEntryDTO> deposits, string storeNumber, string userName)
        {
            foreach (var deposit in deposits)
            {
                if(CheckForExistingDepositRecordByDateAndType(deposit.DateOfEntry, DomainConstants.GL_ACCOUNT_CONSTANTS.CASH_DEPOSIT))
                {
                    DailyDeposit entry = GetDepositRecordByDateAndType(deposit.DateOfEntry, DomainConstants.GL_ACCOUNT_CONSTANTS.CASH_DEPOSIT);
                    entry.Amount = deposit.CashDeposit;
                    entry.StoreNumber = storeNumber;
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

                if (CheckForExistingDepositRecordByDateAndType(deposit.DateOfEntry, DomainConstants.GL_ACCOUNT_CONSTANTS.MISC_DEPOSIT))
                {
                    DailyDeposit entry = GetDepositRecordByDateAndType(deposit.DateOfEntry, DomainConstants.GL_ACCOUNT_CONSTANTS.MISC_DEPOSIT);
                    entry.Amount = deposit.MiscDeposit;
                    entry.StoreNumber = storeNumber;
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
    }
}
