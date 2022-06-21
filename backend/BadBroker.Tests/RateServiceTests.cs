using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BadBroker.Application;
using BadBroker.Core.Enums;
using BadBroker.Core.Models;
using BadBroker.Core.Repositories;
using Moq;
using NUnit.Framework;

namespace BadBroker.Tests
{
	public class RateServiceTests
	{
		private Mock<IRateRepository> _rateRepositoryMock;
		
		[SetUp]
		public void Setup()
		{
			_rateRepositoryMock = new Mock<IRateRepository>();
		}
		
		[Test]
		public async Task GetForPeriod_ShouldReturnRates()
		{
			//arrange 
			var from = new DateTime(2014, 12, 12);
			var to = new DateTime(2014, 12, 13);
			var quoteBase = Currency.USD;
			var quoteCurrencies = new[] { Currency.RUB };
			var expectedRates = new []
			{
				new Rate
				{
					Date = new DateTime(2014, 12, 12),
					QuoteBase = quoteBase,
					QuoteCurrenciesValues = new Dictionary<Currency, decimal>{{Currency.RUB, 40m}}
				},
				new Rate
				{
					Date = new DateTime(2014, 12, 13),
					QuoteBase = quoteBase,
					QuoteCurrenciesValues = new Dictionary<Currency, decimal>{{Currency.RUB, 41m}}
				}
			};

			_rateRepositoryMock.Setup(x => x.GetForPeriod(from, to, quoteBase, quoteCurrencies))
				.ReturnsAsync(expectedRates);
			
			var service = new RateService(_rateRepositoryMock.Object);
			
			//act
			var rates = await service.GetForPeriod(from, to, quoteBase, quoteCurrencies);
			
			//assert
			_rateRepositoryMock.Verify(x => x.GetForPeriod(from, to, quoteBase, quoteCurrencies), Times.Once);
			Assert.IsNotNull(rates);
			Assert.IsNotEmpty(rates);
		}
		
		[Test]
		public void GetForPeriod_WrongPeriod_ShouldThrowException()
		{
			//arrange 
			var from = new DateTime(2014, 12, 22);
			var to = new DateTime(2014, 12, 12);
			var quoteBase = Currency.USD;
			var quoteCurrencies = new[] { Currency.RUB };

			_rateRepositoryMock.Setup(x => x.GetForPeriod(from, to, quoteBase, quoteCurrencies))
				.ReturnsAsync(Array.Empty<Rate>);
			
			var service = new RateService(_rateRepositoryMock.Object);
			
			//act
			var exception = Assert.ThrowsAsync<Exception>(async () => await service.GetForPeriod(from, to, quoteBase, quoteCurrencies));
			
			//assert
			_rateRepositoryMock.Verify(x => x.GetForPeriod(from, to, quoteBase, quoteCurrencies), Times.Never);
			Assert.IsNotNull(exception);
		}
		
		[Test]
		public void GetForPeriod_EmptyQuoteCurrencies_ShouldThrowException()
		{
			//arrange 
			var from = new DateTime(2014, 12, 12);
			var to = new DateTime(2014, 12, 22);
			var quoteBase = Currency.USD;
			var quoteCurrencies = Array.Empty<Currency>();
			
			_rateRepositoryMock.Setup(x => x.GetForPeriod(from, to, quoteBase, quoteCurrencies))
				.ReturnsAsync(Array.Empty<Rate>);
			
			var service = new RateService(_rateRepositoryMock.Object);
			
			//act
			var exception = Assert.ThrowsAsync<Exception>(async () => await service.GetForPeriod(from, to, quoteBase, quoteCurrencies));
			
			//assert
			_rateRepositoryMock.Verify(x => x.GetForPeriod(from, to, quoteBase, quoteCurrencies), Times.Never);
			Assert.IsNotNull(exception);
		}
		
