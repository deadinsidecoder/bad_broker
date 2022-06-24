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
				var strategy = FindBestTradingStrategyForCurrency(rates, quoteBase, quoteCurrency, tradingMoney);
				tradingStrategies.Add(strategy);
			}

			return tradingStrategies.OrderByDescending(x => x.Revenue).First();
		}


		private TradingStrategy FindBestTradingStrategyForCurrency(Rate[] rates, Currency quoteBase, Currency quoteCurrency, decimal tradingMoney)
		{
			var minDate = rates.Min(x => x.Date).Date;
			var maxDate = rates.Max(x => x.Date).Date;

			TradingStrategy tradingStrategy = new(minDate, minDate, quoteBase, quoteCurrency, 0);

			for (DateTime buyDate = minDate; buyDate <= maxDate; buyDate = buyDate.AddDays(1))
			{
				var buyRate = rates.FirstOrDefault(x => x.Date.Date == buyDate);
				
				if(buyRate == null)
					continue;
				
				for (DateTime sellDate = buyDate.AddDays(1); sellDate <= maxDate; sellDate = sellDate.AddDays(1))
				{
					var daysAfterBuy = (sellDate - buyDate).Days;
					var sellRate = rates.FirstOrDefault(x => x.Date.Date == sellDate);

					if(sellRate == null)
						continue;
					
					var moneyAfterSell = buyRate.QuoteCurrenciesValues[quoteCurrency] * tradingMoney /
						sellRate.QuoteCurrenciesValues[quoteCurrency] - daysAfterBuy;
					
					var revenue = moneyAfterSell - tradingMoney;
					
					if (revenue > tradingStrategy.Revenue)
					{
						tradingStrategy.BuyDate = buyDate;
						tradingStrategy.SellDate = sellDate;
						tradingStrategy.Revenue = revenue;
					}
				}
			}

			return tradingStrategy;
		}
	}
}