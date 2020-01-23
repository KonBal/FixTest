using ExchangeCache.API.Models;
using ExchangeCache.API.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeCache.Test.Fakes
{
    class FakeSourceService : IRateSourceService
    {
        List<string> _currencies;
        private decimal _value;

        public FakeSourceService(List<string> currencies, decimal value)
        {
            _currencies = currencies;
            _value = value;
        }

        public Task<List<string>> GetAllCurrencies()
        {
            return Task.FromResult(_currencies);
        }

        public Task<SourceRateResponse> GetRateInfoAsync(string fromCurrency, params string[] toCurrencies)
        {
            var resp = new SourceRateResponse { Base = fromCurrency, Rates = new Dictionary<string, decimal>() };
            if (toCurrencies.Length == 0)
            {
                foreach (var cur in toCurrencies)
                {
                    resp.Rates[cur] = _value;
                }
            }
            else
                foreach (var cur in toCurrencies)
                {
                    if (_currencies.Contains(cur))
                    {
                        resp.Rates[cur] = _value;
                    }
                }
            return Task.FromResult(resp);
        }
    }
}
