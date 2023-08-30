using Microsoft.EntityFrameworkCore;
using NoticiasAPI.Model;

namespace TechChallenge.NoticiasAPI.Data;

public class NoticiasContext : DbContext
{
    public NoticiasContext(DbContextOptions<NoticiasContext> opts) : base(opts) { }
    public DbSet<Noticia> Noticias { get; set; }
}
