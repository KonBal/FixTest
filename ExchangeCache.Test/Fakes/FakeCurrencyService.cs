using ExchangeCache.API.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeCache.Test.Fakes
{
    class FakeCurrencyService : ICurrencyService
    {
        List<string> _currencies = new List<string> { "USD", "RUB", "EUR" };

        public IReadOnlyList<string> AvailableCurrencies => _currencies;

        public FakeCurrencyService(List<string> currs)
        {
            _currencies = currs;
        }

    }
}
