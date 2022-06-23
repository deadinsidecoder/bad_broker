using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BadBroker.Api.Contracts;
using BadBroker.Core.Enums;
using BadBroker.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace BadBroker.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class RatesController : ControllerBase
	{
		private readonly IRateService _rateService;

		public RatesController(IRateService rateService)
		{
			_rateService = rateService;
		}

		[HttpGet]
		[Route("best")]
		public async Task<IActionResult> GetBest(DateTime startDate, DateTime endDate, decimal moneyUsd)
		{
			var quoteCurrencies = new[] { Currency.EUR, Currency.GBP, Currency.RUB, Currency.JPY };
			var rates = await _rateService.GetForPeriod(startDate, endDate, Currency.USD, quoteCurrencies);
			var bestStrategy = _rateService.FindBestTradingStrategy(rates, Currency.USD, quoteCurrencies, moneyUsd);

			var response = new GetBestTradingStrategyResponse()
			{
				BuyDate = bestStrategy.BuyDate,
				SellDate = bestStrategy.SellDate,
				Tool = bestStrategy.QuoteCurrency,
				Revenue = bestStrategy.Revenue
			};

			var responseRates = new List<Rate>();
			foreach (var rate in rates)
			{
				var responseRate = new Rate();
				responseRate.Date = rate.Date;
				responseRate.Eur = rate.QuoteCurrenciesValues[Currency.EUR];
				responseRate.Gbp = rate.QuoteCurrenciesValues[Currency.GBP];
				responseRate.Jpy = rate.QuoteCurrenciesValues[Currency.JPY];
				responseRate.Rub = rate.QuoteCurrenciesValues[Currency.RUB];
				responseRates.Add(responseRate);
			}
			response.Rates = responseRates.ToArray();
			return Ok(response);
		}
	}
}


