using PgVectorWithCSharp;
using PgVectorWithCSharp.Routes;

var builder = WebApplication.CreateBuilder(args);

builder.MapServices();

var app = builder.Build();
app.MapProductRoute();
app.MapSeedRoute();
app.MapPromptRoute();

app.Run();
