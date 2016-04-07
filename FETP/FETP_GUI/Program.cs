using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using FETP;

namespace FETP_GUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FETP_Form());
            //Application.Run(new Full_DragAndDropCalendar());
            //Application.Run(new Single_DragAndDropCalendar());

            //FETP_Controller FETP_Controller = new FETP_Controller();
            //FETP_Controller.ReadEnrollmentFile();
            //FETP_Controller.ParseClasses();
        }
    }
}
