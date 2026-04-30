using Pgvector;

namespace PgVectorWithCSharp.Models;

public class Product
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Inventory { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
     public Vector Embedding { get; set; } = null!;
}