using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel.Embeddings;
using OllamaSharp;
using Pgvector;
using PgVectorWithCSharp.Data;
using PgVectorWithCSharp.Models;

namespace PgVectorWithCSharp.Routes;

public static class SeedRoute
{
    public static void MapSeedRoute(this WebApplication app)
    {
        app.MapGet("/v1/seed", async (
            ApplicationDbContext db,
            OllamaApiClient ollamaClient) =>
        {
            var products = await db.Products.ToListAsync();
            var textEmbeddingGenerationService = ollamaClient.AsTextEmbeddingGenerationService();
            
            foreach (var product in products)
            {
                var embeddings = await textEmbeddingGenerationService.GenerateEmbeddingAsync(product.Category);

                var recomendation = new Recomendation
                {
                    Title = product.Title,
                    Category = product.Category,
                    Embedding = new Vector(embeddings)
                };

                await db.Recomendations.AddAsync(recomendation);
                await db.SaveChangesAsync();
            }

            return Results.Ok(new { message = "Seeded" });
        });
    }
}