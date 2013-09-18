using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    [Serializable]
    public class Student : Person
    {
        //constructor calls base for all fields
        public Student(string Name, string Phone, string Email, bool IsTeacher, List<classItem> CoursesWith)
            : base(Name, Phone, Email, IsTeacher, CoursesWith)
        {
        }

    }
}
