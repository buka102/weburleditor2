using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebUrlEditor.Filters;
using WebUrlEditor.Models;
using WebUrlEditor.Services.Interfaces;

namespace WebUrlEditor.Controllers;

[Route("api")]
[ApiController]
public class UrlShortenerController : ControllerBase
{
    private readonly IUrlShortenerService _urlShortenerService;
    private readonly ILogger<UrlShortenerController> _logger;

    public UrlShortenerController(IUrlShortenerService urlShortenerService, ILogger<UrlShortenerController> logger)
    {
        _urlShortenerService = urlShortenerService;
        _logger = logger;
    }

    [HttpPost]
    [Route("shorten")]
    [UrlValidationFilter]
    [ProducesResponseType(typeof(ShortenedUrlResponse), 200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<ShortenedUrlResponse>> ShortenUrl([FromBody] OriginalUrlRequest request)
    {
        _logger.LogInformation("ShortenUrl endpoint called.");
        var response = await _urlShortenerService.ShortenUrlAsync(request);
        _logger.LogInformation("URL Shortened: {OriginalUrl} to {ShortenedUrl}", request.OriginalUrl,
            response.ShortenedUrl);
        return Ok(response);
    }

    [HttpGet("{shortId}")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<string>> ResolveUrl(string shortId)
    {
        _logger.LogInformation("ResolveUrl endpoint called.");
        var originalUrl = await _urlShortenerService.ResolveUrlAsync(shortId);
        if (originalUrl != null)
        {
            _logger.LogInformation("URL Resolved: {ShortId} to {OriginalUrl}", shortId, originalUrl);
            return Ok(originalUrl);
        }

        _logger.LogWarning("URL Not Found for ShortId: {ShortId}", shortId);
        return NotFound();
    }
}