using System;
using System.Collections.Generic;
using BadBroker.Core.Enums;
using Newtonsoft.Json;

namespace BadBroker.ExchangeratesIntegration.Models
{
	public class ExchangeratesRateResponse
	{
		public DateTime Date { get; set; }

		[JsonProperty(PropertyName = "base")]
		public Currency QuoteBase { get; set; }
	
		[JsonProperty(PropertyName = "rates")]
		public Dictionary<Currency, decimal> QuoteCurrenciesValues { get; set; }
	}
}