using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SchulDb
{
    public class SchulDbException : Exception
    {
        public SchulDbException()
        {
        }

        public SchulDbException(string message) : base(message)
        {
        }

        public SchulDbException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SchulDbException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
