using ExchangeCache.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeCache.Domain.Models
{
    /// <summary>
    /// Интерфейс репозитория курсов валют
    /// </summary>
    public interface IExchangeRateRepository : IRepository
    {
        Task<ExchangeRate> GetLatestForPairAsync(string sourceCurrency, string targetCurrency);
        Task<List<ExchangeRate>> GetLatestAsync(string sourceCurrency);
        ExchangeRate Add(ExchangeRate rate);
        void AddRange(IEnumerable<ExchangeRate> rates);
    }
}