using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeCache.API.Exceptions
{
    [Serializable]
    public class InvalidAppIdException : Exception
    {
        public InvalidAppIdException() { }

        public InvalidAppIdException(string appId)
          : base($"Invalid or empty app_id: {appId}")
        { }
    }
}
