using D_Squared.Domain.Entities;
using D_Squared.Domain.TransferObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Data.Commands
{
    public class EmailCommands
    {
        #region Email Settings
        private const string HOST = "172.18.100.60";

        private const int PORT = 25;
        #endregion

        SmtpClient EmailClient { get; set; }

        public EmailCommands()
        {
            EmailClient = new SmtpClient(HOST, PORT)
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                //Credentials = CredentialCache.DefaultNetworkCredentials,
                UseDefaultCredentials = false
            };
        }

        private string BuildSubject(string locationId)
        {
            return locationId + " Redbook";
        }

        private string BuildBody(RedbookEntry entry, SalesForecastExportDTO dto)
        {
            StringBuilder sb = new StringBuilder();

            // Get two digit year part of current year (This works until 2099)
            int currentYear = DateTime.Now.Year % 1000;
            //content header
            sb.AppendLine("Submission Summary for location <u>" + entry.LocationId + "</u> and date <u>" + entry.BusinessDate.ToShortDateString() + "</u><br><br>");

            //redbook data fields
            sb.AppendLine("<b>AM Manager: </b>" + entry.ManagerOnDutyAM + "<br>");
            sb.AppendLine("<b>PM Manager: </b>" + entry.ManagerOnDutyPM + "<br>");
            sb.AppendLine("<b>AM Weather: </b>" + entry.SelectedWeatherAM + "<br>");
            sb.AppendLine("<b>PM Weather: </b>" + entry.SelectedWeatherPM + "<br>");
            sb.AppendLine("<b>Daily Notes: </b>" + entry.DailyNotes + "<br>");
            sb.AppendLine("<b>To Do Today: </b>" + entry.ToDoToday + "<br>");
            sb.AppendLine("<b>R&M Issues: </b>" + entry.RMIssues + "<br>");
            sb.AppendLine("<b>Employee Notes: </b>" + entry.EmployeeNotes + "<br>");
            sb.AppendLine("<b>Food and Beverage: </b>" + entry.FoodAndBeverage + "<br>");
            sb.AppendLine("<b>M Power: </b>" + entry.MPower + "<br>");
            sb.Append("<b>Events Affecting Sales: </b>");

            //event listing
            List<RedbookSalesEvent> eventList = entry.SalesEvents.ToList();

            if (eventList != null && eventList.Count > 0)
            {
                RedbookSalesEvent lastEvent = eventList.Last();

                foreach (RedbookSalesEvent e in eventList)
                {
                    if (e.Equals(lastEvent))
                    {
                        sb.AppendLine(e.Event + "<br>");
                    }
                    else
                    {
                        sb.Append(e.Event + ", ");
                    }
                }
            }
            else
            {
                sb.AppendLine("<i>N/A</i>");
            }

            //sales forecast
            sb.AppendLine("<br><br><br><br>Sales Forecast Details<br><br>");
            sb.AppendLine("<b>Day of Week: </b>" + dto.Record.DayOfWeek + "<br>");
            sb.AppendLine("<b>Total Forecast: </b>" + dto.Record.ForecastAmount.ToString("C0") + "<br>");
            sb.AppendLine("<b>AM Forecast: </b>" + dto.Record.ForecastAM.ToString("C0") + "<br>");
            sb.AppendLine("<b>PM Forecast: </b>" + dto.Record.ForecastPM.ToString("C0") + "<br>");
            sb.AppendLine($"<b>FY{currentYear - 1} Sales: </b>" + dto.Record.PriorYearSales.ToString("C0") + "<br>");
            sb.AppendLine($"<b>FY{currentYear - 2} Sales: </b>" + dto.Record.Prior2YearSales.ToString("C0") + "<br>");
            sb.AppendLine("<b>6 Week Average: </b>" + dto.Record.AverageSalesPerMonth.ToString("C0") + "<br>");
            sb.AppendLine("<b>Labor Forecast: </b>" + dto.Record.LaborForecast.ToString("C0") + "<br><br>");

            sb.AppendLine("<b>Rec. Labor FOH: </b>" + dto.Calculations.RecommendedFOHLabor.ToString("C0") + "<br>");
            sb.AppendLine("<b>FOH Variance: </b>" + dto.Calculations.VarianceFOH.ToString("C0") + "<br>");
            sb.AppendLine("<b>Rec. Labor BOH: </b>" + dto.Calculations.RecommendedBOHLabor.ToString("C0") + "<br>");
            sb.AppendLine("<b>BOH Variance: </b>" + dto.Calculations.VarianceBOH.ToString("C0") + "<br>");
            sb.AppendLine("<b>Rec. Labor: </b>" + dto.Calculations.RecommendedLabor.ToString("C0") + "<br>");
            sb.AppendLine("<b>Variance: </b>" + dto.Calculations.Variance.ToString("C0") + "<br><br><br>");


            //weekday breakdown
            sb.AppendLine("<br><br>Weekly Sales Forecast Details<br><br>");

            //open table
            sb.Append("<table class='table'>");

            //open thead
            sb.Append("<thead>");

            //open tr
            sb.Append("<tr>");
            sb.Append("<th>Date</th>");
            sb.Append("<th>Day of Week</th>");
            sb.Append("<th>Total Forecast</th>");
            sb.Append("<th>AM Forecast</th>");
            sb.Append("<th>PM Forecast</th>");
            sb.Append($"<th>FY{currentYear - 1} Sales</th>");
            sb.Append($"<th>FY{currentYear - 2} Sales</th>");
            sb.Append("<th>6 Week Average</th>");
            sb.Append("<th>Labor Forecast</th>");
            sb.Append("</tr>");
            //close tr

            //close thead
            sb.Append("</thead>");

            if (dto.Weekdays != null && dto.Weekdays.Count > 0)
            {
                //open tbody
                sb.Append("<tbody>");

                foreach (SalesForecastDTO day in dto.Weekdays)
                {
                    //open tr
                    sb.Append("<tr>");

                    sb.Append("<td>" + day.DateOfEntry.ToShortDateString() + "</td>");
                    sb.Append("<td>" + day.DayOfWeek + "</td>");
                    sb.Append("<td>" + day.ForecastAmount.ToString("C0") + "</td>");
                    sb.Append("<td>" + day.ForecastAM.ToString("C0") + "</td>");
                    sb.Append("<td>" + day.ForecastPM.ToString("C0") + "</td>");
                    sb.Append("<td>" + day.PriorYearSales.ToString("C0") + "</td>");
                    sb.Append("<td>" + day.Prior2YearSales.ToString("C0") + "</td>");
                    sb.Append("<td>" + day.AverageSalesPerMonth.ToString("C0") + "</td>");
                    sb.Append("<td>" + day.LaborForecast.ToString("C0") + "</td>");

                    //close tr
                    sb.Append("</tr>");
                }

                //close tbody
                sb.Append("</tbody>");
            }
            else
            {
                sb.AppendLine("<br><i>N/A</i>");
            }

            //close table
            sb.Append("</table>");

            return sb.ToString();
        }

        public void SendRedbookSubmitEmail(RedbookEntry entry, SalesForecastExportDTO dto)
        {
            MailMessage msg = new MailMessage(new MailAddress("redbook@millersalehouse.com"), new MailAddress(entry.LocationId + "redbook@millersalehouse.com"))
            {
                Subject = BuildSubject(entry.LocationId),
                Body = BuildBody(entry, dto),
                IsBodyHtml = true
            };

            msg.CC.Add(new MailAddress("MAH" + entry.LocationId + "@millersalehouse.com"));
            msg.ReplyToList.Add(new MailAddress("MAH" + entry.LocationId + "@millersalehouse.com"));

            EmailClient.Send(msg);

            msg.Dispose();
        }
    }
}
