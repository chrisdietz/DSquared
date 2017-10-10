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
                List<DepositEntryDTO> weekdays = ddq.GetCurrentWeekAsDepositEntryDTOList(DateTime.Today, employee.StoreNumber);

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

                ddq.AddOrUpdateDeposits(model.Weekdays, storeNumber, userName);
            }
            catch (Exception e)
            {
                Warning("Error occurred. If this error persists, please contact an administrator.");

                return RedirectToAction("Index");
            }

            Success("Data was saved successfully!");
            return RedirectToAction("Index");
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