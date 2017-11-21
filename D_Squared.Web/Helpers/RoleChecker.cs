using System.Security.Principal;
using D_Squared.Domain;

namespace D_Squared.Web.Helpers
{
    public static class RoleChecker
    {
        public static bool IsGeneralManager(WindowsPrincipal User)
        {
            return User.IsInRole(DomainConstants.RoleNames.GeneralManagerGroup);
        }
    }
}