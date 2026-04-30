using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel.Embeddings;
using OllamaSharp;
using Pgvector;
using PgVectorWithCSharp.DTO;
using PgVectorWithCSharp.Infra.Data;
using PgVectorWithCSharp.Models;

namespace PgVectorWithCSharp.Routes;

public static class ProductRoute
{
   public static void MapProductRoute(this WebApplication app)
   {
      app.MapPost("/v1/products", async (
         CreateProductDto model,
         ApplicationDbContext db,
         OllamaApiClient ollamaClient) =>
      {
         var textEmbeddingGenerationService = ollamaClient.AsTextEmbeddingGenerationService();
         var embeddings = await textEmbeddingGenerationService.GenerateEmbeddingAsync(model.Category);

         var recomendation = new Recomendation
         {
            Title = model.Title,
            Category = model.Category,
            Embedding = new Vector(embeddings)
         };

         await db.Recomendations.AddAsync(recomendation);
         await db.SaveChangesAsync();

         return Results.Created($"/v1/products", null);
      });

      app.MapGet("/v1/products", async (ApplicationDbContext db) => 
         await db
            .Products
            .AsNoTracking()
            .ToListAsync());
   }
}