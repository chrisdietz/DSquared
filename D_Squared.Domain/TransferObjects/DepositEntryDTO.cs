using D_Squared.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.TransferObjects
{
    public class DepositEntryDTO
    {
        public DepositEntryDTO()
        {

        }
        public DepositEntryDTO(DateTime newDailyDepositDate)
        {
            DayOfWeek = newDailyDepositDate.DayOfWeek.ToString();
            DateOfEntry = newDailyDepositDate;
        }

        public DepositEntryDTO(List<DailyDeposit> preexistingDeposits)
        {
            foreach (var deposit in preexistingDeposits)
            {
                if (deposit.GlAccount == DomainConstants.GL_ACCOUNT_CONSTANTS.CASH_DEPOSIT)
                    CashDeposit = deposit.Amount;
                if (deposit.GlAccount == DomainConstants.GL_ACCOUNT_CONSTANTS.MISC_DEPOSIT)
                    MiscDeposit = deposit.Amount;

                DayOfWeek = deposit.BusinessDate.DayOfWeek.ToString();
                DateOfEntry = deposit.BusinessDate;
            }
        }

        [Display(Name = "Day of Week")]
        public string DayOfWeek { get; set; }

        [Display(Name = "Date")]
        public DateTime DateOfEntry { get; set; }

        [Display(Name = "Cash Deposit")]
        public decimal CashDeposit { get; set; }

        [Display(Name = "Change Order Returns")]
        public decimal MiscDeposit { get; set; }
    }
}
