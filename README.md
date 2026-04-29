# PgVectorWithCSharp

Este projeto é uma aplicação ASP.NET Core Minimal API em C# que demonstra o uso de **PgVector** (uma extensão do PostgreSQL para trabalhar com vetores/embeddings) para implementar um sistema de recomendações baseado em similaridade semântica. Ele integra o **Ollama** para gerar embeddings de texto usando modelos de IA locais, permitindo buscas vetoriais eficientes no banco de dados.

A aplicação gerencia produtos (ex.: cafés) e gera recomendações baseadas em categorias, utilizando embeddings para encontrar similaridades via busca por proximidade vetorial (usando distância cosseno).

## Índice

- [Pré-requisitos](#pré-requisitos)
- [Como Rodar o Projeto](#como-rodar-o-projeto)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Endpoints da API](#endpoints-da-api)
- [Contribuição](#contribuição)
- [Licença](#licença)

## Pré-requisitos

Para rodar o projeto, você precisa dos seguintes itens instalados e configurados:

1. **Ollama**:
   - Instale o Ollama (disponível em [ollama.ai](https://ollama.ai)). É uma ferramenta para rodar modelos de IA localmente.
   - Após instalar, baixe o modelo de embeddings especificado no código: `mxbai-embed-large` (um modelo de embeddings otimizado para tarefas de similaridade).
     - Comando: `ollama pull mxbai-embed-large`.
   - Certifique-se de que o Ollama esteja rodando na porta padrão (11434), pois o código se conecta a `http://localhost:11434`.

2. **Modelo para Geração de Embeddings**:
   - O projeto usa o modelo `mxbai-embed-large` via Ollama para gerar vetores de embeddings (de dimensão 1024, conforme definido no schema SQL).
   - Esse modelo converte texto (ex.: categorias de produtos ou perguntas) em vetores numéricos, permitindo comparações de similaridade.

3. **Docker**:
   - Instale o Docker (ou Docker Desktop) para rodar o banco de dados PostgreSQL com a extensão PgVector.
   - O projeto inclui um `compose.yaml` que configura um container PostgreSQL com PgVector pré-instalado.

4. **C# 10+ (.NET 6+)**:
   - O projeto usa .NET 10.0 (uma versão futura/preview do .NET, que suporta C# 12+). Certifique-se de ter o .NET SDK 10.0 instalado (disponível em [dotnet.microsoft.com](https://dotnet.microsoft.com)).
   - Se você não tiver .NET 10.0, pode ajustar o `TargetFramework` no `.csproj` para `net8.0` ou `net6.0` (compatível com C# 10+), mas teste para compatibilidade com as dependências.
   - Dependências principais (via NuGet):
     - `Microsoft.SemanticKernel.Connectors.Ollama` (para integração com Ollama).
     - `Microsoft.SemanticKernel.Connectors.PgVector` (para operações vetoriais).
     - `Npgsql.EntityFrameworkCore.PostgreSQL` (Entity Framework para PostgreSQL).
     - `Pgvector.EntityFrameworkCore` (suporte a PgVector no EF).

## Como Rodar o Projeto

Siga estes passos para configurar e executar:

1. **Clone ou navegue até o diretório do projeto**:
   - Certifique-se de estar no diretório raiz: `PgVectorWithCSharp`.

2. **Suba o Banco de Dados com Docker**:
   - Execute `docker compose up` (ou `docker-compose up` se usar uma versão antiga do Docker).
   - Isso cria um container PostgreSQL (porta 5432) com a extensão PgVector instalada. Os scripts SQL em `/src/Init/` são executados automaticamente:
     - `01_extensions.sql`: Instala a extensão `vector`.
     - `02_create_schema.sql`: Cria tabelas `recomendations` (com coluna `embedding` do tipo `vector(1024)`) e `products`.
     - `03_seed.sql`: Insere 15 produtos de exemplo (cafés de diferentes origens).
   - O banco fica acessível em `localhost:5432` com usuário/senha `postgres/postgres` e database `PgVectorWithCSharpDb`.

3. **Rode o Aplicativo C#**:
   - No terminal, execute `dotnet restore` para instalar dependências.
   - Em seguida, `dotnet run` para iniciar o servidor web (padrão: `https://localhost:5001` ou `http://localhost:5000`, conforme `launchSettings.json`).
   - O app usa Minimal APIs e está configurado para desenvolvimento (com connection string em `appsettings.Development.json`).

4. **Teste Inicial**:
   - Acesse os endpoints via ferramentas como Postman, curl ou o arquivo `PgVectorWithCSharpEndpoints.http` incluído no projeto.
   - Primeiro, chame `GET /v1/seed` para gerar embeddings dos produtos existentes e popular a tabela `recomendations`.
   - Em seguida, teste `POST /v1/prompt` com uma pergunta como `{"Prompt": "Café forte e encorpado"}` para ver recomendações similares.

## Estrutura do Projeto

- `src/Data/`: Contexto do Entity Framework e configurações do banco.
- `src/Models/`: Entidades `Product` e `Recomendation` (com suporte a vetores).
- `src/Routes/`: Definições dos endpoints da API.
- `src/Init/`: Scripts SQL para inicialização do banco.
- `compose.yaml`: Configuração Docker para PostgreSQL com PgVector.
- `appsettings.json`: Configurações da aplicação (connection string em `Development`).

## Endpoints da API

O projeto define três grupos de endpoints, implementados como extensões para o `WebApplication` (usando Minimal APIs). Eles lidam com produtos, seed de dados e consultas baseadas em prompts.

### ProductRoute (`/v1/products`)
- `POST /v1/products`: Cria um novo produto. Recebe um `CreateProductDto` (com `Title` e `Category`). Gera embeddings da categoria usando Ollama, salva em `recomendations` (não em `products` diretamente — parece um design para recomendações baseadas em categorias). Retorna 201 Created.
- `GET /v1/products`: Lista todos os produtos da tabela `products` (sem embeddings, apenas dados básicos como título, categoria, resumo e descrição).

### SeedRoute (`/v1/seed`)
- `GET /v1/seed`: Endpoint para "semear" recomendações. Itera sobre todos os produtos existentes, gera embeddings das categorias via Ollama, e insere em `recomendations`. Útil para inicializar dados vetoriais após subir o banco. Retorna `{"message": "Seeded"}`.

### PromptRoute (`/v1/prompt`)
- `POST /v1/prompt`: Recebe um `QuestionDto` com `Prompt` (ex.: uma pergunta ou descrição). Gera embeddings do prompt, busca as 3 recomendações mais similares em `recomendations` usando **distância cosseno** (métrica de similaridade vetorial). Retorna uma lista com `Title` e `Category` das recomendações encontradas. Exemplo de uso: Recomendar cafés similares a uma descrição como "Café suave e floral".

Esses endpoints demonstram um fluxo completo: criar produtos, semear embeddings, e consultar recomendações via similaridade semântica. O projeto é uma prova de conceito para sistemas de recomendação usando IA local e bancos vetoriais.

## Contribuição

Sinta-se à vontade para abrir issues ou pull requests. Para mudanças significativas, abra uma issue primeiro para discutir.

## Licença

Este projeto é de código aberto. Consulte o arquivo LICENSE para detalhes.</content>
<filePath>/var/home/salesluis/Developer/Studies/PgVectorWithCSharp/README.md