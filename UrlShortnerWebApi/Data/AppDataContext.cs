using Microsoft.EntityFrameworkCore;
using System.Runtime.Serialization.DataContracts;
using UrlShortnerWebApi.Models;

namespace UrlShortnerWebApi.Data;

public class AppDataContext : DbContext
{
    public AppDataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Url> Urls { get; set; }
}