using D_Squared.Data.Context;
using D_Squared.Data.Queries;
using D_Squared.Domain.TransferObjects;
using D_Squared.Domain.TransferObjects.Attributes;
using D_Squared.Web.Helpers;
using D_Squared.Web.Models;
using System.Text;
using System.Web.Mvc;

namespace D_Squared.Web.Controllers
{
    public class SalesReportingController : BaseController
    {
        private readonly D_SquaredDbContext db;
        private readonly SalesDataQueries sdq;
        private readonly SalesReportingInitializer init;

        /// <summary>
        /// Constructor for SalesReportingController to initialize entity model contexts and SalesReportingInitializer
        /// </summary>
        public SalesReportingController()
        {
            db = new D_SquaredDbContext();
            sdq = new SalesDataQueries(db);

            init = new SalesReportingInitializer(eq, sdq);
        }

        /// <summary>
        /// Sales Reports landing page action
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            string username = User.TruncatedName;
            EmployeeDTO employee = eq.GetEmployeeInfo(username);
            SalesReportSearchViewModel model = new SalesReportSearchViewModel() { EmployeeInfo = employee };
            return View(model);
        }

        #region Sales Reports - Search Screens actions
        public ActionResult SalesSearch(SalesDataSearchDTO searchDTO)
        {
            SalesReportSearchViewModel model = init.InitializeSalesReportSearchViewModel(User, searchDTO);
            model.SearchDTO = searchDTO;
            return View(model);
        }

        public ActionResult IdealCashSearch(IdealCashSearchDTO searchDTO)
        {
            IdealCashReportSearchViewModel model = init.InitializeIdealCashReportSearchViewModel(User, searchDTO);
            model.SearchDTO = searchDTO;
            return View(model);
        }

        public ActionResult PaidInOutSearch(PaidInOutSearchDTO searchDTO)
        {
            PaidInOutSearchViewModel model = init.InitializePaidInOutSearchViewModel(User, searchDTO);
            model.SearchDTO = searchDTO;
            return View(model);
        }

        public ActionResult ServerSalesSearch(ServerSalesSearchDTO searchDTO)
        {
            ServerSalesSearchViewModel model = init.InitializeServerSalesSearchViewModel(User, searchDTO);
            model.SearchDTO = searchDTO;
            return View(model);
        }

        public ActionResult HourlySalesSearch(HourlySalesSearchDTO searchDTO)
        {
            HourlySalesSearchViewModel model = init.InitializeHourlySalesSearchViewModel(User, searchDTO);
            model.SearchDTO = searchDTO;
            return View(model);
        }
        #endregion

        #region Sales Reprots - View Screens actions
        public ActionResult PreviousWeek(string actionName)
        {
            return RedirectToAction(actionName, new { isLastWeek = true });
        }

        public ActionResult IdealCashView(bool isLastWeek = false)
        {
            IdealCashReportViewModel model = init.InitializeIdealCashReportViewModel(User, isLastWeek);
            return View(model);
        }

        public ActionResult HourlySalesView(bool isLastWeek = false)
        {
            HourlySalesViewModel model = init.InitializeHourlySalesViewModel(User, isLastWeek);
            return View(model);
        }

        public ActionResult PaidInOutView(bool isLastWeek = false)
        {
            PaidInOutViewModel model = init.InitializePaidInOutViewModel(User, isLastWeek);
            return View(model);
        }

        public ActionResult SalesView(bool isLastWeek = false)
        {
            SalesReportViewModel model = init.InitializeSalesReportViewModel(User, isLastWeek);
            return View(model);
        }

        public ActionResult ServerSalesView(bool isLastWeek = false)
        {
            ServerSalesViewModel model = init.InitializeServerSalesViewModel(User, isLastWeek);
            return View(model);
        }
        #endregion

