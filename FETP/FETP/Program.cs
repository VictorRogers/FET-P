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
            while(true)
            {
                Console.WriteLine("Would you like to test");
                Console.WriteLine("1. Reading from Enrollment File");
                Console.WriteLine("2. Reading from Constraints File");
                string input = Console.ReadLine();
                Console.WriteLine();
                if(input == "1")
                {
                    FileStream inFile = File.OpenRead(@"../../../../Example Data/Spring 2015 Total Enrollments by Meeting times.csv");
                    // List<Class> allClasses = FETP_Controller.sortClassesByEnrollment(FETP_Controller.readInputDataFile(inFile));
                    List<Class> allClasses = FETP_Controller.sortClassesByOverlappingDays(FETP_Controller.readInputDataFile(inFile));

                    foreach (Class cl in allClasses)
                    {
                        cl.Display();

                        Console.WriteLine("Overlaps with: {0} other classes", FETP_Controller.getNumberOfOverlappingDays(allClasses, cl));
                    }
                }
                else if(input == "2")
                {
                    FileStream inFile = File.OpenRead(@"../../../../Example Data/Ben Made Constraints Sample.txt");
                    // List<Class> allClasses = FETP_Controller.sortClassesByEnrollment(FETP_Controller.readInputDataFile(inFile));
                    Schedule blankSchedule = FETP_Controller.readInputConstraintsFile(inFile);
                    blankSchedule.Display();
                   
                   
                }
                Console.WriteLine();
            }
            
           
        }
       
    }
}
