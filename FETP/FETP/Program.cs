using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FETP
{
    class Program
    {
        static void Main(string[] args)
        {
            FileStream inFile = File.OpenRead(@"../../../../Example Data/Spring 2015 Total Enrollments by Meeting times.csv");
            List<Class> allClasses = FETP_Controller.readInputDataFile(inFile);
            foreach(Class cl in allClasses)
            {
                cl.Display();
            }

            Console.ReadKey();
           
        }
       
    }
}
