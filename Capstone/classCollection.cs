using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    //class to store classItems
    class classCollection : IEnumerable<classItem>
    {
        //list of scheduled classes as classItems
        private List<classItem> scheduledClasses = new List<classItem>();

        //property
        public List<classItem> ScheduledClasses
        {
            get
            {
                return scheduledClasses;
            }
        }

        //add method
        public void Add(classItem ClassItem)
        {
            if (ScheduledClasses.Count != 0)
            {
                classItem highestID = ScheduledClasses.OrderByDescending(ci => ci.ItemNumber).First();
                ClassItem.SetID(highestID.ItemNumber + 1);
            }
            else
            {
                ClassItem.SetID(1);
            }
            ScheduledClasses.Add(ClassItem);
            scheduledClasses.Sort();
        }
        
        //implementing IEnumerable so I can use a foreach--------------------
        public IEnumerator<classItem> GetEnumerator()
        {
            return scheduledClasses.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return scheduledClasses.GetEnumerator();
        }
    }
}
