using System;
using BadBroker.Core.Enums;

namespace BadBroker.Core.Models
{
	public class TradingStrategy
	{
		public Currency QuoteBase { get; set; }
		public Currency QuoteCurrency { get; set; }
		
		public DateTime BuyDate { get; set; }
		public DateTime SellDate { get; set; }
		
		public decimal Revenue { get; set; }

		public TradingStrategy(DateTime buyDate, DateTime sellDate, Currency quoteBase, Currency quoteCurrency, decimal revenue)
		{
			QuoteBase = quoteBase;
			QuoteCurrency = quoteCurrency;
			BuyDate = buyDate;
			SellDate = sellDate;
			Revenue = revenue;
		}
		
		public TradingStrategy()
		{
			
		}
	}
}