using Microsoft.Extensions.AI;
using PgVectorWithCSharp.AI.Abstractions;

namespace PgVectorWithCSharp.AI.Agents;

public class RouterAgent : AgentCreator
{
    protected override string Uri => "http://localhost:11434";
    protected override string Model => "phi3:latest";
    protected override string SystemPrompt => """
                                              Você é um classificador de intenções.
                                              Dado um prompt do usuário, responda APENAS com uma das palavras abaixo,
                                              sem texto adicional, sem explicações:

                                              - recomendation  (quando o usuário pede sugestões ou recomendações)
                                              - inventory      (quando o usuário pergunta sobre estoque ou preço)
                                              - generic        (quando o usuário fala algo fora do escopo das outras)

                                              Exemplos:
                                              "me indique um cafe" → recomendation
                                              "qual o preço do produto X?" → inventory
                                              Caso seja algo mais genérico como um simples "oi" -> generic
                                              """;

    protected override AIFunction[] Tools => [];
}