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
using System.Web;
using System.Web.Mvc;
using ROLES = D_Squared.Domain.DomainConstants.RoleNames;

namespace D_Squared.Web.Controllers
{
    public class SalesForecastController : BaseController
    {
        private readonly D_SquaredDbContext db;
        private readonly EmployeeDbContext e_db;
        private readonly ForecastDataDbContext f_db;

        private readonly SalesForecastQueries sfq;
        private readonly EmployeeQueries eq;

        public SalesForecastController()
        {
            db = new D_SquaredDbContext();
            f_db = new ForecastDataDbContext();
            sfq = new SalesForecastQueries(db, f_db);

            e_db = new EmployeeDbContext();
            eq = new EmployeeQueries(e_db);
        }


        public ActionResult Index()
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
                DateTime today = DateTime.Now.ToLocalTime();
                EmployeeDTO employee = eq.GetEmployeeInfo(username);
                List<SalesForecastDTO> weekdays = sfq.GetSpecificWeekAsSalesForecastDTOList(DateTime.Today.ToLocalTime(), employee.StoreNumber);

                SalesForecastViewModel model = new SalesForecastViewModel(weekdays, today, employee);

                return View(model);
            }
        }

        [HttpPost]
        [FormAction]
        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        public ActionResult Index(SalesForecastViewModel model)
        {
            try
            {
                string userName = User.TruncatedName;
                string storeNumber = eq.GetStoreNumber(userName);

                if (ModelState.IsValid)
                {
                    sfq.AddOrUpdateSalesForecasts(model.Weekdays, storeNumber, User.Identity.Name);
                    Success("The Sales Forecasts for Restaurant: <u>" + model.EmployeeInfo.StoreNumber + "</u> have been saved successfully. You may close this window");
                }
                else
                {
                    Warning("Double Request detected; only the first submission was captured");
                    RedirectToAction("Index");
                }
            }
            catch
            {
                Warning("Error occurred. If this error persists, please contact an administrator.");

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [AuthorizeGroup(ROLES.GeneralManagerGroup)]
        public ActionResult SalesForecastReport()
        {
            string username = User.TruncatedName;
            if (!eq.EmployeeExists(username))
            {
                ErrorViewModel error = new ErrorViewModel
                {
                    Username = username
                };

                return View("EmployeeError", "Home", error);
            }
            else
            {
                DateTime currentDate = DateTime.Today.ToLocalTime();
                List<DateTime> theWeek = sfq.GetCurrentWeek(currentDate);

                SalesForecastReportViewModel model = new SalesForecastReportViewModel()
                {
                    CurrentDate = DateTime.Now.ToLocalTime(),
                    SearchDTO = new SalesForecastSummarySearchDTO(currentDate),
                    SummaryList = sfq.GetSalesForecastSummaryList(currentDate, eq.GetLocationList()),
                    ColumnTotalList = sfq.GetWeeklyReportColumnTotals(currentDate),
                    EndingPeriod = theWeek.LastOrDefault(),
                    StartingPeriod = theWeek.FirstOrDefault()
                };

                return View("SalesForecastReport", model);
            }

        }

        [HttpPost]
        [AuthorizeGroup(ROLES.GeneralManagerGroup)]
        public ActionResult SalesForecastReport(SalesForecastReportViewModel model)
        {
            List<DateTime> theWeek = sfq.GetCurrentWeek(model.SearchDTO.DesiredDate);

            model.CurrentDate = DateTime.Now.ToLocalTime();
            model.SearchDTO = new SalesForecastSummarySearchDTO(model.SearchDTO.DesiredDate);
            model.SummaryList = sfq.GetSalesForecastSummaryList(model.SearchDTO.DesiredDate, eq.GetLocationList());
            model.ColumnTotalList = sfq.GetWeeklyReportColumnTotals(model.SearchDTO.DesiredDate);
            model.EndingPeriod = theWeek.LastOrDefault();
            model.StartingPeriod = theWeek.FirstOrDefault();

            return View("SalesForecastReport", model);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                e_db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}