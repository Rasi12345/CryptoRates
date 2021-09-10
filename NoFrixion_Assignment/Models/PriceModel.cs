using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoFrixion_Assignment.Models
{
    public class PriceModel
    {
        public double Rate { get; set; }
        public string CurrencyName { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}