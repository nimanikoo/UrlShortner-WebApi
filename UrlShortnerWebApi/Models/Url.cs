using System.ComponentModel.DataAnnotations;

namespace UrlShortnerWebApi.Models;

public class Url
{
    [Key]
    public long Id { get; set; }
    [Required]
    public string OrginalUrl { get; set; }
    public string ShortUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
}
