using D_Squared.Data.Context;
using D_Squared.Data.Employees.Context;
using D_Squared.Data.Employees.Queries;
using D_Squared.Data.Queries;
using D_Squared.Domain.TransferObjects;
using D_Squared.Web.Helpers;
using D_Squared.Web.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;


namespace D_Squared.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly D_SquaredDbContext db;
        private readonly EmployeeDbContext e_db;
        private readonly DailyDepositQueries ddq;
        private readonly EmployeeQueries eq;

        public HomeController()
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

                return View("EmployeeError", error);
            }
            else
            {
                DateTime today = DateTime.Now.ToLocalTime();
                EmployeeDTO employee = eq.GetEmployeeInfo(username);
                List<DepositEntryDTO> weekdays = ddq.GetSpecificWeekAsDepositEntryDTOList(DateTime.Today.ToLocalTime(), employee.StoreNumber);

                DailyDepositViewModel model = new DailyDepositViewModel(weekdays, today, employee);

                return View(model);
            }

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
            catch (Exception e)
            {
                Warning("Error occurred. If this error persists, please contact an administrator.");

                return RedirectToAction("Index");
            }

            Success("The Daily Deposits for Restaurant: <u>" + model.EmployeeInfo.StoreNumber + "</u> have been saved successfully. You may close this window");
            return RedirectToAction("Index");
        }

        public ActionResult DepositReport()
        {
            DateTime currentDate = DateTime.Today.ToLocalTime();

            DepositReportViewModel model = new DepositReportViewModel()
            {
                CurrentDate = DateTime.Now.ToLocalTime(),
                SearchDTO = new DepositSummarySearchDTO(currentDate),
                SummaryList = ddq.GetDepositSummaryList(currentDate, eq.GetLocationList()),
                ColumnTotalList = ddq.GetWeeklyReportColumnTotals(currentDate)
            };

            ddq.GetWeeklyReportColumnTotals(currentDate);

            return View("DepositReport", model);
        }

        [HttpPost]
        public ActionResult DepositReport(DepositReportViewModel model)
        {
            model.CurrentDate = DateTime.Now.ToLocalTime();
            model.SearchDTO = new DepositSummarySearchDTO(model.SearchDTO.DesiredDate);
            model.SummaryList = ddq.GetDepositSummaryList(model.SearchDTO.DesiredDate, eq.GetLocationList());
            model.ColumnTotalList = ddq.GetWeeklyReportColumnTotals(model.SearchDTO.DesiredDate);

            return View("DepositReport", model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}