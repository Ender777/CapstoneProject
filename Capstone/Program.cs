using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Still to do:    1. Add coursetimes collections to Person and Classroom and somehow schedule things when they aren't in use.
//2. Finish the displaying of the desired information.  Need times to work properly to do any more on this.
//3. Make it savable
//4. Checklist for features and requirements
//5. TODOs
//6. Error checking

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
                        lookUpInfo();
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

        //=====================================================method to add student==================================================
        public static void AddStudent()
        {
            string name;
            string phone; //TODO: This may need to be an int or at least have error checking for a phone NUMBER not letters
            string email;

            Console.Write("Enter Name\n");
            name = Console.ReadLine();
            Console.Write("Enter Phone number\n");
            phone = Console.ReadLine();
            Console.Write("Enter Email address\n");
            email = Console.ReadLine();

            if (name != "" && phone != "" && email != "")
            {
                //create an empty classItem list to pass to constructor to be used when scheduling classes
                List<classItem> classesWith = new List<classItem>();
                //set bool value to false on all students
                bool isTeacher = false;
                Student s = new Student(name, phone, email, isTeacher, classesWith);
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

        //======================================method to add teacher==========================================
        public static void AddTeacher()
        {
            string name;
            string phone; //TODO: This may need to be an int or at least have error checking for a phone NUMBER not letters
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
                string answer = Console.ReadLine();
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
                //create empty list of classes to be used when scheduling a class
                List<classItem> classesWith = new List<classItem>();
                //set bool value to true on all teachers
                bool isTeacher = true;
                Teacher t = new Teacher(name, phone, email, isTeacher, classesWith, approvedCourses);
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



        //=======================================method to schedule a class===================================================



        public static void ScheduleClass()
        {
            //declare variables to pass to classItem constructors here.
            Cours courseToSchedule = null;
            Classroom roomToSchedule = null;
            CourseTime timeToSchedule = null;
            Teacher teacherToSchedule = null;
            List<Student> studentsToSchedule = new List<Student>();

            if (hasTeacher == false || hasStudent == false)//condition to check if student and teacher have been created
            {
                Console.WriteLine("You can't schedule a class until a student and a teacher has been added.");
                return;
            }

            //--------------------------display courses and get ID of course--------------------------------------------------
            //TODO: This needs checking to make sure desired course has an eligible teacher!
            foreach (Cours c in dm.DBCourses)
            {
                Console.WriteLine("Course ID: {0}\tName: {1}", c.CourseID, c.CourseName);
            }
            Console.WriteLine("Enter the Course ID number of the course to schedule");
            string IDAnswer = Console.ReadLine();
            //this part below should be a method, it would make this less lengthy
            int courseID;
            bool IDResult = int.TryParse(IDAnswer, out courseID);
            if (IDResult == false || courseID > 14 || courseID <= 0)
            {
                Console.WriteLine("Invalid ID entered");
                return;
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
            if (roomIDResult == false)
            {
                Console.WriteLine("Enter a valid course ID");
                return;
            }
            else
            {
                bool success = false;
                Classroom temp = null;
                foreach (Classroom c in courseToSchedule.Classrooms)
                {
                    if (roomID.CompareTo(c.ClassroomID) == 0)
                    {
                        success = true;
                        temp = c;
                    }
                }
                if (success == true)
                {
                    roomToSchedule = temp;
                }
                else
                {
                    Console.WriteLine("Entry did not match a valid room ID");
                    return;
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
                return;
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
            //temporary list of teachers to access teacher-specific approvedCourses list
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
            //int teacher = parseInput(teacherAnswer);
            int teacherID;
            bool teacherIDResult = int.TryParse(teacherAnswer, out teacherID);
            if (teacherIDResult == false)//TODO: possible additional error checking for invalid teacher ID entries
            {
                Console.WriteLine("Enter a valid teacher ID");
                return;
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
            //temporary list of students to be able to manipulate only students
            List<Person> studentList = new List<Person>();
            //keeps the list of students below at or below the maximum as determined by the roomsize
            studentList.Capacity = roomToSchedule.RoomSize;
            //populate studentList with only students
            foreach (Person p in people)
            {
                if (p.IsTeacher == false)
                {
                    studentList.Add(p);
                }
            }
            //display students' data
            foreach (Person p in studentList)
            {
                Console.WriteLine("Student ID: {0}, student name: {1}", p.ID, p.Name);
            }
            //while loop so we can add as many students as desired
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
                    return;
                }
                else
                {
                    bool success = false;
                    foreach (Student s in studentList)
                    {
                        //if input matches a valid student AND isnt' already in the studentsToSchedule list, add him/her
                        if (studentID.CompareTo(s.ID) == 0 && studentsToSchedule.Contains(s) == false)
                        {
                            studentsToSchedule.Add(s);
                            Console.WriteLine("{0} added successfully!", s.Name);
                            success = true;
                        }
                    }
                    if (success == false)
                    {
                        Console.WriteLine("Invalid ID entered, student not added");
                        return;
                    }
                    Console.WriteLine("{0} students in this class out of a possible {1}.  Add another student?\n1...Yes\n2...No", studentsToSchedule.Count, studentList.Capacity);
                    string answer = Console.ReadLine();
                    switch (answer)
                    {
                        case "1":
                            if (studentsToSchedule.Count == studentList.Capacity)
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
            Console.WriteLine("Course name: {0}", courseToSchedule.CourseName);
            Console.WriteLine("Teacher's ID: {0}, Teacher's name: {1}", teacherToSchedule.ID, teacherToSchedule.Name);
            Console.WriteLine("Classroom ID: {0}, Classroom number: {1}, Classroom size: {2}", roomToSchedule.ClassroomID, roomToSchedule.RoomNumber, roomToSchedule.RoomSize);
            Console.WriteLine("Course time: {0}", timeToSchedule.TimeFrame);
            Console.WriteLine("Students enrolled:");
            foreach (Student s in studentsToSchedule)
            {
                Console.WriteLine("\tStudent's ID: {0}, Student's name: {1}", s.ID, s.Name);
            }
            //double check info is correct and scheduling is desired
            Console.WriteLine("Would you like to schedule this class?\n1...Yes\n2...No");
            string scheduleClass = Console.ReadLine();
            //if yes, schedule class, if no, delete data and start over
            bool addClass = true;
            while (addClass == true)
            {
                switch (scheduleClass)
                {
                    case "1":
                        //create new classItem passing in all needed info to create a new class.
                        classItem newClass = new classItem(timeToSchedule, teacherToSchedule, studentsToSchedule, roomToSchedule, courseToSchedule);
                        scheduledClasses.Add(newClass);
                        //register that class in each scheduled students' classItem collection
                        foreach (Student st in studentsToSchedule)
                        {
                            st.CoursesWith.Add(newClass);
                        }
                        //register class in the teachers classItem collection
                        teacherToSchedule.CoursesWith.Add(newClass);
                        Console.WriteLine("Class successfully added!");
                        addClass = false;
                        break;
                    case "2":
                        Console.WriteLine("Deleting data...");
                        addClass = false;
                        break;
                    default:
                        Console.WriteLine("Invalid input, please try again");
                        break;
                }
            }

        }

        //===================================method to look up data=============================================

        public static void lookUpInfo()
        {
            Console.WriteLine("Enter what you would like to look up data for");
            Console.WriteLine("1...Student");
            Console.WriteLine("2...Teacher");
            Console.WriteLine("3...Classroom");
            Console.WriteLine("4...Scheduled course");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    //display all students
                    foreach (Person p in people)
                    {
                        if (p.IsTeacher == false)
                        {
                            Console.WriteLine("ID: {0}, Name: {1}", p.ID, p.Name);
                        }
                    }
                    //get the student to look up info for
                    Console.WriteLine("Enter the ID of the student whose information you want displayed");
                    //student variable to store response in and use to pass to methods to extract information
                    Student infoStudent = null;
                    //while input is invalid and unparsable, keep asking for valid input
                    while (infoStudent == null)
                    {
                        string studentID = Console.ReadLine();
                        try
                        {
                            int studentIDint = int.Parse(studentID);
                            foreach (Person p in people)
                            {
                                if (studentIDint.CompareTo(p.ID) == 0)
                                {
                                    infoStudent = (Student)p;
                                }
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Invalid ID entered, enter a valid ID");
                        }
                    }
                    //pass the student to the displayPersonInfo method
                    displayPersonInfo(infoStudent);
                    displayStudentInfo(infoStudent);
                    break;
                case "2":
                    //display all teachers
                    foreach (Person p in people)
                    {
                        if (p.IsTeacher == true)
                        {
                            Console.WriteLine("ID: {0}, Name: {1}", p.ID, p.Name);
                        }
                    }
                    //get the ID of the teacher to look up info for
                    Console.WriteLine("Enter the ID of the teacher whose information you want displayed");
                    //teacher variable to store response in and use to pass to methods to extract information
                    Teacher infoTeacher = null;
                    while (infoTeacher == null)
                    {
                        string teacherID = Console.ReadLine();
                        try
                        {
                            int teacherIDint = int.Parse(teacherID);
                            foreach (Person p in people)
                            {
                                if (teacherID.CompareTo(p.ID) == 0)
                                {
                                    infoTeacher = (Teacher)p;
                                }
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Invalid ID entered");
                        }
                    }
                    displayPersonInfo(infoTeacher);
                    displayTeacherInfo(infoTeacher);
                    break;
                case "3":
                    foreach (Classroom c in dm.DBClassrooms)
                    {
                        Console.WriteLine("Room ID: {0}, Room number: {1}", c.ClassroomID, c.RoomNumber);
                    }
                    Console.WriteLine("Enter ID of room to look up information for");
                    //Classroom variable to store response in and use to pass to methods to extract information
                    Classroom infoRoom = null;
                    while (infoRoom == null)
                    {
                        string roomID = Console.ReadLine();
                        try
                        {
                            int roomIDint = int.Parse(roomID);
                            foreach (Classroom c in dm.DBClassrooms)
                            {
                                if (roomID.CompareTo(c.ClassroomID) == 0)
                                {
                                    infoRoom = c;
                                }
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Invalid ID entered");
                        }
                    }
                    displayClassroomInfo(infoRoom);
                    break;
                case "4":
                    foreach (classItem ci in scheduledClasses)
                    {
                        Console.WriteLine("Item number: {0}, Course name: {1}", ci.ItemNumber, ci.Course.CourseName);
                    }
                    Console.WriteLine("Enter the ID of the course to look up information for");
                    classItem infoClass = null;
                    while (infoClass == null)
                    {
                        string classID = Console.ReadLine();
                        try
                        {
                            int classIDint = int.Parse(classID);
                            foreach (classItem ci in scheduledClasses)
                            {
                                if (classID.CompareTo(ci.ItemNumber) == 0)
                                {
                                    infoClass = ci;
                                }
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Invalid ID entered");
                        }
                    }
                    displayCourseInfo(infoClass);
                    break;
                default:
                    Console.WriteLine("Invalid input");
                    break;
            }
        }
        //-----------helper methods for looking up data---------------------------------------------------

        //displays common info between teacher and student
        public static void displayPersonInfo(Person p)
        {
            Console.WriteLine("ID: {0}, Name: {1}, Phone number: {2}, Email: {3}", p.ID, p.Name, p.Phone, p.Email);
        }
        //displays student-specific information
        public static void displayStudentInfo(Student s)
        {
            if (s.CoursesWith == null)
            { Console.WriteLine("Student is not enrolled in any classes"); }
            else
            {
                Console.WriteLine("Information regarding courses this student is taking:");
                foreach (classItem ci in s.CoursesWith)
                {
                    Console.WriteLine("\tCourse name: {0}, Teacher name: {1}, Classroom number: {2}, Course time: {3}", ci.Course.CourseName, ci.Teacher.Name, ci.Room.RoomNumber, ci.CourseTimes.TimeFrame);
                }
            }
        }
        //displays teacher-specific information
        public static void displayTeacherInfo(Teacher t)
        {
            if (t.CoursesWith == null)
            { Console.WriteLine("Teacher is not scheduled to teach any classes"); }
            else
            {
                Console.WriteLine("List of courses this teacher is approved to teach:");
                foreach (Cours c in t.ApprovedCourses)
                {
                    Console.WriteLine("\t{0}", c.CourseName);
                }
                Console.WriteLine("Information regarding courses this teacher is teaching:");
                foreach (classItem ci in t.CoursesWith)
                {
                    Console.WriteLine("\tCourse name: {0}, Classroom number: {1}, Course time: {2}, Number of students registered {3}", ci.Course.CourseName, ci.Room.RoomNumber, ci.CourseTimes.TimeFrame, ci.EnrolledStudents.Count.ToString());
                }
            }
        }
        //displays classroom information
        public static void displayClassroomInfo(Classroom c)
        {
            Console.WriteLine("ID: {0}, Room number: {1}, Room size: {2}", c.ClassroomID, c.RoomNumber, c.RoomSize);
            //TODO: fill this out once I get the courseTimes thing figured out correctly
            //foreach()
            //{

            //}
        }
        //displays course information
        public static void displayCourseInfo(classItem ci)
        {
            Console.WriteLine("ID: {0}, Course name: {1}, Teacher's ID: {2}, Teacher's name: {3}, Classroom ID: {4}, Classroom number: {5}, Classroom size: {6}, Course time: {7}", ci.ItemNumber, ci.Course.CourseName, ci.Teacher.ID, ci.Teacher.Name, ci.Room.ClassroomID, ci.Room.RoomNumber, ci.Room.RoomSize, ci.CourseTimes.TimeFrame);
            Console.WriteLine("Students enrolled:");
            foreach (Student s in ci.EnrolledStudents)
            {
                Console.WriteLine("\tID: {0}, Name: {1}", s.ID, s.Name);
            }
        }
    }
}
