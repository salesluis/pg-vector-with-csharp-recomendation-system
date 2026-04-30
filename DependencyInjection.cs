using Microsoft.EntityFrameworkCore;
using OllamaSharp;
using PgVectorWithCSharp.Agents;
using PgVectorWithCSharp.Agents.Abstractions;
using PgVectorWithCSharp.Data;
using PgVectorWithCSharp.UseCases;

namespace PgVectorWithCSharp;

public static class DependencyInjection
{
    public static void MapServices(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                               ?? throw new InvalidOperationException("No connection string");

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString, o => o.UseVector());
        });
        builder.Services.AddScoped<IAgentFactory, AgentFactory>();
        builder.Services.AddTransient<AgentRecomendationUseCase>();
        builder.Services.AddTransient<OllamaApiClient>(x => new OllamaApiClient(
            uriString: "http://localhost:11434",
            defaultModel: "mxbai-embed-large"
        ));

    }
}