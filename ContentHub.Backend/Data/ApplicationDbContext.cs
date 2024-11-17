using Microsoft.EntityFrameworkCore;

namespace ContentHub.Backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }


    public DbSet<Article> Articles { get; set; }
    public DbSet<Likes> Likes { get; set; }
    public DbSet<Categories> Categories { get; set; }
    public DbSet<Tags> Tags { get; set; }
    public DbSet<Comments> Comments { get; set; }
    public DbSet<Bookmarks> Bookmarks { get; set; }
}



//TODO : ADD THE CODE TO THE CLASSES AND ADD THEM TO DATABASE AND ADD APPSETING JASON ASWELL