		[Test]
		public void GetForPeriod_NullQuoteCurrencies_ShouldThrowException()
		{
			//arrange 
			var from = new DateTime(2014, 12, 12);
			var to = new DateTime(2014, 12, 22);
			var quoteBase = Currency.USD;
			Currency[] quoteCurrencies = null;
			
			_rateRepositoryMock.Setup(x => x.GetForPeriod(from, to, quoteBase, quoteCurrencies))
				.ReturnsAsync(Array.Empty<Rate>);
			
			var service = new RateService(_rateRepositoryMock.Object);
			
			//act
			var exception = Assert.ThrowsAsync<Exception>(async () => await service.GetForPeriod(from, to, quoteBase, quoteCurrencies));
			
			//assert
			_rateRepositoryMock.Verify(x => x.GetForPeriod(from, to, quoteBase, quoteCurrencies), Times.Never);
			Assert.IsNotNull(exception);
		}
		
		[Test]
		public async Task FindBestStrategy_ShouldReturnExpectedStrategy()
		{
			//arrange
			var from = new DateTime(2014, 12, 15);
			var to = new DateTime(2014, 12, 23);
			var quoteBase = Currency.USD;
			var quoteCurrencies = new[] { Currency.RUB };
			var tradingMoney = 100m;

			var expectedStrategy = new TradingStrategy
			{
				BuyDate = new DateTime(2014, 12, 16),
				SellDate = new DateTime(2014, 12, 22),
				QuoteBase = Currency.USD,
				QuoteCurrency = Currency.RUB,
				Revenue = 27.24m
			};
			
			var rates = new []
			{
				new Rate
				{
					Date = new DateTime(2014, 12, 15),
					QuoteBase = Currency.USD,
					QuoteCurrenciesValues = new Dictionary<Currency, decimal>{{Currency.RUB, 60.17m}}
				},

				new Rate
				{
					Date = new DateTime(2014, 12, 16),
					QuoteBase = Currency.USD,
					QuoteCurrenciesValues = new Dictionary<Currency, decimal>{{Currency.RUB, 72.99m}}
				},
				
				new Rate
				{
					Date = new DateTime(2014, 12, 17),
					QuoteBase = Currency.USD,
					QuoteCurrenciesValues = new Dictionary<Currency, decimal>{{Currency.RUB, 66.01m}}
				},
				
				new Rate
				{
					Date = new DateTime(2014, 12, 18),
					QuoteBase = Currency.USD,
					QuoteCurrenciesValues = new Dictionary<Currency, decimal>{{Currency.RUB, 61.44m}}
				},
				
				new Rate
				{
					Date = new DateTime(2014, 12, 19),
					QuoteBase = Currency.USD,
					QuoteCurrenciesValues = new Dictionary<Currency, decimal>{{Currency.RUB, 59.79m}}
				},
				
				new Rate
				{
					Date = new DateTime(2014, 12, 20),
					QuoteBase = Currency.USD,
					QuoteCurrenciesValues = new Dictionary<Currency, decimal>{{Currency.RUB, 59.79m}}
				},
				
				new Rate
				{
					Date = new DateTime(2014, 12, 21),
					QuoteBase = Currency.USD,
					QuoteCurrenciesValues = new Dictionary<Currency, decimal>{{Currency.RUB, 59.79m}}
				},
				
				new Rate
				{
					Date = new DateTime(2014, 12, 22),
					QuoteBase = Currency.USD,
					QuoteCurrenciesValues = new Dictionary<Currency, decimal>{{Currency.RUB, 54.78m}}
				},
				
				new Rate
				{
					Date = new DateTime(2014, 12, 23),
					QuoteBase = Currency.USD,
					QuoteCurrenciesValues = new Dictionary<Currency, decimal>{{Currency.RUB, 54.80m}}
				}
			};

			_rateRepositoryMock.Setup(x => x.GetForPeriod(from, to, quoteBase, quoteCurrencies))
				.ReturnsAsync(rates);

			var service = new RateService(_rateRepositoryMock.Object);
			
			//act
			var bestStrategy = await service.FindBestTradingStrategy(from, to, quoteBase, quoteCurrencies, tradingMoney);

			//assert
			_rateRepositoryMock.Verify(x => x.GetForPeriod(from, to, quoteBase, quoteCurrencies), Times.Once);
			
			Assert.AreEqual(expectedStrategy.BuyDate.Date, bestStrategy.BuyDate.Date);
			Assert.AreEqual(expectedStrategy.SellDate.Date, bestStrategy.SellDate.Date);
			Assert.AreEqual(expectedStrategy.QuoteBase, bestStrategy.QuoteBase);
			Assert.AreEqual(expectedStrategy.QuoteCurrency, bestStrategy.QuoteCurrency);
			Assert.IsTrue(Math.Abs(expectedStrategy.Revenue - bestStrategy.Revenue) < 0.01m);
		}
		
