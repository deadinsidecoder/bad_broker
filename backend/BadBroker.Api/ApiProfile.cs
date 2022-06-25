using AutoMapper;
using BadBroker.Core.Enums;
using BadBroker.Core.Models;

namespace BadBroker.Api
{
	public class ApiProfile : Profile
	{
		public ApiProfile()
		{
			CreateMap<Rate, Contracts.Rate>()
				.ForMember(x => x.Eur, opt => opt.MapFrom(r => r.QuoteCurrenciesValues[Currency.EUR]))
				.ForMember(x => x.Gbp, opt => opt.MapFrom(r => r.QuoteCurrenciesValues[Currency.GBP]))
				.ForMember(x => x.Jpy, opt => opt.MapFrom(r => r.QuoteCurrenciesValues[Currency.JPY]))
				.ForMember(x => x.Rub, opt => opt.MapFrom(r => r.QuoteCurrenciesValues[Currency.RUB]));
		}
	}
}