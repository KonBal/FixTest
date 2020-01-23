using ExchangeCache.API.Exceptions;
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
    public class ExchangeRateCacheService_Exceptions_Tests
    {
        [TestMethod]
        public void Throw_Invalid_Target()
        {
            var settings = Options.Create(new CacheSettings { ExpirationPeriod = 2 });
            var repo = new FakeExchangeRepository();
            var currencies = new List<string> { "USD", "RUB", "EUR" };
            int value = 10;
            var source = new FakeSourceService(currencies, value);
            var curServ = new FakeCurrencyService(source.GetAllCurrencies().Result);
            var cache = new ExchangeRateCacheService(repo, source, settings, curServ);

            Assert.ThrowsExceptionAsync<InvalidTargetCurrencyException>(
                () => cache.GetRatesAndCacheAsync("USD", "AAA"));
        }
    }
}
