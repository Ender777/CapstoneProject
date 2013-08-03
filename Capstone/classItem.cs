﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    class classItem
    {
        //fields that make a class item
        //needs to be unique
        private int itemNumber;
        private List<CourseTime> courseTimes;
        private Teacher teacher;
        //no duplicates
        private List<Student> enrolledStudents;
        private Classroom room;
        private Cours course;

        //properties
        public int ItemNumber
        {
            get
            {
                return itemNumber;
            }
            //SHOULD THIS ONE BE PRIVATE?
            set
            {
                itemNumber = value;
            }
        }
        public List<CourseTime> CourseTimes
        {
            get
            {
                return courseTimes;
            }
        }
        public Teacher Teacher
        {
            get
            {
                return teacher;
            }
        }
        public List<Student> EnrolledStudents
        {
            //logic to keep duplicates out
            get
            {
                return enrolledStudents;
            }
        }
        public Classroom Room
        {
            get
            {
                return room;
            }
        }
        public Cours Course
        {
            get
            {
                return course;
            }
        }

        //constructor
        public classItem(int ItemNumber, List<CourseTime> CourseTimes, Teacher Teacher, List<Student> EnrolledStudent, Classroom Room, Cours Course)
        {
            itemNumber = ItemNumber;
            courseTimes = CourseTimes;
            teacher = Teacher;
            enrolledStudents = EnrolledStudents;
            room = Room;
            course = Course;
        }
    }
}
