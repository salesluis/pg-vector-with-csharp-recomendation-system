using Microsoft.Extensions.AI;
using PgVectorWithCSharp.Agents.Abstractions;

namespace PgVectorWithCSharp.Agents.Catalog;

// Infrastructure/AI/Catalog/BranchAgent.cs
public class BranchAgent : AgentCreator
{
    protected override string Uri => "http://localhost:11434";
    protected override string Model => "phi3:latest";
    protected override string SystemPrompt => """
                                              Você é um assistente especialista em filiais.
                                              Receberá uma lista de filiais encontradas e uma pergunta do usuário.

                                              Responda em linguagem natural, de forma amigável e objetiva,
                                              informando endereço, cidade e qualquer detalhe relevante das filiais.

                                              Nunca invente filiais que não estejam na lista fornecida.
                                              Se não houver filiais na lista, informe que não foram encontradas filiais.
                                              """;

    protected override AIFunction[] Tools => [];
}