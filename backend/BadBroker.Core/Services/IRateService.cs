using System;
using System.Threading.Tasks;
using BadBroker.Core.Enums;
using BadBroker.Core.Models;

namespace BadBroker.Core.Services
{
	public interface IRateService
	{
		Task<Rate[]> GetForPeriod(DateTime from, DateTime to, Currency quoteBase, Currency[] quoteCurrencies);
		Task<TradingStrategy> FindBestTradingStrategy(DateTime from, DateTime to, Currency quoteBase, Currency[] quoteCurrencies, decimal tradingMoney);
		TradingStrategy FindBestTradingStrategy(Rate[] rates, Currency quoteBase, Currency[] quoteCurrencies, decimal tradingMoney);
	}
}