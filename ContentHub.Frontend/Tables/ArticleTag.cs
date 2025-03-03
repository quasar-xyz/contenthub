namespace ContentHub.Frontend.Tables;

public class ArticleTag
{
    public int ArticleTagId { get; set; }
    public int TagId { get; set; }

    
    public Article Article { get; set; }
    public Tag Tag { get; set; }
}