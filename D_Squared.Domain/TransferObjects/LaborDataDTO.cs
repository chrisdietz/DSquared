﻿using D_Squared.Domain.TransferObjects.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.TransferObjects
{
    public class LaborDataDTO
    {
        public long ID { get; set; }
        [Display(Name = "Business Date")]
        public DateTime BusinessDate { get; set; }
        [Display(Name = "Store Number")]
        public string Store { get; set; }
        public long ClockID { get; set; }
        [Display(Name = "Job ID")]
        [Exportable("Job ID", DataFormatType.String, false, DisplayFor.Condition_1)]
        public string JobName { get; set; }
        [Display(Name = "Center")]
        [Exportable("Center", DataFormatType.String, false, DisplayFor.Condition_2)]
        public string Center { get; set; }
        [Display(Name = "Regular Hours")]
        [Exportable("Regular Hours", DataFormatType.Decimal, true)]
        public double RegularHours { get; set; }
        [Display(Name = "OT Hours")]
        [Exportable("OT Hours", DataFormatType.Decimal, true)]
        public double OTHours { get; set; }
        [Display(Name = "Regular Pay Amount")]
        [Exportable("Regular Pay Amount", DataFormatType.Currency, true)]
        public double RegularPayAmount { get; set; }
        [Display(Name = "OT Pay Amount")]
        [Exportable("OT Pay Amount", DataFormatType.Currency, true)]
        public double OTPayAmount { get; set; }
        [Display(Name = "Total Hours")]
        [Exportable("Total Hours", DataFormatType.Decimal, true)]
        public double TotalHours { get; set; }
        [Display(Name = "Total Pay Amount")]
        [Exportable("Total Pay Amount", DataFormatType.Currency, true)]
        public double TotalPayAmount { get; set; }
        [Display(Name = "Labor %")]
        [Exportable("Labor %", DataFormatType.Decimal, false)]
        public double LaborPercentage { get; set; }
    }

    public class LaborDataSearchDTO
    {
        public const string ReportByDay = "ByDay";
        public const string ReportByDateRange = "ByDateRange";
        public const string ReportByJob = "ByJob";
        public const string ReportByCenter = "ByCenter";

        [Display(Name = "Business Date")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime SelectedDate { get; set; }
        public string SelectedDateFilter { get; set; }
        public string SelectedJobOrCenterFilter { get; set; }

        public string SelectedLocation { get; set; }
        public string SelectedCenter { get; set; }

        [Display(Name = "Business Date Range")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime SelectedDateRangeBegin { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime SelectedDateRangeEnd { get; set; }

        public LaborDataSearchDTO()
        {
            SelectedDate = DateTime.Today;
            SelectedLocation = string.Empty;
            SelectedCenter = string.Empty;
            SelectedDateFilter = ReportByDay;
            SelectedJobOrCenterFilter = ReportByJob;
            SelectedDateRangeBegin = DateTime.MinValue;
            SelectedDateRangeEnd = DateTime.MinValue;
        }

    }
}
