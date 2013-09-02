using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    //class to hold list of people
    public class personCollection : IEnumerable<Person>
    {
        //list that maintains Students and Teachers
        private List<Person> people = new List<Person>();

        //property
        public List<Person> People
        {
            get
            {
                return people;
            }
        }
        //TODO: see if I can move duplicate checking logic here from program.cs
        //add method for people
        public void Add(Person person)
        {
            //ID setting logic for if there are people in the peopleCollection
            if (People.Count != 0)
            {
                Person highestID = People.OrderByDescending(p => p.ID).First();
                person.SetID(highestID.ID + 1);
            }
            //if first person entered, add person
            else
            {
                person.SetID(1);
            }
            People.Add(person);
            People.Sort();
        }

        //implementing IEnumerable so I can use a foreach---------------------------------
        public IEnumerator<Person> GetEnumerator()
        {
            return People.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return People.GetEnumerator();
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
