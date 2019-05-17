using D_Squared.Data.Context;
using D_Squared.Data.Millers.Context;
using D_Squared.Data.Millers.Queries;
using D_Squared.Data.Queries;
using D_Squared.Domain.Entities;
using D_Squared.Domain.TransferObjects;
using D_Squared.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace D_Squared.Web.Controllers
{
    public class TipPercentageController : BaseController
    {
        private readonly D_SquaredDbContext db;
        private readonly TipPercentageQueries tpq;

        public TipPercentageController()
        {
            db = new D_SquaredDbContext();
            tpq = new TipPercentageQueries(db);
        }

        // GET: TipPercentage
        public ActionResult Index()
        {
            DateTime today = DateTime.Now.ToLocalTime();
            EmployeeDTO employee = eq.GetEmployeeInfo(User.TruncatedName);
            List<DateTime> dates = new List<DateTime>();
            List<TipPercentage> tipPercentageList = tpq.GetTipPercentageList(dates);

            TipPercentageSearchViewModel model = new TipPercentageSearchViewModel()
            {
                EmployeeInfo = employee
            };

            return View(model);
        }
    }
}