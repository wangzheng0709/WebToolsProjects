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
    public class User
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public String userName { get; set; }
        public String emailAddress { get; set; }
        public String pwd { get; set; }
        public int userID { get; set; }
        public Double balance { get; set; }
        public String shippingAddress { get; set; }
        public String phoneNo { get; set; }
        public String zipCode { get; set; }
        public String country { get; set; }

        public User(String userName, String emailAddress, String pwd, int userID, Double balance, String shippingAddress, String phoneNo, String zipCode, String country)
        {
            this.userName = userName;
            this.emailAddress = emailAddress;
            this.pwd = pwd;
            this.userID = userID;
            this.balance = balance;
            this.shippingAddress = shippingAddress;
            this.phoneNo = phoneNo;
            this.zipCode = zipCode;
            this.country = country;
        }

        public User()
        {
        }

        public void ResetPwd(int userID, string newPWD)
        {
            string connString = "SERVER=xxxxxxxxxxxxxx.database.azure.com" + ";" +
                                "DATABASE=xxxxxxxxxxxxxx;" +
                                "UID=xxxxxxxxxxxxxx@xxxx;" +
                                "PASSWORD=xxxxxxxxxxxxxx;";

            MySqlConnection cnMySQL = new MySqlConnection(connString);
            cnMySQL.Open();
            MySqlCommand cmdMySQL = cnMySQL.CreateCommand();
            cmdMySQL.CommandText = "update users set pwd = @newPWD where users.userID = @userID;";
            cmdMySQL.Parameters.Add("@newPWD", MySqlDbType.VarChar).Value = newPWD;
            cmdMySQL.Parameters.Add("@userID", MySqlDbType.Int64).Value = userID;
            cmdMySQL.ExecuteNonQuery();
            logger.Trace("user create - Trace"); //Won't log
            logger.Debug("user create - Debug"); //Won't log
            logger.Info("user create - Info");   //Won't log
            logger.Warn("user create - Warn");   //Won't log
            logger.Error("user create - Error"); //Will log
            logger.Fatal("user create - Fatal");
        }
    }
}
