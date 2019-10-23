using D_Squared.Data.Millers.Context;
using D_Squared.Data.Millers.Queries;
using D_Squared.Web.Helpers;
using D_Squared.Web.Models;
using NLog;
using System;
using System.Web.Mvc;

namespace D_Squared.Web.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        internal static Logger logger = LogManager.GetCurrentClassLogger();
        protected readonly EmployeeDbContext e_db;
        protected readonly EmployeeQueries eq;
        public BaseController()
        {
            e_db = new EmployeeDbContext();
            eq = new EmployeeQueries(e_db);
        }
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            string username = User.TruncatedName;
            bool userAuthenticated = User.Identity.IsAuthenticated;
            if (!eq.EmployeeExists(username))
            {
                ErrorViewModel error = new ErrorViewModel(null, null, null)
                {
                    Username = username
                };

                filterContext.Result = View("../Home/EmployeeError", error);
            }
        }
        protected override void OnException(ExceptionContext exceptionContext)
        {
            if (!exceptionContext.ExceptionHandled)
            {
                string controllerName = (string)exceptionContext.RouteData.Values["controller"];
                string actionName = (string)exceptionContext.RouteData.Values["action"];

                ErrorViewModel model = new ErrorViewModel(exceptionContext.Exception, controllerName, actionName);
                model.ErrorGuid = Guid.NewGuid();
                model.ErrorTimeStamp = DateTime.Now;

                exceptionContext.Result = new ViewResult
                {
                    ViewName = "~/Views/Shared/Error.cshtml",
                    ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
                    TempData = exceptionContext.Controller.TempData
                };

                exceptionContext.ExceptionHandled = true;
                string message = string.Join(";", new string[] {
                                                                $"Error GUID = {model.ErrorGuid.ToString()}",
                                                                $"Error TimeStamp = {model.ErrorTimeStamp}",
                                                                $"Controller Name = {controllerName}",
                                                                $"Action Name = {actionName}",
                                                                $"Error Message = { exceptionContext.Exception.Message }"
                                                                });
                logger.Error(exceptionContext.Exception, message);
            }
        }

        protected virtual new CustomClaimsPrincipal User => HttpContext.User as CustomClaimsPrincipal;
        //protected virtual new CustomClaimsPrincipal User
        //{
        //    get { return HttpContext.User as CustomClaimsPrincipal; }
        //}

        #region Alerts
        public void Warning(string message) => TempData.Add(WebConstants.Alerts.WARNING, message);
        //public void Warning(string message)
        //{
        //    TempData.Add(WebConstants.Alerts.WARNING, message);
        //}

        public void Success(string message) => TempData.Add(WebConstants.Alerts.SUCCESS, message);
        //public void Success(string message)
        //{
        //    TempData.Add(WebConstants.Alerts.SUCCESS, message);
        //}

        public void Information(string message) => TempData.Add(WebConstants.Alerts.INFORMATION, message);
        //public void Information(string message)
        //{
        //    TempData.Add(WebConstants.Alerts.INFORMATION, message);
        //}

        public void Error(string message) => TempData.Add(WebConstants.Alerts.ERROR, message);
        //public void Error(string message)
        //{
        //    TempData.Add(WebConstants.Alerts.ERROR, message);
        //}
        #endregion
    }
}