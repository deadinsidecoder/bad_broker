using System;
using System.Net.Http;
using System.Threading.Tasks;
using BadBroker.Core;
using BadBroker.Core.Enums;
using BadBroker.Core.Models;
using BadBroker.ExchangeratesIntegration.Models;
using Newtonsoft.Json;

namespace BadBroker.ExchangeratesIntegration
{
	public class ExchangeratesRatesProvider : IRateProvider
	{
		private const string API_KEY = "C1SWFJBDArLhJdH44uy3et5AFMRl3Zob";
		private const string BASE_URL = "https://api.apilayer.com/exchangerates_data";
		
		public async Task<Rate> Get(DateTime date, Currency quoteBase, params Currency[] quoteCurrencies)
		{
			var quoteCurrenciesSymbols = string.Join(",", quoteCurrencies);
			var uri = new Uri($"{BASE_URL}/{date:yyyy-MM-dd}?base={quoteBase}&symbols={quoteCurrenciesSymbols}");
			
			var request = new HttpRequestMessage();
			request.Headers.Add("apikey", API_KEY);
			request.Method = HttpMethod.Get;
			request.RequestUri = uri;
			
			var httpClient = new HttpClient();
			var httpResponse = await httpClient.SendAsync(request);
			var body = await httpResponse.Content.ReadAsStringAsync();

			var rateResponse = JsonConvert.DeserializeObject<ExchangeratesRateResponse>(body);

			var rate = new Rate
			{
				Date = rateResponse.Date,
				QuoteBase = rateResponse.QuoteBase,
				QuoteCurrenciesValues = rateResponse.QuoteCurrenciesValues
			};

			return rate;
		}
	}
}