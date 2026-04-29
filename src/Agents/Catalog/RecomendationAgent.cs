using Microsoft.Extensions.AI;
using PgVectorWithCSharp.Agents.Abstractions;

namespace PgVectorWithCSharp.Agents.Catalog;

public class RecomendationAgent : AgentCreator
{
    protected override string Uri => "http://localhost:11434";
    protected override string Model => "llama3.1:latest";

    protected override string SystemPrompt => """"
                                              Você é um assistente de recomendações.
                                              O usuário irá te enviar uma lista de itens encontrados no banco de dados
                                              e uma pergunta. Seu papel é formatar essa lista em linguagem natural,
                                              de forma amigável e contextualizada com a pergunta do usuário.
                                              Nunca invente itens que não estejam na lista fornecida e uma de suas tarefas é poupar tokens,
                                              então sela breve das suas palavras.
                                              
                                              SEMPRE responda APENAS com um JSON válido, sem texto adicional, 
                                              sem markdown, sem explicações. Exatamente neste formato:
                                              {
                                                  "summary": "resumo em linguagem natural da recomendação que o usuário pediu",
                                                  "items": [
                                                      {
                                                          "title": "título do item",
                                                          "category": "categoria",
                                                          "reason": "motivo da recomendação e uma breve história  e porque é uma boa 
                                                          opção e em quais ocasioes ele se encaixa, escreva sempre em linguagem natural"
                                                      }
                                                  ]
                                              }
                                              """";
    protected override AITool[] Tools => [];
}