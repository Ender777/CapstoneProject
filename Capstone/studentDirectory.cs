using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    class studentDirectory
    {
        //collection of teacher objects
        List<Student> studentList = new List<Student>();

        //add method for teacher
        public void addTeacher(Student s)
        {
            studentList.Add(s);
            studentList.Sort();
        }

        //find method for teacher
        public void findStudent(Student student)
        {
            foreach (Student s in studentList)
            {
                //TODO: logic to find a student and display their info in the console window
            }
        }

    }
}
