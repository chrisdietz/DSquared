using D_Squared.Data.Context;
using D_Squared.Data.Queries;
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
    public class SalesReportingController : BaseController
    {
        private readonly D_SquaredDbContext db;
        private readonly SalesDataQueries sdq;
        private readonly SalesReportingInitializer init;

        public SalesReportingController()
        {
            db = new D_SquaredDbContext();
            sdq = new SalesDataQueries(db);

            init = new SalesReportingInitializer(eq, sdq);
        }

        public ActionResult Index()
        {
            string username = User.TruncatedName;
            EmployeeDTO employee = eq.GetEmployeeInfo(username);
            OvertimeReportingSearchViewModel model = new OvertimeReportingSearchViewModel() { EmployeeInfo = employee };
            return View(model);
        }
        // GET: 
        public ActionResult OvertimeReport(WeeklyTotalDurationSearchDTO searchDTO)
        {
            string username = User.TruncatedName;
            //DateTime selectedDate = DateTime.Today;
            EmployeeDTO employee = eq.GetEmployeeInfo(username);
            OvertimeReportingSearchViewModel model = init.InitializeOvertimeReportingSearchViewModel(User, searchDTO);
            model.SearchDTO = searchDTO;

            return View(model);
        }
        
    }
}