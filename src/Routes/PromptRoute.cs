using PgVectorWithCSharp.DTO;
using PgVectorWithCSharp.UseCases;

namespace PgVectorWithCSharp.Routes;

public static class PromptRoute
{
    public static void MapPromptRoute(this WebApplication app)
    {
        app.MapPost("/v1/prompt", async (QuestionDto question, AgentRecomendationUseCase useCase) =>
        {
            var response = await useCase.ExecuteAsync(question);
            return Results.Ok(response);
        });
    }
}