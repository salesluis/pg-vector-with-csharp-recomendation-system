using Microsoft.EntityFrameworkCore;
using OllamaSharp;
using PgVectorWithCSharp.AI;
using PgVectorWithCSharp.AI.Abstractions;
using PgVectorWithCSharp.Infra.Data;
using PgVectorWithCSharp.UseCases;

namespace PgVectorWithCSharp;

public static class DependencyInjection
{
    public static void AddDb(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                               ?? throw new InvalidOperationException("No connection string");

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString, o => o.UseVector());
        });
       
    }

    public static void AddAiAgentService(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<OllamaApiClient>(x => new OllamaApiClient(
            uriString: "http://localhost:11434",
            defaultModel: "mxbai-embed-large"
        ));
        
        builder.Services.AddTransient<HandlePromptUseCase>();
    }
    
}