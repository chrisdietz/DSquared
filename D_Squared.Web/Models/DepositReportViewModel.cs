using D_Squared.Domain.Entities;
using D_Squared.Domain.TransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D_Squared.Web.Models
{
    public class DepositReportViewModel
    {
        public DateTime EndingPeriod { get; set; }

        public DateTime StartingPeriod { get; set; }

        public DateTime CurrentDate { get; set; }

        public List<DepositSummaryDTO> SummaryList { get; set; }

        public DepositSummarySearchDTO SearchDTO { get; set; }

        public List<DepositSummaryColumnSumDTO> ColumnTotalList { get; set; }
    }
}