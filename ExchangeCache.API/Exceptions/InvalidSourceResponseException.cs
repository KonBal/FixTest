using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeCache.API.Exceptions
{
    /// <summary>
    /// Исключение, возникающее при невозможности обработать ответ от источника.
    /// Например, если изменилась схема.
    /// </summary>
    [Serializable]
    public class InvalidSourceResponseException : Exception
    {
        public InvalidSourceResponseException() { }

        public InvalidSourceResponseException(string response)
          : base($"The source response coudn't be recognized: {response}")
        { }
    }
}
