using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeCache.API.Exceptions
{
    [Serializable]
    public class InvalidSourceResponseException : Exception
    {
        public InvalidSourceResponseException() { }

        public InvalidSourceResponseException(string response)
          : base($"The source response coudn't be recognized: {response}")
        { }
    }
}
