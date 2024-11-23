namespace WebUrlEditor.Exceptions;

public class UrlShorteningException : Exception
{
    public string CustomProperty { get; set; }

    public UrlShorteningException(string message, Exception innerException)
        : base(message, innerException)
    {
        // Custom formatting for exception
        CustomProperty = "Custom value";
    }

    public override string ToString()
    {
        return $"{base.ToString()}, CustomProperty: {CustomProperty}";
    }
}