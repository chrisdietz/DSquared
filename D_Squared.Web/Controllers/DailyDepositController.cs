using D_Squared.Data.Context;
using D_Squared.Data.Millers.Context;
using D_Squared.Data.Millers.Queries;
using D_Squared.Data.Queries;
using D_Squared.Domain.TransferObjects;
using D_Squared.Web.Helpers;
using D_Squared.Web.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

namespace D_Squared.Web.Controllers
{
    public class DailyDepositController : BaseController
    {
        private readonly D_SquaredDbContext db;
        private readonly EmployeeDbContext e_db;
        private readonly DailyDepositQueries ddq;
        private readonly EmployeeQueries eq;

        public DailyDepositController()
        {
            db = new D_SquaredDbContext();
            ddq = new DailyDepositQueries(db);

            e_db = new EmployeeDbContext();
            eq = new EmployeeQueries(e_db);
        }

        public ActionResult Index()
        {
            string username = User.Identity.Name.Substring(User.Identity.Name.IndexOf('\\') + 1);

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
                List<DepositEntryDTO> weekdays = ddq.GetSpecificWeekAsDepositEntryDTOList(DateTime.Today.ToLocalTime(), employee.StoreNumber);

                DailyDepositViewModel model = new DailyDepositViewModel(weekdays, today, employee, true);

                return View(model);
            }
        }

        public ActionResult DailyDepositHelp()
        {
            return PartialView("_DailyDepositHelp");
        }

        [HttpPost]
        [FormAction]
        public ActionResult Index(DailyDepositViewModel model)
        {
            try
            {
                string userName = User.Identity.Name.Substring(User.Identity.Name.IndexOf('\\') + 1);
                string storeNumber = eq.GetStoreNumber(userName);

                ddq.AddOrUpdateDeposits(model.Weekdays, storeNumber, User.Identity.Name);
            }
            catch
            {
                Warning("Error occurred. If this error persists, please contact an administrator.");

                return RedirectToAction("Index");
            }

            Success("The Daily Deposits for Restaurant: <u>" + model.EmployeeInfo.StoreNumber + "</u> have been saved successfully. You may close this window");

            if (model.CurrentWeekFlag)
                return RedirectToAction("Index");
            else
                return RedirectToAction("PreviousWeek");

        }

        public ActionResult PreviousWeek()
        {
            string username = User.Identity.Name.Substring(User.Identity.Name.IndexOf('\\') + 1);

            DateTime today = DateTime.Now.ToLocalTime();
            EmployeeDTO employee = eq.GetEmployeeInfo(username);
            List<DepositEntryDTO> weekdays = ddq.GetSpecificWeekAsDepositEntryDTOList(DateTime.Today.ToLocalTime().AddDays(-7), employee.StoreNumber);

            DailyDepositViewModel model = new DailyDepositViewModel(weekdays, today, employee, false);

            return View("Index", model);
        }

        public ActionResult DepositReport()
        {
            string username = User.Identity.Name.Substring(User.Identity.Name.IndexOf('\\') + 1);

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
                List<DateTime> theWeek = ddq.GetCurrentWeek(currentDate);

                DepositReportViewModel model = new DepositReportViewModel()
                {
                    CurrentDate = DateTime.Now.ToLocalTime(),
                    SearchDTO = new DepositSummarySearchDTO(currentDate),
                    SummaryList = ddq.GetDepositSummaryList(currentDate, eq.GetLocationList()),
                    ColumnTotalList = ddq.GetWeeklyReportColumnTotals(currentDate),
                    EndingPeriod = theWeek.LastOrDefault(),
                    StartingPeriod = theWeek.FirstOrDefault()
                };

                ddq.GetWeeklyReportColumnTotals(currentDate);

                return View("DepositReport", model);
            }
        }

        [HttpPost]
        public ActionResult DepositReport(DepositReportViewModel model)
        {
            List<DateTime> theWeek = ddq.GetCurrentWeek(model.SearchDTO.DesiredDate);

            model.CurrentDate = DateTime.Now.ToLocalTime();
            model.SearchDTO = new DepositSummarySearchDTO(model.SearchDTO.DesiredDate);
            model.SummaryList = ddq.GetDepositSummaryList(model.SearchDTO.DesiredDate, eq.GetLocationList());
            model.ColumnTotalList = ddq.GetWeeklyReportColumnTotals(model.SearchDTO.DesiredDate);
            model.EndingPeriod = theWeek.LastOrDefault();
            model.StartingPeriod = theWeek.FirstOrDefault();

            return View("DepositReport", model);
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