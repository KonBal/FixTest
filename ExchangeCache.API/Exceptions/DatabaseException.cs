using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeCache.API.Exceptions
{
    /// <summary>
    /// Общее исключение на ошибки при сохранении данных в БД
    /// </summary>
    [Serializable]
    public class DatabaseException : Exception
    {
        public DatabaseException() { }

        public DatabaseException(string message) : base(message) { }
    }
}
