using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeCache.Domain.Models
{
    public class ExchangeRate
    {
        public int Id { get; set; }
        /// <summary>
        /// Отдаваемая валюта
        /// </summary>
        public string SourceCurrency { get; set; }
        /// <summary>
        /// ПОлучаемая валюта
        /// </summary>
        public string TargetCurrency { get; set; }
        /// <summary>
        /// Курс обмена
        /// </summary>
        public decimal Rate { get; set; }
        /// <summary>
        /// Время получения курса
        /// </summary>
        public DateTime ReceivedAt { get; set; }
    }
}
