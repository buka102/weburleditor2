using WebUrlEditor.Models;

namespace WebUrlEditor.Services.Interfaces;

public interface IUrlShortenerService
{
    Task<ShortenedUrlResponse> ShortenUrlAsync(OriginalUrlRequest request);
    Task<string> ResolveUrlAsync(string shortUrlKey);
}
