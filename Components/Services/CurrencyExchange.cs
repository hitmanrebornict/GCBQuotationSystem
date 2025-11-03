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

        public async Task<decimal> getChangedCurrency(string fromCurrency, string toCurrency)
        {
            string url = $"https://api.frankfurter.dev/v1/latest?base={fromCurrency}&symbols={toCurrency}";
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                // Parse JSON
                var json = JObject.Parse(responseBody);
                decimal convertedValue = (decimal)json["rates"][toCurrency]; // adjust based on UniRateAPI response

                Console.WriteLine($"{fromCurrency} = {convertedValue} {toCurrency}");

                return convertedValue;
            }
        }
    }
}
