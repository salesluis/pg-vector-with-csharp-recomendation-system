using System.Text.Json;
using Microsoft.Agents.AI;
using PgVectorWithCSharp.AI;

namespace PgVectorWithCSharp.UseCases;

public class HandlePromptUseCase()
{
    public async Task<AgentResponse?> ExecuteAsync(string prompt)
    {
        var router = AgentFactory.GetAgent("router");
        var intent = (await router!.RunAsync(prompt))
            .ToString()
            .Trim()
            .ToLower();
        
        var especialist = AgentFactory.GetAgent(intent);
        var rawResponse =  await especialist!.RunAsync(prompt);
        return ParseResponse(rawResponse.ToString()!);

    }
    
    private AgentResponse? ParseResponse(string raw)
    {
        try
        {
            return JsonSerializer.Deserialize<AgentResponse>(raw, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch
        {
            // Fallback: LLM não seguiu o formato JSON
            return new AgentResponse();
        }
    }
}