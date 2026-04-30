// using System.Text.Json;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.SemanticKernel.Embeddings;
// using OllamaSharp;
// using Pgvector;
// using Pgvector.EntityFrameworkCore;
// using PgVectorWithCSharp.AI.Abstractions;
// using PgVectorWithCSharp.DTO;
// using PgVectorWithCSharp.Infra.Data;
//
// namespace PgVectorWithCSharp.UseCases;
//
// public class AgentRecomendationUseCase(
//     [FromServices]OllamaApiClient ollamaClient,
//     [FromServices] ApplicationDbContext db,
//     IAgentFactory agentFactory
//     )
// {
//     public async Task<RecomendationResponseDto?> ExecuteAsync(QuestionDto question)
//     {
//         try
//         {
//             var embeddings = await ollamaClient
//                 .AsTextEmbeddingGenerationService()
//                 .GenerateEmbeddingAsync(question.Prompt);
//
//             var recomendations = await db.Recomendations
//                 .OrderBy(d => d.Embedding
//                     //metodo que vem do tipo especial Vector no modelo Recomendation
//                     .CosineDistance(new Vector(embeddings.ToArray())))
//                 .Take(3)
//                 .Select(x => new { x.Title, x.Category })
//                 .AsNoTracking()
//                 .ToListAsync();
//             
//
//             var context = string.Join("\n", recomendations
//                 .Select((r, i) => $"{i + 1}. {r.Title} - catetoria:  {r.Category}"));
//
//             var prompt = $"""
//                 Dados encontrados: 
//                 {context}
//                 Pergunta:
//                 {question.Prompt}
//                 """;
//
//             var agent = agentFactory.GetAgent("recomendation");
//             var raw = await agent!.RunAsync(prompt);
//
//             var response = JsonSerializer.Deserialize<RecomendationResponseDto>(raw.Text, new JsonSerializerOptions
//             {
//                 PropertyNameCaseInsensitive = true
//             });
//
//             return response;
//         }
//         catch (Exception e)
//         {
//             Console.BackgroundColor = ConsoleColor.Red;
//             Console.ForegroundColor = ConsoleColor.Black;
//             Console.WriteLine(e.Message);
//             Console.ResetColor();
//             Console.WriteLine("###################################  StackTrace  ###################################");
//             Console.WriteLine(e.StackTrace);
//             throw;
//         }
//     }
// }