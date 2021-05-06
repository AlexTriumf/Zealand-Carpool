using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Zealand_Carpool.Models
{
    public class DatabaseCon
    {
        string ConnString = "Data Source=andreas-zealand-server-dk.database.windows.net;Initial Catalog=Andreas-database;User ID=Andreas;Password=SecretPassword!;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        
        public SqlConnection GetConnection()
        {
            SqlConnection conn = new SqlConnection(ConnString);
                conn.Open();
                return conn;
        }   

    }
}
