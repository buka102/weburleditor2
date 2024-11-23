using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using WebUrlEditor.Controllers;
using WebUrlEditor.Models;
using WebUrlEditor.Services.Interfaces;
using Xunit;

namespace WebUrlEditor.Tests.UnitTests.Controllers;

public class UrlShortenerControllerTests
{
    private readonly UrlShortenerController _controller;
    private readonly Mock<IUrlShortenerService> _urlShortenerServiceMock;
    private readonly Mock<ILogger<UrlShortenerController>> _loggerMock;

    public UrlShortenerControllerTests()
    {
        _urlShortenerServiceMock = new Mock<IUrlShortenerService>();
        _loggerMock = new Mock<ILogger<UrlShortenerController>>();
        _controller = new UrlShortenerController(_urlShortenerServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task ShortenUrl_ShouldReturnShortenedUrl()
    {
        // Arrange
        var request = new OriginalUrlRequest { OriginalUrl = "https://example.com" };
        var response = new ShortenedUrlResponse { ShortenedUrl = "https://short.url/abc123", ShortId = "abc123" };
        _urlShortenerServiceMock.Setup(service => service.ShortenUrlAsync(request)).ReturnsAsync(response);

        // Act
        var result = await _controller.ShortenUrl(request);

        // Assert
        var actionResult = Assert.IsType<ActionResult<ShortenedUrlResponse>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var returnValue = Assert.IsType<ShortenedUrlResponse>(okResult.Value);
        Assert.Equal(response.ShortenedUrl, returnValue.ShortenedUrl);
    }

    [Fact]
    public async Task ResolveUrl_ShouldReturnOriginalUrl()
    {
        // Arrange
        var shortId = "abc123";
        var originalUrl = "https://example.com";
        _urlShortenerServiceMock.Setup(service => service.ResolveUrlAsync(shortId)).ReturnsAsync(originalUrl);

        // Act
        var result = await _controller.ResolveUrl(shortId);

        // Assert
        var actionResult = Assert.IsType<ActionResult<string>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        Assert.Equal(originalUrl, okResult.Value);
    }

    [Fact]
    public async Task ResolveUrl_ShouldReturnNotFound()
    {
        // Arrange
        var shortId = "nonexistent";
        _urlShortenerServiceMock.Setup(service => service.ResolveUrlAsync(shortId))
            .ReturnsAsync((string)null);

        // Act
        var result = await _controller.ResolveUrl(shortId);

        // Assert
        var actionResult = Assert.IsType<ActionResult<string>>(result);
        Assert.IsType<NotFoundResult>(actionResult.Result);
    }
}