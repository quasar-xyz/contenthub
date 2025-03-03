namespace ContentHub.Frontend.Tables;

public class Category
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }

    // Navigation Property
    public ICollection<Article> Articles { get; set; }
}