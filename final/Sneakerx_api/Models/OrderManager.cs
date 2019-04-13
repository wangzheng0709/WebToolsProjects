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
    public class OrderManager
    {
        List<OrderInfo> _order_history;

        public OrderManager()
        {
            _order_history = new List<OrderInfo>();
            //****************************************  Try 1  *****************************************************
            var dbCon = DatabaseConnection.Instance();
            dbCon.DatabaseName = "final";
            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM order_history";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _order_history.Add(new OrderInfo()
                        {
                            orderID = Convert.ToInt32(reader["orderID"]),
                            userID = Convert.ToInt32(reader["userID"]),
                            itemID = Convert.ToInt32(reader["itemID"]),
                            orderDate = reader["orderDate"].ToString(),
                            itemName = reader["itemName"].ToString(),
                            itemSize = reader["itemSize"].ToString(),
                            itemBought = Convert.ToInt32(reader["itemBought"]),
                            itemPrice = Convert.ToDouble(reader["itemPrice"]),
                            totalCost = Convert.ToDouble(reader["totalCost"])
                        });
                    }
                }
            }
        }

        public int GetMaxID()
        {
            int maxOrderID = (from o in _order_history select o.orderID).Max();
            return maxOrderID;
        }

        public List<OrderInfo> GetOrder(int userID)
        {
            List<OrderInfo> result = new List<OrderInfo>();
            foreach (OrderInfo orderInfo in _order_history)
            {
                if (orderInfo.userID == userID) result.Add(orderInfo);
            }
            return result;
        }

        public void HistoryUpdate()
        {
            _order_history = new List<OrderInfo>();
            var dbCon = DatabaseConnection.Instance();
            dbCon.DatabaseName = "final";
            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM order_history";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _order_history.Add(new OrderInfo()
                        {
                            orderID = Convert.ToInt32(reader["orderID"]),
                            userID = Convert.ToInt32(reader["userID"]),
                            itemID = Convert.ToInt32(reader["itemID"]),
                            orderDate = reader["orderDate"].ToString(),
                            itemName = reader["itemName"].ToString(),
                            itemSize = reader["itemSize"].ToString(),
                            itemBought = Convert.ToInt32(reader["itemBought"]),
                            itemPrice = Convert.ToDouble(reader["itemPrice"]),
                            totalCost = Convert.ToDouble(reader["totalCost"])
                        });
                    }
                }
            }
        }
    }
}
