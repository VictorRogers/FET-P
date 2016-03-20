// public void addDay()

/*
// fills in days with blank timeslots
public void fillInDays()
{
List<Block> classesToBeGrouped = new List<Block>(); // Variable to contain the list of all grouped classes

for (int i = 0; i < this.numberOfDays; i++)
{
    Day newDay = new Day(examsStartTime);
    TimeSpan examCurrentTime = this.examsStartTime;
    for (int j = 0; j < FETP_Controller.getNumberOfTimeSlotsAvailablePerDay(this); j++)
    {
        newDay.blocks.Add(new Timeslot(examCurrentTime, examCurrentTime + examsLength));
        // ? check if lunch and account for it here
        examCurrentTime += examsLength + timeBetweenExams;
    }
}
}
*/




/*
public Block(TimeSpan inStartTime, TimeSpan inEndTime, List<DayOfWeek> inDaysMeet, List<Class> inClasses = null)
    : base(inStartTime, inEndTime, inDaysMeet, )
{
    this.classesInBlock = inClasses;

    // calculate total enrollement
    foreach (Class clas in inClasses)
        this.enrollment += clas.Enrollment;

}
*/
//public Block(TimeSpan inStartTime, TimeSpan inEndTime, int inEnrollment, List<Class> inClasses)
//    : base(inStartTime, inEndTime, inEnrollment)
//{
//    if (inClasses == null)
//        this.classesInBlock = new List<Class>();
//    else
//        this.classesInBlock = inClasses;
//}






//public Block(Class inClass, List<Class> inClasses = null)
//    : base(inClass.StartTime, inClass.EndTime, 0)
//{
//    this.classesInBlock = inClasses;
//    addClass(inClass);
//}




// ? let's get dirty

public volatile int smallestGroupSizeFound;
public volatile List<List<Block>> smallestGroupsFound = new List<List<Block>>();



//// finds the smallest grouping
//public void groupClasses(List<Class> classes)
//{
//    List<Block> blocks = new List<Block>();
//    //groupingHelper(blocks, classes);
//    //helpMe(blocks, classes);
//    //Console.WriteLine("{0}", findSmallestGroup(blocks, classes));
//    Console.WriteLine("{0}", findNextSmallestGroup(blocks, classes, this.smallestGroupSizeFound));
//}









//// Recursive Helper
//protected List<Block> groupingHelper(List<Block> blocks, List<Class> classes)
//{

//    if (classes.Count == 0)
//    {
//        return blocks;
//    }
//    else
//    {
//        //List<Block> newBlocks = blocks;

//        foreach (Class cl in classes)
//        {
//            blocks = groupClass(blocks, cl);
//        }

//        if (blocks.Count <= this.smallestGroupSizeFound)
//        {
//            this.smallestGroupSizeFound = blocks.Count;
//            this.smallestGroupsFound.Add(blocks);
//        }

//        foreach (Class cl in classes)
//        {
//            List<Block> newBlocks = blocks;
//            List<Class> newClasses = classes;
//            newClasses.Remove(cl);
//            newBlocks = groupClass(newBlocks, cl);

//            newBlocks = groupingHelper(newBlocks, classes);
//            if(newBlocks.Count < this.smallestGroupSizeFound)
//            {
//                this.smallestGroupSizeFound = newBlocks.Count;
//            }
//        }
//        return groupingHelper
//    }
//}

//public static async Task findSmallest(List<Block>Blocks, List<Class> classes, int smallestThusFar)
//{

//}

//public List<Thread> threads = new List<Thread>();

//// Recursive Helper
//// simulates all possible groups and only saves the smallest
//protected void groupingHelper(List<Block> blocks, List<Class> classes, int smallestThusFar)
//{

//    if (classes.Count != 0 && blocks.Count < smallestThusFar)
//    {
//        //List<Block> newBlocks = blocks;
//        int i = 0;
//        while (i < classes.Count)
//        {
//            List<Block> newBlocks = new List<Block>(this.groupClass(blocks, classes[i]));
//            List<Class> newClasses = new List<Class>(classes);
//            newClasses.RemoveAt(i);

