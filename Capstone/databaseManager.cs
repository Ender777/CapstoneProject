using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
//using System.Data.Sql;
//using System.Data.SqlTypes;


namespace Capstone
{
    //class to establish connection to database and process data
    class databaseManager
    {
        //members to hold database info.
        List<Classroom> dbClassrooms = new List<Classroom>();
        public Courses dbCourses = new Courses();
        List<CourseTime> dbTimes = new List<CourseTime>();

        //Method to connection to SQL Server info and retrieve data
        public void ConnectToSQL()
        {
            //connection context
            ClassroomSchedulerEntities context = new ClassroomSchedulerEntities();

            //store data from db into a var and put it into capstone classes
            var classroomOptions = from rooms in context.Classrooms
                                   orderby rooms.ClassroomID
                                   select rooms;

            foreach (Classroom c in classroomOptions)
            {
                dbClassrooms.Add(c);
            }

            //store data from db into a var and put it into capstone classes
            var courseOptions = from course in context.Courses
                                orderby course.CourseID
                                select course;
                                            
            foreach (Cours co in courseOptions)
            {
                dbCourses.Add(co);
            }

            var timeOptions = from times in context.CourseTimes
                              orderby times.TimeID
                              select times;

            foreach (CourseTime ct in timeOptions)
            {
                dbTimes.Add(ct);
            }



            ////connection to database
            //SqlConnection conn = new SqlConnection("Data Source=HAL\\SQLEXPRESS;Initial Catalog=ClassSchedule;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False");
            ////store the command in a string
            //string roomNumberCommand = "SELECT RoomNumber FROM Classrooms";
            ////create a SqlCommand using the string
            //SqlCommand cmd = new SqlCommand(roomNumberCommand, conn);
            //try
            //{
            //    conn.Open();
            //    //testing connection, should read and dump a column to console window
            //    SqlDataReader dataReader = cmd.ExecuteReader();
            //    while (dataReader.Read())
            //    {
            //        Console.WriteLine(dataReader.GetValue(0));
            //    }
            //    dataReader.Close();

            //}
            //catch (Exception)//TODO: edit this exception handling later
            //{
            //    Console.WriteLine("Failed to connect to data source");
            //}
            //finally
            //{
            //    conn.Close();
            //}
        }
    }
}
