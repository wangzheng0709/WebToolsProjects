using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;

namespace Sneakerx_api.Models
{
    // This class is used to connect to MySQL
    public class DatabaseConnection
    {
        private DatabaseConnection()
        {
        }

        private string databaseName = string.Empty;
        public string DatabaseName
        {
            get { return databaseName; }
            set { databaseName = value; }
        }

        public string Password { get; set; }
        private MySqlConnection connection = null;
        public MySqlConnection Connection
        {
            get { return connection; }
        }

        private static DatabaseConnection _instance = null;
        public static DatabaseConnection Instance()
        {
            if (_instance == null)
                _instance = new DatabaseConnection();
            return _instance;
        }

        public bool IsConnect()
        {
            if (Connection == null)
            {
                if (String.IsNullOrEmpty(databaseName))
                    return false;
                string connString = "SERVER=xxxxxxxxxxxxxx.database.azure.com" + ";" +
                                    "DATABASE=xxxxxxxxxxxxxx;" +
                                    "UID=xxxxxxxxxxxxxx@xxxx;" +
                                    "PASSWORD=xxxxxxxxxxxxxx;";
                connection = new MySqlConnection(connString);
                connection.Open();
            }

            return true;
        }

        public void Close()
        {
            connection.Close();
        }
    }
}
