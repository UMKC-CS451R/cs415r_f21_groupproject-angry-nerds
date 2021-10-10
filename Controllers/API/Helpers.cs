using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace src.Controllers.API
{
    public class Helpers
    {
        public static bool IsEmpty<T>(List<T> list)
        {
            if (list == null)
            {
                return true;
            }
            return !list.Any();
        }
    }
}
