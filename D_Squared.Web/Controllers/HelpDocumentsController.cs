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

namespace D_Squared.Web.Controllers
{
    public class HelpDocumentsController : BaseController
    {
        private D_SquaredDbContext db = new D_SquaredDbContext();

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
                return HttpNotFound();
            }
            return View(helpDocument);
        }

        public ActionResult ModalDetails(string controller, string action)
        {
            HelpDocument helpDocument = new HelpDocument();

            if (db.HelpDocuments.Any(hd => hd.ControllerName == controller && hd.ActionName == action))
            {
                helpDocument = db.HelpDocuments.Where(hd => hd.ControllerName == controller && hd.ActionName == action).FirstOrDefault();
                return PartialView("_DetailModal", helpDocument);
            }            
            else
            {
                helpDocument = new HelpDocument
                {
                    ControllerName = controller,
                    ActionName = action,
                    HelpHtml = "&lt;h4&gt;No Help Document was found for this resource.&lt;/h4&gt;"
                };

                return PartialView("_DetailModal", helpDocument);
            }
        }

        // GET: HelpDocuments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HelpDocuments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HelpDocument helpDocument)
        {
            if (ModelState.IsValid)
            {
                helpDocument.HelpHtml = Server.HtmlEncode(helpDocument.HelpHtml);
                db.HelpDocuments.Add(helpDocument);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(helpDocument);
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
                return HttpNotFound();
            }
            return View(helpDocument);
        }

        // POST: HelpDocuments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HelpDocument helpDocument)
        {
            if (ModelState.IsValid)
            {
                helpDocument.HelpHtml = Server.HtmlEncode(helpDocument.HelpHtml);
                db.Entry(helpDocument).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(helpDocument);
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
                return HttpNotFound();
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
