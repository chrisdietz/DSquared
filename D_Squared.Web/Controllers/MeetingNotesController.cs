using D_Squared.Data.Context;
using D_Squared.Data.Queries;
using D_Squared.Domain.TransferObjects;
using D_Squared.Web.Helpers;
using D_Squared.Web.Models;
using System.Web.Mvc;

namespace D_Squared.Web.Controllers
{
    public class MeetingNotesController : BaseController
    {
        private readonly D_SquaredDbContext db;
        private readonly StoreManagerQueries smq;
        private readonly MeetingNotesInitializer init;
        public MeetingNotesController()
        {
            db = new D_SquaredDbContext();
            smq = new StoreManagerQueries(db);
            init = new MeetingNotesInitializer(smq, eq);
        }
        // GET: MeetingNotes
        public ActionResult NotesEntry()
        {
            string username = User.TruncatedName;

            EmployeeDTO employee = eq.GetEmployeeInfo(username);

            var model = init.InitializeMeetingNotesBaseViewModel(User.TruncatedName);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "action", Argument = "CreateNewNotes")]
        public ActionResult CreateNewNotes()
        {
            string username = User.TruncatedName;

            EmployeeDTO employee = eq.GetEmployeeInfo(username);

            var model = init.InitializeMeetingNotesBaseViewModel(User.TruncatedName, true);

            return View("NotesEntry", model);
        }

        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        [MultipleButton(Name = "action", Argument = "NotesEntrySave")]
        public ActionResult NotesEntrySave(MeetingNotesBaseViewModel model)
        {
            string username = User.TruncatedName;
 
            try
            {
                EmployeeDTO employee = eq.GetEmployeeInfo(username);
                model.NotesDTO.Store = employee.StoreNumber;
                if (ModelState.IsValid)
                {
                    if(model.NotesDTO.ID > 0)
                    {
                        smq.UpdateMeetingNotes(model.NotesDTO, username);
                    }
                    else
                    {
                        smq.InsertMeetingNotes(model.NotesDTO, username);
                    }
                    Success("Meeting Notes has been saved successfully. You may close this message");
                }
                else
                {
                    Warning("Error Occured: Invalid Model State. If this error persists, please contact an administrator.");
                    return RedirectToAction("NotesEntry");
                }
            }
            catch
            {
                Warning("Internal Error occurred. If this error persists, please contact an administrator.");

                return RedirectToAction("Entry");
            }

            //only success reaches this far
            //reinit model

            return Redirect("NotesEntry");
        }

        public ActionResult PreviousWeek(string actionName) => RedirectToAction(actionName, new { isLastWeek = true });

        public ActionResult NotesView(bool isLastWeek = false) => View(init.InitializeMeetingNotesListViewModel(User, isLastWeek));

        public ActionResult HuddleNotesView(string store) => View("HuddleNotesView", init.InitializeMostRecentMeetingNotesViewModel(store));
    }
}