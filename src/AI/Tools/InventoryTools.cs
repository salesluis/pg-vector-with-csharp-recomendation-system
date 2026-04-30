using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel.Embeddings;
using OllamaSharp;
using Pgvector;
using Pgvector.EntityFrameworkCore;
using PgVectorWithCSharp.Infra.Data;
using PgVectorWithCSharp.Models;

namespace PgVectorWithCSharp.AI.Tools;

public static class InventoryTools
{
    private static ApplicationDbContext _db = null!;
    private static OllamaApiClient _ollama = null!;
    
    public static void Initialize(ApplicationDbContext db, OllamaApiClient ollama)
    {
        _db = db;
        _ollama = ollama;
    }

    [Description("Busca um produto pelo nome exato")]
    public static async Task<Product?> GetProductByName(string productName) =>
        await _db.Products
            .Where(p => p.Title.ToLower() == productName.ToLower())
            .FirstOrDefaultAsync();

    [Description("Busca produtos similares por contexto quando o nome exato não é conhecido")]
    public static async Task<List<Product>> GetSimilarProducts(
        [Description("contexto que o usuário mandou")]string context)
    {
        var embeddings = await _ollama
            .AsTextEmbeddingGenerationService()
            .GenerateEmbeddingAsync(context);

        var p =  await _db.Products
            .OrderBy(p => p.Embedding.CosineDistance(new Vector(embeddings.ToArray())))
            .Take(3)
            .AsNoTracking()
            .ToListAsync();

        return p;
    }
}