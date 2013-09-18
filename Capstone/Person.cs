using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    [Serializable]
    //parent class for Student and Teacher
    public abstract class Person : IComparable, IEquatable<Person>
    {
        //I think I need this to be able to set times for everyone?
        databaseManager dm = new databaseManager();

        //fields common to Student and Teacher classes
        private int id;
        private string name;
        private string phone;
        private string email;
        private bool isTeacher; //use this as a hook to use in a foreach to cycle through only teachers or only students
        private List<classItem> coursesWith; //list of courses that each student or teacher is either taking or teaching
        private List<CourseTime> timesUsed; //list of course times check off used ones and compare against

        //properties
        public int ID
        {
            get
            {
                return id;
            }
            private set 
            { 
                id = value; 
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
        }
        public string Phone
        {
            get
            {
                return phone;
            }
        }
        public string Email
        {
            get
            {
                return email;
            }
        }
        public bool IsTeacher
        {
            get
            {
                return isTeacher;
            }
        }
        public List<classItem> CoursesWith
        {
            get
            {
                return coursesWith;
            }
        }
        public List<CourseTime> TimesUsed
        {
            get
            {
                return timesUsed;
            }
            private set
            {
                timesUsed = value;
            }
        }
        
        //constructor for all common fields
        public Person(string Name, string Phone, string Email, bool IsTeacher, List<classItem> CoursesWith)
        {
            name = Name;
            phone = Phone;
            email = Email;
            isTeacher = IsTeacher;
            coursesWith = CoursesWith;
        }
        
        //method to set the ID of each person
        public void SetID(int id)
        {
            this.ID = id;
        }

        //I think this allows me to call this method when I add people to set course times for all people?
        public void SetTimes()
        {
            dm.ConnectToSQL();
            this.TimesUsed = dm.DBTimes;
        }

        //IComparable implementation to be able to sort Person objects---------------
        public int CompareTo(object obj)
        {
            //cast obj into Person
            Person p = (Person)obj;
            int result = 0;
            //make sure the passed value is not null
            if (p != null)
            {
                result = p.Name.ToUpper().CompareTo(this.Name.ToUpper());
            }
            else
            {
                Console.WriteLine("CompareTo Method failed");
            }
            return result;
        }
        //IEquatable implementation to utilize Contains() and kick out duplicates----------------
        public bool Equals(Person other)
        {
            if (this.Name == other.Name || this.Phone == other.Phone || this.Email == other.Email)
            { 
                return true;
            }
            else 
            { 
                return false; 
            }
        }
    }
}