		[Test]
		public async Task FindBestStrategy_ShouldReturnExpectedStrategy_RatesNotForEveryDayInPeriod()
		{
			//arrange
			var from = new DateTime(2012, 01, 05);
			var to = new DateTime(2012, 01, 19);
			var quoteBase = Currency.USD;
			var quoteCurrencies = new[] { Currency.RUB };
			var tradingMoney = 50m;

			var expectedStrategy = new TradingStrategy
			{
				BuyDate = new DateTime(2012, 01, 05),
				SellDate = new DateTime(2012, 01, 07),
				QuoteBase = Currency.USD,
				QuoteCurrency = Currency.RUB,
				Revenue = 5.14m
			};
			
			var rates = new []
			{
				new Rate
				{
					Date = new DateTime(2012, 01, 05),
					QuoteBase = Currency.USD,
					QuoteCurrenciesValues = new Dictionary<Currency, decimal>{{Currency.RUB, 40m}}
				},

				new Rate
				{
					Date = new DateTime(2012, 01, 07),
					QuoteBase = Currency.USD,
					QuoteCurrenciesValues = new Dictionary<Currency, decimal>{{Currency.RUB, 35m}}
				},
				
				new Rate
				{
					Date = new DateTime(2012, 01, 19),
					QuoteBase = Currency.USD,
					QuoteCurrenciesValues = new Dictionary<Currency, decimal>{{Currency.RUB, 30m}}
				}
			};

			_rateRepositoryMock.Setup(x => x.GetForPeriod(from, to, quoteBase, quoteCurrencies))
				.ReturnsAsync(rates);

			var service = new RateService(_rateRepositoryMock.Object);
			
			//act
			var bestStrategy = await service.FindBestTradingStrategy(from, to, quoteBase, quoteCurrencies, tradingMoney);

			//assert
			_rateRepositoryMock.Verify(x => x.GetForPeriod(from, to, quoteBase, quoteCurrencies), Times.Once);
			
			Assert.AreEqual(expectedStrategy.BuyDate.Date, bestStrategy.BuyDate.Date);
			Assert.AreEqual(expectedStrategy.SellDate.Date, bestStrategy.SellDate.Date);
			Assert.AreEqual(expectedStrategy.QuoteBase, bestStrategy.QuoteBase);
			Assert.AreEqual(expectedStrategy.QuoteCurrency, bestStrategy.QuoteCurrency);
			Assert.IsTrue(Math.Abs(expectedStrategy.Revenue - bestStrategy.Revenue) < 0.01m);
		}
	
		[TestCase(0)]
		[TestCase(-10)]
		public void FindBestStrategy_WrongTradingMoney_ShouldThrowException(decimal tradingMoney)
		{
			//arrange
			var from = new DateTime(2012, 01, 05);
			var to = new DateTime(2012, 01, 19);
			var quoteBase = Currency.USD;
			var quoteCurrencies = new[] { Currency.RUB };
			
			_rateRepositoryMock.Setup(x => x.GetForPeriod(from, to, quoteBase, quoteCurrencies))
				.ReturnsAsync(Array.Empty<Rate>());

			var service = new RateService(_rateRepositoryMock.Object);
			
			//act
			var exception = Assert.ThrowsAsync<Exception>(async () =>
				await service.FindBestTradingStrategy(from, to, quoteBase, quoteCurrencies, tradingMoney));

			//assert
			Assert.IsNotNull(exception);
		}

