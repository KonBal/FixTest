using ExchangeCache.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeCache.API.Services
{
    interface IRateSourceService
    {
        /// <summary>
        /// Получить актуальный курс валют
        /// </summary>
        /// <param name="fromCurrency">продаваемая валюта</param>
        /// <param name="toCurrency">покупаемая валюта</param>
        /// <returns>Курс валют</returns>
        /// <exception cref="HttpRequestException"></exception>
        /// <exception cref="InvalidSourceResponseException"></exception>
        Task<SourceRateResponse> GetRateInfoAsync(string fromCurrency, params string[] toCurrencies);

        /// <summary>
        /// Получить все доступные источнику валюты
        /// </summary>
        /// <returns>список кодов валют</returns>
        Task<List<string>> GetAllCurrencies();
    }
}
