namespace ContentHub.Frontend.Tables;

public class Like
{
    public int LikeId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int ArticleId { get; set; }

  
    public Article Article { get; set; }
}