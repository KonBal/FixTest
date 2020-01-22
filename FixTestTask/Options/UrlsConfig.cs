using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeCache.API.Options
{
    public class UrlsConfig
    {
        public class RateSourceOperations
        {
            public static string GetLatest(string fromCurrency, params string[] toCurrencies)
                => $"/api/latest.json?base={fromCurrency}&symbols={string.Join(',', toCurrencies)}";

            public static string GetCurrencies()
                => $"/api/currencies.json";
        }

        public string RateSource { get; set; }

    }
}
