using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace TechChallenge.NoticiasAPI.Data;
public class IdentityContext : IdentityDbContext<IdentityUser> ///Essa herança nos ajuda a implementar as funcionalidades do Identity
{
    public IdentityContext(DbContextOptions<IdentityContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
