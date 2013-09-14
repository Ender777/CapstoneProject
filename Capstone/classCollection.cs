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
            //scheduledClasses.Sort(); TODO: This broke after 1 teacher had 2 classes to teach, fix this
        }

        //TODO: determine if I can delete this
        //method to check user-generated classItem IDs for duplicates
        //public bool IDChecker(int i)
        //{
        //    bool checkResult = false;
        //    foreach (classItem ci in ScheduledClasses)
        //    {
        //        if (ci.ItemNumber.CompareTo(i) == 0)
        //        {
        //            checkResult = true;
        //        }
        //    }
        //    return checkResult;
        //}
        
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
