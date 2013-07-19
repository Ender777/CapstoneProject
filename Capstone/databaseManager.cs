using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace Capstone
{
    //class to establish connection to database and process data
    class databaseManager
    {
        //Method to connection to SQL Server info and retrieve data
        public void ConnectToSQL()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Data Source=HAL\\SQLEXPRESS;Initial Catalog=ClassSchedule;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";
            string command = "SELECT * FROM Classrooms";
            sqlCommand cmd = new sqlCommand(command, conn);

            try
            {
                conn.Open();
                //logic to process data
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
