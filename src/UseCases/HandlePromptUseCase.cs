using Microsoft.Agents.AI;
using PgVectorWithCSharp.Agents.Abstractions;
using PgVectorWithCSharp.DTO;

namespace PgVectorWithCSharp.UseCases;

public class HandlePromptUseCase(IAgentFactory agentFactory)
{
    public async Task<AgentResponse> ExecuteAsync(string prompt)
    {
        var router = agentFactory.GetAgent("router");
        var intent = (await router!.RunAsync(prompt))
            .ToString()
            .Trim()
            .ToLower();
        
        var especialist = agentFactory.GetAgent(intent);

        return await especialist!.RunAsync(prompt);
    }
}