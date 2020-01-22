using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeCache.API.Services
{
    public interface ICurrencyService
    {
        IReadOnlyList<string> AvailableCurrencies { get; }
    }
}
