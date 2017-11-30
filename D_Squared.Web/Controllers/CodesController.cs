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
using D_Squared.Web.Helpers;
using ROLES = D_Squared.Domain.DomainConstants.RoleNames;

namespace D_Squared.Web.Controllers
{
    [AuthorizeGroup(ROLES.GeneralManagerGroup)]
    public class CodesController : BaseController
    {
        private D_SquaredDbContext db = new D_SquaredDbContext();

        // GET: Codes
        public ActionResult Index()
        {
            return View(db.Codes.ToList());
        }

        // GET: Codes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Code code = db.Codes.Find(id);
            if (code == null)
            {
                Error("Error: Unable to locate the information!");
                return RedirectToAction("Index");
            }
            return View(code);
        }

        // GET: Codes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Codes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Code code)
        {
            if (ModelState.IsValid)
            {
                code.CreatedBy = User.Identity.Name;
                code.CreatedDate = DateTime.Now;
                code.UpdatedBy = User.Identity.Name;
                code.UpdatedDate = DateTime.Now;
                db.Codes.Add(code);
                db.SaveChanges();

                Success("Success:  Your information was saved!");
                return RedirectToAction("Index");
            }

            Error("Error: Unable to save the information, please check the values entered!");
            return View(code);
        }

        // GET: Codes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Code code = db.Codes.Find(id);
            if (code == null)
            {
                Error("Error: Unable to locate the information!");
                return RedirectToAction("Index");
            }
            return View(code);
        }

        // POST: Codes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Code code)
        {
            if (ModelState.IsValid)
            {
                code.UpdatedBy = User.Identity.Name;
                code.UpdatedDate = DateTime.Now;
                db.Entry(code).State = EntityState.Modified;
                db.SaveChanges();

                Success("Success:  Your information was saved!");
                return RedirectToAction("Index");
            }

            Error("Error: Unable to save the information, please check the values entered!");
            return View(code);
        }

        // GET: Codes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Code code = db.Codes.Find(id);
            if (code == null)
            {
                Error("Error: Unable to locate the information!");
                return RedirectToAction("Index");
            }

            return View(code);
        }

        // POST: Codes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Code code = db.Codes.Find(id);
            db.Codes.Remove(code);
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
