using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models.API
{
    public class RequestTransactionHistory
    {
        public string ID { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
