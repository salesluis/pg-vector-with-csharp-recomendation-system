using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel.Embeddings;
using OllamaSharp;
using Pgvector;
using Pgvector.EntityFrameworkCore;
using PgVectorWithCSharp.Data;
using PgVectorWithCSharp.DTO;

namespace PgVectorWithCSharp.Routes;

public static class PromptRoute
{
    public static void MapPromptRoute(this WebApplication app)
    {
        app.MapPost("/v1/prompt", async (
            QuestionDto question,
            ApplicationDbContext db,
            OllamaApiClient ollamaClient) =>
        {
            var textEmbeddingGenerationService = ollamaClient.AsTextEmbeddingGenerationService();
            var embeddings = await textEmbeddingGenerationService.GenerateEmbeddingAsync(question.Prompt);

            var recomendations = await db.Recomendations
                .OrderBy(d => d.Embedding
                        //metodo que vem do tipo especial Vector no modelo Recomendation
                    .CosineDistance(new Vector(embeddings.ToArray())))
                .Take(3)
                .Select(x => new
                {
                    x.Title, x.Category
                })
                .AsNoTracking()
                .ToListAsync();

            return Results.Ok(recomendations);
        });
    }
}