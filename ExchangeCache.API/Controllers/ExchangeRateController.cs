using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeCache.API.Exceptions;
using ExchangeCache.API.Models;
using ExchangeCache.API.Options;
using ExchangeCache.API.Services;
using ExchangeCache.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeCache.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeRateController : ControllerBase
    {
        private readonly IExchangeRateCacheService _cacheService;
        private readonly ILogger<ExchangeRateController> _logger;
        private readonly int _expirationPeriod;

        public ExchangeRateController(IExchangeRateCacheService cacheService,
            ILogger<ExchangeRateController> logger, IOptions<CacheSettings> options)
        {
            _cacheService = cacheService;
            _logger = logger;
            _expirationPeriod = options.Value.ExpirationPeriod;
        }

        [HttpGet]
        [Route("rates/{from}")]
        [ProducesResponseType(typeof(RatesResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllRates(string from)
        {
            try
            {
                var rates = await _cacheService.GetRatesAndCacheAsync(from.ToUpper());
                var result = new RatesResponse
                {
                    From = from,
                    Rates = rates.Select(r => ToRateInfo(r)).ToArray()
                };
                return Ok(result);
            }//так как формат результата при ошибках не обозначен в задании, сделал общий BadRequest с сообщением
            catch(HttpRequestException ex)
            {
                _logger.LogError("Bad request to the exchange rate source: {ex}", ex);
                return BadRequest(ex.Message);
            }
            catch (InvalidSourceResponseException ex)
            {
                _logger.LogError("Invalid data was recieved from the exchange rate source: {ex}", ex);
                return BadRequest(ex.Message);
            }
            catch (DatabaseException ex)
            {
                _logger.LogError("Error occured while handling database: {ex}",  ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Critical service error: {ex}", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("rates/{from}/{to}")]
        [ProducesResponseType(typeof(RatesResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetRates(string from, string to)
        {
            try
            {
                var rate = await _cacheService.GetRatesAndCacheAsync(from.ToUpper(), to.ToUpper());
                var result = new RatesResponse
                {
                    From = rate.SourceCurrency,
                    Rates = new RateInfo[] { ToRateInfo(rate) }
                };
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Bad request to the exchange rate source: {ex}", ex);
                return BadRequest(ex.Message);
            }
            catch (InvalidSourceResponseException ex)
            {
                _logger.LogError("Invalid data was recieved from the exchange rate source: {ex}", ex);
                return BadRequest(ex.Message);
            }
            catch (InvalidTargetCurrencyException ex)
            {
                _logger.LogError("The target currency {cur} is not valid!", to);
                return BadRequest(ex.Message);
            }
            catch (DatabaseException ex)
            {
                _logger.LogError("Error occured while handling database: {ex}", ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Critical service error: {ex}", ex);
                return BadRequest(ex.Message);
            }
        }

        private RateInfo ToRateInfo(ExchangeRate rate)
        => new RateInfo
        {
            Rate = rate.Rate,
            To = rate.TargetCurrency,
            ExpireAt = rate.ReceivedAt + TimeSpan.FromMinutes(_expirationPeriod)
        };

    }
}