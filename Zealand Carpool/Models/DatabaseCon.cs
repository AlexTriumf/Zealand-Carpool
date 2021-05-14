﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Zealand_Carpool.Models
{
    /// <summary>
    /// A Singleton class to which a Database connection is made and opened
    /// made by Andreas
    /// </summary>
    public class DatabaseCon
    {
        string ConnString = "Data Source=andreas-zealand-server-dk.database.windows.net;Initial Catalog=Andreas-database;User ID=Andreas;Password=SecretPassword!;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private static DatabaseCon _instance = new DatabaseCon();
        private DatabaseCon()
        {
                            
        }

        public SqlConnection SqlConnection()
        {
                SqlConnection conn = new SqlConnection(ConnString);
            conn.Open();    
            return conn;
        }

        public void SqlConnectionClose()
        {
            SqlConnection conn = new SqlConnection(ConnString);
            conn.CloseAsync();
            
            
        }

        public static DatabaseCon Instance => _instance;

    }
}
