using ExchangeCache.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeCache.Test.Fakes
{
    class FakeUnitOfWork : IUnitOfWork
    {
        public void Dispose()
        {
            
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(0);
        }
    }
}
