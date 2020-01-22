using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeCache.API.Exceptions
{
    [Serializable]
    public class NotFoundException
        : Exception
    {
        public NotFoundException() { }

        public NotFoundException(string description)
          : base($"A non-existent source is requested : {description}")
        { }
    }
}
