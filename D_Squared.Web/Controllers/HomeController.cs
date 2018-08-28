﻿using D_Squared.Data.Context;
using D_Squared.Data.Millers.Context;
using D_Squared.Data.Millers.Queries;
using D_Squared.Data.Queries;
using D_Squared.Domain.TransferObjects;
using D_Squared.Web.Helpers;
using D_Squared.Web.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using D_Squared.Domain.Entities;

namespace D_Squared.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly D_SquaredDbContext db;
        private readonly EmployeeDbContext e_db;

        private readonly EmployeeQueries eq;

        public HomeController()
        {
            db = new D_SquaredDbContext();
            e_db = new EmployeeDbContext();

            eq = new EmployeeQueries(e_db);
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
                HomeViewModel model = new HomeViewModel(eq.GetEmployeeInfo(username));
                return View(model);
            }
        }

        //refactor to use a helpdocuments query class function instead of direct db call
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
    }
}