using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Sneakerx_api.Models
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class Item
    {
        public int itemID { get; set; }
        public String itemName { get; set; }
        public string itemSize { get; set; }
        public int itemAmount { get; set; }
        public Double price { get; set; }
        public String description { get; set; }
        public String picLink { get; set; }

        public Item(int itemID, string itemName, string itemSize, int itemAmount, double price, string description, string picLink)
        {
            this.itemID = itemID;
            this.itemName = itemName;
            this.itemSize = itemSize;
            this.itemAmount = itemAmount;
            this.price = price;
            this.description = description;
            this.picLink = picLink;
        }

        public Item()
        {
        }
    }
}
