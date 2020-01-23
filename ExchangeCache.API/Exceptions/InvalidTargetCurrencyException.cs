using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeCache.API.Exceptions
{
    /// <summary>
    /// Исключение при запросе не представленной в источнике валюты.
    /// </summary>
    [Serializable]
    public class InvalidTargetCurrencyException : Exception
    {
        public InvalidTargetCurrencyException() { }

        public InvalidTargetCurrencyException(string targetCurrency)
          : base($"Invalid target currency: {targetCurrency}")
        { }
    }
}
