using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Sneakerx_api.Models
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CardInfo
    {


        public string cardNo { get; set; }
        public string cardPwd { get; set; }
        public string cardName { get; set; }
        public double cardBalance { get; set; }
        public int cardMonth { get; set; }
        public int cardYear { get; set; }
        public string cardCvc { get; set; }

        public CardInfo(string cardNo, string cardPwd, string cardName, double cardBalance, int cardMonth, int cardYear, string cardCvc)
        {
            this.cardNo = cardNo;
            this.cardPwd = cardPwd;
            this.cardName = cardName;
            this.cardBalance = cardBalance;
            this.cardMonth = cardMonth;
            this.cardYear = cardYear;
            this.cardCvc = cardCvc;
        }

        public CardInfo()
        {
        }
    }
}
