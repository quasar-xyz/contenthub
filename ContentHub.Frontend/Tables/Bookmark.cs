namespace ContentHub.Frontend.Tables;

public class Bookmark
{
    public int BookmarkId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int ArticleId { get; set; }
    public DateTime BookmarkDate { get; set; }

   
    public Article? Article { get; set; }
}