//            thread.Add(new Thread((groupingHelper(newBlocks, newClasses, smallestThusFar));

//            i++;
//        }


//        //foreach (Class cl in classes)
//        //{
//        //    List<Block> newBlocks = this.groupClass(blocks, cl);
//        //    List<Class> newClasses = classes;
//        //    Console.WriteLine("removing class");
//        //    newClasses.Remove(cl);
//        //    Console.WriteLine("removed class");
//        //    groupingHelper(newBlocks, newClasses);

//        //}
//    }
//    else if (blocks.Count <= this.smallestGroupSizeFound)
//    {
//        this.smallestGroupsFound.Add(blocks);
//    }
//}




//public async Task<> helpMe(List<Block> blocks, List<Class> classes)
//{
//    await groupingHelper(blocks, classes);
//}


// Recursive Helper
// simulates all possible groups and only saves the smallest
//protected async void groupingHelper(List<Block> blocks, List<Class> classes)
//{

//    if (classes.Count != 0 && blocks.Count < this.smallestGroupSizeFound)
//    {
//        //List<Block> newBlocks = blocks;
//        int i = 0;
//        while (i < classes.Count)
//        {
//            List<Block> newBlocks = new List<Block>(Schedule.groupClass(blocks, classes[i]));
//            List<Class> newClasses = new List<Class>(classes);
//            newClasses.RemoveAt(i);

//            groupingHelper(newBlocks, newClasses);

//            i++;
//        }


//        //foreach (Class cl in classes)
//        //{
//        //    List<Block> newBlocks = this.groupClass(blocks, cl);
//        //    List<Class> newClasses = classes;
//        //    Console.WriteLine("removing class");
//        //    newClasses.Remove(cl);
//        //    Console.WriteLine("removed class");
//        //    groupingHelper(newBlocks, newClasses);

//        //}
//    }
//    else if (blocks.Count <= this.smallestGroupSizeFound)
//    {
//        this.smallestGroupsFound.Add(blocks);
//    }
//}


//protected static async void findSmallgroup(List<Block> blocks, List<Class> classes)
//{
//    int currentSmallest;
//    List<Block> newBlocks = new List<Block>(blocks);
//    foreach (Class cl in classes)
//    {
//        newBlocks = groupClass(newBlocks, cl);
//    }

//    currentSmallest = newBlocks.Count;

//    foreach (Class cl in classes)
//    {
//        List<Class> newClasses = new List<Class>(classes); // ?
//        newClasses.RemoveAt(i);
//        int number = findSmallestGroup(groupClass((new List<Block>(blocks)), classes[i]), newClasses);
//        if (number < currentSmallest)
//        {
//            currentSmallest = number;
//        }
//    }

//    Console.WriteLine("Current smallest: {0}", currentSmallest);


//}

//// ? make task 
//protected static async Task<int> findSmallestGroup(List<Block> blocks, List<Class> classes)
//{
//    if (classes.Count == 0)
//    {

//        return blocks.Count;

//    }
//    else
//    {
//        List<Block> newBlocks = new List<Block>(blocks);
//        foreach (Class cl in classes)
//        {
//            newBlocks = groupClass(newBlocks, cl);
//        }

//        int currentSmallest = newBlocks.Count;

//        int i = 0;
//        while (i < classes.Count)
//        {

//            List<Class> newClasses = new List<Class>(classes); // ?
//            newClasses.RemoveAt(i);
//            int number = findSmallestGroup(groupClass((new List<Block>(blocks)), classes[i]), newClasses);
//            if (number < currentSmallest)
//            {
//                currentSmallest = number;
//            }

//        }
//        return currentSmallest;
//    }
//}

//protected int numberOfExecutions = 0;
//protected int findNextSmallestGroup(List<Block> blocks, List<Class> classes, int currentSmallest)
//{
//    Console.WriteLine(numberOfExecutions++);
//    if (blocks.Count > currentSmallest)
//    {
//        return currentSmallest;
//    }
//    else if (classes.Count == 0) // count must be smaller or equal to smallest thus far
//    {
//        return blocks.Count;
//    }
//    else
//    {

//        List<Block> newBlocks = new List<Block>(blocks);

