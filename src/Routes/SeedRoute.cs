using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel.Embeddings;
using OllamaSharp;
using Pgvector;
using PgVectorWithCSharp.Infra.Data;
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
            
            foreach (var product in products)
            {
                var embeddings = await textEmbeddingGenerationService.GenerateEmbeddingAsync(product.Description);

                var p = new Product
                {
                    Id = product.Id,
                    Category =  product.Category,
                    Description = product.Description,
                    Title = product.Title,
                    Inventory = product.Inventory,
                    Summary = product.Summary,
                    Embedding = new Vector(embeddings)
                };

                await db.Products.AddAsync(p);
                await db.SaveChangesAsync();
            }

            return Results.Ok(new { message = "Seeded" });
        });
    }
}