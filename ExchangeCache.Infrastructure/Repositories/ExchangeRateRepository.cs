using ExchangeCache.Domain.Models;
using ExchangeCache.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeCache.Infrastructure.Repositories
{
    /// <summary>
    /// РЕпозиторий курса валют
    /// </summary>
    public class ExchangeRateRepository : IExchangeRateRepository
    {
        private readonly ExchangeRateContext _context;

        public ExchangeRateRepository(ExchangeRateContext context)
        {
            _context = context;
        }
        public IUnitOfWork UnitOfWork => _context;

        public ExchangeRate Add(ExchangeRate rate)
        {
            return _context.ExchangeRates.Add(rate).Entity;
        }

        public void AddRange(IEnumerable<ExchangeRate> rates)
        {
            _context.ExchangeRates.AddRange(rates);
        }

        public async Task<ExchangeRate> GetLatestForPairAsync(string sourceCurrency, string targetCurrency)
        {
            return await _context.ExchangeRates
                .Where(r => r.SourceCurrency == sourceCurrency
                            && r.TargetCurrency == targetCurrency)
                .OrderByDescending(r => r.ReceivedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<List<ExchangeRate>> GetLatestAsync(string sourceCurrency)
        {
            return await _context.ExchangeRates
                .Where(r => r.SourceCurrency == sourceCurrency)
                .GroupBy(r => r.TargetCurrency)
                .Select(g => g.OrderByDescending(r => r.ReceivedAt).FirstOrDefault())
                .ToListAsync();
        }
    }
}
