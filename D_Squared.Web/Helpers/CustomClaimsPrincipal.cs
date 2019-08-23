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

            //if (ConfigurationManager.AppSettings["ApplicationRegion"] == "Development")
            //{
            //    Name = "CJohnson";
            //    TruncatedName = Name;

            //    identity.AddClaim(new Claim(ClaimTypes.GivenName, Name));
            //}
            //else
            //{
            Name = identity.Name;
            TruncatedName = identity.Name.Substring(identity.Name.IndexOf('\\') + 1);

            identity.AddClaim(new Claim(ClaimTypes.GivenName, identity.Name));
            //}
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

        public bool IsRedbookManager()
        {
            if (ConfigurationManager.AppSettings["ApplicationRegion"] == "Development")
                return true;
            else //quick check for general manager role
                return IsInRole(DomainConstants.RoleNames.RedbookManagerGroup);
        }

        public bool IsRegionalManager()
        {
            if (ConfigurationManager.AppSettings["ApplicationRegion"] == "Development")
                return true;
            else //quick check for general manager role
                return IsInRole(DomainConstants.RoleNames.RedbookRegionalGroup);
        }

        public bool IsDivisionalVP()
        {
            if (ConfigurationManager.AppSettings["ApplicationRegion"] == "Development")
                return true;
            else //quick check for general manager role
                return IsInRole(DomainConstants.RoleNames.RedbookDivisionalVPGroup);
        }

        public bool IsDSquaredTipReporting()
        {
            if (ConfigurationManager.AppSettings["ApplicationRegion"] == "Development")
                return true;
            else //quick check for general manager role
                return IsInRole(DomainConstants.RoleNames.DSquaredTipReportingGroup);
        }

        public bool IsDSquaredSpreadHours()
        {
            if (ConfigurationManager.AppSettings["ApplicationRegion"] == "Development")
                return true;
            else //quick check for general manager role
                return IsInRole(DomainConstants.RoleNames.DSquaredSpreadHoursGroup);
        }

        public bool IsDSquaredNYSMandatedHours()
        {
            if (ConfigurationManager.AppSettings["ApplicationRegion"] == "Development")
                return true;
            else //quick check for NYS Mandated hours role
                return IsInRole(DomainConstants.RoleNames.DSquaredMandatedHoursGroup);
        }

        public bool IsDSquaredReports()
        {
            if (ConfigurationManager.AppSettings["ApplicationRegion"] == "Development")
                return true;
            else //quick check for Reports role
                return IsInRole(DomainConstants.RoleNames.DSquaredReportsGroup);
        }
        public bool IsDSquaredAdmin()
        {
            if (ConfigurationManager.AppSettings["ApplicationRegion"] == "Development")
                return true;
            else //quick check for general manager role
                return IsInRole(DomainConstants.RoleNames.DSquaredAdminGroup);
        }

        public bool IsDSquaredDailyDepoists()
        {
            if (ConfigurationManager.AppSettings["ApplicationRegion"] == "Development")
                return true;
            else //quick check for general manager role
                return IsInRole(DomainConstants.RoleNames.DSquaredDailyDepositsGroup);
        }

        public bool IsDSquaredSalesForecasts()
        {
            if (ConfigurationManager.AppSettings["ApplicationRegion"] == "Development")
                return true;
            else //quick check for general manager role
                return IsInRole(DomainConstants.RoleNames.DSquaredSalesForecastsGroup);
        }

        public bool IsDSquaredTipPercentage()
        {
            if (ConfigurationManager.AppSettings["ApplicationRegion"] == "Development")
                return true;
            else //quick check for general manager role
                return IsInRole(DomainConstants.RoleNames.DSquaredTipPercentageGroup);
        }

        public bool DailyDepositsAllowed()
        {
            return (IsDSquaredDailyDepoists() || IsDSquaredAdmin() || IsRedbookManager());
        }
        public bool SalesForecastsEntryAllowed()
        {
            return (IsDSquaredSalesForecasts() || IsDSquaredAdmin() || IsRedbookManager());
        }
        public bool SalesForecastsSearchAllowed()
        {
            return (IsDSquaredSalesForecasts() || IsDSquaredAdmin() || IsRegionalManager() || IsDivisionalVP());
        }
        public bool EmployeeManagementAllowed()
        {
            return (SpreadHoursViewAllowed() || SpreadHoursSearchAllowed() || MandatedHoursViewAllowed() || MandatedHoursSearchAllowed());
        }
        public bool SpreadHoursViewAllowed()
        {
            return (IsDSquaredSpreadHours() || IsDSquaredAdmin());
        }

        public bool SpreadHoursSearchAllowed()
        {
            return (IsDSquaredAdmin() || IsRegionalManager() || IsDivisionalVP());
        }

        public bool MandatedHoursViewAllowed()
        {
            return (IsDSquaredNYSMandatedHours() || IsDSquaredAdmin());
        }

        public bool MandatedHoursSearchAllowed()
        {
            return (IsDSquaredAdmin() || IsRegionalManager() || IsDivisionalVP());
        }
        public bool RedbookEntryAllowed()
        {
            return (IsDSquaredAdmin() || IsRedbookManager());
        }
        public bool RedbookSearchAllowed()
        {
            return (IsDSquaredAdmin() || IsRegionalManager() || IsDivisionalVP());
        }
        public bool GratuityManagementAllowed()
        {
            return (TipReportViewAllowed() || TipReportSearchAllowed() || TipPercentageViewAllowed() || TipPercentageSearchAllowed());
        }
        public bool TipReportViewAllowed()
        {
            return (IsDSquaredTipReporting() || IsRedbookManager() || IsDSquaredAdmin());
        }
        public bool TipReportSearchAllowed()
        {
            return (IsDSquaredAdmin() || IsRegionalManager() || IsDivisionalVP());
        }
        public bool TipPercentageViewAllowed()
        {
            return (IsDSquaredTipPercentage() || IsRedbookManager() || IsDSquaredAdmin());
        }
        public bool TipPercentageSearchAllowed()
        {
            return (IsDSquaredAdmin() || IsRegionalManager() || IsDivisionalVP());
        }
        public bool ReportsAllowed()
        {
            return (IsDSquaredReports() || IsDSquaredAdmin() || IsRegionalManager() || IsDivisionalVP());
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