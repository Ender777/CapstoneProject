using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    //class to hold all the courses from the database
    public class Courses:IEnumerable<Cours>
    {
        //auto generated code from edmx named it Cours instead of Course
        private List<Cours> courseCollection = new List<Cours>();

        public List<Cours> CourseCollection
        {
            get
            {
                return courseCollection;
            }
        }
        //method to add courses to collection
        public void Add(Cours c)
        {
            CourseCollection.Add(c);
        }

        //implementing IEnumerable
        public IEnumerator<Cours> GetEnumerator()
        {
            return CourseCollection.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return CourseCollection.GetEnumerator();
        }

    }
}
