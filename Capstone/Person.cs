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
        
        //constructor for all common fields
        public Person(string Name, string Phone, string Email)
        {
            name = Name;
            phone = Phone;
            email = Email;
        }
        
        public void SetID(int id)
        {
            this.ID = id;
        }

        //IComparable implementation to be able to sort personCollection by Person---------------
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
