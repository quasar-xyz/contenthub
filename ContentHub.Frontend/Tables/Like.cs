namespace ContentHub.Frontend.Tables;

public class Like
{
    public int LikeId { get; set; }
    public int UserId { get; set; }
    public int ArticleId { get; set; }

  
    public Article Article { get; set; }
}