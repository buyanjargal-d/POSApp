using POSApp.Core.Exceptions;

public class UnauthorizedAccessException : AppException
{
    public UnauthorizedAccessException() : base("Access is denied.") { }
}