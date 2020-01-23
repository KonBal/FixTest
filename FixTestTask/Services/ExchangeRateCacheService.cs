using ExchangeCache.API.Exceptions;
using ExchangeCache.API.Models;
using ExchangeCache.API.Options;
using ExchangeCache.Domain.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeCache.API.Services
{
    public class ExchangeRateCacheService : IExchangeRateCacheService
    {
        private readonly IExchangeRateRepository _rateRepo;
        private readonly IRateSourceService _rateSource;
        private readonly CacheSettings _cacheSettings;
        private readonly ICurrencyService _currency;

        public ExchangeRateCacheService(IExchangeRateRepository rateRepo, IRateSourceService rateSource,
            IOptions<CacheSettings> settings, ICurrencyService currency)
        {
            _rateRepo = rateRepo;
            _rateSource = rateSource;
            _cacheSettings = settings.Value;
            _currency = currency;
        }

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
        public async Task<ExchangeRate> GetRatesAndCacheAsync(string fromCurrency, string toCurrency)
        {
            var cachedRate = await _rateRepo.GetLatestForPairAsync(fromCurrency, toCurrency);
            if (cachedRate == null || IsExpired(cachedRate))
            {
                var newRate = await _rateSource.GetRateInfoAsync(fromCurrency, toCurrency);
                if (newRate.Rates.Count == 0)
                    throw new InvalidTargetCurrencyException(toCurrency);
                ExchangeRate rateEntity = MapToSingleExchangeRate(newRate);
                _rateRepo.Add(rateEntity);
                try
                {
                    await _rateRepo.UnitOfWork.SaveChangesAsync();
                }
                catch(Exception ex)
                {
                    throw new DatabaseException(ex.Message);
                }
                cachedRate = rateEntity;
            }
            return cachedRate;
        }

        /// <summary>
        /// Получить кэшированный курс для всех валют относительно заданной.
        /// </summary>
        /// <param name="fromCurrency">продаваемая валюта</param>
        /// <returns>Курс обмена для всех валют относительно продаваемой</returns>
        /// <exception cref="HttpRequestException"></exception>
        /// <exception cref="InvalidSourceResponseException"></exception>
        /// <exception cref="DatabaseException"></exception>
        public async Task<IEnumerable<ExchangeRate>> GetRatesAndCacheAsync(string fromCurrency)
        {
            //получаем все курсы с этой валютой из базы
            var cachedCurrencies = await _rateRepo.GetLatestAsync(fromCurrency);
            //здесь будем хранить коды валют, курсы которых в базе устарели или вообще отсутствуют, для запроса к источнику
            var currencyList = _currency.AvailableCurrencies.ToList();
            var result = new List<ExchangeRate>();
            foreach (var cached in cachedCurrencies)
            {
                if (!IsExpired(cached))
                {
                    result.Add(cached);
                    //курс свежий, не нужно получать из источника
                    currencyList.Remove(cached.TargetCurrency);
                }
            }
            if (currencyList.Count > 0) //не все валюты есть или не все свежие
            {
                var actualRates = await _rateSource.GetRateInfoAsync(fromCurrency, currencyList.ToArray());
                var rates = MapToExchangeRates(actualRates);
                _rateRepo.AddRange(rates); //обновляем базу
                try
                {
                    await _rateRepo.UnitOfWork.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new DatabaseException(ex.Message);
                }
                result.AddRange(rates);
            }
            return result;
        }

        #region Tools

        private IEnumerable<ExchangeRate> MapToExchangeRates(SourceRateResponse sourceRate)
        {
            var result = new List<ExchangeRate>();
            var recievedTime = DateTime.Now;

            foreach (var rate in sourceRate.Rates)
            {
                result.Add(new ExchangeRate
                {
                    Rate = rate.Value,
                    ReceivedAt = recievedTime,
                    SourceCurrency = sourceRate.Base,
                    TargetCurrency = rate.Key
                });
            }
            return result;
        }

        private ExchangeRate MapToSingleExchangeRate(SourceRateResponse sourceRate)
        {

            var rate = new ExchangeRate
            {
                ReceivedAt = DateTime.Now,
                SourceCurrency = sourceRate.Base,
                TargetCurrency = sourceRate.Rates.Keys.First()
            };
            rate.Rate = sourceRate.Rates[rate.TargetCurrency];
            return rate;
        }

        private bool IsExpired(ExchangeRate cachedRate)
        {
            return cachedRate.ReceivedAt < DateTime.Now - TimeSpan.FromMinutes(_cacheSettings.ExpirationPeriod);
        }

        #endregion

    }
}
