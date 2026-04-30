using Microsoft.Extensions.AI;
using PgVectorWithCSharp.Agents.Abstractions;

namespace PgVectorWithCSharp.Agents.Catalog;

public class InventoryAgent : AgentCreator
{
    protected override string Uri => "http://localhost:11434";
    protected override string Model => "phi3:latest";
    protected override string SystemPrompt => """
                                              Você é um assistente especialista em estoque e preços.
                                              Receberá uma lista de produtos com seus preços e quantidades disponíveis.

                                              Responda em linguagem natural, de forma clara e objetiva,
                                              informando disponibilidade, preço e qualquer detalhe relevante do produto.

                                              Nunca invente produtos ou preços que não estejam na lista fornecida.
                                              Se o produto não estiver disponível, informe claramente ao usuário.
                                              """;

    protected override AIFunction[] Tools => [];
}