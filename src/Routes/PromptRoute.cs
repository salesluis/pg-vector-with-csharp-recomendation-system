using PgVectorWithCSharp.DTO;
using PgVectorWithCSharp.UseCases;

namespace PgVectorWithCSharp.Routes;

public static class PromptRoute
{
    public static void MapPromptRoute(this WebApplication app)
    {
        app.MapPost("/v1/prompt", async (QuestionDto question, HandlePromptUseCase useCase) =>
        {
            var response = await useCase.ExecuteAsync(question.Prompt);
            return Results.Ok(response);
        });
    }
}