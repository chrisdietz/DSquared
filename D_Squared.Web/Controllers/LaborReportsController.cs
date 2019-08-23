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
    public class LaborReportsController : BaseController
    {
        private readonly D_SquaredDbContext db;
        private readonly LaborDataQueries ldq;
        private readonly LaborReportsInitializer init;

        public LaborReportsController()
        {
            db = new D_SquaredDbContext();
            ldq = new LaborDataQueries(db);

            init = new LaborReportsInitializer(eq, ldq);
        }

        // GET: LaborReports
        public ActionResult Index()
        {
            string username = User.TruncatedName;
            EmployeeDTO employee = eq.GetEmployeeInfo(username);
            OvertimeReportingSearchViewModel model = new OvertimeReportingSearchViewModel() { EmployeeInfo = employee };
            return View(model);
        }

        public ActionResult PreviousWeek(string actionName)
        {
            return RedirectToAction(actionName, new { isLastWeek = true });
        }

        public ActionResult OvertimeView(bool isLastWeek = false)
        {
            OvertimeReportingViewModel model = init.InitializeOvertimeReportingViewModel(User, isLastWeek);
            return View(model);
        }

        public ActionResult OvertimeSearch(WeeklyTotalDurationSearchDTO searchDTO)
        {
            OvertimeReportingSearchViewModel model = init.InitializeOvertimeReportingSearchViewModel(User, searchDTO);
            model.SearchDTO = searchDTO;

            return View(model);
        }
        
        public ActionResult LaborSummaryView(bool isLastWeek = false)
        {
            LaborSummaryViewModel model = init.InitializeLaborSummaryViewModel(User, isLastWeek);
            return View(model);
        }

        public ActionResult LaborSummarySearch(LaborDataSearchDTO searchDTO)
        {
            LaborSummarySearchViewModel model = init.InitializeLaborSummarySearchViewModel(User, searchDTO);
            model.SearchDTO = searchDTO;

            return View(model);
        }

        public ActionResult Labor8020View(bool isLastWeek = false)
        {
            Labor8020ViewModel model = init.InitializeLabor8020ViewModel(User, isLastWeek);
            return View(model);
        }

        public ActionResult Labor8020Search(Labor8020SearchDTO searchDTO)
        {
            Labor8020SearchViewModel model = init.InitializeLabor8020SearchViewModel(User, searchDTO);
            model.SearchDTO = searchDTO;

            return View(model);
        }

        public ActionResult TimeClockDetailView(bool isLastWeek = false)
        {
            TimeClockDetailViewModel model = init.InitializeTimeClockDetailViewModel(User, isLastWeek);
            return View(model);
        }

        public ActionResult TimeClockDetailSearch(TimeClockDetailSearchDTO searchDTO)
        {
            TimeClockDetailSearchViewModel model = init.InitializeTimeClockDetailSearchViewModel(User, searchDTO);
            model.SearchDTO = searchDTO;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "action", Argument = "ExportLabor8020RptCSV")]
        public ActionResult ExportLabor8020RptCSV(Labor8020SearchDTO searchDTO)
        {
            Labor8020SearchViewModel model = init.InitializeLabor8020SearchViewModel(User, searchDTO);
            string exportData = ReportExportHelper<Labor8020DTO>.BuildExportString(model.SearchResults,
                                                                    (searchDTO.SelectedDateFilter == Labor8020SearchDTO.ReportByDay) ? DisplayFor.Condition_1 : DisplayFor.Condition_2);
            return new Export("Labor8020ReportExport.csv", Encoding.ASCII.GetBytes(exportData));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "action", Argument = "ExportLaborSummaryRptCSV")]
        public ActionResult ExportLaborSummaryRptCSV(LaborDataSearchDTO searchDTO)
        {
            LaborSummarySearchViewModel model = init.InitializeLaborSummarySearchViewModel(User, searchDTO);
            string exportData = ReportExportHelper<LaborDataDTO>.BuildExportString(model.SearchResults,
                                                                    (searchDTO.SelectedJobOrCenterFilter == LaborDataSearchDTO.ReportByJob) ? DisplayFor.Condition_1 : DisplayFor.Condition_2);
            return new Export("LaborSummaryReportExport.csv", Encoding.ASCII.GetBytes(exportData));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "action", Argument = "ExportOvertimeRptCSV")]
        public ActionResult ExportOvertimeRptCSV(WeeklyTotalDurationSearchDTO searchDTO)
        {
            OvertimeReportingSearchViewModel model = init.InitializeOvertimeReportingSearchViewModel(User, searchDTO);
            Dictionary<string, string> dynamicColumnNames = new Dictionary<string, string>
            {
                { "Hours Over 35", $"Hours Over {searchDTO.SelectedHours}" }
            };
            string exportData = ReportExportHelper<WeeklyTotalDurationDTO>.BuildExportString(model.SearchResults, DisplayFor.Condition_2, dynamicColumnNames);
            return new Export("OvertimeReportExport.csv", Encoding.ASCII.GetBytes(exportData));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "action", Argument = "ExportTimeClockDetailRptCSV")]
        public ActionResult ExportTimeClockDetailRptCSV(TimeClockDetailSearchDTO searchDTO)
        {
            TimeClockDetailSearchViewModel model = init.InitializeTimeClockDetailSearchViewModel(User, searchDTO);
            string exportData = ReportExportHelper<TimeClockDetailDTO>.BuildExportString(model.SearchResults, DisplayFor.Condition_2);
            return new Export("TimeClockDetailReportExport.csv", Encoding.ASCII.GetBytes(exportData));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "action", Argument = "ExportLabor8020ViewCSV")]
        public ActionResult ExportLabor8020ViewCSV(bool isLastWeek = false)
        {
            Labor8020ViewModel model = init.InitializeLabor8020ViewModel(User, isLastWeek);
            string exportData = ReportExportHelper<Labor8020DTO>.BuildExportString(model.Labor8020List, DisplayFor.Condition_2);
            return new Export("Labor8020ViewExport.csv", Encoding.ASCII.GetBytes(exportData));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "action", Argument = "ExportLaborSummaryViewCSV")]
        public ActionResult ExportLaborSummaryViewCSV(bool isLastWeek = false)
        {
            LaborSummaryViewModel model = init.InitializeLaborSummaryViewModel(User, isLastWeek);
            string exportData = ReportExportHelper<LaborDataDTO>.BuildExportString(model.LaborDataList, DisplayFor.Condition_1);
            return new Export("LaborSummaryViewExport.csv", Encoding.ASCII.GetBytes(exportData));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "action", Argument = "ExportOvertimeViewCSV")]
        public ActionResult ExportOvertimeViewCSV(bool isLastWeek = false)
        {
            OvertimeReportingViewModel model = init.InitializeOvertimeReportingViewModel(User, isLastWeek);
            string exportData = ReportExportHelper<WeeklyTotalDurationDTO>.BuildExportString(model.WeeklyTotalDurationList, DisplayFor.Condition_2);
            return new Export("OvertimeViewExport.csv", Encoding.ASCII.GetBytes(exportData));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "action", Argument = "ExportTimeClockDetailViewCSV")]
        public ActionResult ExportTimeClockDetailViewCSV(bool isLastWeek = false)
        {
            TimeClockDetailViewModel model = init.InitializeTimeClockDetailViewModel(User, isLastWeek);
            string exportData = ReportExportHelper<TimeClockDetailDTO>.BuildExportString(model.TimeClockDetailList, DisplayFor.Condition_2);
            return new Export("TimeClockDetailViewExport.csv", Encoding.ASCII.GetBytes(exportData));
        }
    }
}