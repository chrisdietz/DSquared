using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D_Squared.Web.Helpers
{
    public class WebConstants
    {
        public static class Alerts
        {
            public const string SUCCESS = "success";
            public const string WARNING = "warning";
            public const string ERROR = "danger";
            public const string INFORMATION = "info";

            public static string[] ALL
            {
                get { return new[] { SUCCESS, WARNING, INFORMATION, ERROR }; }
            }
        }

        public static class ApplicationRegions
        {
            public const string DEVELOPMENT = "Development";
            public const string PRODUCTION = "Production";

            public const string DEVELOPMENTMESSAGE = "DEVELOPMENT Region";
            public const string PRODUCTIONMESSAGE = "PRODUCTION Region";
            public const string UNKNOWNMESSAGE = "UNKNOWN Region, Application Region must be defined in configuration as Development or Production.";
        }
    }
}