//        //Stopwatch stopWatch = new Stopwatch();
//        //stopWatch.Start();
//        foreach (Class cl in classes)
//        {
//            newBlocks = groupClass(newBlocks, cl);
//        }

//        //stopWatch.Stop();
//        //Console.WriteLine(stopWatch.Elapsed);
//        if (blocks.Count < currentSmallest)
//        {
//            return blocks.Count;
//        }
//        //else
//        //{
//        Parallel.ForEach(classes, (cl) =>
//            {
//                // newBlocks = groupClass(newBlocks, classes[i]);
//                List<Class> newClasses = new List<Class>(classes); // ?
//                newClasses = removeClass(newClasses, cl);
//                int number = findNextSmallestGroup(groupClass((new List<Block>(blocks)), cl), newClasses, currentSmallest);
//                if (number < currentSmallest)
//                {
//                    // currentSmallest = number;
//                    currentSmallest = number;
//                    return;
//                }

//            });
//            return currentSmallest;
//        }





//        return currentSmallest;

//        //    foreach(Class cl in classes)
//        //{
//        //    // newBlocks = groupClass(newBlocks, classes[i]);
//        //    List<Class> newClasses = new List<Class>(classes); // ?
//        //    removeClass(newClasses, cl);
//        //    int number = findNextSmallestGroup(groupClass((new List<Block>(blocks)), cl), newClasses, currentSmallest);
//        //    if (number < currentSmallest)
//        //    {
//        //        // currentSmallest = number;
//        //        return number;
//        //    }
//        //}

//        //int i = 0;
//        //while (i < classes.Count)
//        //{
//        //    // newBlocks = groupClass(newBlocks, classes[i]);
//        //    List<Class> newClasses = new List<Class>(classes); // ?
//        //    newClasses.RemoveAt(i);
//        //    int number = findNextSmallestGroup(groupClass((new List<Block>(blocks)), classes[i]), newClasses, currentSmallest);
//        //    if (number < currentSmallest)
//        //    {
//        //        // currentSmallest = number;
//        //        return number;
//        //    }

//        //}


//    //}
//}




//// ? make task 
//protected static int findSmallestGroup(List<Block> blocks, List<Class> classes)
//{
//    if(classes.Count == 0)
//    {

//        return blocks.Count;

//    } 
//    else
//    {
//        List<Block> newBlocks = new List<Block>(blocks);
//        foreach(Class cl in classes)
//        {
//            newBlocks = groupClass(newBlocks, cl);
//        }

//        int currentSmallest = newBlocks.Count;

//        int i = 0;
//        while (i < classes.Count)
//        {

//            List<Class> newClasses = new List<Class>(classes); // ?
//            newClasses.RemoveAt(i);
//            int number = findSmallestGroup(groupClass((new List<Block>(blocks)), classes[i]), newClasses);
//            if(number < currentSmallest)
//            {
//                currentSmallest = number;
//            }

//        }
//        return currentSmallest;
//    }
//}


//protected List<Block> checkAllPossibleGroups(List<Block> blocks, List<Class> classes)
//{

//}


//protected void helpMe(List<Block> blocks, List<Class> classes)
//{
//    if(classes.Count == 0)
//    {
//        if(blocks.Count < this.smallestGroupSizeFound)
//        {
//            this.smallestGroupSizeFound = blocks.Count;
//        }
//    }
//    else
//    {
//        //List <Block> newBlocks = new List<Block>(blocks);
//        //foreach(Class cl in classes)
//        //{
//        //    groupClass(newBlocks, cl);
//        //}
//        //if(newBlocks.Count < this.smallestGroupSizeFound)
//        //{
//        //    this.smallestGroupSizeFound = newBlocks.Count;
//        //}
//        //List<Block> newBlocks = blocks;

//        int i = 0;
//        while (i < classes.Count)
//        {
//            List<Class> what = new List<Class>(classes); // ?
//            what.RemoveAt(i);
//            helpMe(groupClass((new List<Block>(blocks)),classes[i]), what);
//        }
//    }
//}













//// playing with TimeSlot class
////hmmmm
//// ? did i really just make this?
//// it's LITERALLY just a block... damn
//public class Timeslot : Class
//{
//    protected Block groupOfClasses;

