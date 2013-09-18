using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
//Still to do:
//3. Make it savable
//5. TODOs
//6. Error checking
namespace Capstone
{
    [Serializable]
    class Program
    {
        //create instances of items needed
        static personCollection people = new personCollection();
        static databaseManager dm = new databaseManager();
        static classCollection scheduledClasses = new classCollection();

        //bools for if a Teacher or Student has been added has been added to people
        static bool hasTeacher = false;
        static bool hasStudent = false;
        static bool hasClass = false;

        static void Main(string[] args)
        {
            Console.WriteLine("Loading...");
            //Try to call method to check for serialized data and load it if it exists
            try
            {
                loadPeopleSerializedData("people.dat");
                loadScheduledClassSerializedData("scheduledClasses.dat");
            }
            catch
            {
                Console.WriteLine("Didn't work");
            }

            //Call the method to load the database manager with info
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
                //Call method defined below based on choice input
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
                        Console.WriteLine("saving...");
                        saveAndQuit(people, "people.dat");
                        saveAndQuit(scheduledClasses, "scheduledClasses.dat");
                        programRunning = false;
                        break;
                    default:
                        Console.WriteLine("Invalid input, please try again");
                        break;
                }
            }
        }

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
            //variables for inputting students' info
            string name;
            //phone is a string because int wasn't big enough to hold high area code numbers
            string phone;
            string email;
            //check variable used to make sure numbers are entered for a phone number.  Ints ran into upper range problems with area codes that were too high so I used longs.
            long check;
            Console.Write("Enter Name\n");
            name = Console.ReadLine().Trim();
            Console.Write("Enter 10 digit Phone number\n");
            //store input data
            string input = Console.ReadLine();
            //checking for valid phone number length and type
            try
            {
                check = Convert.ToInt64(input);
            }
            catch
            {
                Console.WriteLine("Phone number must be 10 numerical digits");
                return;
            }
            if (input.ToString().Length != 10)
            {
                Console.WriteLine("Phone number entered is not 10 digits, please try again");
                return;
            }
            else
            {
                phone = input;
            }
            Console.Write("Enter Email address\n");
            email = Console.ReadLine().Trim();

            if (name != "" && email != "")
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
                Console.WriteLine("One or more fields left empty, please try again");
            }
        }

        //======================================method to add teacher==========================================
        public static void AddTeacher()
        {
            string name;
            string phone;
            string email;
            Courses approvedCourses = new Courses();
            long check;
            Console.Write("Enter Name\n");
            name = Console.ReadLine();
            Console.Write("Enter 10 digit Phone number\n");
            string input = Console.ReadLine();
            try
            {
                check = Convert.ToInt64(input);
            }
            catch
            {
                Console.WriteLine("Phone number must be 10 numerical digits");
                return;
            }
            if (input.ToString().Length != 10)
            {
                Console.WriteLine("Phone number entered is not 10 digits, please try again");
                return;
            }
            else
            {
                phone = input;
            }
            Console.Write("Enter Email address\n");
            email = Console.ReadLine();
            //display all course options
            if (name == "" || email == "")
            {
                Console.WriteLine("One or more fields left empty, please try again");
                return;
            }
            foreach (Cours c in dm.DBCourses)
            {
                Console.WriteLine("Course ID: {0}\tName: {1}", c.CourseID, c.CourseName);
            }

            //bool and while loop to keep adding courses until the user stops
            bool keepAdding = true;
            //loops through all courses comparing entered ID to courseIDs.  If a match is found, bool is set to true and if statement is entered, assigning match
            //to the list.  After this is completed, ask user if they want to add more classes and loop again.
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
                    return;
                }
                Console.WriteLine("Can this teacher teach another course?\n1...Yes\n2...No");
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
                    return;
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
                Console.WriteLine("You can't schedule a class until a student and a teacher have been added.");
                return;
            }

            //--------------------------display courses and get ID of course--------------------------------------------------
            foreach (Cours c in dm.DBCourses)
            {
                Console.WriteLine("Course ID: {0}\tName: {1}", c.CourseID, c.CourseName);
            }
            Console.WriteLine("Enter the Course ID number of the course to schedule");
            string IDAnswer = Console.ReadLine();
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
            //create a list of just teachers to scan through to make sure there's one that can teach selected course
            List<Person> tempList = new List<Person>();
            foreach (Person p in people)
            {
                if (p.IsTeacher == true)
                {
                    tempList.Add(p);
                }
            }
            //Make sure there's a teacher in the system that can teach the course or the program will loop indefinitely later on waiting for a valid teacher input that isn't in the system
            bool teacherExists = false;
            foreach (Teacher t in tempList)
            {
                if (t.ApprovedCourses.Contains(courseToSchedule))
                {
                    teacherExists = true;
                }
            }
            if (teacherExists == false)
            {
                Console.WriteLine("There is no teacher as yet able to teach this course in the system.  Unable to schedule course at this time");
                return;
            }

            //Start of time selection--------------------------------------------------------------------------------------------------------------------------------
            foreach (CourseTime ct in dm.DBTimes)
            {
                Console.WriteLine("Time ID: {0}, Timeframe: {1}", ct.TimeID, ct.TimeFrame);
            }
            Console.WriteLine("Select a time ID");
            string timeAnswer = Console.ReadLine();
            int timeID;
            bool timeIDResult = int.TryParse(timeAnswer, out timeID);
            if (timeIDResult == false)
            {
                Console.WriteLine("invalid time ID entered");
                return;
            }
            else if (courseToSchedule.HoursPerSession == 2 && timeID == 14)
            {
                Console.WriteLine("Scheduling a 2 hour class at that time is too late");
                return;
            }
            else if (courseToSchedule.HoursPerSession == 3 && (timeID == 13 || timeID == 14))
            {
                Console.WriteLine("Scheduling a 3 hour class at that time is too late");
                return;
            }
            else
            {
                //bool to determine if a match was found between input and database info
                bool validID = false;
                foreach (CourseTime ct in dm.DBTimes)
                {
                    if (timeID.CompareTo(ct.TimeID) == 0)
                    {
                        timeToSchedule = ct;
                        //if match was found, set success to true
                        validID = true;
                    }
                }
                if (validID == false)
                {
                    Console.WriteLine("The time ID entered did not match a valid option");
                    return;
                }
            }
            //Start of classroom selection--------------------------------------------------------
            bool validRoom = false;
            foreach (Classroom c in courseToSchedule.Classrooms)
            {

                //checks if those teachers that can teach the course are already teaching at that time
                if (c.TimesUsed.Contains(timeToSchedule))
                {
                    Console.WriteLine("{0} is already scheduled during that time", c.RoomNumber);
                }
                else
                {
                    Console.WriteLine("Classroom ID: {0}, Classroom Number: {1}, Maximum students: {2}", c.ClassroomID, c.RoomNumber, c.RoomSize); 
                    validRoom = true;
                }
            }
            if (validRoom == false)
            {
                Console.WriteLine("No rooms are available for the time selected");
                return;
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
            //Start of teacher selection---------------------------------------------------------------------------------------------------------
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
            teacherList.Sort();
            bool validTeacher = false;
            foreach (Teacher t in teacherList)
            {
                //checks each teacher's list of approved courses
                if (t.ApprovedCourses.Contains(courseToSchedule))
                {
                    //checks if those teachers that can teach the course are already teaching at that time
                    if (t.TimesUsed.Contains(timeToSchedule))
                    {
                        Console.WriteLine("{0} is already scheduled during that time", t.Name);
                    }
                    else
                    {
                        Console.WriteLine("Teacher ID: {0}, teacher name: {1}", t.ID, t.Name);
                        validTeacher = true;
                    }
                }
            }
            if (validTeacher == false)
            {
                Console.WriteLine("No teachers entered are able to teach at the time selected");
                return;
            }
            Console.WriteLine("Enter ID of teacher to teach course");
            string teacherAnswer = Console.ReadLine();
            int teacherID;
            bool teacherIDResult = int.TryParse(teacherAnswer, out teacherID);
            if (teacherIDResult == false)
            {
                Console.WriteLine("Enter a valid teacher ID");
                return;
            }
            else
            {
                bool validID = false;
                foreach (Teacher t in teacherList)
                {
                    if (teacherID.CompareTo(t.ID) == 0)
                    {
                        teacherToSchedule = t;
                        validID = true;
                    }
                }
                if (validID == false)
                {
                    Console.WriteLine("Entry did not match a valid teacher ID");
                    return;
                }
            }
            //Start of student selection-----------------------------------------------------------
            //temporary list of students to be able to manipulate only students
            List<Person> studentList = new List<Person>();
            //populate studentList with only students
            foreach (Person p in people)
            {
                if (p.IsTeacher == false)
                {
                    studentList.Add(p);
                }
            }
            studentList.Sort();
            bool validStudent = false;
            //display students' data
            foreach (Student s in studentList)
            {
                //checks if students are already taking a class at that time
                if (s.TimesUsed.Contains(timeToSchedule))
                {
                    Console.WriteLine("{0} is already scheduled during that time", s.Name);
                }
                else
                {
                    Console.WriteLine("Student ID: {0}, student name: {1}", s.ID, s.Name);
                    validStudent = true;
                }
            }
            if (validStudent == false)
            {
                Console.WriteLine("There are no students that can take this class at this time");
                return;
            }
            Console.WriteLine("Classroom selected can have {0} students", roomToSchedule.RoomSize);
            //while loop so we can add as many students as desired
            bool moreStudents = true;
            while (moreStudents == true && studentsToSchedule.Count <= roomToSchedule.RoomSize)
            {
                Console.WriteLine("Enter ID of a student to take course");
                string studentAnswer = Console.ReadLine();
                int studentID;
                bool studentIDResult = int.TryParse(studentAnswer, out studentID);
                if (studentIDResult == false)
                {
                    Console.WriteLine("Invalid entry");
                }
                else
                {
                    bool success = false;
                    foreach (Student s in studentList)
                    {
                        //if input matches a valid student AND isn't already in the studentsToSchedule list, add him/her
                        if (studentID.CompareTo(s.ID) == 0)// && studentsToSchedule.Contains(s) == false)
                        {
                            if (studentsToSchedule.Contains(s) == false)
                            {
                                studentsToSchedule.Add(s);
                                Console.WriteLine("{0} added successfully!", s.Name);
                                success = true;
                            }
                            else
                            {
                                Console.WriteLine("This student has already been added to this class");
                            }
                        }
                    }
                    if (success == false)
                    {
                        Console.WriteLine("Invalid ID entered, student not added");
                    }
                    Console.WriteLine("{0} students in this class out of a possible {1}.  Add another student?\n1...Yes\n2...No", studentsToSchedule.Count, roomToSchedule.RoomSize);
                    string answer = Console.ReadLine();
                    switch (answer)
                    {
                        case "1":
                            if (studentsToSchedule.Count >= roomToSchedule.RoomSize)
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
                            Console.WriteLine("Invalid input");
                            break;
                    }
                }
            }
            //------------------start of displaying info---------------------------------------------------------------
            Console.WriteLine("\nSo here's what we have so far:");
            Console.WriteLine("Course ID number: {0}", courseToSchedule.CourseID);
            Console.WriteLine("Course name: {0}", courseToSchedule.CourseName);
            Console.WriteLine("Teacher's ID: {0}, Teacher's name: {1}", teacherToSchedule.ID, teacherToSchedule.Name);
            Console.WriteLine("Classroom ID: {0}, Classroom number: {1}, Classroom size: {2}", roomToSchedule.ClassroomID, roomToSchedule.RoomNumber, roomToSchedule.RoomSize);
            Console.WriteLine("Course time: {0}", timeToSchedule.StartTime.ToString());
            Console.WriteLine("Students enrolled:");
            foreach (Student s in studentsToSchedule)
            {
                Console.WriteLine("\tStudent's ID: {0}, Student's name: {1}", s.ID, s.Name);
            }
            //double check info is correct and scheduling is desired
            Console.WriteLine("Would you like to schedule this class?\n1...Yes\n2...No");
            string scheduleClass = Console.ReadLine();
            //if yes, schedule class, if no, start over
            bool addClass = true;
            while (addClass == true)
            {
                switch (scheduleClass)
                {
                    case "1":
                        //create new classItem passing in all needed info to create a new class.
                        classItem newClass = new classItem(timeToSchedule, teacherToSchedule, studentsToSchedule, roomToSchedule, courseToSchedule);
                        scheduledClasses.Add(newClass);
                        //register class in the teacher's classItem collection
                        teacherToSchedule.CoursesWith.Add(newClass);

                        //register time used in the teacher's and students' coursetime collection
                        int ID = timeToSchedule.TimeID;
                        CourseTime usedTime = null;
                        //for loop to increment time ID equal to the number of hours in the class session
                        for (int i = 0; i < courseToSchedule.HoursPerSession; i++)
                        {
                            foreach (CourseTime ct in dm.DBTimes)
                            {
                                if (ID == ct.TimeID)
                                {
                                    usedTime = ct;
                                }
                            }
                            roomToSchedule.TimesUsed.Add(usedTime);
                            teacherToSchedule.TimesUsed.Add(usedTime);
                            foreach (Student st in studentsToSchedule)
                            {
                                st.TimesUsed.Add(usedTime);
                            }
                            ID++;
                        }
                        Console.WriteLine("Class successfully added!");
                        hasClass = true;
                        addClass = false;
                        break;
                    case "2":
                        Console.WriteLine("Deleting data...");
                        addClass = false;
                        break;
                    default:
                        Console.WriteLine("Invalid input, please try again");
                        scheduleClass = Console.ReadLine();
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
                    if (hasStudent == false)
                    {
                        Console.WriteLine("No students have been added, add a student first!");
                        return;
                    }
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
                            if (infoStudent == null)
                            {
                                Console.WriteLine("Invalid entry, please try again");
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
                    if (hasTeacher == false)
                    {
                        Console.WriteLine("No teachers have been added, add a teacher first!");
                        return;
                    }
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
                                if (teacherIDint.CompareTo(p.ID) == 0)
                                {
                                    infoTeacher = (Teacher)p;
                                }
                            }
                            if (infoTeacher == null)
                            {
                                Console.WriteLine("Invalid entry please try again");
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Invalid ID entered, enter a valid ID");
                        }
                    }
                    displayPersonInfo(infoTeacher);
                    displayTeacherInfo(infoTeacher);
                    break;
                case "3":
                    if (hasClass == false)
                    {
                        Console.WriteLine("No classes have been scheduled in any rooms yet, schedule a class first!");
                        return;
                    }
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
                                if (roomIDint.CompareTo(c.ClassroomID) == 0)
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
                    if (hasClass == false)
                    {
                        Console.WriteLine("No classes have been scheduled yet, schedule a class first!");
                        return;
                    }
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
                                if (classIDint.CompareTo(ci.ItemNumber) == 0)
                                {
                                    infoClass = ci;
                                }
                            }
                            if (infoClass == null)
                            {
                                Console.WriteLine("Invalid entry please try again");
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
            if (s.CoursesWith.Count == 0)
            { Console.WriteLine("Student is not enrolled in any classes"); }
            else
            {
                Console.WriteLine("Information regarding courses this student is taking:");
                foreach (classItem ci in s.CoursesWith)
                {
                    Console.WriteLine("\tCourse name: {0}\n\tTeacher name: {1}\n\tClassroom number: {2}\n\tCourse time: {3}", ci.Course.CourseName, ci.Teacher.Name, ci.Room.RoomNumber, ci.CourseTimes.TimeFrame);
                }
            }
        }
        //displays teacher-specific information
        public static void displayTeacherInfo(Teacher t)
        {
            if (t.CoursesWith.Count == 0)
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
                    Console.WriteLine("\tCourse name: {0}\n\tClassroom number: {1}\n\tCourse time: {2}\n\tNumber of students registered: {3}", ci.Course.CourseName, ci.Room.RoomNumber, ci.CourseTimes.TimeFrame, ci.EnrolledStudents.Count);
                }
            }
        }
        //displays classroom information
        public static void displayClassroomInfo(Classroom c)
        {
            Console.WriteLine("ID: {0}, Room number: {1}, Room size: {2}", c.ClassroomID, c.RoomNumber, c.RoomSize);
            Console.WriteLine("Information regarding courses scheduled in this room:");
            bool classWasScheduled = false;
            foreach(classItem ci in scheduledClasses)
            {
                if (c.ClassroomID.Equals(ci.Room.ClassroomID))
                {
                    Console.WriteLine("\tCourse name: {0}\n\tTeacher name: {1}\n\tNumber of students enrolled: {2}", ci.Course.CourseName, ci.Teacher.Name, ci.EnrolledStudents.Count);
                    classWasScheduled = true;
                }
            }
            if (classWasScheduled == false)
            {
                Console.WriteLine("No courses have been scheduled in this room");
            }
        }
        //displays course information
        public static void displayCourseInfo(classItem ci)
        {
            Console.WriteLine("ID: {0}, Course name: {1}\nTeacher's ID: {2}\nTeacher's name: {3}\nClassroom ID: {4}\nClassroom number: {5}\nClassroom size: {6}\nCourse time: {7}", ci.ItemNumber, ci.Course.CourseName, ci.Teacher.ID, ci.Teacher.Name, ci.Room.ClassroomID, ci.Room.RoomNumber, ci.Room.RoomSize, ci.CourseTimes.TimeFrame);
            Console.WriteLine("Students enrolled:");
            foreach (Student s in ci.EnrolledStudents)
            {
                Console.WriteLine("\tID: {0}, Name: {1}", s.ID, s.Name);
            }
        }
        //save and quit method definition
        static void saveAndQuit(object objGraph, string fileName)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (Stream fstream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                
                bf.Serialize(fstream, objGraph);
            }
        }
        //method to load serialized people data
        static void loadPeopleSerializedData(string fileName)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (Stream fstream = File.OpenRead(fileName))
            {
                personCollection p = (personCollection)bf.Deserialize(fstream);
                people = p;
            }
        }
        //method to load serialized scheduled class data
        static void loadScheduledClassSerializedData(string fileName)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (Stream fstream = File.OpenRead(fileName))
            {
                classCollection cc = (classCollection)bf.Deserialize(fstream);
                scheduledClasses = cc;
            }
        }
    }
}
