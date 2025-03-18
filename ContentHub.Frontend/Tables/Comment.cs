using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ContentHub.Frontend.Tables;

public class Comment
{
    [Key]
    public int CommentId { get; set; }
    public string Content { get; set; }
    
    public DateTime CommentDate { get; set; }
    public string AuthorId { get; set; }
    public int ArticleId { get; set; }
    
    public Article? Article { get; set; }
}