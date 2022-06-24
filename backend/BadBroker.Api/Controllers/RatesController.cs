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
			if (endDate > startDate)
				return BadRequest("The end date cannot be greater than the start date!");

			if(startDate.Year < 1999 || endDate.Year < 1999)
				return BadRequest("No data before 1999");

			if(endDate.Date > DateTime.Now.Date)
				return BadRequest("The end date cannot be greater than it is now!");
			
			if(moneyUsd <= 0)
				return BadRequest("The money cannot be less than or equal to zero!");
			
			var quoteCurrencies = new[] { Currency.EUR, Currency.GBP, Currency.RUB, Currency.JPY };
			var rates = await _rateService.GetForPeriod(startDate, endDate, Currency.USD, quoteCurrencies);
			var bestStrategy = _rateService.FindBestTradingStrategy(rates, Currency.USD, quoteCurrencies, moneyUsd);

			var response = new BestTradingStrategyResponse()
			{
				BuyDate = bestStrategy.BuyDate,
				SellDate = bestStrategy.SellDate,
				Tool = bestStrategy.QuoteCurrency,
				Revenue = bestStrategy.Revenue
			};

			var responseRates = new List<Rate>();
			foreach (var rate in rates)
			{
				var responseRate = new Rate
				{
					Date = rate.Date,
					Eur = rate.QuoteCurrenciesValues[Currency.EUR],
					Gbp = rate.QuoteCurrenciesValues[Currency.GBP],
					Jpy = rate.QuoteCurrenciesValues[Currency.JPY],
					Rub = rate.QuoteCurrenciesValues[Currency.RUB]
				};

				responseRates.Add(responseRate);
			}
			response.Rates = responseRates.ToArray();
			return Ok(response);
		}
	}
}


