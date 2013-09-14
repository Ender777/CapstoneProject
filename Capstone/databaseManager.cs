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
        private List<Classroom> dbClassrooms = new List<Classroom>();
        private Courses dbCourses = new Courses();
        private List<CourseTime> dbTimes = new List<CourseTime>();
        //connection context
        ClassroomSchedulerEntities1 context = new ClassroomSchedulerEntities1();

        public List<Classroom> DBClassrooms
        {
            get
            {
                return dbClassrooms;
            }
        }

        public Courses DBCourses
        {
            get
            {
                return dbCourses;
            }
        }

        public List<CourseTime> DBTimes
        {
            get
            {
                return dbTimes;
            }
        }

        //Method to connect to SQL Server info and retrieve data
        public void ConnectToSQL()
        {
            //store data from db into a var and put it into capstone classes
            var classroomOptions = from rooms in context.Classrooms
                                   orderby rooms.ClassroomID
                                   select rooms;

            foreach (Classroom c in classroomOptions)
            {
                DBClassrooms.Add(c);
            }

            //store data from db into a var and put it into capstone classes
            var courseOptions = from course in context.Courses
                                orderby course.CourseID
                                select course;
                                            
            foreach (Cours co in courseOptions)
            {
                DBCourses.Add(co);
            }

            var timeOptions = from times in context.CourseTimes
                              orderby times.TimeID
                              select times;

            foreach (CourseTime ct in timeOptions)
            {
                DBTimes.Add(ct);
            }

        }
    }
}
