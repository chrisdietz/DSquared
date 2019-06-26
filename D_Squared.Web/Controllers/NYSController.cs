using D_Squared.Data.Context;
using D_Squared.Data.Queries;
using D_Squared.Domain.TransferObjects;
using D_Squared.Web.Helpers;
using D_Squared.Web.Models;
using System.Text;
using System.Web.Mvc;

namespace D_Squared.Web.Controllers
{
    public class NYSController : BaseController
    {
        private readonly D_SquaredDbContext db;

        private readonly CodeQueries cq;
        private readonly NYSQueries nysq;

        private readonly NYSInitializer init;

        public NYSController()
        {
            db = new D_SquaredDbContext();

            cq = new CodeQueries(db);
            nysq = new NYSQueries(db);

            init = new NYSInitializer(eq, nysq);
        }

        // GET: NYS
        public ActionResult Index(bool isLastWeek = false)
        {
            NYSViewModel model = init.InitializeNYSViewModel(User.TruncatedName, isLastWeek);

            return View(model);
        }

        public ActionResult PreviousWeek()
        {
            return RedirectToAction("Index", new { isLastWeek = true });
        }

        public ActionResult Search()
        {
            NYSSearchViewModel model = init.InitializeNYSSearchViewModel(User.TruncatedName, User.IsRegionalManager(), User.IsDivisionalVP(), User.IsDSquaredAdmin());

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "action", Argument = "Search")]
        public ActionResult Search(NYSSearchViewModel model)
        {
            model = init.InitializeNYSSearchViewModel(model.SearchDTO, User.TruncatedName, User.IsRegionalManager(), User.IsDivisionalVP(), User.IsDSquaredAdmin());

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "action", Argument = "ExportCSV")]
        public ActionResult ExportCSV(NYSSearchViewModel model)
        {
            string username = User.TruncatedName;
            model = init.InitializeNYSSearchViewModel(model.SearchDTO, username, User.IsRegionalManager(), User.IsDivisionalVP(), User.IsDSquaredAdmin());

            return new Export("NYSExport.csv", Encoding.ASCII.GetBytes(NYSExportHelper.ExportNYS(model.SearchResults, false).ToString()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "action", Argument = "ExportByDayCSV")]
        public ActionResult ExportByDayCSV(NYSSearchViewModel model)
        {
            string username = User.TruncatedName;
            model = init.InitializeNYSSearchViewModel(model.SearchDTO, username, User.IsRegionalManager(), User.IsDivisionalVP(), User.IsDSquaredAdmin());

            return new Export("NYSExportByDay.csv", Encoding.ASCII.GetBytes(NYSExportHelper.ExportNYS(model.SearchResults, true).ToString()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "action", Argument = "IndexExportCSV")]
        public ActionResult IndexExportCSV(NYSViewModel model)
        {
            NYSSearchDTO dto = new NYSSearchDTO(model.EndingPeriod, model.EndingPeriod)
            {
                SelectedLocation = model.EmployeeInfo.StoreNumber
            };

            string username = User.TruncatedName;
            NYSSearchViewModel result = init.InitializeNYSSearchViewModel(dto, username, User.IsRegionalManager(), User.IsDivisionalVP(), User.IsDSquaredAdmin());

            return new Export("NYSExport.csv", Encoding.ASCII.GetBytes(NYSExportHelper.ExportNYS(result.SearchResults, false).ToString()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "action", Argument = "IndexExportByDayCSV")]
        public ActionResult IndexExportByDayCSV(NYSViewModel model)
        {
            NYSSearchDTO dto = new NYSSearchDTO(model.EndingPeriod, model.EndingPeriod)
            {
                SelectedLocation = model.EmployeeInfo.StoreNumber
            };

            string username = User.TruncatedName;
            NYSSearchViewModel result = init.InitializeNYSSearchViewModel(dto, username, User.IsRegionalManager(), User.IsDivisionalVP(), User.IsDSquaredAdmin());

            return new Export("NYSExport.csv", Encoding.ASCII.GetBytes(NYSExportHelper.ExportNYS(result.SearchResults, true).ToString()));
        }
    }
}