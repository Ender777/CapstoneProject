using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    public class classItem
    {
        //fields that make a class item
        private int itemNumber;
        private CourseTime courseTimes;
        private Teacher teacher;
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
            //TODO: SHOULD THIS ONE BE PRIVATE?
            set
            {
                itemNumber = value;
            }
        }
        public CourseTime CourseTimes
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
        public classItem(int ItemNumber, CourseTime CourseTimes, Teacher Teacher, List<Student> EnrolledStudent, Classroom Room, Cours Course)
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
