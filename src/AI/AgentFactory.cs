using Microsoft.Agents.AI;
using PgVectorWithCSharp.AI.Abstractions;
using PgVectorWithCSharp.AI.Agents;

namespace PgVectorWithCSharp.AI;

public static class AgentFactory
{
    private static readonly Dictionary<string, AgentCreator> _agents = new()
    {
        ["router"]          = new RouterAgent(),
        ["generic"]         = new GenericAgent(),
        ["recomendation"]   = new RecomendationAgent(),
        ["inventory"]       = new InventoryAgent(), 
    };

    public static ChatClientAgent? GetAgent(string agentName)
    {
        if (!_agents.TryGetValue(agentName, out var agent))
            throw new InvalidOperationException(@$"O agente {agentName}, não foi encontrado.
                                                 Verifique se digitou o nome correto ou se foi criado o agente");
        return agent!.CreateAgent();
    }
    
}