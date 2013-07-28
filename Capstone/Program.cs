using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//WORKING ON: Defining classes for remaining things like courses, classes, classitems.
//Getting the Teacher list of courses established.
//These two will eventually lead to reading info from database into the correct class.

namespace Capstone
{
    class Program
    {
        static void Main(string[] args)
        {
            personCollection pc = new personCollection();

            Student a = new Student("James", "253-304-8675", "James@gmail.com");
            pc.Add(a);
            Student b = new Student("Eddie", "253-304-4646", "Eddie@gmail.com");
            pc.Add(b);
            Student c = new Student("Eddie", "253-304-4646", "Eddie@gmail.com");
            pc.Add(c);


            foreach (Student student in pc)
            {
                Console.WriteLine(student.ID + "\t" + student.Name + "\t" + student.Phone + "\t" + student.Email);
            }

            //databaseManager dbm = new databaseManager();
            //dbm.ConnectToSQL();


            Console.ReadLine();

        }
    }
}
