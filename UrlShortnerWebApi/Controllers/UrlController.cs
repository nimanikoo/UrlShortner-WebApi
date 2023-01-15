using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UrlShortnerWebApi.Models;
using UrlShortnerWebApi.Models.Dtos;
using UrlShortnerWebApi.Services.Interfaces;

namespace UrlShortnerWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UrlController : ControllerBase
{
    private readonly IUrlService _urlService;

    public UrlController(IUrlService urlService)
    {
        _urlService = urlService;
    }

    [HttpPost("/Generatelink")]
    public IActionResult GenerateShortLink([FromBody] UrlDto urlDto)
    {
        var ResultUrl = _urlService.GenerateShortLink(urlDto);
        if (ResultUrl != null)
        {
            UrlResponseDto urlResponseDto = new UrlResponseDto()
            {
                OrginalUrl = ResultUrl.OrginalUrl,
                ShortUrl = ResultUrl.ShortUrl,
                ExpiresAt = ResultUrl.ExpiresAt,
            };
            return Ok(urlResponseDto);
        }

        UrlErrorResponseDto urlErrorResponseDto = new UrlErrorResponseDto()
        {
            Status = "404 NotFound",
            ErrorMessage = "There was an error processing your request. please try again."
        };
        return BadRequest(urlErrorResponseDto);
    }

    [HttpGet("/{shortlink}")]
    public IActionResult redirectToOrginalUrl([FromRoute] string shortlink)
    {
        if (string.IsNullOrEmpty(shortlink))
        {
            UrlErrorResponseDto urlErrorResponseDto = new UrlErrorResponseDto()
            {
                Status = "400",
                ErrorMessage = "Url does not exist or it might have expired!"
            };
            return BadRequest(urlErrorResponseDto);
        }

        Url resultUrl = _urlService.GetEncodedUrl(shortlink);

        if (resultUrl == null)
        {
            UrlErrorResponseDto urlErrorResponseDto = new UrlErrorResponseDto()
            {
                Status = "400",
                ErrorMessage = "Url does not exist or it might have expired!"
            };
            return BadRequest(urlErrorResponseDto);
        }

        if (resultUrl.ExpiresAt < DateTime.Now)
        {
            _urlService.DeleteShortLink(resultUrl);
            UrlErrorResponseDto urlErrorResponseDto = new UrlErrorResponseDto()
            {
                Status = "200",
                ErrorMessage = "Url Expired. Please try generating a fresh one."
            };
            return Ok(urlErrorResponseDto);
        }
        return Redirect(resultUrl.OrginalUrl);
    }
}