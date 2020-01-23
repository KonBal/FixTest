using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ExchangeCache.API.Exceptions;
using ExchangeCache.API.Models;
using ExchangeCache.API.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExchangeCache.API.Services
{
    /// <summary>
    /// HTTP клиент для получения актуальных курсов из источника
    /// </summary>
    public class RateSourceService : IRateSourceService
    {
        private readonly ILogger<RateSourceService> _logger;
        private readonly UrlsConfig _urls;
        private readonly HttpClient _apiClient;
        private readonly SourceSettings _sourceConfig;

        public RateSourceService(HttpClient httpClient, ILogger<RateSourceService> logger,
            IOptions<UrlsConfig> config, IOptions<SourceSettings> sourceOpt)
        {
            _logger = logger;
            _urls = config.Value;
            _apiClient = httpClient;
            _sourceConfig = sourceOpt.Value;
        }
        /// <summary>
        /// Получить актуальные курсы валют от заданного источника
        /// </summary>
        /// <param name="fromCurrency">код продаваемой валюты</param>
        /// <param name="toCurrencies">код покупаемой валюты</param>
        /// <exception cref="HttpRequestException"></exception>
        /// <exception cref="InvalidSourceResponseException"></exception>
        /// <returns></returns>
        public async Task<SourceRateResponse> GetRateInfoAsync(string fromCurrency, params string[] toCurrencies)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get,
                    _urls.RateSource + UrlsConfig.RateSourceOperations.GetLatest(fromCurrency, toCurrencies)))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue(_sourceConfig.AuthScheme, _sourceConfig.AppId);
                using (var response = await _apiClient
                    .SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                    .ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();
                    var data = await response.Content.ReadAsStringAsync();
                    var rateResp =  !string.IsNullOrEmpty(data) ? JsonConvert.DeserializeObject<SourceRateResponse>(data) : null;
                    if (rateResp == null)
                        throw new InvalidSourceResponseException(data);
                    return rateResp;
                }
            }
        }

        /// <summary>
        /// Получить все доступные источнику валюты
        /// </summary>
        /// <returns>список кодов валют</returns>
        public async Task<List<string>> GetAllCurrencies()
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get,
                    _urls.RateSource + UrlsConfig.RateSourceOperations.GetCurrencies()))
            {
                using (var response = await _apiClient
                    .SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                    .ConfigureAwait(false))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        return JObject.Parse(data).Properties()
                            .Select(p => p.Name).ToList();
                    }
                    return new List<string>();
                }
            }
        }
    }
}
