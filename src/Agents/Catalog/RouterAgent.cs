using Microsoft.Extensions.AI;
using PgVectorWithCSharp.Agents.Abstractions;

namespace PgVectorWithCSharp.Agents.Catalog;

public class RouterAgent : AgentCreator
{
    protected override string Uri => "http://localhost:11434";
    protected override string Model => "phi3:latest";
    protected override string SystemPrompt => """
                                              Você é um classificador de intenções.
                                              Dado um prompt do usuário, responda APENAS com uma das palavras abaixo,
                                              sem texto adicional, sem explicações:

                                              - recomendation  (quando o usuário pede sugestões ou recomendações)
                                              - branch          (quando o usuário menciona cidade, filial ou localização)
                                              - inventory       (quando o usuário pergunta sobre estoque ou preço)

                                              Exemplos:
                                              "me indique um cafe" → recomendation
                                              "tem filial em tal cidade?" → branch
                                              "qual o preço do produto X?" → inventory
                                              """;

    protected override AIFunction[] Tools => [];
}