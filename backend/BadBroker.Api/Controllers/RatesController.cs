using System;
using System.Threading.Tasks;
using BadBroker.Api.Contracts;
using BadBroker.Core.Enums;
using BadBroker.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace BadBroker.Api.Controllers
{
	public class RatesController : ControllerBase
	{
		private readonly IRateService _rateService;

		public RatesController(IRateService rateService)
		{
			_rateService = rateService;
		}

		public async Task<IActionResult> GetBest(DateTime startDate, DateTime endDate, decimal moneyUsd)
		{
			var quoteCurrencies = new[] { Currency.EUR, Currency.GBP, Currency.RUB, Currency.JPY };
			var rates = await _rateService.GetForPeriod(startDate, endDate, Currency.USD, quoteCurrencies);
			var bestStrategy = _rateService.FindBestTradingStrategy(rates, Currency.USD, quoteCurrencies, moneyUsd);

			var response = new GetBestTradingStrategyResponse()
			{
				Rates = rates,
				BuyDate = bestStrategy.BuyDate,
				SellDate = bestStrategy.SellDate,
				Tool = bestStrategy.QuoteCurrency,
				Revenue = bestStrategy.Revenue
			};
			
			return Ok(response);
		}
	}
}