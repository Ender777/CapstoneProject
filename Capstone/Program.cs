using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//WORKING ON: query writing in databaseManager method to be called by main during the scheduling
//of a class

namespace Capstone
{
    class Program
    {
        //create instances of items needed
        static personCollection people = new personCollection();
        static databaseManager dm = new databaseManager();
        static classCollection scheduledClasses = new classCollection();
        //static List<Classroom> classrooms;
        //static List<CourseTime> courseTimes;
        //static Courses courses = new Courses();

        //bools for if a Teacher or Student has been added has been added to people
        static bool hasTeacher = false;
        static bool hasStudent = false;


        static void Main(string[] args)
        {
            Console.WriteLine("Loading...");
            //create and call databaseManager to load data from db upon startup
            //databaseManager dm = new databaseManager();
            dm.ConnectToSQL();
            Console.WriteLine("Welcome to the program!");
            //while loop to keep program going
            bool programRunning = true;
            while (programRunning)
            {
                //show options
                DisplayMenu();
                //get choice
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddStudent();
                        break;
                    case "2":
                        AddTeacher();
                        break;
                    case "3":
                        ScheduleClass();
                        break;
                    case "4":
                        //TODO: call method to look up data
                        break;
                    case "5":
                        Console.WriteLine("Closing...");
                        programRunning = false;
                        break;
                    default:
                        Console.WriteLine("Invalid input, please try again");
                        break;
                }
            }
        }

        //helper methods
        //method to display the main options
        public static void DisplayMenu()
        {
            Console.WriteLine("Here are your options");
            Console.WriteLine("1...add a student");
            Console.WriteLine("2...add a teacher");
            Console.WriteLine("3...schedule a class");
            Console.WriteLine("4...look up some info");
            Console.WriteLine("5...close program");
        }

        //method to add student-----------------------------------------------------------
        public static void AddStudent()
        {
            string name;
            string phone;
            string email;

            Console.Write("Enter Name\n");
            name = Console.ReadLine();
            Console.Write("Enter Phone number\n");
            phone = Console.ReadLine();
            Console.Write("Enter Email address\n");
            email = Console.ReadLine();

            if (name != "" && phone != "" && email != "")
            {
                bool isTeacher = false;
                Student s = new Student(name, phone, email, isTeacher);
                if (people.Contains(s))
                {
                    Console.WriteLine("The name, phone number, or email entered is already taken, please enter a new one");
                }
                else
                {
                    people.Add(s);
                    //bool used in scheduling a class for error checking
                    hasStudent = true;
                    Console.WriteLine("{0} added successfully!", s.Name);
                }
            }
            else
            {
                Console.WriteLine("Error inputting information, please try again");
            }
        }

        //method to add teacher-----------------------------------------------------------
        public static void AddTeacher()
        {
            string name;
            string phone;
            string email;
            Courses approvedCourses = new Courses();

            Console.Write("Enter Name\n");
            name = Console.ReadLine();
            Console.Write("Enter Phone number\n");
            phone = Console.ReadLine();
            Console.Write("Enter Email address\n");
            email = Console.ReadLine();
            //display all course options
            foreach (Cours c in dm.DBCourses)
            {
                Console.WriteLine("Course ID: {0}\tName: {1}", c.CourseID, c.CourseName);
            }

            //bool and while loop to keep adding courses until the user stops
            bool keepAdding = true;
            //loops through all courses comparing entered ID to courseIDs.  If a match
            //is found, bool is set to true and if statement is entered, assigning match
            //to the list.  After this is completed, ask user if they want to add more 
            //classes and loop again.
            while (keepAdding)
            {
                Console.WriteLine("Enter ID of a course this teacher can teach");

                //setting up tryparse for error checking
                string answer= Console.ReadLine();
                int id;
                bool result = int.TryParse(answer, out id);

                bool match = false;
                Cours matchedCourse = null;
                foreach (Cours c in dm.DBCourses)
                {
                    if (id == c.CourseID)
                    {
                        match = true;
                        matchedCourse = c;
                    }
                    //TODO: check if this part will break
                }
                if (match == true)
                {
                    //nested if statement checks for duplicates and writes and error message if found.
                    if (approvedCourses.Contains(matchedCourse))
                    {
                        Console.WriteLine("this course has already been added, you can't add it twice!");
                    }
                    else
                    {
                        approvedCourses.Add(matchedCourse);
                        Console.WriteLine("class successfully added");
                    }
                }
                else
                {
                    Console.WriteLine("invalid class number entered");
                }
                Console.WriteLine("Add another class?\n1...Yes\n2...No");
                string anotherClass = Console.ReadLine();
                switch (anotherClass)
                {
                    case "1":
                        break;
                    case "2":
                        keepAdding = false;
                        break;
                }
            }
            //once all classes are assigned, check for null, then create teacher with those classes
            //and add him/her to list
            if (approvedCourses.CourseCollection.Count != 0)
            {
                bool isTeacher = true;
                Teacher t = new Teacher(name, phone, email, isTeacher, approvedCourses);
                if (people.Contains(t))
                {
                    Console.WriteLine("The name, phone number, or email entered is already taken, please enter a new one");
                }
                else
                {
                    people.Add(t);
                    //bool used in scheduling a class for error checking
                    hasTeacher = true;
                    Console.WriteLine("{0} added Successfully!", t.Name);
                }
            }
            else
            {
                Console.WriteLine("No courses were successfully added that this teacher can teach.  Teacher was not added.");
            }
        }



