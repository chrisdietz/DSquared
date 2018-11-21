using D_Squared.Data.Context;
using D_Squared.Data.Millers.Context;
using D_Squared.Data.Millers.Queries;
using D_Squared.Data.Queries;
using D_Squared.Web.Helpers;
using D_Squared.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace D_Squared.Web.Controllers
{
    public class TipReportingController : BaseController
    {
        private readonly D_SquaredDbContext db;
        private readonly EmployeeDbContext e_db;

        private readonly EmployeeQueries eq;
        private readonly TipQueries tq;

        private readonly TipReportingInitializer init;

        public TipReportingController()
        {
            db = new D_SquaredDbContext();
            e_db = new EmployeeDbContext();

            eq = new EmployeeQueries(e_db);
            tq = new TipQueries(db);

            init = new TipReportingInitializer(eq, tq);
        }

        public ActionResult Index(bool isLastWeek = false)
        {
            string username = User.TruncatedName;

            if (!eq.EmployeeExists(username))
            {
                ErrorViewModel error = new ErrorViewModel
                {
                    Username = username
                };

                return View("../Home/EmployeeError", error);
            }
            else
            {
                TipReportingViewModel model = init.InitializeTipReportingViewModel(username, User.IsRegionalManager(), User.IsDivisionalVP(), User.IsDSquaredAdmin(), isLastWeek);

                return View(model);
            }
        }

        public ActionResult PreviousWeek()
        {
            return RedirectToAction("Index", new { isLastWeek = true });
        }

        public ActionResult Search()
        {
            string username = User.TruncatedName;

            if (!eq.EmployeeExists(username))
            {
                ErrorViewModel error = new ErrorViewModel
                {
                    Username = username
                };

                return View("../Home/EmployeeError", error);
            }
            else
            {
                TipReportingSearchViewModel model = init.InitializeTipReportingSearchViewModel(username, User.IsRegionalManager(), User.IsDivisionalVP(), User.IsDSquaredAdmin());

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(TipReportingSearchViewModel model)
        {
            string username = User.TruncatedName;

            if (!eq.EmployeeExists(username))
            {
                ErrorViewModel error = new ErrorViewModel
                {
                    Username = username
                };

                return View("../Home/EmployeeError", error);
            }
            else
            {
                model = init.InitializeTipReportingSearchViewModel(model.SearchDTO, username,  User.IsRegionalManager(), User.IsDivisionalVP(), User.IsDSquaredAdmin());

                return View(model);
            }
        }
    }
}