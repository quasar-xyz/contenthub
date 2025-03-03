using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ContentHub.Backend.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }



    public DbSet<Article> Articles { get; set; }
    public DbSet<Likes> Likes { get; set; }
    public DbSet<Categories> Categories { get; set; }
    public DbSet<Tags> Tags { get; set; }
    public DbSet<Comments> Comments { get; set; }
    public DbSet<Bookmarks> Bookmarks { get; set; }
}



