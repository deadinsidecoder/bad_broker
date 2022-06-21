using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BadBroker.Core.Enums;
using BadBroker.Core.Models;
using BadBroker.ExchangeratesIntegration;
using Newtonsoft.Json;

namespace BadBroker.Exchangerates.ApiTest
{
	internal class Program
	{
		private static async Task Main()
		{
			var exchangeratesClient = new ExchangeratesCurrencyDataClient();
			var currencyPairs = await exchangeratesClient.Get(DateTime.Now.AddDays(-1), Currency.USD, Currency.RUB);
			Console.WriteLine();
		}
	}
}

