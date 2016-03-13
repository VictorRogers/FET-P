using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FETP
{
    /**************************************************************************\
    Class: GA_Constraints (Genetic Algorithm Constraints)
    Description: The Constraints class contains all of the methods needed for
                 checking if a chromosome is meeting soft and hard constraints. 
    \**************************************************************************/ 
    class GA_Constraints
    {

        /**************************************************************************\
        GA_Constraints - Data Members 
        \**************************************************************************/

        /**************************************************************************\
        GA_Constraints - Methods 
        \**************************************************************************/
        /**************************************************************************\
        Constructor: Default 
        Description: 
        \**************************************************************************/ 
        GA_Constraints()
        {
        }

        /**************************************************************************\
        Method: CheckHardConstraints 
        Description:  
        \**************************************************************************/ 
        public bool CheckHardConstraints()
        {
            return false;
        }


        /**************************************************************************\
        Method: CheckSoftConstraints 
        Description:  
        \**************************************************************************/ 
        public int CheckSoftConstraints()
        {
            return 0;
        }
    }
}
