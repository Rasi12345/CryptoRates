using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoFrixion_Assignment.Models
{
    public class CryptoModel
    {
        public int id { get; set; }
        public string CryptoName { get; set; }
        public List<PriceModel> CryptoCurrencyandRate { get; set; }

    }
}