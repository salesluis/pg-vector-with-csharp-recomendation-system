using System.Text.Json.Serialization;

namespace PgVectorWithCSharp.DTO;

public record AgentItemDto(
    string Title,
    string Category,
    string Reason);

public record AgentResponseDto(
    string Summary,
    List<AgentItemDto> Items);