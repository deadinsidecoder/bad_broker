using System;
using System.Collections.Generic;
using BadBroker.Core.Enums;

namespace BadBroker.Core.Models
{
	public class Rate
	{
		public DateTime Date { get; set; }
		public Currency QuoteBase { get; set; }
		public Dictionary<Currency, decimal> QuoteCurrenciesValues { get; set; }
	}
}