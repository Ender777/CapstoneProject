using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    class Teacher : Person
    {
        public Teacher(int ID, string Name, string Phone, string Email)
            : base(ID, Name, Phone, Email)
        {
            //list of courses for each teacher
            List<string> courses = new List<string>(); //TODO: this may need to change from strings to course items when I get those up and running


            //TODO: need a method that reads courses teachers can teach from database and adds them to the list for each teacher item
        }
    }
}
