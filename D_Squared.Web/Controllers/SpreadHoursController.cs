using D_Squared.Data.Context;
using D_Squared.Data.Millers.Context;
using D_Squared.Data.Millers.Queries;
using D_Squared.Data.Queries;
using D_Squared.Domain.TransferObjects;
using D_Squared.Web.Helpers;
using D_Squared.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace D_Squared.Web.Controllers
{
    public class SpreadHoursController : BaseController
    {
        private readonly D_SquaredDbContext db;

        private readonly CodeQueries cq;
        private readonly SpreadHourQueries shq;

        private readonly SpreadHoursInitializer init;

        public SpreadHoursController()
        {
            db = new D_SquaredDbContext();

            cq = new CodeQueries(db);
            shq = new SpreadHourQueries(db);

            init = new SpreadHoursInitializer(eq, shq);
        }

        // GET: SpreadHours
        public ActionResult Index(bool isLastWeek = false)
        {
            SpreadHourViewModel model = init.InitializeSpreadHourViewModel(User.TruncatedName, isLastWeek);

            return View(model);
        }

        public ActionResult PreviousWeek()
        {
            return RedirectToAction("Index", new { isLastWeek = true });
        }

        public ActionResult Search()
        {
            SpreadHourSearchViewModel model = init.InitializeSpreadHourSearchViewModel(User.TruncatedName, User.IsRegionalManager(), User.IsDivisionalVP(), User.IsDSquaredAdmin());

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "action", Argument = "Search")]
        public ActionResult Search(SpreadHourSearchViewModel model)
        {
                model = init.InitializeSpreadHourSearchViewModel(model.SearchDTO, User.TruncatedName, User.IsRegionalManager(), User.IsDivisionalVP(), User.IsDSquaredAdmin());

                return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "action", Argument = "ExportCSV")]
        public ActionResult ExportCSV(SpreadHourSearchViewModel model)
        {
            string username = User.TruncatedName;
            model = init.InitializeSpreadHourSearchViewModel(model.SearchDTO, username, User.IsRegionalManager(), User.IsDivisionalVP(), User.IsDSquaredAdmin());

            return new Export("SpreadHourExport.csv", Encoding.ASCII.GetBytes(SpreadHourExportHelper.ExportSpreadHours(model.SearchResults, false).ToString()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "action", Argument = "ExportByDayCSV")]
        public ActionResult ExportByDayCSV(SpreadHourSearchViewModel model)
        {
            string username = User.TruncatedName;
            model = init.InitializeSpreadHourSearchViewModel(model.SearchDTO, username, User.IsRegionalManager(), User.IsDivisionalVP(), User.IsDSquaredAdmin());

            return new Export("SpreadHourExportByDay.csv", Encoding.ASCII.GetBytes(SpreadHourExportHelper.ExportSpreadHours(model.SearchResults, true).ToString()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "action", Argument = "IndexExportCSV")]
        public ActionResult IndexExportCSV(SpreadHourViewModel model)
        {
            SpreadHourSearchDTO dto = new SpreadHourSearchDTO(model.EndingPeriod, model.EndingPeriod)
            {
                SelectedLocation = model.EmployeeInfo.StoreNumber
            };

            string username = User.TruncatedName;
            SpreadHourSearchViewModel result = init.InitializeSpreadHourSearchViewModel(dto, username, User.IsRegionalManager(), User.IsDivisionalVP(), User.IsDSquaredAdmin());

            return new Export("SpreadHourExport.csv", Encoding.ASCII.GetBytes(SpreadHourExportHelper.ExportSpreadHours(result.SearchResults, false).ToString()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "action", Argument = "IndexExportByDayCSV")]
        public ActionResult IndexExportByDayCSV(SpreadHourViewModel model)
        {
            SpreadHourSearchDTO dto = new SpreadHourSearchDTO(model.EndingPeriod, model.EndingPeriod)
            {
                SelectedLocation = model.EmployeeInfo.StoreNumber
            };

            string username = User.TruncatedName;
            SpreadHourSearchViewModel result = init.InitializeSpreadHourSearchViewModel(dto, username, User.IsRegionalManager(), User.IsDivisionalVP(), User.IsDSquaredAdmin());

            return new Export("SpreadHourExport.csv", Encoding.ASCII.GetBytes(SpreadHourExportHelper.ExportSpreadHours(result.SearchResults, true).ToString()));
        }
    }
}