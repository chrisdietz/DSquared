using D_Squared.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.TransferObjects
{
    public class DepositSummaryDTO
    {
        public DepositSummaryDTO(string location, List<DepositEntryDTO> depositList)
        {
            LocationNumber = location;
            WeeklyDepositRecords = depositList;
        }

        public string LocationNumber { get; set; }

        public List<DepositEntryDTO> WeeklyDepositRecords { get; set; }
    }

    public class DepositSummarySearchDTO
    {
        public DepositSummarySearchDTO()
        {
            DesiredDate = DateTime.MinValue;
        }

        public DepositSummarySearchDTO(DateTime currentDate)
        {
            DesiredDate = currentDate;
        }

        [DisplayFormat(DataFormatString = "{0:MM-dd-yy}", ApplyFormatInEditMode = true)]
        public DateTime DesiredDate { get; set; }
    }

    public class DepositSummaryColumnSumDTO
    {
        public DepositSummaryColumnSumDTO(DateTime day, List<DailyDeposit> depositListByDay)
        {
            DayOfWeek = day;
            TotalCashDeposit = depositListByDay.Where(wdl => wdl.GlAccount == DomainConstants.GL_ACCOUNT_CONSTANTS.CASH_DEPOSIT).AsQueryable().Sum(dd => dd.Amount);
            TotalMiscDeposit = depositListByDay.Where(wdl => wdl.GlAccount == DomainConstants.GL_ACCOUNT_CONSTANTS.MISC_DEPOSIT).AsQueryable().Sum(dd => dd.Amount);
        }

        public DateTime DayOfWeek { get; set; }

        public decimal TotalCashDeposit { get; set; }

        public decimal TotalMiscDeposit { get; set; }
    }
}
