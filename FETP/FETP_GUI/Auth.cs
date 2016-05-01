using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FETP_GUI
{
    public partial class Auth : UserControl
    {
        //    [System.Runtime.InteropServices.DllImport("advapi32.dll")]
        //    public static extern bool LogonUser(string userName, string domainName, string password, int LogonType, int LogonProvider, ref IntPtr phToken);

        public delegate void LoginClickHandler(object sender, EventArgs e);
        public event LoginClickHandler Login;

        public Auth()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(Login != null)
            {
                Login(this, e);
            }

            //This block and the functions called in it get moved into the FETP_Form
            //bool isValid = false;
            //string userName = GetLoggedInUserName();

            //if (userName.ToLowerInvariant().Contains(txtUserName.Text.Trim().ToLowerInvariant()) &&
            //        userName.ToLowerInvariant().Contains(txtDomain.Text.Trim().ToLowerInvariant()))
            //{
            //    isValid = IsValidCredentials(txtUserName.Text.Trim(), txtPwd.Text.Trim(), txtDomain.Text.Trim());
            //}

            //if (isValid)
            //{
            //}
            //else
            //{
            //    System.Windows.Forms.MessageBox.Show("Invalid Windows username or password.");
            //}
        }

        //private string GetLoggedInUserName()
        //{
        //    System.Security.Principal.WindowsIdentity currentUser = System.Security.Principal.WindowsIdentity.GetCurrent();
        //    return currentUser.Name;
        //} 

        //private bool IsValidCredentials(string userName, string password, string domain)
        //{
        //    if (domain == "")
        //    {
        //        domain = System.Environment.MachineName;
        //    }

        //    IntPtr tokenHandler = IntPtr.Zero;
        //    bool isValid = LogonUser(userName, domain, password, 2, 0, ref tokenHandler);
        //    return isValid;
        //}
    }
}
