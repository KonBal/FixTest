using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeCache.API.Options
{
    public class CacheSettings
    {
        /// <summary>
        /// Время инвалидации кэша (в минутах)
        /// </summary>
        public int ExpirationPeriod { get; set; }
    }
}
