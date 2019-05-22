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
using ROLES = D_Squared.Domain.DomainConstants.RoleNames;

namespace D_Squared.Web.Controllers
{
    public class RedbookController : BaseController
    {
        private readonly D_SquaredDbContext db;
        private readonly ForecastDataDbContext f_db;

        private readonly RedbookEntryQueries rbeq;
        private readonly CodeQueries cq;
        private readonly SalesForecastQueries sfq;
        private readonly BudgetQueries bq;
        private readonly SalesDataQueries sd;

        private readonly RedbookEntryInitializer init;
        private readonly SalesForecastInitializer s_init;

        public RedbookController()
        {
            db = new D_SquaredDbContext();
            f_db = new ForecastDataDbContext();

            rbeq = new RedbookEntryQueries(db);
            cq = new CodeQueries(db);
            sfq = new SalesForecastQueries(db, f_db);
            bq = new BudgetQueries(db);
            sd = new SalesDataQueries(db);

            init = new RedbookEntryInitializer(rbeq, cq, sfq, sd, eq);
            s_init = new SalesForecastInitializer(sfq, bq, eq, cq);
        }

        // GET: RedbookEntry
        public ActionResult Entry(string selectedDate)
        {
            string username = User.TruncatedName;

            EmployeeDTO employee = eq.GetEmployeeInfo(username);

            if (selectedDate == null)
            {
                RedbookEntryBaseViewModel model = init.InitializeBaseViewModel(DateTime.Today.ToLocalTime().ToShortDateString(), employee.StoreNumber, username);

                return View(model);
            }
            else
            {
                RedbookEntryBaseViewModel model = init.InitializeBaseViewModel(selectedDate, employee.StoreNumber, username);

                return View(model);
            }
            
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        [MultipleButton(Name = "action", Argument = "Entry")]
        public ActionResult Entry(RedbookEntryBaseViewModel model)
        {
            model = init.InitializeBaseViewModel(model.SelectedDateString, model.SelectedLocation, User.TruncatedName);
            ModelState.Clear();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        [MultipleButton(Name = "action", Argument = "EntrySave")]
        public ActionResult EntrySave(RedbookEntryBaseViewModel model)
        {
            string username = User.TruncatedName;

            try
            {
                //bind 
                model.RedbookEntry = init.BindPostValuesToEntity(model.RedbookEntry, model.SelectedDateString, model.SelectedLocation);

                //manually verify model state
                if (model.RedbookEntry.BusinessDate != default(DateTime))
                    ModelState.Remove("RedbookEntry.BusinessDate");

                if (!string.IsNullOrEmpty(model.RedbookEntry.LocationId) && eq.GetLocationList().Contains(model.RedbookEntry.LocationId))
                    ModelState.Remove("RedbookEntry.LocationId");

                if (ModelState.IsValid)
                {
                    model.SalesDataDTO = init.InitializeSalesDataDTO(model.RedbookEntry.LocationId);
                    model.RedbookEntry.Sales = model.SalesDataDTO.Sales;
                    model.RedbookEntry.Discounts = model.SalesDataDTO.Discounts;
                    model.RedbookEntry.Checks = model.SalesDataDTO.Checks;

                    rbeq.SaveRedbookEntry(model.RedbookEntry, model.EventDTOs, username);
                    Success("The Redbook for Restaurant: <u>" + model.RedbookEntry.LocationId + "</u> and Date: <u>" + model.RedbookEntry.BusinessDate.ToShortDateString() + "</u> has been saved successfully. You may close this window");
                }
                else
                {
                    Warning("Error Occured: Invalid Model State. If this error persists, please contact an administrator.");
                    return RedirectToAction("Entry");
                }
            }
            catch
            {
                Warning("Internal Error occurred. If this error persists, please contact an administrator.");

                return RedirectToAction("Entry");
            }

            //only success reaches this far
            //reinit model
            model = init.InitializeBaseViewModel(model, username);

            return View("Entry", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        [MultipleButton(Name = "action", Argument = "EntrySubmit")]
        public ActionResult EntrySubmit(RedbookEntryBaseViewModel model)
        {
            string username = User.TruncatedName;

            try
            {
                //bind 
                model.RedbookEntry = init.BindPostValuesToEntity(model.RedbookEntry, model.SelectedDateString, model.SelectedLocation);

                //manually verify model state
                if (model.RedbookEntry.BusinessDate != default(DateTime))
                    ModelState.Remove("RedbookEntry.BusinessDate");

                if (!string.IsNullOrEmpty(model.RedbookEntry.LocationId) && eq.GetLocationList().Contains(model.RedbookEntry.LocationId))
                    ModelState.Remove("RedbookEntry.LocationId");

                if (ModelState.IsValid)
                {
                    // Set Sales and discounts data in RedbookEntry
                    model.SalesDataDTO = init.InitializeSalesDataDTO(model.RedbookEntry.LocationId);
                    model.RedbookEntry.Sales = model.SalesDataDTO.Sales;
                    model.RedbookEntry.Discounts = model.SalesDataDTO.Discounts;
                    model.RedbookEntry.Checks = model.SalesDataDTO.Checks;

                    SalesForecastExportDTO dto = s_init.GetSalesForecastExportDTO(username, model.SelectedDateString);
                    rbeq.SubmitRedbookEntry(model.RedbookEntry, model.EventDTOs, dto, username);
                    Success("The Redbook for Restaurant: <u>" + model.RedbookEntry.LocationId + "</u> and Date: <u>" + model.RedbookEntry.BusinessDate.ToShortDateString() + "</u> has been submitted successfully. You may close this window");
                }
                else
                {
                    Warning("Error Occured: Invalid Model State. If this error persists, please contact an administrator.");
                    return RedirectToAction("Entry");
                }
            }
            catch (Exception e)
            {
                Warning("Internal Error occurred. If this error persists, please contact an administrator.\n"
                            + "Error Details: " + e.Message + "---" + e.InnerException.Message);

                return RedirectToAction("Entry");
            }

            return RedirectToAction("Entry", "Redbook", new { selectedDate = model.RedbookEntry.BusinessDate.ToShortDateString() });
        }

        public ActionResult CreateCompetitiveEvent(int redbookId)
        {
            CompetitiveEventCreateEditViewModel partial = init.InitializeCompetitiveEventCreateEditViewModel(redbookId);

            return PartialView("~/Views/Redbook/_CompetitiveEventCreate.cshtml", partial);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CompetitiveEventCreate(CompetitiveEventCreateEditViewModel model)
        {
            string username = User.TruncatedName;

            if (ModelState.IsValid)
            {
                rbeq.SaveCompetitiveEvent(model.CompetitiveEvent, username);
                Success("Successfully added Competitive Event!");
            }
            else
            {
                Warning("Error Occured: Invalid Model State. If this error persists, please contact an administrator.");
            }

            return RedirectToAction("Entry", "Redbook", new { selectedDate = model.RedbookDate.ToShortDateString() });
        }

        public ActionResult Details(int redbookId, bool isLastYear, string date)
        {
            string username = User.TruncatedName;

            RedbookEntryDetailPartialViewModel partial = init.InitializeRedbookEntryDetailPartialViewModel(redbookId, username, isLastYear, date);

            return PartialView("~/Views/Redbook/_RedbookEntryDetail.cshtml", partial);
        }

        public ActionResult Index()
        {
            RedbookEntrySearchViewModel model = init.InitializeRedbookEntrySearchViewModel(eq.GetEmployeeInfo(User.TruncatedName), User.IsRegionalManager(), User.IsDivisionalVP(), User.IsDSquaredAdmin());

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(RedbookEntrySearchViewModel model)
        {
            model.EmployeeInfo = eq.GetEmployeeInfo(User.TruncatedName);
            model = init.InitializeRedbookEntrySearchViewModel(model, User.IsRegionalManager(), User.IsDivisionalVP(), User.IsDSquaredAdmin());
            model.SearchResults = rbeq.GetRedbookEntries(model.SearchViewModel.SearchDTO, model.SearchViewModel.LocationSelectList.Select(l => l.Text.Substring(0, 3)).ToList());

            return View(model);
        }

        public ActionResult UpdateSearchPartialDropdowns(string lId, string mAM, string mPM)
        {
            RedbookEntrySearchPartialViewModel partialModel = init.FilterDropdownLists(eq.GetEmployeeInfo(User.TruncatedName), lId, mAM, mPM, User.IsRegionalManager(), User.IsDivisionalVP(), User.IsDSquaredAdmin());

            return PartialView("~/Views/Redbook/_SearchPartial.cshtml", partialModel);
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