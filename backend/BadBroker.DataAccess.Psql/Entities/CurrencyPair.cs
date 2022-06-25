using System;
using BadBroker.Core.Enums;

namespace BadBroker.DataAccess.Psql.Entities
{
	public class CurrencyPair
	{
		public DateTime Date { get; set; }
		public Currency QuoteBase { get; set; }
		public Currency QuoteCurrency { get; set; }
		public decimal Value { get; set; }
	}
}