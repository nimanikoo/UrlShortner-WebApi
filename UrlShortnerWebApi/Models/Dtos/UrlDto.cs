namespace UrlShortnerWebApi.Models.Dtos;

public class UrlDto
{
    public string Url { get; set; }
    public DateTime ExpiresAt { get; set; }
}
