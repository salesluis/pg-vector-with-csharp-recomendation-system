namespace PgVectorWithCSharp.DTO;

public record CreateProductDto(string Title,
    string Category,
    string Summary,
    string Description);