//    public Timeslot(TimeSpan inStartTime, TimeSpan inEndTime, Block inBlock = null, List<DayOfWeek> inDaysMeet = null, int inEnrollment = 0)
//        : base(inStartTime, inEndTime, inDaysMeet, inEnrollment)
//    {
//        this.groupOfClasses = inBlock;
//    }



//    public override void Display()
//    {
//        base.Display();
//    }

//}

// ? oh gawd this hurts. why ben? why? why did you do this? it's not good
//public class Day
//{
//    public List<Block> blocks; // ? lazy
//    protected TimeSpan dayStartTime;
//    protected TimeSpan lunchStartTime;
//    protected TimeSpan lunchEndTime;


//    public Day(TimeSpan dayStartTime, TimeSpan lunchStartTime = TimeSpan.Zero, TimeSpan lunchEndTime = TimeSpan.Zero, List<Block> inBlocks = null) // 0 because there won't always be lunch
//    {
//        this.dayStartTime = dayStartTime;
//        this.lunchStartTime = lunchStartTime;
//        this.lunchEndTime = lunchEndTime;
//        this.blocks = inBlocks;
//    }



//    public void Display()
//    {
//        foreach (Timeslot block in blocks)
//            block.Display();
//    }
//}




//// checks if class overlaps with any class in a block
//public static bool doesClassOverlapBlock(Block block, Class inClass)
//{
//    foreach (Class cl in block.ClassesInBlock)
//    {
//        if (!doClassesOverlap(cl, inClass))
//        {
//            return false;
//        }
//    }
//    return true;
//}

// Helper function to find the total days two classes have in common.



//// ? maybe correct?
//public static int getSmallestPossibleGrouping(List<Class> classes)
//{
//    return (coalesceClassesTogether(sortClassesByOverlappingDays(classes))).Count;
//}

// Find if any classes overlap in list of classes


//// Checks if enough timeslots are available
//public static bool areThereEnoughTimeslots(Schedule schedule, List<Class> classes)
//{
//    return (getNumberOfTimeSlotsAvailable(schedule) >= getSmallestPossibleGrouping(classes));
//}

/*
// ? in theory, this will automatically find what days a block meets.
// a block should only meet on MWF or TR
//return a key value
//      Day: #
//      Number of those days: #
public static List<DayOfWeek> getMostCommonDays(List<Class> classes)
{
    foreach (Class class1 in classes)
    {
        foreach (Class class2 in classes)
        {
            if (doClassDaysOverlap(class1, class2))
                return true;
        }
    }
    return false;
}
*/


//public static TimeSpan putInClosestPossibleBlock()
//{
//    // first look for closest block with start time, then go to next largest

//        // preference to being later then earlier


//}
// takes in a list of classes 
// returns a new list of groups of classes
// can be used to find a grouping size to beat

// ? original
//// takes in a list of classes and coalesces them into a list of blocks of classes
//public static List<Block> coalesceClassesTogether(List<Class> classes)
//{
//    List<Block> classesToBeGrouped = new List<Block>(); // Variable to contain the list of all grouped classes

//    foreach (Class cl in classes)
//    {
//        bool doesItOverlap = false;
//        foreach(Block block in classesToBeGrouped)
//        {
//            if (doClassesOverlap(cl, block.ClassesInBlock[0])) // ? changed to 0. makes no difference when making smallest classes, but should make it work with blank days
//            {
//                block.addClass(cl);
//                doesItOverlap = true;
//                break; // ?bug THIS WASN'T THERE :'( // maybe not a bug due to ordering?
//            }
//        }
//        if(!doesItOverlap)
//            classesToBeGrouped.Add(new Block(cl));
//    }
//    return classesToBeGrouped;

//}

// takes in a list of classes and coalesces them into a list of blocks of classes




//// takes in a list of classes and coalesces them into a list of blocks of classes
//public static List<Block> coalesceClassesTogether(List<Class> classes, List<Block> groups)
//{
//    // ? check if possible maybe?
//    //

//    if (groups == null)
//    {
//        groups = new List<Block>(); // Variable to contain the list of all grouped classes
//    }

