using System;

namespace OPTEL.Entity.Helpers.Exceptions
{
    public class ExternalFileHandlingException : Exception
    {
        public ExternalFileHandlingException(string message)
        : base(message)
        { }

        public ExternalFileHandlingException(string message, Exception innerException)
        : base(message, innerException)
        { }
    }
}