		[Test]
		public void FindBestStrategy_WrongPeriod_ShouldThrowException()
		{
			//arrange
			var from = new DateTime(2012, 01, 20);
			var to = new DateTime(2012, 01, 10);
			var quoteBase = Currency.USD;
			var quoteCurrencies = new[] { Currency.RUB };
			var tradingMoney = 100m;
			
			_rateRepositoryMock.Setup(x => x.GetForPeriod(from, to, quoteBase, quoteCurrencies))
				.ReturnsAsync(Array.Empty<Rate>());

			var service = new RateService(_rateRepositoryMock.Object);
			
			//act
			var exception = Assert.ThrowsAsync<Exception>(async () =>
				await service.FindBestTradingStrategy(from, to, quoteBase, quoteCurrencies, tradingMoney));

			//assert
			_rateRepositoryMock.Verify(x => x.GetForPeriod(from, to, quoteBase, quoteCurrencies), Times.Never);

			Assert.IsNotNull(exception);
		}
		
		[Test]
		public void FindBestStrategy_EmptyQuoteCurrencies_ShouldThrowException()
		{
			//arrange
			var from = new DateTime(2012, 01, 20);
			var to = new DateTime(2012, 01, 10);
			var quoteBase = Currency.USD;
			var quoteCurrencies = new Currency[0];
			var tradingMoney = 100m;
			
			_rateRepositoryMock.Setup(x => x.GetForPeriod(from, to, quoteBase, quoteCurrencies))
				.ReturnsAsync(Array.Empty<Rate>());

			var service = new RateService(_rateRepositoryMock.Object);
			
			//act
			var exception = Assert.ThrowsAsync<Exception>(async () =>
				await service.FindBestTradingStrategy(from, to, quoteBase, quoteCurrencies, tradingMoney));

			//assert
			_rateRepositoryMock.Verify(x => x.GetForPeriod(from, to, quoteBase, quoteCurrencies), Times.Never);

			Assert.IsNotNull(exception);
		}
		
		[Test]
		public void FindBestStrategy_NullQuoteCurrencies_ShouldThrowException()
		{
			//arrange
			var from = new DateTime(2012, 01, 20);
			var to = new DateTime(2012, 01, 10);
			var quoteBase = Currency.USD;
			Currency[] quoteCurrencies = null;
			var tradingMoney = 100m;
			
			_rateRepositoryMock.Setup(x => x.GetForPeriod(from, to, quoteBase, quoteCurrencies))
				.ReturnsAsync(Array.Empty<Rate>());

			var service = new RateService(_rateRepositoryMock.Object);
			
			//act
			var exception = Assert.ThrowsAsync<Exception>(async () =>
				await service.FindBestTradingStrategy(from, to, quoteBase, quoteCurrencies, tradingMoney));

			//assert
			_rateRepositoryMock.Verify(x => x.GetForPeriod(from, to, quoteBase, quoteCurrencies), Times.Never);

			Assert.IsNotNull(exception);
		}
		
		[Test]
		public void FindBestStrategy_EmptyRates_ShouldThrowException()
		{
			//arrange
			var quoteBase = Currency.USD;
			Currency[] quoteCurrencies = { Currency.RUB};
			var tradingMoney = 100m;
			Rate[] rates = Array.Empty<Rate>();
			
			var service = new RateService(_rateRepositoryMock.Object);
			
			//act
			var exception = Assert.Throws<Exception>(() => service.FindBestTradingStrategy(rates, quoteBase, quoteCurrencies, tradingMoney));

			//assert
			Assert.IsNotNull(exception);
		}
		
		[Test]
		public void FindBestStrategy_NullRates_ShouldThrowException()
		{
			//arrange
			var quoteBase = Currency.USD;
			Currency[] quoteCurrencies = { Currency.RUB};
			var tradingMoney = 100m;
			Rate[] rates = null;
			
			var service = new RateService(_rateRepositoryMock.Object);
			
			//act
			var exception = Assert.Throws<Exception>(() => service.FindBestTradingStrategy(rates, quoteBase, quoteCurrencies, tradingMoney));

			//assert
			Assert.IsNotNull(exception);
		}
	}
}