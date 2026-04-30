using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel.Embeddings;
using OllamaSharp;
using Pgvector;
using Pgvector.EntityFrameworkCore;
using PgVectorWithCSharp.AI.Abstractions;
using PgVectorWithCSharp.DTO;
using PgVectorWithCSharp.Infra.Data;
using PgVectorWithCSharp.Models;

namespace PgVectorWithCSharp.AI.Tools;

public static class RecomendationTools
{
    private static ApplicationDbContext _db = default!;
    private static OllamaApiClient _ollama = default!;

    public static void Initialize(ApplicationDbContext db, OllamaApiClient ollama)
    {
        _db = db;
        _ollama = ollama;
    }
    
    public static async Task<List<Product>> GetRecomendations(string prompt)
    {
        try
        {
            var embeddings = await _ollama
                .AsTextEmbeddingGenerationService()
                .GenerateEmbeddingAsync(prompt);

            var recomendation = await _db
                .Products
                .OrderBy(p => p.Embedding
                    .CosineDistance(new Vector(embeddings.ToArray())))
                .Take(3)
                .AsNoTracking()
                .ToListAsync();
            
            return recomendation;
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
}