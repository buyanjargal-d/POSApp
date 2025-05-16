using System;

namespace POSApp.Core.Exceptions
{
    public class AppException : Exception
    {
        public AppException(string message) : base(message) { }
    }
}