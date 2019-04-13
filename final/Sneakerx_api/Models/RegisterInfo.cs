using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Sneakerx_api.Models
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class RegisterInfo
    {
        public string userName { private set; get; }
        public string emailAddress { private set; get; }
        public string pwd { private set; get; }
        public Double balance { private set; get; }
        public string shippingAddress { private set; get; }
        public string phoneNo { private set; get; }
        public string zipCode { private set; get; }
        public string country { private set; get; }

        public RegisterInfo(String userName, String emailAddress, String pwd, Double balance, String shippingAddress, String phoneNo, String zipCode, String country)
        {
            this.userName = userName;
            this.emailAddress = emailAddress;
            this.pwd = pwd;
            this.balance = balance;
            this.shippingAddress = shippingAddress;
            this.phoneNo = phoneNo;
            this.zipCode = zipCode;
            this.country = country;
        }
    }
}
