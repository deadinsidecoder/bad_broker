using System;
using BadBroker.Core.Enums;
using BadBroker.Core.Models;

namespace BadBroker.Api.Contracts
{
	public class GetBestTradingStrategyResponse
	{
		public DateTime BuyDate { get; set; }
		public DateTime SellDate { get; set; }
		public Currency Tool { get; set; }
		public decimal Revenue { get; set; }
		public Rate[] Rates { get; set; }
	}
}