//    foreach (Class cl in classes)
//    {
//        bool isFound = false;

//        TimeSpan weight = TimeSpan.Zero;
//        while (!isFound) // ?hello infinity
//        {

//            foreach (Block block in groups)
//            {
//                if (block.StartTime - cl.StartTime < weight && (block.ClassesInBlock == null || doClassesOverlap(cl, block.ClassesInBlock[0]))) // could reference null, but should drop out
//                {
//                    block.addClass(cl);
//                    isFound = true;
//                    break; // ? BAD BAD BAD :'(
//                }
//            }
//            weight += TimeSpan.FromMinutes(30);
//        }

//    }
//    return groups;
//}

//public static getClosest

//// ? need to rewrite and combine these
//// takes in a list of classes and coalesces them into a list of blocks of classes
//public static List<Block> coalesceClassesTogether(List<Class> classes)
//{
//    List<Block> classesToBeGrouped = new List<Block>(); // Variable to contain the list of all grouped classes

//    // ? oh gawd this hurts
//    List<Block> allGroups = new List<Block>;

//    foreach (Class cl in classes)
//    {
//        bool doesItOverlap = false;
//        foreach (Block block in classesToBeGrouped)
//        {
//            if (doClassesOverlap(cl, block.ClassesInBlock[0])) // ? changed to 0. makes no difference when making smallest classes, but should make it work with blank days
//            {
//                block.addClass(cl);
//                doesItOverlap = true;
//            }
//        }
//        if (!doesItOverlap)
//            classesToBeGrouped.Add(new Block(cl));
//    }
//    return classesToBeGrouped;

//}


//public static List<Day> coalesceClassesIntoDays(Schedule schedule, List<Class> classes)
//{
//    //List<Block> classesToBeGrouped = new List<Block>(); // Variable to contain the list of all grouped classes

//    //for(int i = 0; i < schedule.NumberOfDays; i++)
//    //{
//    //    Day newDay = new Day();
//    //    TimeSpan examCurrentTime = schedule.ExamsStartTime;
//    //    for(int j = 0; j < getNumberOfTimeSlotsAvailablePerDay(schedule); j++)
//    //    {
//    //        Block newBlock
//    //    }
//    //}

//    //return classesToBeGrouped;

//     List<Block> classesToBeGrouped = new List<Block>(); // Variable to contain the list of all grouped classes

//     foreach (Class cl in classes)
//     {
//         bool doesItOverlap = false;
//         foreach (Day day in schedule.days)
//         {
//             foreach (day )
//             if (doClassesOverlap(cl, day.block))
//             {
//                 block.addClass(cl);
//                 doesItOverlap = true;
//             }
//         }
//         if (!doesItOverlap)
//             classesToBeGrouped.Add(new Block(cl));
//     }
//     return classesToBeGrouped;


// }



//// fills in days with blank timeslots
//public static List<Block> createShellDays(Schedule schedule)
//{
//    // List<Block> classesToBeGrouped = new List<Block>(); // Variable to contain the list of all grouped classes

//    List<Block> groups = new List<Block>();

//    for (int i = 0; i < schedule.NumberOfDays; i++)
//    {
//        TimeSpan examCurrentTime = schedule.ExamsStartTime;
//        for (int j = 0; j < FETP_Controller.getNumberOfTimeSlotsAvailablePerDay(schedule); j++)
//        {
//            groups.Add(new Block(examCurrentTime, examCurrentTime + schedule.ExamsLength));
//            // ? check if lunch and account for it here
//            examCurrentTime += schedule.ExamsLength + schedule.TimeBetweenExams;
//        }
//    }

//    return groups;
//}
/*
// fills in days with blank timeslots
public List<Days> createShellDays(int numberOfDays, int numberOfTimeslotsPerDay, int examsStartTime, TimeSpan examsLength, TimeSpan timeBetweenExams)
{
    List<Block> classesToBeGrouped = new List<Block>(); // Variable to contain the list of all grouped classes

    for (int i = 0; i < this.numberOfDays; i++)
    {
        Day newDay = new Day(examsStartTime);
        TimeSpan examCurrentTime = this.examsStartTime;
        for (int j = 0; j < FETP_Controller.getNumberOfTimeSlotsAvailablePerDay(this); j++)
        {
            newDay.blocks.Add(new Timeslot(examCurrentTime, examCurrentTime + examsLength));
            // ? check if lunch and account for it here
            examCurrentTime += examsLength + timeBetweenExams;
        }
    }
}
*/


