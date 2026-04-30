using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OllamaSharp;

namespace PgVectorWithCSharp.AI.Abstractions;

public abstract class AgentCreator
{
    protected abstract string Uri { get; }
    protected abstract string Model { get; }
    protected abstract string SystemPrompt { get; }
    protected abstract AITool[] Tools { get; }

    public ChatClientAgent CreateAgent() => new 
        OllamaApiClient(Uri, Model)
        .AsAIAgent(
            SystemPrompt,
            tools: Tools);
}