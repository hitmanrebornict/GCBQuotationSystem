using GCBQuotationSystem.Models;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;

namespace GCBQuotationSystem.Components.Services
{
	public class CurrencyExchange
    {
		private QuotationSystemContext _dbContext;

        public CurrencyExchange(QuotationSystemContext dbContext)
        {
            _dbContext = dbContext;
        }

        private string apiKey = "QBiepMmnomawFEg83OlMwDalf1FrcZkuYnccKkiAKgdy4MlxwFFBkzPmIeEw3ThT";

        public async Task<decimal> getChangedCurrency(string fromCurrency, string toCurrency)
        {
            string url = $"https://api.unirateapi.com/api/rates?api_key={apiKey}&from={fromCurrency}&to={toCurrency}&format=json";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                // Parse JSON
                var json = JObject.Parse(responseBody);
                decimal convertedValue = (decimal)json["result"]; // adjust based on UniRateAPI response

                Console.WriteLine($"{fromCurrency} = {convertedValue} {toCurrency}");

                return convertedValue;
            }
        }
    }
}
