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

        private readonly TipQueries tq;

        private readonly TipReportingInitializer init;

        public TipReportingController()
        {
            db = new D_SquaredDbContext();

            tq = new TipQueries(db);

            init = new TipReportingInitializer(eq, tq);
        }

        public ActionResult Index(bool isLastWeek = false)
        {
            TipReportingViewModel model = init.InitializeTipReportingViewModel(User.TruncatedName, User.IsRegionalManager(), User.IsDivisionalVP(), User.IsDSquaredAdmin(), isLastWeek);

            return View(model);
        }

        public ActionResult PreviousWeek()
        {
            return RedirectToAction("Index", new { isLastWeek = true });
        }

        public ActionResult Search()
        {
            TipReportingSearchViewModel model = init.InitializeTipReportingSearchViewModel(User.TruncatedName, User.IsRegionalManager(), User.IsDivisionalVP(), User.IsDSquaredTips() ? true : User.IsDSquaredAdmin());

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(TipReportingSearchViewModel model)
        {
            model = init.InitializeTipReportingSearchViewModel(model.SearchDTO, User.TruncatedName,  User.IsRegionalManager(), User.IsDivisionalVP(), User.IsDSquaredTips() ? true : User.IsDSquaredAdmin());

            return View(model);
        }
    }
}