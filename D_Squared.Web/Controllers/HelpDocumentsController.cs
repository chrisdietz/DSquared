using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using D_Squared.Data.Context;
using D_Squared.Domain.Entities;
using D_Squared.Data.Queries;
using D_Squared.Web.Models;
using D_Squared.Web.Helpers;
using ROLES = D_Squared.Domain.DomainConstants.RoleNames;

namespace D_Squared.Web.Controllers
{
    [AuthorizeGroup(ROLES.DSquaredAdminGroup)]
    public class HelpDocumentsController : BaseController
    {
        private readonly D_SquaredDbContext db;
        private readonly CodeQueries cq;
        private readonly HelpDocumentQueries hdq;

        public HelpDocumentsController()
        {
            db = new D_SquaredDbContext();
            cq = new CodeQueries(db);
            hdq = new HelpDocumentQueries(db);
        }

        // GET: HelpDocuments
        public ActionResult Index()
        {
            return View(db.HelpDocuments.ToList());
        }

        // GET: HelpDocuments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HelpDocument helpDocument = db.HelpDocuments.Find(id);
            if (helpDocument == null)
            {
                Error("Error: Unable to locate the information!");
            }
            return View(helpDocument);
        }

        // GET: HelpDocuments/Create
        public ActionResult Create()
        {
            HelpDocumentCreateViewModel model = new HelpDocumentCreateViewModel()
            {
                ControllerList = new SelectList(cq.GetDistinctListByCodeCategory("Controller")),
                ActionList = new SelectList(cq.GetDistinctListByCodeCategory("Action")),
                HelpDocument = new HelpDocument()
            };

            return View(model);
        }

        // POST: HelpDocuments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HelpDocumentCreateViewModel model)
        {
            if(hdq.CheckForExisitingHelpDocument(model.SelectedAction, model.SelectedController))
            {
                HelpDocument existingDocument = hdq.GetHelpDocument(model.SelectedAction, model.SelectedController);

                Warning("This help document already exists -- you have been redirected to the edit page.");
                return RedirectToAction("Edit", new { id = existingDocument.Id });
            }
            else
            {
                model.HelpDocument = new HelpDocument();

                if (ModelState.IsValid)
                {
                    model.HelpDocument.ControllerName = model.SelectedController;
                    model.HelpDocument.ActionName = model.SelectedAction;
                    model.HelpDocument.HelpHtml = Server.HtmlEncode(model.HelpHtml);
                    model.HelpDocument.CreatedBy = User.Identity.Name;
                    model.HelpDocument.CreatedDate = DateTime.Now;
                    model.HelpDocument.UpdatedBy = User.Identity.Name;
                    model.HelpDocument.UpdatedDate = DateTime.Now;
                    db.HelpDocuments.Add(model.HelpDocument);
                    db.SaveChanges();

                    Success("Success:  Your information was saved!");
                    return RedirectToAction("Index");
                }

                model.ActionList = new SelectList(cq.GetDistinctListByCodeCategory("Action"));
                model.ControllerList = new SelectList(cq.GetDistinctListByCodeCategory("Controller"));

                Error("Error: Unable to save the information, please check the values entered!");
                return View(model);
            }   
        }

        // GET: HelpDocuments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HelpDocument helpDocument = db.HelpDocuments.Find(id);
            if (helpDocument == null)
            {
                Error("Error: Unable to locate the information!");
                return RedirectToAction("Index");
            }

            HelpDocumentCreateViewModel model = new HelpDocumentCreateViewModel()
            {
                ControllerList = new SelectList(cq.GetDistinctListByCodeCategory("Controller")),
                ActionList = new SelectList(cq.GetDistinctListByCodeCategory("Action")),
                HelpDocument = helpDocument,
                SelectedAction = helpDocument.ActionName,
                SelectedController = helpDocument.ControllerName,
                HelpHtml = Server.HtmlDecode(helpDocument.HelpHtml)
            };

            return View(model);
        }

        // POST: HelpDocuments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HelpDocumentCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.HelpDocument.ControllerName = model.SelectedController;
                model.HelpDocument.ActionName = model.SelectedAction;
                model.HelpDocument.HelpHtml = Server.HtmlEncode(model.HelpHtml);
                model.HelpDocument.UpdatedBy = User.Identity.Name;
                model.HelpDocument.UpdatedDate = DateTime.Now;
                db.Entry(model.HelpDocument).State = EntityState.Modified;
                db.SaveChanges();

                Success("Success:  Your information was saved!");
                return RedirectToAction("Index");
            }

            model.ActionList = new SelectList(cq.GetDistinctListByCodeCategory("Action"));
            model.ControllerList = new SelectList(cq.GetDistinctListByCodeCategory("Controller"));

            Error("Error: Unable to save the information, please check the values entered!");
            return View(model);
        }

        // GET: HelpDocuments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HelpDocument helpDocument = db.HelpDocuments.Find(id);
            if (helpDocument == null)
            {
                Error("Error: Unable to locate the information!");
                return RedirectToAction("Index");
            }
            return View(helpDocument);
        }

        // POST: HelpDocuments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HelpDocument helpDocument = db.HelpDocuments.Find(id);
            db.HelpDocuments.Remove(helpDocument);
            db.SaveChanges();

            Warning("The information was successfully deleted.");
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
