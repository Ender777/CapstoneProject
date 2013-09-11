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
    }
}