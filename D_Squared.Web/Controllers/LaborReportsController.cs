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
    public class LaborReportsController : BaseController
    {
        private readonly D_SquaredDbContext db;
        private readonly LaborDataQueries ldq;
        private readonly LaborReportsInitializer init;

        public LaborReportsController()
        {
            db = new D_SquaredDbContext();
            ldq = new LaborDataQueries(db);

            init = new LaborReportsInitializer(eq, ldq);
        }

        // GET: LaborReports
        public ActionResult Index()
        {
            string username = User.TruncatedName;
            EmployeeDTO employee = eq.GetEmployeeInfo(username);
            OvertimeReportingSearchViewModel model = new OvertimeReportingSearchViewModel() { EmployeeInfo = employee };
            return View(model);
        }
        public ActionResult OvertimeReport(WeeklyTotalDurationSearchDTO searchDTO)
        {
            string username = User.TruncatedName;
            EmployeeDTO employee = eq.GetEmployeeInfo(username);
            OvertimeReportingSearchViewModel model = init.InitializeOvertimeReportingSearchViewModel(User, searchDTO);
            model.SearchDTO = searchDTO;

            return View(model);
        }

        public ActionResult LaborSummary(LaborDataSearchDTO searchDTO)
        {
            string username = User.TruncatedName;
            EmployeeDTO employee = eq.GetEmployeeInfo(username);
            
            LaborSummarySearchViewModel model = init.InitializeLaborSummarySearchViewModel(User, searchDTO);
            model.SearchDTO = searchDTO;

            return View(model);
        }

    }
}