using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    //class to store classItems
    class classCollection
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
                ClassItem.ItemNumber = highestID.ItemNumber + 1;
            }
            else
            {
                ClassItem.ItemNumber = 1;
            }
            ScheduledClasses.Add(ClassItem);
            scheduledClasses.Sort();
        }
    }
}
