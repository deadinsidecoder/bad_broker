using System;
using System.Threading.Tasks;
using BadBroker.Core.Enums;
using BadBroker.Core.Models;

namespace BadBroker.Core.Repositories
{
	public interface IRateRepository
	{
		Task<Rate[]> GetForPeriod(DateTime from, DateTime to, Currency quoteBase, Currency[] quoteCurrencies);
		Task<Rate> Get(DateTime date, Currency quoteBase, Currency[] quoteCurrencies);
	}
}