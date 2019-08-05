using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D_Squared.Domain.TransferObjects.Attributes
{
    public class ExportableAttribute : Attribute
    {
        public string DisplayName { get; set; }
        public DataFormatType DataFormatType { get; set; }
        public bool AddToTotal { get; set; }
        public DisplayFor DisplayFor { get; set; }
        public ExportableAttribute(string displayName, DataFormatType dataFormatType, bool addToTotal, DisplayFor displayFor = DisplayFor.NA)
        {
            DisplayName = displayName;
            DataFormatType = dataFormatType;
            AddToTotal = addToTotal;
            DisplayFor = displayFor;
        }

    }
    public enum DataFormatType
    {
        String,
        Currency,
        Decimal,
        WholeNumber,
        BigNumber,
        Time,
        Date,
        TimeStamp
    }
    public enum DisplayFor
    {
        Daily,
        Weekly,
        NA
    }
}