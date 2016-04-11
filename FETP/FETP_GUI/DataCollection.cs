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
    public partial class DataCollection : UserControl
    {
        public delegate void GenerateClickHandler(object sender, EventArgs e);
        public event GenerateClickHandler GenerateSchedule;

        public delegate void ClearClickHandler(object sender, EventArgs e);
        public event ClearClickHandler ClearForm;

        public DataCollection()
        {
            InitializeComponent();
        }

        private void generate_Click(object sender, EventArgs e)
        {
            if (GenerateSchedule != null)
            {
                GenerateSchedule(this, e);
            }
        }

        private void clear_Click(object sender, EventArgs e)
        {
            if (ClearForm != null)
            {
                ClearForm(this, e);
            }
        }
    }
}
