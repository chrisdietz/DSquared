using System.Web;
using System.Web.Mvc;
using D_Squared.Domain;

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
}