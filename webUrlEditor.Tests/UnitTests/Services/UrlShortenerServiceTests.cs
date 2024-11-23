using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using WebUrlEditor.Models;
using WebUrlEditor.Services;
using Xunit;

namespace WebUrlEditor.Tests.UnitTests.Services;

public class UrlShortenerServiceTests
{
    private readonly UrlShortenerService _urlShortenerService;
    private readonly Mock<ILogger<UrlShortenerService>> _loggerMock;

    public UrlShortenerServiceTests()
    {
        _loggerMock = new Mock<ILogger<UrlShortenerService>>();
        _urlShortenerService = new UrlShortenerService(_loggerMock.Object);
    }

    [Fact]
    public async Task ShortenUrlAsync_ShouldReturnShortenedUrl()
    {
        // Arrange
        var request = new OriginalUrlRequest { OriginalUrl = "https://example.com" };

        // Act
        var result = await _urlShortenerService.ShortenUrlAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.StartsWith("https://short.url/", result.ShortenedUrl);
    }

    [Fact]
    public async Task ResolveUrlAsync_ShouldReturnOriginalUrl()
    {
        // Arrange
        var request = new OriginalUrlRequest { OriginalUrl = "https://example.com" };
        var shortenResult = await _urlShortenerService.ShortenUrlAsync(request);

        // Act
        var result = await _urlShortenerService.ResolveUrlAsync(shortenResult.ShortId);

        // Assert
        Assert.Equal(request.OriginalUrl, result);
    }

    [Fact]
    public async Task ShortenUrlAsync_ShouldHandleEmptyUrl()
    {
        // Arrange
        var request = new OriginalUrlRequest { OriginalUrl = string.Empty };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _urlShortenerService.ShortenUrlAsync(request));
    }

    [Fact]
    public async Task ShortenUrlAsync_ShouldHandleNullUrl()
    {
        // Arrange
        var request = new OriginalUrlRequest { OriginalUrl = null };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _urlShortenerService.ShortenUrlAsync(request));
    }

    [Fact]
    public async Task ResolveUrlAsync_ShouldReturnNullForNonExistentKey()
    {
        // Act
        var result = await _urlShortenerService.ResolveUrlAsync("nonexistentkey");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task ShortenUrlAsync_ShouldHandleLongUrl()
    {
        // Arrange
        var longUrl = new string('a', 2048);
        var request = new OriginalUrlRequest { OriginalUrl = longUrl };

        // Act
        var result = await _urlShortenerService.ShortenUrlAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.StartsWith("https://short.url/", result.ShortenedUrl);
    }
}