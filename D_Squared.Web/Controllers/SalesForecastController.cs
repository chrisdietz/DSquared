using D_Squared.Data.Context;
using D_Squared.Data.Millers.Context;
using D_Squared.Data.Millers.Queries;
using D_Squared.Data.Queries;
using D_Squared.Domain.Entities;
using D_Squared.Domain.TransferObjects;
using D_Squared.Web.Helpers;
using D_Squared.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
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
        private readonly BudgetQueries bq;
        private readonly CodeQueries cq;

        private readonly SalesForecastInitializer init;

        public SalesForecastController()
        {
            db = new D_SquaredDbContext();
            f_db = new ForecastDataDbContext();
            sfq = new SalesForecastQueries(db, f_db);
            bq = new BudgetQueries(db);
            cq = new CodeQueries(db);

            e_db = new EmployeeDbContext();
            eq = new EmployeeQueries(e_db);

            init = new SalesForecastInitializer(sfq, bq, eq, cq);
        }


        public ActionResult Index()
        {
            string username = User.TruncatedName;

            if (!eq.EmployeeExists(username))
            {
                EmployeeErrorViewModel error = new EmployeeErrorViewModel
                {
                    Username = username
                };

                return View("../Home/EmployeeError", error);
            }
            else
            {
                SalesForecastViewModel model = init.InitializeSalesForecastEntryViewModel(username);

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        [MultipleButton(Name = "action", Argument = "Index")]
        public ActionResult Index(SalesForecastViewModel model)
        {
            string username = User.TruncatedName;

            try
            {
                string storeNumber = eq.GetStoreNumber(username);

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

            model = init.InitializeSalesForecastEntryViewModel(username, model.SelectedDateString);

            ModelState.Clear();

            return View("Index", model);
        }

        public ActionResult Search(SalesForecastSearchDTO searchDTO)
        {
            string username = User.TruncatedName;

            if (!eq.EmployeeExists(username))
            {
                EmployeeErrorViewModel error = new EmployeeErrorViewModel
                {
                    Username = username
                };

                return View("../Home/EmployeeError", error);
            }
            else
            {
                SalesForecastSearchViewModel model = new SalesForecastSearchViewModel();

                if (TempData["SearchDTO"] != null)
                    model = init.InitializeSalesForecastSearchViewModel((SalesForecastSearchDTO)TempData["SearchDTO"], username, User.IsRegionalManager(), User.IsDivisionalVP(), User.IsDSquaredAdmin());
                else
                    model = init.InitializeSalesForecastSearchViewModel(username, User.IsRegionalManager(), User.IsDivisionalVP(), User.IsDSquaredAdmin());

                return View("Search", model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(SalesForecastSearchViewModel model)
        {
            string username = User.TruncatedName;

            if (!eq.EmployeeExists(username))
            {
                EmployeeErrorViewModel error = new EmployeeErrorViewModel
                {
                    Username = username
                };

                return View("../Home/EmployeeError", error);
            }
            else
            {
                model.EmployeeInfo = eq.GetEmployeeInfo(username);
                model = init.InitializeSalesForecastSearchViewModel(model, User.IsRegionalManager(), User.IsDivisionalVP(), User.IsDSquaredAdmin());

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        [MultipleButton(Name = "action", Argument = "Refresh")]
        public ActionResult Refresh(SalesForecastViewModel model)
        {
            string username = User.TruncatedName;

            try
            {
                string storeNumber = eq.GetStoreNumber(username);

                if (ModelState.IsValid)
                {
                    sfq.RefreshSalesForecastData(model.Weekdays, storeNumber, User.Identity.Name);
                    Success("The Sales Forecasts for Restaurant: <u>" + model.EmployeeInfo.StoreNumber + "</u> have been refreshed and saved successfully. You may close this window");
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

            model = init.InitializeSalesForecastEntryViewModel(username, model.SelectedDateString);

            ModelState.Clear();

            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "action", Argument = "SearchDate")]
        public ActionResult SearchDate(SalesForecastViewModel model)
        {
            string username = User.TruncatedName;

            if (!eq.EmployeeExists(username))
            {
                EmployeeErrorViewModel error = new EmployeeErrorViewModel
                {
                    Username = username
                };

                return View("../Home/EmployeeError", error);
            }
            else
            {
                model = init.InitializeSalesForecastEntryViewModel(username, model.SelectedDateString);

                ModelState.Clear();

                return View("Index", model);
            }
        }

        [AuthorizeGroup(ROLES.GeneralManagerGroup)]
        public ActionResult SalesForecastReport()
        {
            string username = User.TruncatedName;
            if (!eq.EmployeeExists(username))
            {
                EmployeeErrorViewModel error = new EmployeeErrorViewModel
                {
                    Username = username
                };

                return View("EmployeeError", "Home", error);
            }
            else
            {
                SalesForecastReportViewModel model = init.InitializeSalesForecastReportViewModel();

                return View("SalesForecastReport", model);
            }

        }

        [HttpPost]
        [AuthorizeGroup(ROLES.GeneralManagerGroup)]
        public ActionResult SalesForecastReport(SalesForecastReportViewModel model)
        {
            model = init.InitializeSalesForecastReportViewModel(model);

            return View("SalesForecastReport", model);
        }

        public ActionResult Details(string date, string storeNumber)
        {
            string username = User.TruncatedName;

            SalesForecastDetailPartialViewModel partial = init.InitializeSalesForecastDetailPartialViewModel(username, date, storeNumber);

            return PartialView("~/Views/SalesForecast/_SalesForecastEntryDetail.cshtml", partial);
        }

        public ActionResult EditDetail(string date, string storeNumber)
        {
            string username = User.TruncatedName;

            SalesForecastEditDetailPartialViewModel partial = init.InitializeSalesForecastEditDetailPartialViewModel(date, storeNumber, username);

            return PartialView("~/Views/SalesForecast/_SalesForecastEditDetail.cshtml", partial);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDetail(SalesForecastEditDetailPartialViewModel model)
        {
            string username = User.TruncatedName;

            try
            {
                if (ModelState.IsValid)
                {
                    sfq.AddOrUpdateSalesForecasts(model.Weekdays, model.StoreNumber, username);
                    Success("Sales Forecast records saved successfully. You may close this window");
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

                return RedirectToAction("Search");
            }

            SalesForecastSearchDTO searchDTO = new SalesForecastSearchDTO()
            {
                LocationId = model.StoreNumber,
                StartDate = model.Weekdays.FirstOrDefault().DateOfEntry,
                EndDate = model.Weekdays.LastOrDefault().DateOfEntry
            };

            TempData["SearchDTO"] = searchDTO;
            return RedirectToAction("Search");
            //return Search(searchDTO);
        }

        //public ActionResult Edit(int id = 0)
        //{
        //    string username = User.TruncatedName;

        //    SalesForecastCreateEditPartialViewModel partial = init.InitializeSalesForecastEditViewModel(id, username);

        //    return PartialView("~/Views/SalesForecast/_Edit.cshtml", partial);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SalesForecastCreateEditPartialViewModel model)
        {
            string username = User.TruncatedName;

            try
            {
                if (ModelState.IsValid)
                {
                    sfq.UpdateSalesForecast(model.SalesForecast, User.Identity.Name);
                    Success("Sales Forecast record saved successfully. You may close this window");
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

                return RedirectToAction("Search");
            }

            return RedirectToAction("Search");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                e_db.Dispose();
                f_db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}