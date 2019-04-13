using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using NLog;
using NLog.Targets;
using Sneakerx_api.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sneakerx_api.Controllers
{
    public class ServerController : Controller
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        static UserManager um = new UserManager();
        static Cart cart = new Cart();
        static ItemManager im = new ItemManager();
        static CardManager cm = new CardManager();
        static OrderManager om = new OrderManager();


        [HttpGet]
        [Route("api/[controller]/allUsers")]
        public IEnumerable<User> Get()
        {
            logger.Trace("");
            return um.GetAll;
        }

        [HttpGet]
        [Route("api/[controller]/allItems")]
        public List<Item> GetItem()
        {
            List<Item> itemlist = new List<Item>();
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
                        itemlist.Add(new Item()
                        {
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
            logger.Trace("");
            return itemlist;
        }

        // POST api/values
        [HttpPost]
        [Route("api/[controller]/login")]
        public async Task<User> PostLogin([FromBody]LoginInfo loginInfo)
        {
            string logEmailAddress = loginInfo.emailAddress;
            string logPwd = loginInfo.pwd;
            User loginUser = um.Login(logEmailAddress, logPwd);
            if (loginUser.pwd.Equals("noExist")) logger.Trace("************* User: " + loginInfo.emailAddress+ " does not exist! *************"); 
            else if (loginUser.pwd.Equals("wrong")) logger.Trace("************* User: " + loginInfo.emailAddress + " log in unsuccessfully with a wrong password! *************"); 
            else logger.Trace("************* User: "+loginUser.emailAddress+ " log in successfully *************!");
            return loginUser;
        }

        [HttpPost]
        [Route("api/[controller]/register")]
        public async Task<User> PostRegister([FromBody]RegisterInfo registerInfo)
        {

            String userName = registerInfo.userName;
            //logger.Info(userName);
            String email = registerInfo.emailAddress;
            String pwd = registerInfo.pwd;
            double balance = registerInfo.balance;
            String shippingAddress = registerInfo.shippingAddress;
            string phone = registerInfo.phoneNo;
            string zip = registerInfo.zipCode;
            string country = registerInfo.country;
            User user = um.RegisterUser(userName, email, pwd, balance, shippingAddress, phone, zip, country);
            logger.Trace("User: "+registerInfo.emailAddress + " register successfully with a balance: "+ registerInfo.balance+" !");
            return user;
        }

        [HttpPost]
        [Route("api/[controller]/toCart")]
        public async Task PostAddToCart([FromBody]CartInfo cartInfo)
        {
            int itemID = cartInfo.itemID;
            int amount = im.GetItemAmount(itemID);
            cart.Update(cartInfo);
            cart.CartUpdate();
            logger.Trace("User add item: "+ cartInfo.itemID+" to cart successfully!");
        }

        [HttpPost]
        [Route("api/[controller]/cartInfo")]
        public List<CartInfo> GetCartAsync([FromBody]int userID)
        {
            cart.RemoveZero();
            cart.CartUpdate();
            logger.Trace("");
            return cart.GetUserCart(userID);
        }

        [HttpPost]
        [Route("api/[controller]/checkOut")]
        public Double CheckOutAsync([FromBody]int userID)
        {
            int nextOrderID = om.GetMaxID() + 1;
            double totalCost = 0.0;
            DateTime now = DateTime.Now;
            String orderDate = now.ToString();
            List<CartInfo> itemBought = new List<CartInfo>();
            itemBought = cart.GetUserCart(userID);
            Double moneySpent = 0.0;
            string connString = "SERVER=xxxxxxxxxxxxxx.database.azure.com" + ";" +
                                "DATABASE=xxxxxxxxxxxxxx;" +
                                "UID=xxxxxxxxxxxxxx@xxxx;"+
                                "PASSWORD=xxxxxxxxxxxxxx;";

            MySqlConnection cnMySQL = new MySqlConnection(connString);
            cnMySQL.Open();

            MySqlCommand cmdMySQL_cart = cnMySQL.CreateCommand();

            foreach (CartInfo ci in itemBought)
            {
                MySqlCommand cmdMySQL_users = cnMySQL.CreateCommand();
                MySqlCommand cmdMySQL_items = cnMySQL.CreateCommand();
                MySqlCommand cmdMySQL_order1 = cnMySQL.CreateCommand();
                cmdMySQL_items.CommandText = "update items set itemAmount = itemAmount-@itemInCartAmount where items.itemID=@ci.itemID;";
                cmdMySQL_items.Parameters.Add("@ci.itemID", MySqlDbType.Int32).Value = ci.itemID;
                cmdMySQL_items.Parameters.Add("@itemInCartAmount", MySqlDbType.Int32).Value = ci.itemInCartAmount;

                cmdMySQL_order1.CommandText = "insert into order_history(orderID, userID, itemID, orderDate, itemName, itemSize, itemBought, itemPrice, totalCost) values (@orderID, @userID, @itemID, @orderDate, @itemName, @itemSize, @itemBought, @itemPrice, @totalCost);";
                cmdMySQL_order1.Parameters.Add("@orderID", MySqlDbType.Int32).Value = nextOrderID;
                cmdMySQL_order1.Parameters.Add("@userID", MySqlDbType.Int32).Value = userID;
                cmdMySQL_order1.Parameters.Add("@itemID", MySqlDbType.Int32).Value = ci.itemID;
                cmdMySQL_order1.Parameters.Add("@orderDate", MySqlDbType.VarChar).Value = orderDate;
                cmdMySQL_order1.Parameters.Add("@itemName", MySqlDbType.VarChar).Value = ci.itemName;
                cmdMySQL_order1.Parameters.Add("@itemSize", MySqlDbType.VarChar).Value = ci.itemSize;
                cmdMySQL_order1.Parameters.Add("@itemBought", MySqlDbType.Int32).Value = ci.itemInCartAmount;
                cmdMySQL_order1.Parameters.Add("@itemPrice", MySqlDbType.Double).Value = ci.price;
                cmdMySQL_order1.Parameters.Add("@totalCost", MySqlDbType.Double).Value = 0;
                cmdMySQL_order1.ExecuteNonQuery();

                moneySpent = ci.price * ci.itemInCartAmount;
                totalCost += moneySpent;

                cmdMySQL_users.CommandText = "update users set balance = balance-@moneySpent where users.userID=@userID;";
                cmdMySQL_users.Parameters.Add("@userID", MySqlDbType.Int32).Value = userID;
                cmdMySQL_users.Parameters.Add("@moneySpent", MySqlDbType.Double).Value = moneySpent;
                cmdMySQL_users.ExecuteNonQuery();
                cmdMySQL_items.ExecuteNonQuery();
            }
            cmdMySQL_cart.CommandText = "delete FROM cart WHERE cart.userID=@userID;";
            cmdMySQL_cart.Parameters.Add("@userID", MySqlDbType.Int32).Value = userID;
            cmdMySQL_cart.ExecuteNonQuery();

            MySqlCommand cmdMySQL_order2 = cnMySQL.CreateCommand();
            cmdMySQL_order2.CommandText = "update order_history set totalCost = @totalCost where orderID = @orderID";
            cmdMySQL_order2.Parameters.Add("@totalCost", MySqlDbType.Int32).Value = totalCost;
            cmdMySQL_order2.Parameters.Add("@orderID", MySqlDbType.Double).Value = nextOrderID;
            cmdMySQL_order2.ExecuteNonQuery();
            um.UmUpdate();
            logger.Trace("User: "+userID + " check out for an order successfully!");
            return um.GetBalance(userID);
        }

        [HttpPost]
        [Route("api/[controller]/newPassword")]
        public Task<StatusCodeResult> PwdResetting([FromBody]User user)
        {
            if (user != null)
            {
                string new_PWD = user.pwd;
                int user_ID = user.userID;
                user.ResetPwd(user_ID, new_PWD);
                um.UmUpdate();
                logger.Trace("User: " + user.userID + " reset password successfully!");
                return Task.FromResult(new StatusCodeResult(200));
            }
            else {
                logger.Trace("User: " + user.userID + " reset password unsuccessfully!");
                return Task.FromResult(new StatusCodeResult(404));
            }
        }


        [HttpPost]
        [Route("api/[controller]/charge")]
        public Task<StatusCodeResult> Charge([FromBody]CardInfo cardInfo)
        {
            Boolean result = cm.Charge(cardInfo);
            if (result == true)
            {
                logger.Trace("User add: " + cardInfo.cardBalance + " to account successfully!");
                return Task.FromResult(new StatusCodeResult(200));
            }
            else {
                logger.Trace("User add: " + cardInfo.cardBalance + " to account unsuccessfully!");
                return Task.FromResult(new StatusCodeResult(404)); 
                }
        }

        [HttpPost]
        [Route("api/[controller]/balanceUpdate")]
        public void BalanceUpdate([FromBody]User user)
        {
            um.UpdateBalance(user);
        }

        [HttpPost]
        [Route("api/[controller]/myOrder")]
        public List<OrderInfo> GetMyOrderAsync([FromBody]int userID)
        {
            om.HistoryUpdate();
            List<OrderInfo>MyOrder = om.GetOrder(userID);
            logger.Trace("");
            return MyOrder;
        }
    }
}
