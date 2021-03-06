﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    [Serializable]
    public class Teacher : Person
    {
        //field unique to teachers
        private Courses approvedCourses;

        //property unique to teachers
        public Courses ApprovedCourses
        {
            get
            {
                return approvedCourses;
            }
        }

        //constructor calling base class for common fields and implementing unique one
        public Teacher(string Name, string Phone, string Email, bool IsTeacher, List<classItem> CoursesWith, Courses ApprovedCourses)
            : base(Name, Phone, Email, IsTeacher, CoursesWith)
        {
            approvedCourses = ApprovedCourses;
        }
    }
}
