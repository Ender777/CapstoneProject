using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
//using System.Data.Sql;
//using System.Data.SqlTypes;


namespace Capstone
{
    //class to establish connection to database and process data
    class databaseManager
    {
        //Method to connection to SQL Server info and retrieve data
        public void ConnectToSQL()
        {
            //connection to database
            SqlConnection conn = new SqlConnection("Data Source=HAL\\SQLEXPRESS;Initial Catalog=ClassSchedule;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False");
            //store the command in a string
            string roomNumberCommand = "SELECT RoomNumber FROM Classrooms";
            //create a SqlCommand using the string
            SqlCommand cmd = new SqlCommand(roomNumberCommand, conn);
            try
            {
                //TODO: this should read data to a class or struct that sends it all to
                //the correct fully functional class, i.e. Teacher
                conn.Open();
                //testing connection, should read and dump a column to console window
                SqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    Console.WriteLine(dataReader.GetValue(0));
                }
                dataReader.Close();

            }
            catch (Exception)//TODO: edit this exception handling later
            {
                Console.WriteLine("Failed to connect to data source");
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
