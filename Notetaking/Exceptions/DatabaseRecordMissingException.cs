using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notetaking.Exceptions
{
    internal class DatabaseRecordMissingException : Exception
    {
        public DatabaseRecordMissingException() : base("The requested database record does not exist")
        {
        }

        public DatabaseRecordMissingException(string message) : base(message)
        {
        }

        public DatabaseRecordMissingException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
