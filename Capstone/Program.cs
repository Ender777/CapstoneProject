using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//WORKING ON: establishing connection string and trying to read data

namespace Capstone
{
    class Program
    {
        static void Main(string[] args)
        {
            databaseManager dbm = new databaseManager();
            dbm.ConnectToSQL();
        }
    }
}
