using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;

namespace Sneakerx_api.Models
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CardManager
    {
        //private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        List<CardInfo> _cards;

        public CardManager()
        {
            _cards = new List<CardInfo>();

            var dbCon = DatabaseConnection.Instance();
            dbCon.DatabaseName = "final";
            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM credit_card";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _cards.Add(new CardInfo()
                        {
                            cardNo = reader["cardNo"].ToString(),
                            cardPwd = reader["cardPwd"].ToString(),
                            cardName = reader["cardName"].ToString(),
                            cardBalance = Convert.ToDouble(reader["cardBalance"]),
                            cardMonth = Convert.ToInt32(reader["cardMonth"]),
                            cardYear = Convert.ToInt32(reader["cardYear"]),
                            cardCvc = reader["cardCvc"].ToString()
                        });
                    }
                }
                //dbCon.Close();
            }
        }

        public Boolean Charge(CardInfo cardInfo)
        {
            if (_cards.Any(o => o.cardCvc.Equals(cardInfo.cardCvc)
                 && o.cardNo.Equals(cardInfo.cardNo)
                 && o.cardPwd.Equals(cardInfo.cardPwd)
                 && o.cardName.Equals(cardInfo.cardName)
                 && o.cardYear == cardInfo.cardYear
                 && o.cardMonth == cardInfo.cardMonth
                 && o.cardBalance >= cardInfo.cardBalance))
            {
                string connString = "SERVER=xxxxxxxxxxxxxx.database.azure.com" + ";" +
                                    "DATABASE=xxxxxxxxxxxxxx;" +
                                    "UID=xxxxxxxxxxxxxx@xxxx;" +
                                    "PASSWORD=xxxxxxxxxxxxxx;";

                MySqlConnection cnMySQL = new MySqlConnection(connString);
                cnMySQL.Open();
                MySqlCommand cmdMySQL = cnMySQL.CreateCommand();
                cmdMySQL.CommandText = "update credit_card set cardBalance = cardBalance - @cardInfo.cardBalance where credit_card.cardNo = @cardInfo.cardNo;";
                cmdMySQL.Parameters.Add("@cardInfo.cardNo", MySqlDbType.VarChar).Value = cardInfo.cardNo;
                cmdMySQL.Parameters.Add("@cardInfo.cardBalance", MySqlDbType.Double).Value = cardInfo.cardBalance;
                cmdMySQL.ExecuteNonQuery();
                return true;
            }
            else return false;
        }
    }
}
