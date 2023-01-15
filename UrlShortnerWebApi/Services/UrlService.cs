using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using UrlShortnerWebApi.Data;
using UrlShortnerWebApi.Models;
using UrlShortnerWebApi.Models.Dtos;
using UrlShortnerWebApi.Services.Interfaces;

namespace UrlShortnerWebApi.Services;

public class UrlService : IUrlService
{
    private readonly AppDataContext _context;
    public UrlService( AppDataContext context)
    {
        _context = context;
    }
    public void DeleteShortLink(Url url)
    {
        _context.Remove(url);
    }

    public Url GenerateShortLink(UrlDto urlDto)
    {
        if (!string.IsNullOrEmpty(urlDto.Url))
        {
            String encodedUrl = encodeUrl(urlDto.Url);
            Url urlToPersist = new Url()
            {
                CreatedAt = DateTime.Now,
                OrginalUrl = urlDto.Url.ToLower(),
                ShortUrl = encodedUrl,
                ExpiresAt= DateTime.Now.AddMinutes(5)
            };
           // urlToPersist.ExpiresAt = GetExpirationDate(urlDto.ExpiresAt.ToString(), urlToPersist.CreatedAt);

            Url urlToReturn = PersistShortLink(urlToPersist);
            if (urlToReturn != null)
            {
                return urlToReturn;
            }
            return null;
        }
        return null;
    }

    private DateTime GetExpirationDate(String expirationDate, DateTime creationDate)
    {
        if (string.IsNullOrEmpty(expirationDate))
        {
            return creationDate.AddSeconds(60);
        }
        DateTime expirationDateToRet = DateTime.Parse(expirationDate);
        return expirationDateToRet;
    }

    private String encodeUrl(String url)
    {
        var random = new Random();
        const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ123456789@#$%";
        var randomString = new string(Enumerable.Repeat(characters, 8)
        .Select(c => c[random.Next(c.Length)]).ToArray());
        return randomString;
    }

    public Url GetEncodedUrl(string url)
    {
        Url urlToReturn = _context.Urls.FirstOrDefault(u => u.ShortUrl == url);
        return urlToReturn;
    }

    public Url PersistShortLink(Url url)
    {
        _context.Add(url);
        _context.SaveChanges();
        return url;
    }
}
