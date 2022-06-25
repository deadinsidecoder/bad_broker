using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BadBroker.Core;
using BadBroker.Core.Enums;
using BadBroker.Core.Models;
using BadBroker.Core.Repositories;
using BadBroker.DataAccess.Psql.Entities;
using Microsoft.EntityFrameworkCore;

namespace BadBroker.DataAccess.Psql.Repositories
{
	public class RateRepository : IRateRepository
	{
		private readonly IRateProvider _rateProvider;
		private readonly ApplicationContext _context;
		
		public RateRepository(IRateProvider rateProvider, ApplicationContext context)
		{
			_rateProvider = rateProvider;
			_context = context;
		}
		
		public async Task<Rate[]> GetForPeriod(DateTime from, DateTime to, Currency quoteBase, Currency[] quoteCurrencies)
		{
			var rates = new List<Rate>();
			for (DateTime date = from.Date; date <= to.Date; date = date.AddDays(1))
			{
				var rate = await Get(date , quoteBase, quoteCurrencies);
				rates.Add(rate);
			}
			
			return rates.ToArray();
		}

		public async Task<Rate> Get(DateTime date, Currency quoteBase, Currency[] quoteCurrencies)
		{
			var currencyPairs = await _context.CurrencyPairs
				.Where(x => x.Date == date && x.QuoteBase == quoteBase && quoteCurrencies.Contains(x.QuoteCurrency))
				.ToListAsync();

			var cachedCurrencies = currencyPairs.Select(x => x.QuoteCurrency);
			var nonCachedCurrencies = quoteCurrencies
				.Except(cachedCurrencies)
				.ToArray();

			var rate = new Rate { Date = date, QuoteBase = quoteBase }; 
			
			if (nonCachedCurrencies.Any())
			{
				rate = await FetchAndCacheRate(date, quoteBase, nonCachedCurrencies);
			}
			
			currencyPairs.ForEach(pair => rate.QuoteCurrenciesValues.Add(pair.QuoteCurrency, pair.Value));
			return rate;
		}

		private async Task<Rate> FetchAndCacheRate(DateTime date, Currency quoteBase, Currency[] quoteCurrencies)
		{
			var rate = await _rateProvider.Get(date, quoteBase, quoteCurrencies);
			await SaveRate(rate);
			return rate;
		}

		private async Task SaveRate(Rate rate)
		{
			var currencyPairs = new List<CurrencyPair>();
			foreach (var key in rate.QuoteCurrenciesValues.Keys)
			{
				var currencyPair = new CurrencyPair
				{
					Date = rate.Date,
					QuoteBase = rate.QuoteBase,
					QuoteCurrency = key,
					Value = rate.QuoteCurrenciesValues[key]
				};

				currencyPairs.Add(currencyPair);
			}
				
			_context.CurrencyPairs.AddRange(currencyPairs);
			await _context.SaveChangesAsync();
		}
	}
}