/*
// ? switching up style of functions here to make coupling ?lower?
public static List<Days> makeBlankDays(TimeSpan startTime, TimeSpan endTime, TimeSpan examLength)
*/


//Console.WriteLine("4. Test Advanced Grouping (not ready)");
//Console.WriteLine("5. Test Advanced Advanced Grouping (not ready)");

//else if (input == "4")
//{
//    Console.WriteLine(STARS);
//    Console.WriteLine("Testing Advanced Grouping of Classes");
//    Console.WriteLine(STARS);

//    // ------------------------------------------------------------------------------------------------------
//    // Get data file information
//    FileStream inFile = File.OpenRead(@"../../../../Example Data/Spring 2015 Total Enrollments by Meeting times.csv");

//    // currently sorts all the data
//    List<Class> allClasses = FETP_Controller.readInputDataFile(inFile);
//    // ------------------------------------------------------------------------------------------------------

//    // ------------------------------------------------------------------------------------------------------
//    // Get contraints file information
//    inFile = File.OpenRead(@"../../../../Example Data/Ben Made Constraints Sample.txt");
//    // List<Class> allClasses = FETP_Controller.sortClassesByEnrollment(FETP_Controller.readInputDataFile(inFile));
//    Schedule blankSchedule = FETP_Controller.readInputConstraintsFile(inFile);
//    // ------------------------------------------------------------------------------------------------------


//    allClasses = FETP_Controller.sortClassesByEnrollment(allClasses);
//    List<Block> groupedClasses = FETP_Controller.coalesceClassesTogether(allClasses);


//    Console.WriteLine("Number of Blocks: {0}", groupedClasses.Count);
//    Console.WriteLine();

//    foreach (Block block in groupedClasses)
//    {
//        Console.WriteLine("=================================");
//        Console.WriteLine("Displaying Block");
//        Console.WriteLine("=================================");

//        Console.WriteLine("---------------------------------");
//        Console.WriteLine("Displaying Information of Block");
//        Console.WriteLine("---------------------------------");

//        block.Display();
//        Console.WriteLine("---------------------------------");
//        Console.WriteLine("END Displaying Information of Block");
//        Console.WriteLine("---------------------------------");


//        Console.WriteLine("---------------------------------");
//        Console.WriteLine("Displaying all Classes in Block");
//        Console.WriteLine("---------------------------------");
//        block.DisplayAllClasses();
//        Console.WriteLine("---------------------------------");
//        Console.WriteLine("END Displaying all Classes in Block");
//        Console.WriteLine("---------------------------------");
//        Console.WriteLine();

//        Console.WriteLine("=================================");
//        Console.WriteLine("END Displaying Block");
//        Console.WriteLine("=================================");
//    }

//    Console.WriteLine(STARS);
//    Console.WriteLine("END Testing Advanced Grouping of Classes");
//    Console.WriteLine(STARS);

//}
//else if(input == "5")
//{
//    Console.WriteLine(STARS);
//    Console.WriteLine("Testing Advanced Advanced Grouping of Classes");
//    Console.WriteLine(STARS);

//    // ------------------------------------------------------------------------------------------------------
//    // Get data file information
//    FileStream inFile = File.OpenRead(@"../../../../Example Data/Spring 2015 Total Enrollments by Meeting times.csv");

//    // currently sorts all the data
//    List<Class> allClasses = FETP_Controller.readInputDataFile(inFile);
//    // ------------------------------------------------------------------------------------------------------

//    // ------------------------------------------------------------------------------------------------------
//    // Get contraints file information
//    inFile = File.OpenRead(@"../../../../Example Data/Ben Made Constraints Sample.txt");
//    // List<Class> allClasses = FETP_Controller.sortClassesByEnrollment(FETP_Controller.readInputDataFile(inFile));
//    Schedule blankSchedule = FETP_Controller.readInputConstraintsFile(inFile);
//    // ------------------------------------------------------------------------------------------------------


