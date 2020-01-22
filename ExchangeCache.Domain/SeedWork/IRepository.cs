using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeCache.Domain.SeedWork
{
    /// <summary>
    /// Базовый контракт репозиториев
    /// </summary>
    public interface IRepository
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
