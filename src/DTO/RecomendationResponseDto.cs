namespace PgVectorWithCSharp.DTO;

public record RecomendationItemDto(
    string Title,
    string Category,
    string Reason);

public record RecomendationResponseDto(
    string Summary,
    List<RecomendationItemDto> Items);