using D_Squared.Data.Context;
using D_Squared.Data.Employees.Context;
using D_Squared.Data.Employees.Queries;
using D_Squared.Data.Queries;
using D_Squared.Domain.TransferObjects;
using D_Squared.Web.Helpers;
using D_Squared.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace D_Squared.Web.Controllers
{
    public class SalesForecastController : BaseController
    {
        private readonly D_SquaredDbContext db;
        private readonly EmployeeDbContext e_db;
        private readonly SalesForecastQueries sfq;
        private readonly EmployeeQueries eq;

        public SalesForecastController()
        {
            db = new D_SquaredDbContext();
            sfq = new SalesForecastQueries(db);

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

                return View("EmployeeError", "Home", error);
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
        public ActionResult Index(SalesForecastViewModel model)
        {
            try
            {
                string userName = User.Identity.Name.Substring(User.Identity.Name.IndexOf('\\') + 1);
                string storeNumber = eq.GetStoreNumber(userName);

                sfq.AddOrUpdateSalesForecasts(model.Weekdays, storeNumber, User.Identity.Name);
            }
            catch
            {
                Warning("Error occurred. If this error persists, please contact an administrator.");

                return RedirectToAction("Index");
            }

            Success("The Sales Forecasts for Restaurant: <u>" + model.EmployeeInfo.StoreNumber + "</u> have been saved successfully. You may close this window");

            return RedirectToAction("Index");
        }
    }
}