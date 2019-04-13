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
    public class Cart
    {

        List<CartInfo> _cart;

        public Cart()
        {
            _cart = new List<CartInfo>();

            var dbCon = DatabaseConnection.Instance();
            dbCon.DatabaseName = "final";
            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM cart";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _cart.Add(new CartInfo()
                        {
                            userID = Convert.ToInt32(reader["userID"]),
                            itemID = Convert.ToInt32(reader["itemID"]),
                            itemName = reader["itemName"].ToString(),
                            itemSize = reader["itemSize"].ToString(),
                            itemInCartAmount = Convert.ToInt32(reader["itemInCartAmount"]),
                            price = Convert.ToDouble(reader["price"]),
                            picLink = reader["picLink"].ToString()
                        });
                    }
                }
            }
        }

        public IEnumerable<CartInfo> GetCart { get { return _cart; } }

        public void Update(CartInfo cartInfo)
        {
            string connString = "SERVER=xxxxxxxxxxxxxx.database.azure.com" + ";" +
                                "DATABASE=xxxxxxxxxxxxxx;" +
                                "UID=xxxxxxxxxxxxxx@xxxx;" +
                                "PASSWORD=xxxxxxxxxxxxxx;";

            MySqlConnection cnMySQL = new MySqlConnection(connString);
            cnMySQL.Open();
            MySqlCommand cmdMySQL = cnMySQL.CreateCommand();
            //var cmd = new MySqlCommand();
            //cmdMySQL.Connection = cnMySQL.Connection;
            if (_cart.Any(o => (o.userID == cartInfo.userID) && (o.itemID == cartInfo.itemID)))
            {
                cmdMySQL.CommandText = "update cart set itemInCartAmount=itemInCartAmount+@itemInCartAmount where cart.userID=@userID && cart.itemID=@itemID;";
                cmdMySQL.Parameters.Add("@itemInCartAmount", MySqlDbType.Int32).Value = cartInfo.itemInCartAmount;
                cmdMySQL.Parameters.Add("@userID", MySqlDbType.Int32).Value = cartInfo.userID;
                cmdMySQL.Parameters.Add("@itemID", MySqlDbType.Int32).Value = cartInfo.itemID;
                cmdMySQL.ExecuteNonQuery();
            }
            else
            {
                cmdMySQL.CommandText = "insert into cart(userID,itemID,itemName,itemSize,itemInCartAmount,price,picLink) VALUES(@userID,@itemID,@itemName,@itemSize,@itemInCartAmount,@price,@picLink);";
                cmdMySQL.Parameters.Add("@userID", MySqlDbType.Int32).Value = cartInfo.userID;
                cmdMySQL.Parameters.Add("@itemID", MySqlDbType.Int32).Value = cartInfo.itemID;
                cmdMySQL.Parameters.Add("@itemName", MySqlDbType.VarChar).Value = cartInfo.itemName;
                cmdMySQL.Parameters.Add("@itemSize", MySqlDbType.VarChar).Value = cartInfo.itemSize;
                cmdMySQL.Parameters.Add("@itemInCartAmount", MySqlDbType.Int32).Value = cartInfo.itemInCartAmount;
                cmdMySQL.Parameters.Add("@price", MySqlDbType.Double).Value = cartInfo.price;
                cmdMySQL.Parameters.Add("@picLink", MySqlDbType.VarChar).Value = cartInfo.picLink;
                cmdMySQL.ExecuteNonQuery();
                _cart.Add(cartInfo);
            }
        }

        public List<CartInfo> GetUserCart(int userID)
        {
            List<CartInfo> result = new List<CartInfo>();
            foreach(CartInfo cartInfo in _cart)
            {
                if (cartInfo.userID == userID) result.Add(cartInfo);
            }
            return result;
        }

        public void CartUpdate()
        {
            _cart = new List<CartInfo>();

            var dbCon = DatabaseConnection.Instance();
            dbCon.DatabaseName = "final";
            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM cart";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _cart.Add(new CartInfo()
                        {
                            userID = Convert.ToInt32(reader["userID"]),
                            itemID = Convert.ToInt32(reader["itemID"]),
                            itemName = reader["itemName"].ToString(),
                            itemSize = reader["itemSize"].ToString(),
                            itemInCartAmount = Convert.ToInt32(reader["itemInCartAmount"]),
                            price = Convert.ToDouble(reader["price"]),
                            picLink = reader["picLink"].ToString()
                        });
                    }
                }
            }
        }

        public void RemoveZero()
        {
            string connString = "SERVER=xxxxxxxxxxxxxx.database.azure.com" + ";" +
                                "DATABASE=xxxxxxxxxxxxxx;" +
                                "UID=xxxxxxxxxxxxxx@xxxx;" +
                                "PASSWORD=xxxxxxxxxxxxxx;";

            MySqlConnection cnMySQL = new MySqlConnection(connString);
            cnMySQL.Open();
            MySqlCommand cmdMySQL = cnMySQL.CreateCommand();
            cmdMySQL.CommandText = "delete from cart where itemInCartAmount = 0;";
            cmdMySQL.ExecuteNonQuery();
        }
    }
}
