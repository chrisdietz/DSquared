using System;
using System.Security;
using System.Security.Principal;
using System.Runtime.InteropServices;

namespace D_Squared.Data.Queries
{
    public class MAHImpersonator
    {
        private static int LOGON32_PROVIDER_DEFAULT = 0;

        private static int LOGON32_LOGON_INTERACTIVE = 2;

        private static WindowsImpersonationContext _impersonationContext;

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword,
          int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private extern static bool DuplicateToken(IntPtr ExistingTokenHandle,
          int SECURITY_IMPERSONATION_LEVEL, ref IntPtr DuplicateTokenHandle);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private extern static bool CloseHandle(IntPtr handle);

        /// <summary>
        /// Impersonates Valid User.
        /// </summary>
        /// <param name="userName">User Name</param>
        /// <param name="domainName">Domain Name</param>
        /// <param name="password">Password</param>
        /// <returns>True when Impersonates user elase false</returns>
        public bool ImpersonateValidUser(string userName, string domainName, string password)
        {
            try
            {
                IntPtr tokenHandle = new IntPtr(0);

                bool returnValue = LogonUser(userName, domainName, password, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, ref tokenHandle);

                if (false == returnValue)
                {
                    int ret = Marshal.GetLastWin32Error();
                    throw new System.ComponentModel.Win32Exception(ret);
                }

                WindowsIdentity newId = new WindowsIdentity(tokenHandle);
                tokenHandle = IntPtr.Zero;

                WindowsImpersonationContext impersonatedUser = newId.Impersonate();
                _impersonationContext = impersonatedUser;

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        /// <summary>
        /// Undo Impersonation.
        /// </summary>
        public void UndoImpersonation()
        {
            if (_impersonationContext != null)
            {
                _impersonationContext.Undo();
            }
        }
    }
}