        //method to schedule a class---------------------------------------------------------



        public static void ScheduleClass()
        {
            //declare variables to pass to classItem constructors here.
            int numberOfCourseToSchedule = 0;
            Cours courseToSchedule = null;
            Classroom roomToSchedule = null;
            CourseTime timeToSchedule = null;
            Teacher teacherToSchedule = null;
            List<Student> studentsToSchedule = new List<Student>();

            //list to hold IDs of classItems to check against
            List<int> numberList = new List<int>();

            if (hasTeacher == false || hasStudent == false)//condition to check if student and teacher have been created
            {
                Console.WriteLine("You can't schedule a class until a student and a teacher has been added.");
            }
            else
            {
                Console.WriteLine("Enter a unique number for this course");
                string numberAnswer = Console.ReadLine();
                int number;
                bool numberResult = int.TryParse(numberAnswer, out number);
                //if conversion worked, check for preexisting ID.  If not there, store value to proceed
                if (numberResult == true)
                {
                    if (numberList.Contains(number))
                    {
                        Console.WriteLine("That class number has already been used.");
                        //TODO: Program might break here, test this bit
                    }
                    else
                    {
                        numberList.Add(number);
                        numberOfCourseToSchedule = number;
                    }
                }
                else
                {
                    Console.WriteLine("input invalid, please enter a number");
                }
                //display courses and get ID of course--------------------------------------------------
                foreach (Cours c in dm.DBCourses)
                {
                    Console.WriteLine("Course ID: {0}\tName: {1}", c.CourseID, c.CourseName);
                }
                Console.WriteLine("Enter the Course ID number of the course to schedule");
                string IDAnswer = Console.ReadLine();
                //this part below should be a method, it would make this less lengthy
                int courseID;
                bool IDResult = int.TryParse(IDAnswer, out courseID);
                if (numberResult == false || courseID > 14 || courseID <= 0)
                {
                    Console.WriteLine("Enter a valid course ID");
                    DisplayMenu();//TODO: program can break here, come up with a way to start over if entry is invalid.  DisplayMenu() might work, might also cause recursion problems.  Needs testing.
                }
                else
                {
                    foreach (Cours c in dm.DBCourses)
                    {
                        if (courseID.CompareTo(c.CourseID) == 0)
                        {
                            courseToSchedule = c;
                        }
                    }
                }

                //Start of classroom selection---------------------TODO: This is going to need checking for room availability somehow-----------------------------------
                foreach (Classroom c in courseToSchedule.Classrooms)
                
                {
                    Console.WriteLine("Classroom ID: {0}, Classroom Number: {1}, Maximum students: {2}", c.ClassroomID, c.RoomNumber, c.RoomSize);
                }
                Console.WriteLine("Enter classroom ID");
                string ClassroomAnswer = Console.ReadLine();
                int roomID;
                bool roomIDResult = int.TryParse(ClassroomAnswer, out roomID);
                if (roomIDResult == false)//TODO: possible additional error checking for invalid room ID entries
                {
                    Console.WriteLine("Enter a valid course ID");
                }
                else
                {
                    foreach (Classroom c in courseToSchedule.Classrooms)
                    {
                        if (roomID.CompareTo(c.ClassroomID) == 0)
                        {
                            roomToSchedule = c;
                        }
                    }
                }
                //Start of time selection----------------TODO: This is going to need checking for time availability of room and the ability to block out a certain number of hours somehow------------------------------
                foreach (CourseTime ct in dm.DBTimes)
                {
                    Console.WriteLine("Time ID: {0}, Timeframe: {1}", ct.TimeID, ct.TimeFrame);
                }
                Console.WriteLine("Select a time ID");
                string timeAnswer = Console.ReadLine();
                int timeID;
                bool timeIDResult = int.TryParse(timeAnswer, out timeID);
                if (timeIDResult == false)//TODO: possible additional error checking for invalid time ID entries
                {
                    Console.WriteLine("Enter a valid time ID");
                }
                else
                {
                    foreach (CourseTime ct in dm.DBTimes)
                    {
                        if (timeID.CompareTo(ct.TimeID) == 0)
                        {
                            timeToSchedule = ct;
                        }
                    }
                }
                //Start of teacher selection-----------------------------TODO: Will need to check to see if teacher is available for times----------------------------
                //temporary list of teachers to compare their approved courses to
                List<Person> teacherList = new List<Person>();
                //filters teachers out into list to compare to
                foreach (Person p in people)
                {
                    if (p.IsTeacher == true)
                    {
                        teacherList.Add(p);
                    }
                }
                //checks each teacher's list of approved courses and prints that teacher if eligible to teach the course
                foreach (Teacher t in teacherList)
                {
                    if (t.ApprovedCourses.Contains(courseToSchedule))
                    {
                        Console.WriteLine("Teacher ID: {0}, teacher name: {1}", t.ID, t.Name);
                    }
                }
                Console.WriteLine("Enter ID of teacher to teach course");
                string teacherAnswer = Console.ReadLine();
                int teacherID;
                bool teacherIDResult = int.TryParse(teacherAnswer, out teacherID);
                if (teacherIDResult == false)//TODO: possible additional error checking for invalid teacher ID entries
                {
                    Console.WriteLine("Enter a valid teacher ID");
                }
                else
                {
                    foreach (Teacher t in teacherList)
                    {
                        if (teacherID.CompareTo(t.ID) == 0)
                        {
                            teacherToSchedule = t;
                        }
                    }
                }
                //Start of student selection-----------------------------TODO: will need to check to see if students are available for times------------------------------
                List<Person> studentList = new List<Person>();
                //keeps the list of students below at or below the maximum as determined by the roomsize
                studentList.Capacity = roomToSchedule.RoomSize; //TODO: need a check for this somewhere so program doesn't break when one above the capacity is added
                foreach (Person p in people)
                {
                    if (p.IsTeacher == false)
                    {
                        studentList.Add(p);
                    }
                }
                foreach (Person p in studentList)
                {
                    Console.WriteLine("Student ID: {0}, student name: {1}", p.ID, p.Name);
                }

                bool moreStudents = true;
                while (moreStudents == true)
                {
                    Console.WriteLine("Enter ID of a student to take course");
                    string studentAnswer = Console.ReadLine();
                    int studentID;
                    bool studentIDResult = int.TryParse(studentAnswer, out studentID);
                    if (studentIDResult == false)//TODO: possible additional error checking for invalid student ID entries
                    {
                        Console.WriteLine("Enter a valid student ID");
                    }
                    else
                    {
                        bool success = false;
                        foreach (Student s in studentList)
                        {
                            if (studentID.CompareTo(s.ID) == 0 && studentList.Contains(s) == false)
                            {
                                studentsToSchedule.Add(s);
                                Console.WriteLine("{0} added successfully!", s.Name);
                                success = true;
                            }
                        }
                        if (success == false)
                        {
                            Console.WriteLine("Invalid ID entered, student not added");
                        }
                        Console.WriteLine("{0} students in this class out of a possible {1}.  Add another student?\n1...Yes\n2...No", studentList.Count, studentList.Capacity);
                        string answer = Console.ReadLine();
                        switch (answer)
                        {
                            case "1":
                                if (studentList.Count == studentList.Capacity)
                                {
                                    Console.WriteLine("Class is full, no more students will be enrolled.");
                                    moreStudents = false;
                                }
                                else
                                {
                                    moreStudents = true;
                                }
                                break;
                            case "2":
                                moreStudents = false;
                                break;
                            default:
                                Console.WriteLine("Invalid input, please try again");
                                break;
                        }
                    }
                }
                //------------------start of displaying info---------------------------------------------------------------
                Console.WriteLine("\nSo here's what we have so far:");
                Console.WriteLine("Unique class number: {0}", numberOfCourseToSchedule);
                Console.WriteLine("Course name: {0}", courseToSchedule.CourseName);
                Console.WriteLine("Teacher's ID: {0}, Teacher's name: {1}", teacherToSchedule.ID, teacherToSchedule.Name);
                Console.WriteLine("Classroom ID: {0}, Classroom number: {1}, Classroom size: {2}", roomToSchedule.ClassroomID, roomToSchedule.RoomNumber, roomToSchedule.RoomSize);
                Console.WriteLine("Course time: {0}", timeToSchedule.TimeFrame);
                Console.WriteLine("Students enrolled:");
                foreach (Student s in studentList)
                {
                    Console.WriteLine("\tStudent's ID: {0}, Student's name: {1}", s.ID, s.Name);
                }
                Console.WriteLine("Would you like to schedule this class?\n1...Yes\n2...No");
                string scheduleClass = Console.ReadLine();
                switch (scheduleClass)
                {
                    case "1":
                        classItem newClass = new classItem(numberOfCourseToSchedule, timeToSchedule, teacherToSchedule, studentsToSchedule, roomToSchedule, courseToSchedule);
                        Console.WriteLine("Class successfully added!");
                        break;
                    case "2":
                        Console.WriteLine("Deleting data...");
                        DisplayMenu();//TODO: see if there's a better way to do this without causing stack overflow recursion issues here
                        break;
                    default:
                        Console.WriteLine("Invalid input, please try again");
                        break;
                }
            }
        }
    }
}
