using D_Squared.Data.Context;
using D_Squared.Data.Millers.Context;
using D_Squared.Data.Millers.Queries;
using D_Squared.Data.Queries;
using D_Squared.Domain.Entities;
using D_Squared.Domain.TransferObjects;
using D_Squared.Web.Helpers;
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
        private readonly TipPercentageInitializer init;

        public TipPercentageController()
        {
            db = new D_SquaredDbContext();
            tpq = new TipPercentageQueries(db);

            init = new TipPercentageInitializer(eq, tpq);
        }

        // GET: TipPercentage
        public ActionResult Index(TipPercentageSearchDTO searchDTO)
        {
            string username = User.TruncatedName;
            //DateTime selectedDate = DateTime.Today;
            EmployeeDTO employee = eq.GetEmployeeInfo(username);
            TipPercentageSearchViewModel model = init.InitializeTipPercentageSearchViewModel(User, searchDTO);
            model.SearchDTO = searchDTO;

            return View(model);
        }

        public ActionResult Detail(string storeNumber, string employeeNumber, DateTime startDate, DateTime endDate)
        {
            string username = User.TruncatedName;

            TipPercentagePartialViewModel partial = init.InitializeTipPercentagePartialViewModel(storeNumber, employeeNumber, startDate, endDate);

            return PartialView("~/Views/TipPercentage/_EmployeeSalesAndTipsDetail.cshtml", partial);
        }
    }
}