        #region Expor to CSV for Search screens
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "action", Argument = "ExportIdealCashRptCSV")]
        public ActionResult ExportIdealCashRptCSV(IdealCashSearchDTO searchDTO)
        {
            IdealCashReportSearchViewModel model = init.InitializeIdealCashReportSearchViewModel(User, searchDTO);
            string exportData = ReportExportHelper<IdealCashDTO>.BuildExportString(model.SearchResults, DisplayFor.Condition_2);
            return new Export("IdealCashReportExport.csv", Encoding.ASCII.GetBytes(exportData));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "action", Argument = "ExportPaidInOutRptCSV")]
        public ActionResult ExportPaidInOutRptCSV(PaidInOutSearchDTO searchDTO)
        {
            PaidInOutSearchViewModel model = init.InitializePaidInOutSearchViewModel(User, searchDTO);
            string exportData = ReportExportHelper<PaidInOutDTO>.BuildExportString(model.SearchResults, 
                                                                    (searchDTO.SelectedDayOrWeekFilter == PaidInOutSearchDTO.ReportByDay) ? DisplayFor.Condition_1 : DisplayFor.Condition_2);
            return new Export("PaidInOutReportExport.csv", Encoding.ASCII.GetBytes(exportData));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "action", Argument = "ExportSalesRptCSV")]
        public ActionResult ExportSalesRptCSV(SalesDataSearchDTO searchDTO)
        {
            SalesReportSearchViewModel model = init.InitializeSalesReportSearchViewModel(User, searchDTO);
            string exportData = ReportExportHelper<SalesDataDTO>.BuildExportString(model.SearchResults, 
                                                                    (searchDTO.SelectedReportType == SalesDataSearchDTO.ReportByDay) ? DisplayFor.Condition_1 : DisplayFor.Condition_2 );
            return new Export("SalesReportExport.csv", Encoding.ASCII.GetBytes(exportData));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "action", Argument = "ExportServerSalesRptCSV")]
        public ActionResult ExportServerSalesRptCSV(ServerSalesSearchDTO searchDTO)
        {
            ServerSalesSearchViewModel model = init.InitializeServerSalesSearchViewModel(User, searchDTO);
            string exportData = ReportExportHelper<ServerSalesDTO>.BuildExportString(model.SearchResults,
                                                                    (searchDTO.SelectedDWBWFilter == ServerSalesSearchDTO.ReportByDay) ? DisplayFor.Condition_1 : DisplayFor.Condition_2);
            return new Export("ServerSalesReportExport.csv", Encoding.ASCII.GetBytes(exportData));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "action", Argument = "ExportHourlySalesRptCSV")]
        public ActionResult ExportHourlySalesRptCSV(HourlySalesSearchDTO searchDTO)
        {
            HourlySalesSearchViewModel model = init.InitializeHourlySalesSearchViewModel(User, searchDTO);
            string exportData = ReportExportHelper<HourlySalesDTO>.BuildExportString(model.SearchResults,
                                                                    (searchDTO.SelectedReportType == HourlySalesSearchDTO.ReportByDay) ? DisplayFor.Condition_1 : DisplayFor.Condition_2);
            return new Export("HourlySalesReportExport.csv", Encoding.ASCII.GetBytes(exportData));
        }
        #endregion

        #region Export to CSV for View Screens
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "action", Argument = "ExportIdealCashViewCSV")]
        public ActionResult ExportIdealCashViewCSV(bool isLastWeek = false)
        {
            IdealCashReportViewModel model = init.InitializeIdealCashReportViewModel(User, isLastWeek);
            string exportData = ReportExportHelper<IdealCashDTO>.BuildExportString(model.IdealCashList, DisplayFor.Condition_2);
            return new Export("IdealCashViewExport.csv", Encoding.ASCII.GetBytes(exportData));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "action", Argument = "ExportPaidInOutViewCSV")]
        public ActionResult ExportPaidInOutViewCSV(bool isLastWeek = false)
        {
            PaidInOutViewModel model = init.InitializePaidInOutViewModel(User, isLastWeek);
            string exportData = ReportExportHelper<PaidInOutDTO>.BuildExportString(model.PaidInOutList, DisplayFor.Condition_2);
            return new Export("PaidInOutViewExport.csv", Encoding.ASCII.GetBytes(exportData));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "action", Argument = "ExportSalesViewCSV")]
        public ActionResult ExportSalesViewCSV(bool isLastWeek = false)
        {
            SalesReportViewModel model = init.InitializeSalesReportViewModel(User, isLastWeek);
            string exportData = ReportExportHelper<SalesDataDTO>.BuildExportString(model.SalesList, DisplayFor.Condition_2);
            return new Export("SalesViewExport.csv", Encoding.ASCII.GetBytes(exportData));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "action", Argument = "ExportServerSalesViewCSV")]
        public ActionResult ExportServerSalesViewCSV(bool isLastWeek = false)
        {
            ServerSalesViewModel model = init.InitializeServerSalesViewModel(User, isLastWeek);
            string exportData = ReportExportHelper<ServerSalesDTO>.BuildExportString(model.ServerSalesList, DisplayFor.Condition_2);
            return new Export("ServerSalesViewExport.csv", Encoding.ASCII.GetBytes(exportData));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "action", Argument = "ExportHourlySalesViewCSV")]
        public ActionResult ExportHourlySalesViewCSV(bool isLastWeek = false)
        {
            HourlySalesViewModel model = init.InitializeHourlySalesViewModel(User, isLastWeek);
            string exportData = ReportExportHelper<HourlySalesDTO>.BuildExportString(model.HourlySalesList, DisplayFor.Condition_2);
            return new Export("HourlySalesViewExport.csv", Encoding.ASCII.GetBytes(exportData));
        }
        #endregion
    }
}