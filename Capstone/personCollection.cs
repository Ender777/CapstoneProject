using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    //class to hold people
    //THINK THIS ELIMINATES THE NEED FOR DIRECTORY CLASSES?
    public class personCollection : IEnumerable<Person>
    {
        //list that maintains Students and Teachers
        private List<Person> People = new List<Person>();

        public List<Person> people
        {
            get
            {
                return People;
            }
        }
        //add method for people
        public void Add(Person person)
        {
            //ID setting logic for if there are people in the peopleCollection
            if (people.Count != 0)
            {
                Person highestID = people.OrderByDescending(p => p.ID).First();
                person.SetID(highestID.ID + 1);
            }
            //if first person entered, add person
            else
            {
                person.SetID(1);
            }
            people.Add(person);
            people.Sort();
        }

        //implementing IEnumerable so I can use a foreach
        public IEnumerator<Person> GetEnumerator()
        {
            return people.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return people.GetEnumerator();
        }

        //This is the implementation of IEnumerable from Jonathan's project.  Don't know why it's different.
        //IEnumerator<Person> IEnumerable<Person>.GetEnumerator()
        //{
        //    if (people.Count == 0)
        //    {
        //        yield break;
        //    }

        //    for (int i = 0; i < people.Count; i++)
        //    {
        //        yield return people[i];
        //    }
        //}
        ////GetEnumerator here
        //System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        //{
        //    throw new NotImplementedException("This Method is Not Supported");
        //}
    }
}
