using D_Squared.Data.Context;
using D_Squared.Data.Queries;
using D_Squared.Domain.TransferObjects;
using D_Squared.Domain.TransferObjects.Attributes;
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
    public class SalesReportingController : BaseController
    {
        private readonly D_SquaredDbContext db;
        private readonly SalesDataQueries sdq;
        private readonly SalesReportingInitializer init;

        public SalesReportingController()
        {
            db = new D_SquaredDbContext();
            sdq = new SalesDataQueries(db);

            init = new SalesReportingInitializer(eq, sdq);
        }

        public ActionResult Index()
        {
            string username = User.TruncatedName;
            EmployeeDTO employee = eq.GetEmployeeInfo(username);
            SalesReportSearchViewModel model = new SalesReportSearchViewModel() { EmployeeInfo = employee };
            return View(model);
        }

        public ActionResult SalesView(bool isLastWeek = false)
        {
            SalesReportViewModel model = init.InitializeSalesReportViewModel(User, isLastWeek);
            return View(model);
        }

        public ActionResult PreviousWeek(string actionName)
        {
            return RedirectToAction(actionName, new { isLastWeek = true });
        }

        public ActionResult SalesSearch(SalesDataSearchDTO searchDTO)
        {
            string username = User.TruncatedName;
            EmployeeDTO employee = eq.GetEmployeeInfo(username);
            if (string.IsNullOrEmpty(searchDTO.SelectedReportType)) searchDTO.SelectedReportType = SalesDataSearchDTO.ReportByDay;
            SalesReportSearchViewModel model = init.InitializeSalesReportSearchViewModel(User, searchDTO);
            model.SearchDTO = searchDTO;

            return View(model);
        }

        public ActionResult IdealCashView(bool isLastWeek = false)
        {
            IdealCashReportViewModel model = init.InitializeIdealCashReportViewModel(User, isLastWeek);
            return View(model);
        }

        public ActionResult IdealCashSearch(IdealCashSearchDTO searchDTO)
        {
            string username = User.TruncatedName;
            EmployeeDTO employee = eq.GetEmployeeInfo(username);

            IdealCashReportSearchViewModel model = init.InitializeIdealCashReportSearchViewModel(User, searchDTO);
            model.SearchDTO = searchDTO;

            return View(model);
        }

        public ActionResult PaidInOutView(bool isLastWeek = false)
        {
            PaidInOutViewModel model = init.InitializePaidInOutViewModel(User, isLastWeek);
            return View(model);
        }

        public ActionResult PaidInOutSearch(PaidInOutSearchDTO searchDTO)
        {
            string username = User.TruncatedName;
            EmployeeDTO employee = eq.GetEmployeeInfo(username);

            PaidInOutSearchViewModel model = init.InitializePaidInOutSearchViewModel(User, searchDTO);
            model.SearchDTO = searchDTO;

            return View(model);
        }

        public ActionResult ServerSalesView(bool isLastWeek = false)
        {
            ServerSalesViewModel model = init.InitializeServerSalesViewModel(User, isLastWeek);
            return View(model);
        }

        public ActionResult ServerSalesSearch(ServerSalesSearchDTO searchDTO)
        {
            string username = User.TruncatedName;
            EmployeeDTO employee = eq.GetEmployeeInfo(username);

            ServerSalesSearchViewModel model = init.InitializeServerSalesSearchViewModel(User, searchDTO);
            model.SearchDTO = searchDTO;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "action", Argument = "ExportIdealCashRptCSV")]
        public ActionResult ExportIdealCashRptCSV(IdealCashSearchDTO searchDTO)
        {
            string username = User.TruncatedName;
            EmployeeDTO employee = eq.GetEmployeeInfo(username);
            IdealCashReportSearchViewModel model = init.InitializeIdealCashReportSearchViewModel(User, searchDTO);

            string exportData = ReportExportHelper<IdealCashDTO>.BuildExportString(model.SearchResults, DisplayFor.Weekly);
            return new Export("IdealCashReportExport.csv", Encoding.ASCII.GetBytes(exportData));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "action", Argument = "ExportPaidInOutRptCSV")]
        public ActionResult ExportPaidInOutRptCSV(PaidInOutSearchDTO searchDTO)
        {
            string username = User.TruncatedName;
            EmployeeDTO employee = eq.GetEmployeeInfo(username);
            PaidInOutSearchViewModel model = init.InitializePaidInOutSearchViewModel(User, searchDTO);

            string exportData = ReportExportHelper<PaidInOutDTO>.BuildExportString(model.SearchResults, 
                                                                    (searchDTO.SelectedDayOrWeekFilter == SalesDataSearchDTO.ReportByDay) ? DisplayFor.Daily : DisplayFor.Weekly);
            return new Export("PaidInOutReportExport.csv", Encoding.ASCII.GetBytes(exportData));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "action", Argument = "ExportSalesRptCSV")]
        public ActionResult ExportSalesRptCSV(SalesDataSearchDTO searchDTO)
        {
            string username = User.TruncatedName;
            EmployeeDTO employee = eq.GetEmployeeInfo(username);
            if (string.IsNullOrEmpty(searchDTO.SelectedReportType)) searchDTO.SelectedReportType = SalesDataSearchDTO.ReportByDay;
            SalesReportSearchViewModel model = init.InitializeSalesReportSearchViewModel(User, searchDTO);

            string exportData = ReportExportHelper<SalesDataDTO>.BuildExportString(model.SearchResults, 
                                                                    (searchDTO.SelectedReportType == SalesDataSearchDTO.ReportByDay) ? DisplayFor.Daily : DisplayFor.Weekly );
            return new Export("SalesReportExport.csv", Encoding.ASCII.GetBytes(exportData));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "action", Argument = "ExportServerSalesRptCSV")]
        public ActionResult ExportServerSalesRptCSV(ServerSalesSearchDTO searchDTO)
        {
            string username = User.TruncatedName;
            EmployeeDTO employee = eq.GetEmployeeInfo(username);
            ServerSalesSearchViewModel model = init.InitializeServerSalesSearchViewModel(User, searchDTO);

            string exportData = ReportExportHelper<ServerSalesDTO>.BuildExportString(model.SearchResults,
                                                                    (searchDTO.SelectedDWBWFilter == ServerSalesSearchDTO.ReportByDay) ? DisplayFor.Daily : DisplayFor.Weekly);
            return new Export("ServerSalesReportExport.csv", Encoding.ASCII.GetBytes(exportData));
        }
    }
}