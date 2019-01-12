using D_Squared.Data.Context;
using D_Squared.Data.Millers.Context;
using D_Squared.Data.Millers.Queries;
using D_Squared.Data.Queries;
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
    [AuthorizeGroup(ROLES.DSquaredAdminGroup)]
    public class AdminController : BaseController
    {
        private readonly D_SquaredDbContext db;
        private readonly EmployeeDbContext e_db;

        private readonly EmployeeQueries eq;
        private readonly CodeQueries cq;
        private readonly RedbookEntryQueries rbeq;

        public AdminController()
        {
            db = new D_SquaredDbContext();
            e_db = new EmployeeDbContext();

            eq = new EmployeeQueries(e_db);
            rbeq = new RedbookEntryQueries(db);
            cq = new CodeQueries(db);
        }

        // GET: Admin
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
                AdminViewModel model = new AdminViewModel(eq.GetEmployeeInfo(username));
                return View(model);
            }
        }

        public ActionResult ConvertEventsAffectingSales()
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
                rbeq.AdminConvertRedbookEventsToChildTable(username);

                Success("Successfully converted all Redbook JSON column 'SelectedEvents' values to records in 'RedbookSalesEvents' child table");

                AdminViewModel model = new AdminViewModel(eq.GetEmployeeInfo(username));
                return View("Index", model);
            }
        }
    }
}