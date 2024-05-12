namespace Ethik.Utility.Error;

internal class ErrorResourceException : Exception
{
    public ErrorResourceException(string message)
    : base(message)
    {
    }

    public ErrorResourceException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
