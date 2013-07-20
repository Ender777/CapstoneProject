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
            string command = "SELECT * FROM Classrooms.RoomSize";
            //create a SqlCommand using the string
            SqlCommand cmd = new SqlCommand(command, conn);
            try
            {
                conn.Open();
                //testing connection, should read and dump a column to console window
                SqlDataReader dataReader = cmd.ExecuteReader();
                Console.WriteLine(dataReader.GetValue(0));

            }
            catch (Exception ex)//TODO: edit this exception handling later
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
