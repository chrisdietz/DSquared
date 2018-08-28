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

        protected List<EventDTO> DeserializeSelectedEvents(string selectedEvents)
        {
            return JsonConvert.DeserializeObject<List<EventDTO>>(selectedEvents);
        }

        private string BuildBody(RedbookEntry entry)
        {
            StringBuilder sb = new StringBuilder();

            //content header
            sb.AppendLine("Submission Summary for location <u>" + entry.LocationId + "</u> and date <u>" + entry.BusinessDate.ToShortDateString() + "</u><br><br>");

            //data fields
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

            return sb.ToString();
        }

        public void SendRedbookSubmitEmail(RedbookEntry entry)
        {
            MailMessage msg = new MailMessage(new MailAddress("redbook@millersalehouse.com"), new MailAddress(entry.LocationId + "redbook@millersalehouse.com"))
            {
                Subject = BuildSubject(entry.LocationId),
                Body = BuildBody(entry),
                IsBodyHtml = true
            };

            msg.CC.Add(new MailAddress("MAH" + entry.LocationId + "@millersalehouse.com"));
            msg.ReplyToList.Add(new MailAddress("MAH" + entry.LocationId + "@millersalehouse.com"));

            EmailClient.Send(msg);

            msg.Dispose();
        }
    }
}
