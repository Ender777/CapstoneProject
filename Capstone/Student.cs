using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    class Student : Person
    {
        //constructor calls base for all fields
        public Student(string Name, string Phone, string Email, bool IsTeacher)
            : base(Name, Phone, Email, IsTeacher)
        {
        }

    }
}
