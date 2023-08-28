using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Zadanie4
{
    public class CurrencyData
    {
        public string No { get; set; }
        public string EffectiveDate { get; set; }
        public decimal Mid { get; set; }
    }
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync("http://api.nbp.pl/api/exchangerates/rates/A/usd/");
                var json = await response.Content.ReadAsStringAsync();

                JObject currencyResponse = JObject.Parse(json);
                IList<JToken> results = currencyResponse["rates"].Children().ToList();

                IList<CurrencyData> searchResults = new List<CurrencyData>();
                foreach (JToken result in results)
                {
                    CurrencyData searchResult = result.ToObject<CurrencyData>();
                    searchResults.Add(searchResult);
                }

                var usdInfo = searchResults.First();

                Console.WriteLine("Podaj kwotę PLN: ");
                double PLN = double.Parse(Console.ReadLine());
                decimal USD = (decimal)PLN / usdInfo.Mid;
                Console.WriteLine($"Otrzymasz: {Math.Round(USD, 2)}$");
            }
        }
    }
}
