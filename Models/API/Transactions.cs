using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models.API
{
    public class Transaction
    {
        public int TransactionID { get; set; }
        public int TimeMonth { get; set; }
        public int TimeDay { get; set; }
        public int TimeYear { get; set; }
        public int AmountDollars { get; set; }
        public int AmountCents { get; set; }
        public int EndBalanceDollars { get; set; }
        public int EndBalanceCents { get; set; }
        public string Vendor { get; set; }
    }

    public class TransactionHistory
    {
        public List<Transaction> Transactions { get; set; }
        public TransactionHistory()
        {
            Transactions = new List<Transaction>();
        }
    }
}
