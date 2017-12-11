using D_Squared.Domain.TransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D_Squared.Web.Models
{
    public class SalesForecastReportViewModel
    {
        public DateTime EndingPeriod { get; set; }

        public DateTime StartingPeriod { get; set; }

        public DateTime CurrentDate { get; set; }

        public List<SalesForecastSummaryDTO> SummaryList { get; set; }

        public SalesForecastSummarySearchDTO SearchDTO { get; set; }

        public List<SalesForecastSummaryColumnDTO> ColumnTotalList { get; set; }
    }
}