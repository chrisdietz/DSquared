using D_Squared.Domain;
using D_Squared.Domain.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace D_Squared.Web.Helpers
{
    public class CustomClaimsPrincipal : WindowsPrincipal, ICustomClaimsPrincipal
    {
        public string Name { get; set; }

        public string TruncatedName { get; set; }

        public new ClaimsIdentity Identity { get; private set; }

        public CustomClaimsPrincipal(WindowsIdentity identity) : base(identity)
        {
            Identity = identity;

            Name = identity.Name;
            TruncatedName = identity.Name.Substring(identity.Name.IndexOf('\\') + 1);

            identity.AddClaim(new Claim(ClaimTypes.GivenName, identity.Name));
        }

        /// <summary>
        /// Checks if the user belongs to the passed Windows User Group
        /// </summary>
        /// <param name="role">Windows User Group to check</param>
        /// <returns>Boolean</returns>
        public new bool IsInRole(string role)
        {
            if (ConfigurationManager.AppSettings["ApplicationRegion"] == "Development")
                return true;

            if (role.Contains(","))
            {
                return IsInAnyRoles(role.Split(','));
            }
            return base.IsInRole(role);
        }

        public bool IsInAnyRoles(string[] roles)
        {
            if (ConfigurationManager.AppSettings["ApplicationRegion"] == "Development")
                return true;

            foreach (string searchRole in roles)
            {
                if (base.IsInRole(searchRole))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if user is in "mahGM" group
        /// </summary>
        public bool IsGeneralManager()
        {
            if (ConfigurationManager.AppSettings["ApplicationRegion"] == "Development")
                return true;
            else //quick check for general manager role
                return IsInRole(DomainConstants.RoleNames.GeneralManagerGroup);
        }
    }

    public interface ICustomClaimsPrincipal : IPrincipal
    {
        /// <summary>
        /// Full Name of the User
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Name of the User without the domain attached
        /// </summary>
        string TruncatedName { get; set; }
    }

    public abstract class BaseViewPage : WebViewPage
    {
        public virtual new CustomClaimsPrincipal User
        {
            get { return base.User as CustomClaimsPrincipal; }
        }
    }

    public abstract class BaseViewPage<TModel> : WebViewPage<TModel>
    {
        public virtual new CustomClaimsPrincipal User
        {
            get { return base.User as CustomClaimsPrincipal; }
        }
    }
}