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
        public delegate void LoginClickHandler(object sender, EventArgs e);
        public event LoginClickHandler Login;

        public Auth()
        {
            InitializeComponent();
        }

        //Victor Rogers
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(Login != null)
            {
                Login(this, e);
            }
        }
    }
}
