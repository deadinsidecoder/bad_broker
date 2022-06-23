using System;

namespace BadBroker.Api.Contracts
{
	public class Rate
	{
		public DateTime Date { get; set; }
		public decimal Rub { get; set; }
		public decimal Eur { get; set; }
		public decimal Gbp { get; set; }
		public decimal Jpy { get; set; }
	}
}