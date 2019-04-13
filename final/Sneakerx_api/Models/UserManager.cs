using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using NLog;

namespace Sneakerx_api.Models
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class UserManager
    {
        List<User> _users;
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public UserManager()
        {
            _users = new List<User>();

            var dbCon = DatabaseConnection.Instance();
            dbCon.DatabaseName = "final";
            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM users";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _users.Add(new User()
                        {
                            userName = reader["userName"].ToString(),
                            emailAddress = reader["emailAddress"].ToString(),
                            pwd = reader["pwd"].ToString(),
                            userID = Convert.ToInt32(reader["userID"]),
                            balance = Convert.ToDouble(reader["balance"]),
                            shippingAddress = reader["shippingAddress"].ToString(),
                            phoneNo = reader["phoneNo"].ToString(),
                            zipCode = reader["zipCode"].ToString(),
                            country = reader["country"].ToString()
                        });
                    }
                }
                //dbCon.Close();
            }
            logger.Log(LogLevel.Trace, "Sample trace message");
            logger.Log(LogLevel.Debug, "Sample debug message");
            logger.Log(LogLevel.Info, "Sample informational message");
            logger.Log(LogLevel.Warn, "Sample warning message");
            logger.Log(LogLevel.Error, "Sample error message");
            logger.Log(LogLevel.Fatal, "Sample fatal message");
        }

        //to see whether DB works
        public IEnumerable<User> GetAll { get { return _users; } }

        public User Login(string emailAddress, string password)
        {
            if (_users.Any(o => (o.emailAddress.Equals(emailAddress) && o.pwd.Equals(password))))
                return _users.Where(o => o.emailAddress.Equals(emailAddress)).ToList()[0];
            else if (_users.Any(o => (o.emailAddress.Equals(emailAddress) && (!o.pwd.Equals(password)))))
                return _users[2];
            else return _users[0];
        }

        public User RegisterUser_test(String userName, String emailAddress, String pwd, Double balance, String shippingAddress, String phoneNo, String zipCode, String country)
        {
            return _users[1];

        }

        public User RegisterUser(String userName, String emailAddress, String pwd, Double balance, String shippingAddress, String phoneNo, String zipCode, String country)
        {
            int nextID = (from u in _users select u.userID).Max() + 1;
            User user = new User(userName, emailAddress, pwd, nextID, balance, shippingAddress, phoneNo, zipCode, country);
            if (_users.Any(o => (o.emailAddress.Equals(user.emailAddress))))
            {
                return _users[1];
            }
            else
            {
                //local test for register function;
                _users.Add(user);
                string connString = "SERVER=xxxxxxxxxxxxxx.database.azure.com" + ";" +
                                    "DATABASE=xxxxxxxxxxxxxx;" +
                                    "UID=xxxxxxxxxxxxxx@xxxx;" +
                                    "PASSWORD=xxxxxxxxxxxxxx;";

                MySqlConnection cnMySQL = new MySqlConnection(connString);
                cnMySQL.Open();
                MySqlCommand cmdMySQL = cnMySQL.CreateCommand();
                //var cmd = new MySqlCommand();
                //cmdMySQL.Connection = cnMySQL.Connection;
                cmdMySQL.CommandText = "INSERT INTO users(userName, emailAddress, pwd, userID, balance, shippingAddress, phoneNo, zipCode, country) VALUES(@userName, @emailAddress, @pwd, @userID, @balance, @shippingAddress, @phoneNo, @zipCode, @country);";
                cmdMySQL.Parameters.Add("@userName", MySqlDbType.VarChar).Value = userName;
                cmdMySQL.Parameters.Add("@emailAddress", MySqlDbType.VarChar).Value = emailAddress;
                cmdMySQL.Parameters.Add("@pwd", MySqlDbType.VarChar).Value = pwd;
                cmdMySQL.Parameters.Add("@userID", MySqlDbType.Int64).Value = nextID;
                cmdMySQL.Parameters.Add("@balance", MySqlDbType.Double).Value = balance;
                cmdMySQL.Parameters.Add("@shippingAddress", MySqlDbType.VarChar).Value = shippingAddress;
                cmdMySQL.Parameters.Add("@phoneNo", MySqlDbType.VarChar).Value = phoneNo;
                cmdMySQL.Parameters.Add("@zipCode", MySqlDbType.VarChar).Value = zipCode;
                cmdMySQL.Parameters.Add("@country", MySqlDbType.VarChar).Value = country;
                cmdMySQL.ExecuteNonQuery();
                //cnMySQL.Close();
                //}

                //return _users.Where(o => o.emailAddress.Equals(user.emailAddress)).ToList()[0];
                return user;
            }


            //var dbCon = DatabaseConnection.Instance();
            //dbCon.DatabaseName = "HW_5";
            //if (dbCon.IsConnect())
            //{
            //    //string query = "SELECT * FROM users";
            //    MySqlCommand cmd = new MySqlCommand();
            //    cmd.CommandText = "INSERT INTO users(userName, emailAddress, pwd, userID, balance, shippingAddress, phoneNo, zipCode, country) VALUES(@userName, @emailAddress, @pwd, @userID, @balance, @shippingAddress, @phoneNo, @zipCode, @country);";
            //    cmd.Parameters.AddWithValue("@userName", registerInfo.userName);
            //    cmd.Parameters.AddWithValue("@emailAddress", registerInfo.emailAddress);
            //    cmd.Parameters.AddWithValue("@pwd", registerInfo.pwd);
            //    cmd.Parameters.AddWithValue("@userID", IDnum);
            //    cmd.Parameters.AddWithValue("@balance", registerInfo.balance);
            //    cmd.Parameters.AddWithValue("@shippingAddress", registerInfo.shippingAddress);
            //    cmd.Parameters.AddWithValue("@phoneNo", registerInfo.phoneNo);
            //    cmd.Parameters.AddWithValue("@zipCode", registerInfo.zipCode);
            //    cmd.Parameters.AddWithValue("@country", registerInfo.country);
            //    cmd.ExecuteNonQuery();
            //    dbCon.Close();
            //}
        }

        public Double GetBalance(int userID)
        {
            return _users.Where(o => o.userID == userID).ToList()[0].balance;
        }

        public void UmUpdate()
        {
            _users = new List<User>();
            var dbCon = DatabaseConnection.Instance();
            dbCon.DatabaseName = "final";
            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM users";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _users.Add(new User()
                        {
                            userName = reader["userName"].ToString(),
                            emailAddress = reader["emailAddress"].ToString(),
                            pwd = reader["pwd"].ToString(),
                            userID = Convert.ToInt32(reader["userID"]),
                            balance = Convert.ToDouble(reader["balance"]),
                            shippingAddress = reader["shippingAddress"].ToString(),
                            phoneNo = reader["phoneNo"].ToString(),
                            zipCode = reader["zipCode"].ToString(),
                            country = reader["country"].ToString()
                        });
                    }
                }
            }
        }

        public void UpdateBalance(User user)
        {
            string connString = "SERVER=xxxxxxxxxxxxxx.database.azure.com" + ";" +
                                "DATABASE=xxxxxxxxxxxxxx;" +
                                "UID=xxxxxxxxxxxxxx@xxxx;" +
                                "PASSWORD=xxxxxxxxxxxxxx;";

            MySqlConnection cnMySQL = new MySqlConnection(connString);
            cnMySQL.Open();
            MySqlCommand cmdMySQL = cnMySQL.CreateCommand();
            cmdMySQL.CommandText = "update users set balance = @balance where userID = @userID;";
            cmdMySQL.Parameters.Add("@userID", MySqlDbType.VarChar).Value = user.userID;
            cmdMySQL.Parameters.Add("@balance", MySqlDbType.Double).Value = user.balance;
            cmdMySQL.ExecuteNonQuery();
        }
    }
}
