using System;
using BadBroker.Core.Enums;

namespace BadBroker.Api.Contracts
{
	public class BestTradingStrategyResponse
	{
		public DateTime BuyDate { get; set; }
		public DateTime SellDate { get; set; }
		public Currency Tool { get; set; }
		public decimal Revenue { get; set; }
		public Rate[] Rates { get; set; }
	}
}