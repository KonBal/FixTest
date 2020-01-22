using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeCache.API.Models
{
    /// <summary>
    /// Ответ источника с курсами валют
    /// </summary>
    public class SourceRateResponse
    {
        public int Timestamp { get; set; }
        /// <summary>
        /// Продаваемая валюта
        /// </summary>
        public string Base { get; set; }
        /// <summary>
        /// Список покупаемых валют с соответствующим курсом
        /// </summary>
        public Dictionary<string, decimal> Rates { get; set; }
    }
}
