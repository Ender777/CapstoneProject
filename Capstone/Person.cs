using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace Capstone
{
    //parent class for Student and Teacher
    class Person
    {
        //Maintains a list of ID numbers to check for duplicates
        List<int> IDnumbers = new List<int>();
        //fields common to Student and Teacher classes
        protected int id;
        protected string name;
        protected string phone;
        protected string email;

        //AFTER i CREATE THE PEOPLE i SHOULDN'T NEED TO ALTER THEM RIGHT?  SO THESE
        //PROPERTIES DON'T REALLY NEED TO BE THERE?

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
        //public string Name
        //{
        //    get
        //    {
        //        return name;
        //    }
        //    set { }
        //}

        //public string Phone
        //{
        //    get
        //    {
        //        return phone;
        //    }
        //    set { }
        //}
        //public string Email
        //{
        //    get
        //    {
        //        return email;
        //    }
        //    set { }
        //}
        
        //constructor for all common fields
        public Person(int ID, string Name, string Phone, string Email)
        {
            //THIS SHOULD BE MOVED TO ADD METHOD IN STUDENT AND TEACHER DIRECTORIES MAYBE?
            //duplication checking that kicks out an error message if a duplicate ID is found
            bool hasDuplicate = false;

            foreach (int i in IDnumbers)
            {
                if (ID.CompareTo(i) == 0)
                {
                    Console.WriteLine("ID is a duplicate");
                    break;
                }
                else
                {
                    hasDuplicate = true;
                }
            }
            if (hasDuplicate == true)
            {
                id = ID;
            }

            name = Name;
            phone = Phone;
            email = Email;
        }
    }
}
