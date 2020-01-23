using ExchangeCache.API.Options;
using ExchangeCache.API.Services;
using ExchangeCache.Test.Fakes;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeCache.Test
{
    [TestClass]
    public class ExchangeRateCacheService_From_Tests
    {
        [TestMethod]
        public void Get_rates_only_from_empty_database()
        {
            var settings = Options.Create(new CacheSettings { ExpirationPeriod = 2 });
            var repo = new FakeExchangeRepository();
            var currencies = new List<string> { "USD", "RUB", "EUR" };
            var source = new FakeSourceService( currencies, 10);
            var curServ = new FakeCurrencyService(source.GetAllCurrencies().Result);
            var cache = new ExchangeRateCacheService(repo, source, settings, curServ);

            var rate = cache.GetRatesAndCacheAsync("RUB").Result;
            Assert.AreEqual(currencies.Count, rate.Count());
        }

        [TestMethod]
        public void Get_rates_only_from_full_database_update_exp()
        {
            var settings = Options.Create(new CacheSettings { ExpirationPeriod = 2 });
            var repo = new FakeExchangeRepository();
            int prevVal = 6;

            repo.Add(new Domain.Models.ExchangeRate
            {
                Rate = prevVal,
                ReceivedAt = DateTime.Now - TimeSpan.FromMinutes(settings.Value.ExpirationPeriod+1),
                SourceCurrency = "USD",
                TargetCurrency = "RUB"
            });
            var currencies = new List<string> { "USD", "RUB", "EUR" };
            int value = 10;
            var source = new FakeSourceService(currencies, value);
            var curServ = new FakeCurrencyService(source.GetAllCurrencies().Result);
            var cache = new ExchangeRateCacheService(repo, source, settings, curServ);

            var rate = cache.GetRatesAndCacheAsync("USD").Result;
            Assert.AreEqual(value, rate.First(r => r.TargetCurrency == "RUB").Rate);
            //check if repo has updated
            Assert.AreEqual(value, repo.GetLatestAsync("USD").Result.First(r => r.TargetCurrency == "RUB").Rate);
        }

        [TestMethod]
        public void Get_rates_only_from_full_database_update_actual()
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

            var rate = cache.GetRatesAndCacheAsync("USD").Result;
            Assert.AreEqual(prevVal, rate.First(r => r.TargetCurrency == "RUB").Rate);

            Assert.AreEqual(prevVal, repo.GetLatestAsync("USD").Result.First(r => r.TargetCurrency == "RUB").Rate);

        }



    }
}
