using UrlShortnerWebApi.Models;
using UrlShortnerWebApi.Models.Dtos;

namespace UrlShortnerWebApi.Services.Interfaces;

public interface IUrlService
{
    Url GenerateShortLink(UrlDto urlDto);
    Url PersistShortLink(Url url);
    Url GetEncodedUrl(String url);
    void DeleteShortLink(Url url);
}
