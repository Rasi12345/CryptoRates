using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using NoFrixion_Assignment.DBAccess;
using NoFrixion_Assignment.Models;

namespace NoFrixion_Assignment.Controllers
{
    public class HomeController : Controller
    {
        public static readonly DbOperations _dbOperations = new DbOperations();
        static HomeController()
        {
            LoadData();
            Task.Run(() => {
                while (true)
                {                  
                    Thread.Sleep(60000);
                    LoadData();
                }
                
            });
        }
        public ActionResult Index()
        {          
           ViewBag.Message = _dbOperations.Read();

           return View();
        }

        private static void LoadData()
        {
            CryptoModel cryptoModel = new CryptoModel();
            cryptoModel.CryptoCurrencyandRate = new List<PriceModel>();
            WebClient wc = new WebClient();
            var json = wc.DownloadString("https://api.coindesk.com/v1/bpi/currentprice.json");

            JObject parsed = JObject.Parse(json);
            cryptoModel.CryptoName = Convert.ToString(parsed["chartName"]);
            foreach (var item in parsed.SelectTokens("bpi.*"))
            {
                PriceModel priceModel = new PriceModel();
                priceModel.CurrencyName = item.SelectToken("code").ToString();
                priceModel.Rate = item.SelectToken("rate").Value<double>();
                priceModel.LastUpdated = DateTime.Parse(parsed["time"]["updatedISO"].ToString());

                cryptoModel.CryptoCurrencyandRate.Add(priceModel);
            }
            
            /*
            if (InmemoryData.cryptoModels == null)
            {
                InmemoryData.cryptoModels = new List<CryptoModel>();
            }
            InmemoryData.cryptoModels.Add(cryptoModel);
            */

            _dbOperations.Insert(cryptoModel);
        }
    }
}
