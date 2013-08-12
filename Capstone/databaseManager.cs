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

        //Method to connection to SQL Server info and retrieve data
        public void ConnectToSQL()
        {
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

        }
        public List<Classroom> classroomQuery(Cours c)
        {
            //this is probably all wrong.  Having a tough time with this one.
            var validRooms = from classes in context.Classrooms
                             join courses in context.Courses on classes.ClassroomID equals courses.CourseID
                             select classes.ClassroomID;

            foreach (int i in validRooms)
            {

            }
        }
    }
}
