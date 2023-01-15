namespace UrlShortnerWebApi.Models.Dtos;

public class UrlResponseDto
{
    public string OrginalUrl { get; set; }
    public string ShortUrl { get; set; }
    public DateTime ExpiresAt { get; set; }
}
