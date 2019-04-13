using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Sneakerx_api.Models
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class OrderInfo
    {
        public int orderID { get; set; }
        public int userID { get; set; }
        public int itemID { get; set; }
        public string orderDate { get; set; }
        public String itemName { get; set; }
        public string itemSize { get; set; }
        public int itemBought { get; set; }
        public Double itemPrice { get; set; }
        public Double totalCost { get; set; }


        public OrderInfo(int orderID, int userID, int itemID, string orderDate, string itemName, string itemSize, int itemBought, double itemPrice, double totalCost)
        {
            this.orderID = orderID;
            this.userID = userID;
            this.itemID = itemID;
            this.orderDate = orderDate;
            this.itemName = itemName;
            this.itemSize = itemSize;
            this.itemBought = itemBought;
            this.itemPrice = itemPrice;
            this.totalCost = totalCost;
        }

        public OrderInfo()
        {
        }
    }
}
