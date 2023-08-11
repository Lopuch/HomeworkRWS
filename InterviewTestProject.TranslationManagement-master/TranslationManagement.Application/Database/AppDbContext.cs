using Microsoft.EntityFrameworkCore;
using TranslationManagement.Application.Models;

namespace TranslationManagement.Application.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<TranslationJob> TranslationJobs { get; set; }
    public DbSet<TranslatorModel> Translators { get; set; }
}