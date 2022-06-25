using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
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
		private readonly IMapper _mapper;

		public RatesController(IRateService rateService, IMapper mapper)
		{
			_rateService = rateService;
			_mapper = mapper;
		}

		[HttpGet]
		[Route("best")]
		public async Task<IActionResult> GetBest(DateTime startDate, DateTime endDate, decimal moneyUsd)
		{
			if (startDate > endDate)
				return BadRequest("The start date cannot be greater than the end date!");

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
				Revenue = bestStrategy.Revenue,
				Rates = _mapper.Map<Core.Models.Rate[], Rate[]>(rates)
			};

			return Ok(response);
		}
	}
}


