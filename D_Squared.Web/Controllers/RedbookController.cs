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

namespace D_Squared.Web.Controllers
{
    public class RedbookController : BaseController
    {
        private readonly D_SquaredDbContext db;
        private readonly EmployeeDbContext e_db;
        private readonly ForecastDataDbContext f_db;

        private readonly RedbookEntryQueries rbeq;
        private readonly CodeQueries cq;
        private readonly SalesForecastQueries sfq;
        private readonly EmployeeQueries eq;

        private readonly RedbookEntryInitializer init;

        public RedbookController()
        {
            db = new D_SquaredDbContext();
            e_db = new EmployeeDbContext();
            f_db = new ForecastDataDbContext();

            rbeq = new RedbookEntryQueries(db);
            cq = new CodeQueries(db);
            sfq = new SalesForecastQueries(db, f_db);
            eq = new EmployeeQueries(e_db);

            init = new RedbookEntryInitializer(rbeq, cq, sfq, eq);
        }

        // GET: RedbookEntry
        public ActionResult Entry(string selectedDate)
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
                EmployeeDTO employee = eq.GetEmployeeInfo(username);

                if(selectedDate == null)
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
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        [MultipleButton(Name = "action", Argument = "Entry")]
        public ActionResult Entry(RedbookEntryBaseViewModel model)
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
                model = init.InitializeBaseViewModel(model.SelectedDateString, model.SelectedLocation, username);
                ModelState.Clear();

                return View(model);
            }
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
                model.RedbookEntry = init.BindPostValuesToEntity(model.RedbookEntry, model.EventDTOs, model.SelectedDateString, model.SelectedLocation);

                //manually verify model state
                if (model.RedbookEntry.BusinessDate != default(DateTime))
                    ModelState.Remove("RedbookEntry.BusinessDate");

                if (!string.IsNullOrEmpty(model.RedbookEntry.LocationId) && eq.GetLocationList().Contains(model.RedbookEntry.LocationId))
                    ModelState.Remove("RedbookEntry.LocationId");

                if (ModelState.IsValid)
                {
                    rbeq.SaveRedbookEntry(model.RedbookEntry, username);
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
                model.RedbookEntry = init.BindPostValuesToEntity(model.RedbookEntry, model.EventDTOs, model.SelectedDateString, model.SelectedLocation);

                //manually verify model state
                if (model.RedbookEntry.BusinessDate != default(DateTime))
                    ModelState.Remove("RedbookEntry.BusinessDate");

                if (!string.IsNullOrEmpty(model.RedbookEntry.LocationId) && eq.GetLocationList().Contains(model.RedbookEntry.LocationId))
                    ModelState.Remove("RedbookEntry.LocationId");

                if (ModelState.IsValid)
                {
                    rbeq.SubmitRedbookEntry(model.RedbookEntry, username);
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

            return RedirectToAction("Entry", "Redbook", model.RedbookEntry.BusinessDate.ToShortDateString());
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

            return RedirectToAction("Entry", "Redbook", model.RedbookDate.ToShortDateString());
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