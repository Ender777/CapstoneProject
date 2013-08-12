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
        //bools for if a Teacher or Student has been added has been added to people
        static bool hasTeacher = false;
        static bool hasStudent = false;
        static Courses courses = new Courses();
        static databaseManager dm = new databaseManager();
        static List<Classroom> classrooms;
        static List<CourseTime> courseTimes;



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
                Student s = new Student(name, phone, email);
                if (people.Contains(s))
                {
                    Console.WriteLine("The name, phone number, or email entered is already taken, please enter a new one");
                }
                else
                {
                    people.Add(s);
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
                Console.WriteLine("Add another class?\n1...yes\n2...no");
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
                Teacher t = new Teacher(name, phone, email, approvedCourses);
                if (people.Contains(t))
                {
                    Console.WriteLine("The name, phone number, or email entered is already taken, please enter a new one");
                }
                else
                {
                    people.Add(t);
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
            Cours courseToSchedule = null;

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
                    }
                    else
                    {
                        numberList.Add(number);
                    }
                }
                //display courses and get ID of course
                foreach (Cours c in dm.DBCourses)
                {
                    Console.WriteLine("Course ID: {0}\tName: {1}", c.CourseID, c.CourseName);
                }
                Console.WriteLine("Enter the number of the course to schedule");
                string IDAnswer = Console.ReadLine();
                int ID;
                bool IDResult = int.TryParse(IDAnswer, out ID);
                if (numberResult == false || ID > 14)
                {
                    Console.WriteLine("Enter a valid course ID");
                }

                foreach (Cours c in dm.DBCourses)
                {
                    if (ID.CompareTo(c.CourseID) == 0)
                    {
                        courseToSchedule = c;
                    }
                }
                Console.WriteLine(courseToSchedule.Classrooms);
                //List<Classroom> approvedRooms = dm.classroomQuery(courseToSchedule);
                //foreach (Classroom cr in approvedRooms)
                //{
                //    Console.WriteLine("ID: {0}, Room Number: {1}, Size: {2}", cr.ClassroomID, cr.RoomNumber, cr.RoomSize);
                //}
                Console.WriteLine("Enter classroom ID");
                Console.ReadLine();
                
            }
        }
    }
}
