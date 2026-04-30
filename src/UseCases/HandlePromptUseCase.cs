using Microsoft.Agents.AI;
using PgVectorWithCSharp.AI;

namespace PgVectorWithCSharp.UseCases;

public class HandlePromptUseCase()
{
    public async Task<AgentResponse> ExecuteAsync(string prompt)
    {
        var router = AgentFactory.GetAgent("router");
        var intent = (await router!.RunAsync(prompt))
            .ToString()
            .Trim()
            .ToLower();
        
        var especialist = AgentFactory.GetAgent(intent);

        return await especialist!.RunAsync(prompt);
    }
}