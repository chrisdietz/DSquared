using D_Squared.Web.Helpers;
using System.Web.Mvc;

namespace D_Squared.Web.Controllers
{
    public class BaseController : Controller
    {
        protected virtual new CustomClaimsPrincipal User
        {
            get { return HttpContext.User as CustomClaimsPrincipal; }
        }

        #region Alerts
        public void Warning(string message)
        {
            TempData.Add(WebConstants.Alerts.WARNING, message);
        }

        public void Success(string message)
        {
            TempData.Add(WebConstants.Alerts.SUCCESS, message);
        }

        public void Information(string message)
        {
            TempData.Add(WebConstants.Alerts.INFORMATION, message);
        }

        public void Error(string message)
        {
            TempData.Add(WebConstants.Alerts.ERROR, message);
        }
        #endregion
    }
}