using Microsoft.Agents.AI;
using PgVectorWithCSharp.Agents.Abstractions;
using PgVectorWithCSharp.Agents.Catalog;

namespace PgVectorWithCSharp.Agents;

public static class AgentFactory
{
    private static readonly Dictionary<string, AgentCreator> _agents = new()
    {
        ["recomendation"] = new RecomendationAgent()
    };

    public static ChatClientAgent? GetAgent(string agentName)
    {
        if (!_agents.TryGetValue(agentName, out var agent))
            throw new InvalidOperationException(@$"O agente {agentName}, não foi encontrado.
                                                 Verifique se digitou o nome correto ou se foi criado o agente");
        return agent!.CreateAgent();
    }
}