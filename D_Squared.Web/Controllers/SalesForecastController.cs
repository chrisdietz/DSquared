﻿using D_Squared.Data.Context;
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
                DateTime thursday = weekdays.Where(w => w.DayOfWeek == "Thursday").FirstOrDefault().DateOfEntry;

                SalesForecastViewModel model = new SalesForecastViewModel(weekdays, today, employee, today.ToShortDateString(), bq.GetBudgetByDate(thursday, employee.StoreNumber), bq.GetFY18Budgets(employee.StoreNumber), eq.GetAllValidStoreLocations());
                sfq.RefreshSalesForecastData(model.Weekdays, employee.StoreNumber, User.Identity.Name);

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        [MultipleButton(Name = "action", Argument = "Index")]
        public ActionResult Index(SalesForecastViewModel model)
        {
            string userName = User.TruncatedName;

            try
            {
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

            DateTime.TryParse(model.SelectedDateString, out DateTime convertedSelectedDate);

            EmployeeDTO employee = eq.GetEmployeeInfo(userName);
            List<SalesForecastDTO> weekdays = sfq.GetSpecificWeekAsSalesForecastDTOList(convertedSelectedDate, employee.StoreNumber);
            DateTime thursday = weekdays.Where(w => w.DayOfWeek == "Thursday").FirstOrDefault().DateOfEntry;

            model = new SalesForecastViewModel(weekdays, DateTime.Today.ToLocalTime(), employee, model.SelectedDateString, bq.GetBudgetByDate(thursday, employee.StoreNumber), bq.GetFY18Budgets(employee.StoreNumber), eq.GetAllValidStoreLocations());

            ModelState.Clear();

            return View("Index", model);
        }

        public ActionResult Search()
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
                SalesForecastSearchViewModel model = init.InitializeSalesForecastSearchViewModel(username, User.IsRegionalManager(), User.IsDivisionalVP(), User.IsDSquaredAdmin());

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(SalesForecastSearchViewModel model)
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
                model.EmployeeInfo = eq.GetEmployeeInfo(username);
                model = init.InitializeSalesForecastSearchViewModel(model, User.IsRegionalManager(), User.IsDivisionalVP(), User.IsDSquaredAdmin());
                model.SearchResults = sfq.GetSalesForecastEntries(model.SearchViewModel.SearchDTO, model.SearchViewModel.LocationSelectList.Select(l => l.Text.Substring(0, 3)).ToList());

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        [MultipleButton(Name = "action", Argument = "Refresh")]
        public ActionResult Refresh(SalesForecastViewModel model)
        {
            string userName = User.TruncatedName;

            try
            {
                string storeNumber = eq.GetStoreNumber(userName);

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

            DateTime.TryParse(model.SelectedDateString, out DateTime convertedSelectedDate);

            EmployeeDTO employee = eq.GetEmployeeInfo(userName);
            List<SalesForecastDTO> weekdays = sfq.GetSpecificWeekAsSalesForecastDTOList(convertedSelectedDate, employee.StoreNumber);
            DateTime thursday = weekdays.Where(w => w.DayOfWeek == "Thursday").FirstOrDefault().DateOfEntry;

            model = new SalesForecastViewModel(weekdays, DateTime.Today.ToLocalTime(), employee, model.SelectedDateString, bq.GetBudgetByDate(thursday, employee.StoreNumber), bq.GetFY18Budgets(employee.StoreNumber), eq.GetAllValidStoreLocations());

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
                ErrorViewModel error = new ErrorViewModel
                {
                    Username = username
                };

                return View("../Home/EmployeeError", error);
            }
            else
            {
                DateTime.TryParse(model.SelectedDateString, out DateTime convertedSelectedDate);

                EmployeeDTO employee = eq.GetEmployeeInfo(username);
                List<SalesForecastDTO> weekdays = sfq.GetSpecificWeekAsSalesForecastDTOList(convertedSelectedDate, employee.StoreNumber);
                DateTime thursday = weekdays.Where(w => w.DayOfWeek == "Thursday").FirstOrDefault().DateOfEntry;

                model = new SalesForecastViewModel(weekdays, DateTime.Today.ToLocalTime(), employee, model.SelectedDateString, bq.GetBudgetByDate(thursday, employee.StoreNumber), bq.GetFY18Budgets(employee.StoreNumber), eq.GetAllValidStoreLocations());
                sfq.RefreshSalesForecastData(model.Weekdays, employee.StoreNumber, User.Identity.Name);

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
                f_db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}