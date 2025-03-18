using System.ComponentModel.DataAnnotations;

namespace ContentHub.Frontend.Tables;

public class Article
{
   [Key]
    public int ArticleId { get; set; }
    
    public string Title { get; set; } = string.Empty;
    
    public string Content { get; set; } = string.Empty;
    
    public DateTime PublishDate { get; set; } = DateTime.UtcNow;

    public string AuthorId { get; set; } = string.Empty;
    
    public int? CategoryId { get; set; }

    public Category? Category { get; set; }
    
    public ICollection<ArticleTag> ArticleTags { get; set; } = new List<ArticleTag>();
    
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    
    public ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
    
    public ICollection<Like> Likes { get; set; } = new List<Like>();
    
    public string? PhotoPath { get; set; } 
}