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
    public class ItemManager
    {
        List<Item> _items;

        public ItemManager()
        {
            _items = new List<Item>();
            //****************************************  Try 1  *****************************************************
            var dbCon = DatabaseConnection.Instance();
            dbCon.DatabaseName = "final";
            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM items";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _items.Add(new Item()
                        {
                            //this.itemID = itemID;
                            //this.itemName = itemName;
                            //this.itemAmount = itemAmount;
                            //this.price = price;
                            //this.description = description;
                            //this.picLink = picLink;
                            itemID = Convert.ToInt32(reader["itemID"]),
                            itemName = reader["itemName"].ToString(),
                            itemSize = reader["itemSize"].ToString(),
                            itemAmount = Convert.ToInt32(reader["itemAmount"]),
                            price = Convert.ToDouble(reader["price"]),
                            description = reader["description"].ToString(),
                            picLink = reader["picLink"].ToString()
                        });
                    }
                }
                //dbCon.Close();
            }
        }

        //to see whether DB works
        public IEnumerable<Item> GetAll { get { return _items; } }

        public int GetItemAmount(int itemID)
        {
            foreach (Item item in _items)
            {
                if (item.itemID == itemID) return item.itemAmount;
            }
            return -1;
        }
    }
}
