using Microsoft.Agents.AI;

namespace PgVectorWithCSharp.AI.Abstractions;

public interface IAgentFactory
{
    ChatClientAgent? GetAgent(string agentName);
}