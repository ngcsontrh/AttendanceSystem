using System;
using System.Runtime.Serialization;

namespace RAttendanceSystem.Application.Exceptions
{
    public class RecordNotFoundException : Exception
    {
        private const string DefaultMessage = "The requested record was not found.";

        public RecordNotFoundException() : base(DefaultMessage)
        {
        }

        public RecordNotFoundException(string? message) : base(string.IsNullOrWhiteSpace(message) ? DefaultMessage : message)
        {
        }

        public RecordNotFoundException(string? message, Exception? innerException) : base(string.IsNullOrWhiteSpace(message) ? DefaultMessage : message, innerException)
        {
        }
    }
}
