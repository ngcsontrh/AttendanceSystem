using System;
using System.Runtime.Serialization;

namespace RAttendanceSystem.Application.Exceptions
{
    internal class RecordAlreadyExistsException : Exception
    {
        private const string DefaultMessage = "The record already exists.";

        public RecordAlreadyExistsException() : base(DefaultMessage)
        {
        }

        public RecordAlreadyExistsException(string? message) : base(string.IsNullOrWhiteSpace(message) ? DefaultMessage : message)
        {
        }

        public RecordAlreadyExistsException(string? message, Exception? innerException) : base(string.IsNullOrWhiteSpace(message) ? DefaultMessage : message, innerException)
        {
        }
    }
}
