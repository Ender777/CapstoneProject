using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    class teacherDirectory
    {
        //collection of teacher objects
        List<Teacher> teacherList = new List<Teacher>();

        //add method for teacher
        public void addTeacher(Teacher t)
        {
            teacherList.Add(t);
            teacherList.Sort();
        }

        //find method for teacher
        public void findTeacher(Teacher teacher)
        {
            foreach (Teacher t in teacherList)
            {
                //TODO: logic to find a teacher and display their info in the console window
            }
        }
    }
}
