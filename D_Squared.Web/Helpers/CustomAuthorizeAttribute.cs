using System.Web;
using System.Web.Mvc;
using D_Squared.Domain;
using System;

namespace D_Squared.Web.Helpers
{
    public class AuthorizeGroup : AuthorizeAttribute
    {
        private string RoleGroup;

        public AuthorizeGroup(string group)
        {
            RoleGroup = group;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return (httpContext.User as CustomClaimsPrincipal).IsInRole(RoleGroup);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                //app should never reach this segment since all users are windows authenticated
                base.HandleUnauthorizedRequest(filterContext);
            }
            else
            {
                //can implement custom error handling here
                filterContext.Result = new HttpStatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
            }
        }

    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class PreventDuplicateRequestAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Request["__RequestVerificationToken"] == null)
                return;

            var currentToken = HttpContext.Current.Request["__RequestVerificationToken"].ToString();

            if (HttpContext.Current.Session["LastProcessedToken"] == null)
            {
                HttpContext.Current.Session["LastProcessedToken"] = currentToken;
                return;
            }

            lock (HttpContext.Current.Session["LastProcessedToken"])
            {
                var lastToken = HttpContext.Current.Session["LastProcessedToken"].ToString();

                if (lastToken == currentToken)
                {
                    filterContext.Controller.ViewData.ModelState.AddModelError("", "Looks like you accidentally tried to double post.");
                    return;
                }

                HttpContext.Current.Session["LastProcessedToken"] = currentToken;
            }
        }
    }
}