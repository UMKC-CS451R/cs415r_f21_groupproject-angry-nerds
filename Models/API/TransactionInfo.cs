using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models.API
{
    public class TransactionInfo
    {
        public int TransactionId { get; set; }
        public int AccountId { get ; set; }
        public int TimeMonth { get ; set; }
        public int TimeDay { get ; set; }
        public int TimeYear { get ; set; }
        public int AmountDollars { get ; set; }
        public int AmountCents { get ; set; }
        public int EndBalanceDollars { get ; set; }
        public int EndBalanceCents { get ; set; }
        public string LocationStCd { get ; set; }
        public string CountryCd { get ; set; }
        public string Vendor { get; set; }
    }
}
