using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;


namespace CyberCortex
{
    public class Api
    {
        public Dictionary<string, dynamic> GetInstrument(string symbol, string timeframe = "histohour", string exchange = "CCCAGG", int limit = 2000)
        {
            Http http = new Http();
            string API_BASE_URL = @"https://min-api.cryptocompare.com/data";
            string apiEndpoint = API_BASE_URL  + "/" + timeframe + "?fsym=" + symbol +"&tsym=USD&limit=" + limit + "&aggregate=1&e=" + exchange;
            string json = http.Get(apiEndpoint);
            var jsonObject = new JavaScriptSerializer().Deserialize<Dictionary<string, dynamic>>(json);
 
            return jsonObject;
        }
    }
}
