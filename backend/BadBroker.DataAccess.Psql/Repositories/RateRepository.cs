using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BadBroker.Core;
using BadBroker.Core.Enums;
using BadBroker.Core.Models;
using BadBroker.Core.Repositories;

namespace BadBroker.DataAccess.Psql.Repositories
{
	public class RateRepository : IRateRepository
	{
		private readonly IRateProvider _rateProvider;
		
		public RateRepository(IRateProvider rateProvider)
		{
			_rateProvider = rateProvider;
		}
		
		public async Task<Rate[]> GetForPeriod(DateTime from, DateTime to, Currency quoteBase, Currency[] quoteCurrencies)
		{
			var tasks = new List<Task<Rate>>();
			
			for (DateTime date = from.Date; date <= to.Date; date = date.AddDays(1))
			{
				tasks.Add(Get(date , quoteBase, quoteCurrencies));
			}

			await Task.WhenAll(tasks);
			var rates = tasks.Select(task => task.Result).ToArray();
			return rates;
		}

		public async Task<Rate> Get(DateTime date, Currency quoteBase, Currency[] quoteCurrencies)
		{
			return await _rateProvider.Get(date, quoteBase, quoteCurrencies);
		}
	}
}