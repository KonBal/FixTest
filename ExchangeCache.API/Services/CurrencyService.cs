using ExchangeCache.API.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeCache.API.Services
{
    public class CurrencyService : ICurrencyService
    {
        private List<string> _availableCurrencies;

        public CurrencyService(List<string> currencies)
        {
            _availableCurrencies = currencies;
        }

        /// <summary>
        /// Локальный список доступных валют.
        /// </summary>
        public IReadOnlyList<string> AvailableCurrencies => _availableCurrencies;
    }
}
