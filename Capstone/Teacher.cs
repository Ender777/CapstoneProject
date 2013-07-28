using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    //TODO: need to establish other Classes first, like Classes, Courses, ClassItems
    class Teacher : Person
    {
        public Teacher(string Name, string Phone, string Email)
            : base(Name, Phone, Email)
        {
            //list of courses for each teacher
            //List<string> courses;


            //TODO: need a method that reads courses teachers can teach from database
            //and adds them to the list for each teacher item.  This might go in the
            //databaseManager class?
        }
    }
}
