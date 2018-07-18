using System.Collections.Generic;

namespace D_Squared.Domain
{
    public class DomainConstants
    {
        public static class GL_ACCOUNT_CONSTANTS
        {
            public const int CASH_DEPOSIT = 40000;
            public const int MISC_DEPOSIT = 40200;
        }

        public static class RoleNames
        {
            public const string GeneralManagerGroup = "mahGM";

            public const string RedbookManagerGroup = "redbookMgr";
            public const string RedbookRegionalGroup = "redbookReg";
            public const string RedbookDivisionalVPGroup = "redbookDvp";

            public const string DSquaredAdminGroup = "d2Admins";
        }

        public static class CompetitiveEventConstants
        {
            public static List<string> DistanceRanges()
            {
                return new List<string>()
                {
                    ZeroToOne,
                    OneToThree,
                    ThreeToFive,
                    FiveOrGreater
                };
            }

            public const string ZeroToOne = "0-1 miles";
            public const string OneToThree = "1-3 miles";
            public const string ThreeToFive = "3-5 miles";
            public const string FiveOrGreater = "5+ miles";

            public const string Opening = "Opening";
            public const string Closing = "Closing";
        }
    }
}
