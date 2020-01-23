using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeCache.API.Services
{
    /// <summary>
    /// Хранит локально список валют доступных в источнике.
    /// </summary>
    public interface ICurrencyService
    {
        IReadOnlyList<string> AvailableCurrencies { get; }
    }
}
