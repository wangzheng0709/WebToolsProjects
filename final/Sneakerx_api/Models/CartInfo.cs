using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Sneakerx_api.Models
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CartInfo
    {
        public int userID { get; set; }
        public int itemID { get; set; }
        public String itemName { get; set; }
        public string itemSize { get; set; }
        public int itemInCartAmount { get; set; }
        public Double price { get; set; }
        public String picLink { get; set; }

        public CartInfo(int userID, int itemID, string itemName, string itemSize, int itemInCartAmount, double price, string picLink)
        {
            this.userID = userID;
            this.itemID = itemID;
            this.itemName = itemName;
            this.itemSize = itemSize;
            this.itemInCartAmount = itemInCartAmount;
            this.price = price;
            this.picLink = picLink;
        }

        public CartInfo()
        {
        }
    }
}
