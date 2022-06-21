using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BadBroker.Core.Enums;
using BadBroker.Core.Models;
using BadBroker.Core.Repositories;
using BadBroker.Core.Services;

namespace BadBroker.Application
{
	public class RateService : IRateService
	{
		private readonly IRateRepository _rateRepository;

		public RateService(IRateRepository rateRepository)
		{
			_rateRepository = rateRepository;
		}

		public async Task<Rate[]> GetForPeriod(DateTime from, DateTime to, Currency quoteBase, Currency[] quoteCurrencies)
		{
			if (from > to)
				throw new Exception("The from date is larger than to date!");

			if (quoteCurrencies == null || quoteCurrencies.Length == 0)
				throw new Exception("Quote currencies are empty!");
				
			return await _rateRepository.GetForPeriod(from, to, quoteBase, quoteCurrencies);
		}

		public async Task<TradingStrategy> FindBestTradingStrategy(DateTime from, DateTime to, Currency quoteBase, Currency[] quoteCurrencies, decimal tradingMoney)
		{
			var rates = await GetForPeriod(from, to, quoteBase, quoteCurrencies);
			var bestStrategy = FindBestTradingStrategy(rates, quoteBase, quoteCurrencies, tradingMoney);
			return bestStrategy;
		}

		public TradingStrategy FindBestTradingStrategy(Rate[] rates, Currency quoteBase, Currency[] quoteCurrencies, decimal tradingMoney)
		{
			if (tradingMoney <= 0)
				throw new Exception("Trading money is equal to or less than zero!");
			
			if (quoteCurrencies == null || quoteCurrencies.Length == 0)
				throw new Exception("Quote currencies are empty!");
			
			if (rates == null || rates.Length == 0)
				throw new Exception("Rates are empty!");
			
			var tradingStrategies = new List<TradingStrategy>();
			
			foreach (var quoteCurrency in quoteCurrencies)
			{
				var maxRate = rates
					.OrderByDescending(x => x.QuoteCurrenciesValues[quoteCurrency])
					.First();

				TradingStrategy tradingStrategy = new(maxRate.Date, maxRate.Date, quoteBase, quoteCurrency, 0);

				for (DateTime date = maxRate.Date.AddDays(1); date < rates.Max(x => x.Date).Date; date = date.AddDays(1))
				{
					var daysAfterBuy = (date - maxRate.Date).Days;
					var currentRate = rates.FirstOrDefault(x => x.Date.Date == date);

					if(currentRate == null)
						continue;
					
					var moneyAfterSell = maxRate.QuoteCurrenciesValues[quoteCurrency] * tradingMoney /
						currentRate.QuoteCurrenciesValues[quoteCurrency] - daysAfterBuy;
					
					var revenue = moneyAfterSell - tradingMoney;
					
					if (revenue > tradingStrategy.Revenue)
					{
						tradingStrategy.SellDate = date;
						tradingStrategy.Revenue = revenue;
					}
				}
				
				tradingStrategies.Add(tradingStrategy);
			}

			return tradingStrategies.OrderByDescending(x => x.Revenue).First();
		}
	}
}