using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models.API
{
    public class Message
    {
        public string Contents { get; set; }
        public int TimeYear { get; set; }
        public int TimeMonth { get; set; }
        public int TimeDay { get; set; }
    }
}
