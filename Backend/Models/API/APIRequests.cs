using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models.API
{
    public class RequestTransactionHistory
    {
        public int AccountId { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }

    public class RequestTransaction
    {
        public int TransactionId { get; set; }
    }

    public class RequestUser
    {
        public int UserId { get; set; }
    }
}
