using D_Squared.Data.Context;
using D_Squared.Data.Millers.Context;
using D_Squared.Data.Millers.Queries;
using D_Squared.Data.Queries;
using D_Squared.Domain.TransferObjects;
using D_Squared.Web.Helpers;
using D_Squared.Web.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;


namespace D_Squared.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "DailyDeposit");
        }
    }
}