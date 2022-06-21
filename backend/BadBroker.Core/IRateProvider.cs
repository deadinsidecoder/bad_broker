using System;
using System.Threading.Tasks;
using BadBroker.Core.Enums;
using BadBroker.Core.Models;

namespace BadBroker.Core
{
	public interface IRateProvider
	{
		Task<Rate> Get(DateTime date, Currency quoteBase, Currency[] quoteCurrencies);
	}
}