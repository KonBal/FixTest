using ExchangeCache.API.Options;
using ExchangeCache.API.Services;
using ExchangeCache.Test.Fakes;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeCache.Test
{
    [TestClass]
    public class ExchangeRateCacheService_FromTo_Tests
    {
        [TestMethod]
        public void Get_rates_from_to_empty_database()
        {
            var settings = Options.Create(new CacheSettings { ExpirationPeriod = 2 });
            var repo = new FakeExchangeRepository();
            var currencies = new List<string> { "USD", "RUB", "EUR" };
            int value = 10;
            var source = new FakeSourceService(currencies, value);
            var curServ = new FakeCurrencyService(source.GetAllCurrencies().Result);
            var cache = new ExchangeRateCacheService(repo, source, settings, curServ);

            var rate = cache.GetRatesAndCacheAsync("USD", "RUB").Result;
            Assert.AreEqual(value, rate.Rate);
        }

        [TestMethod]
        public void Get_rates_from_to_full_database_exp()
        {
            var settings = Options.Create(new CacheSettings { ExpirationPeriod = 2 });
            var repo = new FakeExchangeRepository();
            int prevVal = 6;
            repo.Add(new Domain.Models.ExchangeRate
            {
                Rate = prevVal,
                ReceivedAt = DateTime.Now - TimeSpan.FromMinutes(settings.Value.ExpirationPeriod + 1),
                SourceCurrency = "USD",
                TargetCurrency = "RUB"
            });
            var currencies = new List<string> { "USD", "RUB", "EUR" };
            int value = 10;
            var source = new FakeSourceService(currencies, value);
            var curServ = new FakeCurrencyService(source.GetAllCurrencies().Result);
            var cache = new ExchangeRateCacheService(repo, source, settings, curServ);

            var rate = cache.GetRatesAndCacheAsync("USD", "RUB").Result;
            Assert.AreEqual(value, rate.Rate);
            Assert.AreEqual(value, repo.GetLatestForPairAsync("USD", "RUB").Result.Rate);
        }

        [TestMethod]
        public void Get_rates_from_to_full_database_actual()
        {
            var settings = Options.Create(new CacheSettings { ExpirationPeriod = 2 });
            var repo = new FakeExchangeRepository();
            int prevVal = 6;
            repo.Add(new Domain.Models.ExchangeRate
            {
                Rate = prevVal,
                ReceivedAt = DateTime.Now,
                SourceCurrency = "USD",
                TargetCurrency = "RUB"
            });
            var currencies = new List<string> { "USD", "RUB", "EUR" };
            int value = 10;
            var source = new FakeSourceService(currencies, value);
            var curServ = new FakeCurrencyService(source.GetAllCurrencies().Result);
            var cache = new ExchangeRateCacheService(repo, source, settings, curServ);

            var rate = cache.GetRatesAndCacheAsync("USD", "RUB").Result;
            Assert.AreEqual(prevVal, rate.Rate);
        }
    }
}
