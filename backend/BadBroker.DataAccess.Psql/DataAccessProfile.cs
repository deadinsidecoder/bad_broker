using System.Linq;
using AutoMapper;
using BadBroker.Core.Models;
using BadBroker.DataAccess.Psql.Entities;

namespace BadBroker.DataAccess.Psql
{
	public class DataAccessProfile : Profile
	{
		public DataAccessProfile()
		{
			CreateMap<CurrencyPair[], Rate>()
				.ForMember(x => x.Date, opt => opt.MapFrom(cp => cp.First().Date))
				.ForMember(x => x.QuoteBase, opt => opt.MapFrom(cp => cp.First().QuoteBase))
				.ForMember(x => x.QuoteCurrenciesValues, opt =>
					opt.MapFrom(cp => cp.ToDictionary(x => x.QuoteCurrency, x => x.Value)));
		}
	}
}