//    allClasses = FETP_Controller.sortClassesByOverlappingDays(allClasses);

//    //blankSchedule.blocks = FETP_Controller.createShellDays(blankSchedule);

//    List<Block> groupedClasses = Schedule.coalesceClassesTogether(allClasses);

//    blankSchedule.smallestGroupSizeFound = groupedClasses.Count;

//    Console.WriteLine("Smallest Group Found First: {0}", blankSchedule.smallestGroupSizeFound);

//    blankSchedule.groupClasses(allClasses);

//    Console.WriteLine("New Smallest Group Found: {0}", blankSchedule.smallestGroupSizeFound);







//foreach (Block block in groupedClasses)
//{
//    Console.WriteLine("=================================");
//    Console.WriteLine("Displaying Block");
//    Console.WriteLine("=================================");

//    Console.WriteLine("---------------------------------");
//    Console.WriteLine("Displaying Information of Block");
//    Console.WriteLine("---------------------------------");

//    block.Display();
//    Console.WriteLine("---------------------------------");
//    Console.WriteLine("END Displaying Information of Block");
//    Console.WriteLine("---------------------------------");


//    Console.WriteLine("---------------------------------");
//    Console.WriteLine("Displaying all Classes in Block");
//    Console.WriteLine("---------------------------------");
//    block.DisplayAllClasses();
//    Console.WriteLine("---------------------------------");
//    Console.WriteLine("END Displaying all Classes in Block");
//    Console.WriteLine("---------------------------------");
//    Console.WriteLine();

//    Console.WriteLine("=================================");
//    Console.WriteLine("END Displaying Block");
//    Console.WriteLine("=================================");
//}

//Console.WriteLine("Number of Blocks: {0}", groupedClasses.Count);
//Console.WriteLine();
//}













// Prototype function for example of how to sort
public static List<Class> sortClassesByEnrollment(List<Class> classes)
{
    return classes.OrderByDescending(c => c.Enrollment).ToList();
}

// Prototype function for example of how to sort
public static List<Class> sortClassesByOverlappingDays(List<Class> classes)
{
    return classes.OrderByDescending(c => getNumberOfOverlappingDays(classes, c)).ToList();
}



/**************************************************************************\
        Method: getNumberOfTimeSlotsAvailablep
        Description: 
        \**************************************************************************/
public static int getNumberOfTimeSlotsAvailable(Schedule schedule)
{
    return getNumberOfTimeSlotsAvailablePerDay(schedule) * schedule.NumberOfDays;
}


/**************************************************************************\
Method:  
Description: 
\**************************************************************************/
public static int getNumberOfTimeSlotsAvailablePerDay(Schedule schedule)
{
    TimeSpan latestTime = TimeSpan.ParseExact(TIME_EXAMS_MUST_END_BY, @"hhmm", CultureInfo.InvariantCulture); // latest exams can go

    TimeSpan lengthOfExamDay = latestTime - schedule.ExamsStartTime; // Figure out how much time available for exams

    // if the lunch time is longer than the break time, account for it and the extra break time it will give you
    if (schedule.LunchLength > schedule.TimeBetweenExams)
    {
        lengthOfExamDay -= (schedule.LunchLength - schedule.TimeBetweenExams); // takes the lunch break out of available time. also pads for how the lunch will count as a break.
    }


    TimeSpan examFootprint = schedule.ExamsLength + schedule.TimeBetweenExams;

    int numberOfExams = 0;
    // ? bug if exam break is too big
    while ((lengthOfExamDay - schedule.ExamsLength) >= TimeSpan.Zero)
    {
        lengthOfExamDay -= schedule.ExamsLength;
        numberOfExams++;

        // checks if a break is needed due to their being room for another exam after a break
        if ((lengthOfExamDay - (schedule.TimeBetweenExams + schedule.ExamsLength) >= TimeSpan.Zero))
        {
            lengthOfExamDay -= schedule.TimeBetweenExams;
        }
    }
    return numberOfExams;
}
