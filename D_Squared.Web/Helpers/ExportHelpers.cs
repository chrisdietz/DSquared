using D_Squared.Domain.TransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
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
}