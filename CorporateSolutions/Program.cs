using CorporateSolutions;
using CorporateSolutions.Seeders;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddHttpContextAccessor()
    .AddApplicationControllers()
    .AddApplicationDbContext(builder.Configuration)
    .AddApplicationServices()
    .AddApplicationRepositories()
    .AddApplicationSeeders()
    .AddJwtAuthentication(builder.Configuration);

var app = builder.Build();

app.UseApplicationMiddleware();

await DbInitializer.InitialiseAsync(app.Services);

app.Run();
