using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//WORKING ON: There's a disconnect between the databaseManager class and the program class.
//the data isn't being loaded when I try and display the courses available.

namespace Capstone
{
    class Program
    {
        //create instances of collections needed
        static personCollection people = new personCollection();
        static Courses courses = new Courses();

        static void Main(string[] args)
        {
            Console.WriteLine("Loading...");
            //create and call databaseManager to load data from db upon startup
            databaseManager dm = new databaseManager();
            dm.ConnectToSQL();

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
                }
            }
        }

        //helper methods
        //method to display the main options
        public static void DisplayMenu()
        {
            Console.WriteLine("Welcome to the program!");
            Console.WriteLine("Here are your options...");
            Console.WriteLine("1...add a student");
            Console.WriteLine("2...add a teacher");
            Console.WriteLine("3...schedule a class");
            Console.WriteLine("4...look up some info");
        }

        //method to add student
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

            if (name != null && phone != null && email != null)
            {
                Student s = new Student(name, phone, email);
                people.Add(s);
                Console.WriteLine("{0} added successfully!", s.Name);
            }
            else
            {
                Console.Write("Error inputting information, please try again");
            }
        }

        //method to add teacher
        public static void AddTeacher()
        {
            string name;
            string phone;
            string email;
            Courses tempList = new Courses();

            Console.Write("Enter Name\n");
            name = Console.ReadLine();
            Console.Write("Enter Phone number\n");
            phone = Console.ReadLine();
            Console.Write("Enter Email address\n");
            email = Console.ReadLine();
            Console.Write("Enter Course ID for course this teacher can teach");
            //display all course options
            foreach (Cours c in courses)
            {
                Console.WriteLine("Course ID: {0}\tName: {1}", c.CourseID, c.CourseName);
            }

            //bool and while loop to keep adding courses until the user stops
            bool keepAdding = true;
            while (keepAdding)
            {
                Console.WriteLine("Enter ID of course to add\n");
                int id = int.Parse(Console.ReadLine());


                //loops through all courses comparing entered ID to courseIDs.  If a match
                //is found, bool is set to true and if statement is entered, assigning match
                //to the list.  After this is completed, ask user if they want to add more 
                //classes and loop again.
                bool match = false;
                Cours matchedCourse = null;
                foreach (Cours c in courses)
                {
                    if (c.CourseID == id)
                    {
                        match = true;
                        matchedCourse = c;
                    }
                }

                if (match == true)
                {
                    tempList.Add(matchedCourse);
                }
                else
                {
                    Console.WriteLine("Incorrect ID entered");
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
            if (tempList.CourseCollection.Count != 0)
            {
                Teacher t = new Teacher(name, phone, email, tempList);
                people.Add(t);
                Console.WriteLine("{0} added Successfully!", t.Name);
            }
            else
            {
                Console.WriteLine("No courses were successfully added that this teacher can teach.  Teacher was not added.");
            }
        }
    }
}
