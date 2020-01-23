using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeCache.API.Models
{
    /// <summary> Ответ на запрос курсов </summary>
    public class RatesResponse
    {
        /// <summary> Продаваемая валюта </summary>
        public string From { get; set; }

        /// <summary> Данные по запрошенным курсам </summary>
        public RateInfo[] Rates { get; set; }
    }
}
