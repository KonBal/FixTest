using ExchangeCache.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeCache.API.Services
{
    public interface IExchangeRateCacheService
    {
        /// <summary>
        /// Получить кэшированный курс для указанной валюты.
        /// В случае истечения курса получает данные из источника и обновляет кэш.
        /// </summary>
        /// <param name="fromCurrency">код продаваемой валюты</param>
        /// <param name="toCurrency">код покупаемой валюты</param>
        /// <returns>Курс обмена для одной пары валют</returns>
        /// <exception cref="HttpRequestException"></exception>
        /// <exception cref="InvalidTargetCurrencyException"></exception>
        /// <exception cref="InvalidSourceResponseException"></exception>
        /// <exception cref="DatabaseException"></exception>
        Task<ExchangeRate> GetRatesAndCacheAsync(string fromCurrency, string toCurrency);

        /// <summary>
        /// Получить кэшированный курс для всех валют относительно заданной.
        /// </summary>
        /// <param name="fromCurrency">продаваемая валюта</param>
        /// <returns>Курс обмена для всех валют относительно продаваемой</returns>
        /// <exception cref="HttpRequestException"></exception>
        /// <exception cref="InvalidSourceResponseException"></exception>
        /// <exception cref="DatabaseException"></exception>
        Task<IEnumerable<ExchangeRate>> GetRatesAndCacheAsync(string fromCurrency);
    }
}