using System.Security.Cryptography;
using System.Text;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using WebUrlEditor.Models;
using WebUrlEditor.Services.Interfaces;
using WebUrlEditor.Exceptions;

namespace WebUrlEditor.Services;

public class UrlShortenerService : IUrlShortenerService
{
    private readonly ConcurrentDictionary<string, string> _urlMappings = new();
    private readonly ILogger<UrlShortenerService> _logger;

    public UrlShortenerService(ILogger<UrlShortenerService> logger)
    {
        _logger = logger;
    }

    public async Task<ShortenedUrlResponse> ShortenUrlAsync(OriginalUrlRequest request)
    {
        if (string.IsNullOrEmpty(request.OriginalUrl))
        {
            _logger.LogError("URL cannot be null or empty");
            throw new ArgumentNullException("URL cannot be null or empty", nameof(request.OriginalUrl));
        }

        try
        {
            string shortUrlKey = GenerateShortUrlKey(request.OriginalUrl);
            _urlMappings[shortUrlKey] = request.OriginalUrl;
            _logger.LogInformation("URL shortened: {OriginalUrl} to {ShortenedUrl}", request.OriginalUrl, shortUrlKey);
            return new ShortenedUrlResponse
            {
                ShortenedUrl = $"https://short.url/{shortUrlKey}",
                ShortId = shortUrlKey
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error shortening URL: {OriginalUrl}", request.OriginalUrl);
            throw new UrlShorteningException("An error occurred while shortening the URL.", ex);
        }
    }

    public async Task<string> ResolveUrlAsync(string shortUrlKey)
    {
        if (string.IsNullOrEmpty(shortUrlKey))
        {
            _logger.LogError("Short URL key cannot be null or empty");
            throw new ArgumentException("Short URL key cannot be null or empty", nameof(shortUrlKey));
        }

        if (_urlMappings.TryGetValue(shortUrlKey, out var longUrl))
        {
            _logger.LogInformation("URL resolved: {ShortUrlKey} to {LongUrl}", shortUrlKey, longUrl);
            return longUrl;
        }
        else
        {
            _logger.LogWarning("URL not found for key: {ShortUrlKey}", shortUrlKey);
            return null;
        }
    }

    private string GenerateShortUrlKey(string longUrl)
    {
        try
        {
            using var sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(longUrl));
            return Convert.ToBase64String(hashBytes).Substring(0, 8); // Using only first 8 characters for simplicity
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating short URL key for: {LongUrl}", longUrl);
            throw new UrlShorteningException("An error occurred while generating the short URL key.", ex);
        }
    }
}