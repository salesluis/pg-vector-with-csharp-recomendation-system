using Microsoft.Agents.AI;

namespace PgVectorWithCSharp.Agents.Abstractions;

public interface IAgentFactory
{
    ChatClientAgent? GetAgent(string agentName);
}