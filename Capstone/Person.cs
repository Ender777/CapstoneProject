using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//Working on adding Student and Teacher derived classes and reading about get/set accessors to
//make sure my constructors and properties are good to go


namespace Capstone
{
    //parent class for Student and Teacher
    class Person
    {
        //Maintains a list of ID numbers to check for duplicates
        List<int> IDnumbers = new List<int>();
        //fields common to Student and Teacher with properties for access
        //NOT SURE I NEED TO ACCESS THEM, IF I READ DATA FROM THE DATABASE WHY WOULD i NEED
        //TO ALTER THE FIELDS OF THE PEOPLE?
        protected int id;
        //public int ID
        //{
        //    get
        //    {
        //        return id;
        //    }
        //    set
        //    {
        //        //if no duplicate found in constructor, add the ID number to the collection to be
        //        //checked against in future additions  NOT SURE THIS IS IN THE RIGHT PLACE
        //        IDnumbers.Add(id);
        //    }
        //}
        protected string name;
        //public string Name
        //{
        //    get
        //    {
        //        return name;
        //    }
        //    set { }
        //}

        protected string phone;
        //public string Phone
        //{
        //    get
        //    {
        //        return phone;
        //    }
        //    set { }
        //}
        protected string email;
        //public string Email
        //{
        //    get
        //    {
        //        return email;
        //    }
        //    set { }
        //}
        
        //empty constructor
        public Person() { }
        //constructor with all fields filled
        public Person(int ID, string Name, string Phone, string Email)
        {
            //duplication checking that kicks out an error message if a duplicate ID is found

            foreach (int i in IDnumbers)
            {
                bool hasDuplicate = false;
                if (ID.CompareTo(i) == 0)
                {
                    Console.WriteLine("ID is a duplicate");
                    break;
                }
                else
                {
                    hasDuplicate = true;
                }
                if (hasDuplicate == true)
                {
                    id = ID;
                }
            }

            name = Name;
            phone = Phone;
            email = Email;
        }

    }

    //Student class that inherits from Person
    class Student : Person
    {

    }
    //Teacher class that inherits from Person
    class Teacher : Person
    {

    }
}
