using D_Squared.Domain.TransferObjects;
using D_Squared.Domain.TransferObjects.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace D_Squared.Web.Helpers
{
    public static class SpreadHourExportHelper
    {
        public static StringBuilder ExportSpreadHours(List<SpreadHourDTO> spreadHours, bool orderByDays)
        {
            StringBuilder sb = new StringBuilder();

            if(!orderByDays)
            {
                sb.AppendLine("Store Number,Date,Employee,Position,First Clock In,Last Clock Out,Total Hours,Spread Hours,Spread Hour Pay*");

                foreach (SpreadHourDTO sh in spreadHours)
                {
                    sb.AppendLine(sh.SpreadHour.StoreNumber + ","
                                    + sh.SpreadHour.BusinessDate + ","
                                    + sh.SpreadHour.EmployeeName + ","
                                    + sh.SpreadHour.Job + ","
                                    + sh.SpreadHour.FirstClockIn.ToShortDateString() + ","
                                    + sh.SpreadHour.LastClockOut.ToShortDateString() + ","
                                    + sh.SpreadHour.TotalHours + ","
                                    + sh.SpreadHour.SpreadHours + ","
                                    + sh.SpreadHourPay.ToString("C"));
                }

                sb.AppendLine(" , , , , , , ," + spreadHours.Sum(s => s.SpreadHour.SpreadHours) + "," + spreadHours.Sum(s => s.SpreadHourPay).ToString("C0"));
            }
            else
            {
                sb.AppendLine("Store Number,Date,Day of Week,,,,,Spread Hours,Spread Hour Pay*");

                var spreadHoursByStore = spreadHours.GroupBy(sh => sh.SpreadHour.StoreNumber).ToList();

                foreach (var shbs in spreadHoursByStore)
                {
                    var spreadHoursByDay = shbs.GroupBy(gb => gb.SpreadHour.BusinessDate).ToList();
                    decimal hoursTotal = 0;
                    decimal payTotal = 0;

                    foreach (var shbd in spreadHoursByDay)
                    {
                        hoursTotal += shbd.Sum(s => s.SpreadHour.SpreadHours);
                        payTotal += shbd.Sum(s => s.SpreadHourPay);

                        sb.AppendLine(shbs.Key + "," + shbd.Key.ToShortDateString() + "," + shbd.Key.DayOfWeek + "," + " , , , ," + shbd.Sum(s => s.SpreadHour.SpreadHours) + "," + shbd.Sum(s => s.SpreadHourPay).ToString("C0"));
                    }

                    sb.AppendLine(" , , , , , , ," + hoursTotal + "," + payTotal.ToString("C0"));
                    sb.AppendLine();
                }
            }

            return sb;
        }
    }

    public static class NYSExportHelper
    {
        public static StringBuilder ExportNYS(List<NYSDTO> nYSDTOs, bool orderByDays)
        {
            StringBuilder sb = new StringBuilder();

            if (!orderByDays)
            {
                sb.AppendLine("Store Number,Date,Employee,Position,First Clock In,Last Clock Out,Total Hours,NYS,NYS Pay*");

                foreach (NYSDTO nys in nYSDTOs)
                {
                    sb.AppendLine(nys.NYS.StoreNumber + ","
                                    + nys.NYS.BusinessDate + ","
                                    + nys.NYS.EmployeeName + ","
                                    + nys.NYS.Job + ","
                                    + nys.NYS.FirstClockIn.ToShortDateString() + ","
                                    + nys.NYS.LastClockOut.ToShortDateString() + ","
                                    + nys.NYS.TotalHours + ","
                                    + nys.NYS.NYSHours + ","
                                    + nys.NYSPay.ToString("C"));
                }

                sb.AppendLine(" , , , , , , ," + nYSDTOs.Sum(s => s.NYS.NYSHours) + "," + nYSDTOs.Sum(s => s.NYSPay).ToString("C0"));
            }
            else
            {
                sb.AppendLine("Store Number,Date,Day of Week,,,,,NYS,NYS Pay*");

                var nysByStore = nYSDTOs.GroupBy(nys => nys.NYS.StoreNumber).ToList();

                foreach (var nysbs in nysByStore)
                {
                    var nysByDay = nysbs.GroupBy(gb => gb.NYS.BusinessDate).ToList();
                    decimal hoursTotal = 0;
                    decimal payTotal = 0;

                    foreach (var nysbd in nysByDay)
                    {
                        hoursTotal += nysbd.Sum(s => s.NYS.NYSHours);
                        payTotal += nysbd.Sum(s => s.NYSPay);

                        sb.AppendLine(nysbs.Key + "," + nysbd.Key.ToShortDateString() + "," + nysbd.Key.DayOfWeek + "," + " , , , ," + nysbd.Sum(s => s.NYS.NYSHours) + "," + nysbd.Sum(s => s.NYSPay).ToString("C0"));
                    }

                    sb.AppendLine(" , , , , , , ," + hoursTotal + "," + payTotal.ToString("C0"));
                    sb.AppendLine();
                }
            }

            return sb;
        }
    }

    public static class ReportExportHelper<T>
    {
        /// <summary>
        /// Builds a string containing Comma Separated Values by iterating through a list of data objects.
        /// Uses reflection to find the object and it's properties. Checks the ExportableAttribute decoration
        /// for the property to determine how the data should be formatted, set the column name, etc.
        /// </summary>
        /// <param name="dataObjects"></param>
        /// <param name="displayFor"></param>
        /// <param name="dynamicColumnNames"></param>
        /// <returns></returns>
        public static string BuildExportString(List<T> dataObjects, DisplayFor displayFor, Dictionary<string, string> dynamicColumnNames = null)
        {
            StringBuilder builder = new StringBuilder();
            List<string> columnHeaders = new List<string>();
            List<string> dataRow = new List<string>();
            List<string> totalsRow = new List<string>();
            Dictionary<string, object> totalsMap = new Dictionary<string, object>();
            bool headerRowComplete = false;
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (var obj in dataObjects)
            {
                foreach (var property in properties)
                {
                    var customAttribs = property.GetCustomAttributes(true);
                    foreach (var customAttrib in customAttribs)
                    {
                        if(customAttrib is ExportableAttribute)
                        {
                            var eaAttrib = (ExportableAttribute)customAttrib;
                            if (!headerRowComplete && (eaAttrib.DisplayFor == DisplayFor.NA || eaAttrib.DisplayFor == displayFor))
                            {
                                // Some of the column names can be dynamic. Check for dynamic names existence.
                                if(dynamicColumnNames != null && dynamicColumnNames.ContainsKey(eaAttrib.DisplayName))
                                {
                                    columnHeaders.Add(dynamicColumnNames[eaAttrib.DisplayName]);
                                }
                                else
                                {
                                    columnHeaders.Add(eaAttrib.DisplayName);
                                }
                            }
                                
                            if(eaAttrib.DisplayFor == DisplayFor.NA || eaAttrib.DisplayFor == displayFor)
                                dataRow.Add("\"" + FormatDisplayValue(property.GetValue(obj), property, eaAttrib, totalsMap) + "\"");
                        }
                    }
                }
                if (!headerRowComplete) builder.AppendLine(string.Join(",", columnHeaders));
                headerRowComplete = true;
                builder.AppendLine(string.Join(",", dataRow));
                dataRow.Clear();
            }
            // Totals row
            if (totalsMap.Values.Count > 0)
            {
                foreach (var property in properties)
                {
                    var customAttribs = property.GetCustomAttributes(true);
                    foreach (var customAttrib in customAttribs)
                    {
                        if (customAttrib is ExportableAttribute)
                        {
                            var eaAttrib = (ExportableAttribute)customAttrib;
                            if (eaAttrib.AddToTotal)
                            {
                                var totalColValue = totalsMap[eaAttrib.DisplayName];
                                totalsRow.Add("\"" + FormatDisplayValue(totalColValue, property, eaAttrib) + "\"");
                            }
                            else
                            {
                                if (eaAttrib.DisplayFor == DisplayFor.NA || eaAttrib.DisplayFor == displayFor)
                                    totalsRow.Add(string.Empty);
                            }
                        }
                    }
                }
            }
            if (totalsRow.Count > 0) builder.AppendLine(string.Join(",", totalsRow));
            return builder.ToString();
        }

        /// <summary>
        /// Formats the CSV display value using the ExportableAttribute attribute values set on the property in the data object's class.
        /// </summary>
        /// <param name="objVal"></param>
        /// <param name="property"></param>
        /// <param name="attrib"></param>
        /// <param name="totalsMap"></param>
        /// <returns></returns>
        private static string FormatDisplayValue(object objVal, PropertyInfo property, ExportableAttribute attrib, Dictionary<string, object> totalsMap = null)
        {
            string formattedVal = string.Empty;
            if(objVal != null && objVal.ToString().Length > 0)
            {
                switch (attrib.DataFormatType)
                {
                    case DataFormatType.String:
                        formattedVal = objVal.ToString();
                        break;
                    case DataFormatType.Currency:
                        formattedVal = string.Format("{0:C2}", Convert.ToDecimal(objVal));
                        if(totalsMap != null) AddToTotalsMap(totalsMap, objVal, attrib);
                        break;
                    case DataFormatType.Decimal:
                        formattedVal = string.Format("{0:0.00}", objVal);
                        if (totalsMap != null) AddToTotalsMap(totalsMap, objVal, attrib);
                        break;
                    case DataFormatType.WholeNumber:
                        formattedVal = string.Format("{0,0}", objVal); 
                        break;
                    case DataFormatType.BigNumber:
                        formattedVal = $"=Text({string.Format("{0,0}", objVal)}, 0)"; 
                        break;
                    case DataFormatType.Time:
                        formattedVal = ((DateTime)objVal).ToShortTimeString();
                        break;
                    case DataFormatType.Date:
                        formattedVal = ((DateTime)objVal).ToShortDateString();
                        break;
                    case DataFormatType.TimeStamp:
                        formattedVal = $"{((DateTime)objVal).ToShortDateString()} - {((DateTime)objVal).ToShortTimeString()}";
                        break;
                    default:
                        formattedVal = string.Empty;
                        break;
                }
            }
            return formattedVal;
        }

        private static void AddToTotalsMap(Dictionary<string, object> totalsMap, object dataValue, ExportableAttribute attrib)
        {
            if (attrib.AddToTotal)
            {
                if (totalsMap.ContainsKey(attrib.DisplayName))
                {
                    totalsMap[attrib.DisplayName] = Convert.ToDecimal(totalsMap[attrib.DisplayName]) + Convert.ToDecimal(dataValue);
                }
                else
                {
                    totalsMap.Add(attrib.DisplayName, Convert.ToDecimal(dataValue));
                }
            }
        }

    }

}