using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeCache.API.Exceptions
{
    [Serializable]
    public class AccessDeniedException : Exception
    {
        public AccessDeniedException() { }

        public AccessDeniedException(string description)
          : base($"Access restricted: {description}")
        { }
    }
}
