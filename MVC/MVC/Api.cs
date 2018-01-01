using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;


namespace CyberCortex
{
    class Api
    {
        public Dictionary<string, dynamic> GetInstrument(string symbol, int limit = 2000, string exchange = "CCCAGG")
        {
            HTTP http = new HTTP();
            string API_BASE_URL = @"https://min-api.cryptocompare.com/data";
            string apiEndpoint = API_BASE_URL  + "/histohour?fsym=" + symbol +"&tsym=USD&limit=" + limit + "&aggregate=3&e=" + exchange;
            string json = http.Get(apiEndpoint);
            var jsonObject = new JavaScriptSerializer().Deserialize<Dictionary<string, dynamic>>(json);
 
            return jsonObject;
        }
    }
}
