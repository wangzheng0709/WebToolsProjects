using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Sneakerx_api.Models
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class LoginInfo
    {
        public string emailAddress { private set; get; }
        public string pwd { private set; get; }

        public LoginInfo(String emailAddress, string pwd)
        {
            this.emailAddress = emailAddress;
            this.pwd = pwd;
        }
    }
}
