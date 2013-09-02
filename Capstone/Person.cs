using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    //parent class for Student and Teacher
    public abstract class Person : IComparable, IEquatable<Person>
    {
        //fields common to Student and Teacher classes
        private int id;
        private string name;
        private string phone;
        private string email;
        private bool isTeacher; //use this as a hook to use in a foreach to cycle through only teachers or only students
        private List<classItem> coursesWith; //list of courses that each student or teacher is either taking or teaching

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
            set
            {
                phone = setPhoneLength();
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
        
        //constructor for all common fields
        public Person(string Name, string Phone, string Email, bool IsTeacher, List<classItem> CoursesWith)//TODO: need to add coursesWith somehow
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

        //method to ensure length of phone number is correct
        public void setPhoneLength()//TODO: Use internet bookmark to see if I can enforce length of Phone here

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
