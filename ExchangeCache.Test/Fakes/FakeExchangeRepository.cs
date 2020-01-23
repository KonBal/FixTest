using ExchangeCache.Domain.Models;
using ExchangeCache.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace ExchangeCache.Test.Fakes
{
    class FakeExchangeRepository : IExchangeRateRepository
    {
        private List<ExchangeRate> _exchangeRates;
        private IUnitOfWork _uow;
        private int _id;
        public FakeExchangeRepository()
        {
            _exchangeRates = new List<ExchangeRate>();
            _uow = new FakeUnitOfWork();
        }


        public IUnitOfWork UnitOfWork => _uow;

        public ExchangeRate Add(ExchangeRate rate)
        {
            rate.Id = _id++;
            _exchangeRates.Add(rate);
            return rate;
        }

        public void AddRange(IEnumerable<ExchangeRate> rates)
        {
            _exchangeRates.AddRange(rates);
        }

        public Task<ExchangeRate> GetLatestForPairAsync(string sourceCurrency, string targetCurrency)
        {
            return Task.FromResult(_exchangeRates
                .Where(r => r.SourceCurrency == sourceCurrency
                            && r.TargetCurrency == targetCurrency)
                .OrderByDescending(r => r.ReceivedAt)
                .FirstOrDefault());
        }

        public Task<List<ExchangeRate>> GetLatestAsync(string sourceCurrency)
        {
            return Task.FromResult(_exchangeRates
                .Where(r => r.SourceCurrency == sourceCurrency)
                .GroupBy(r => r.TargetCurrency)
                .Select(g => g.OrderByDescending(r => r.ReceivedAt).FirstOrDefault())
                .ToList());
        }
    }
}
