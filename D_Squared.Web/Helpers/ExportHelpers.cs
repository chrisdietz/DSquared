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
}