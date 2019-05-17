using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace D_Squared.Web.Models
{
    public class ErrorViewModel : HandleErrorInfo
    {
        public ErrorViewModel(Exception exception, string controllerName, string actionName): base(exception, controllerName, actionName)
        {
        }
        public string Username { get; set; }
        public Guid ErrorGuid { get; set; }
        public DateTime ErrorTimeStamp { get; set; }
    }
}