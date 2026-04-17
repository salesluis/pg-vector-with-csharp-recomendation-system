using Microsoft.EntityFrameworkCore;
using OllamaSharp;
using PgVectorWithCSharp.Data;

namespace PgVectorWithCSharp;

public static class DependencyInjection
{
    public static void MapServices(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString, o => o.UseVector());
        });

        builder.Services.AddTransient<OllamaApiClient>(x => new OllamaApiClient(
            uriString: "http://localhost:11434",
            defaultModel: "mxbai-embed-large"
        ));

    }
}