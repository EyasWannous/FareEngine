using FareEngine.API;
using FareEngine.API.DependencyInjection.Extensions;
using FareEngine.Domain.SoldProducts;
using FareEngine.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddDomain()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseExceptionHandler();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var soldProductManager = scope.ServiceProvider.GetRequiredService<SoldProductManager>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    var pendingMigrations = await context.Database.GetPendingMigrationsAsync();

    var pendingMigrationList = pendingMigrations.ToList();
    
    if (pendingMigrationList.Count is not 0)
    {
        logger.LogInformation("Applying {Count} pending migration(s)...", pendingMigrationList.Count);
        await context.Database.MigrateAsync();
        logger.LogInformation("Migrations applied successfully.");
    }
    else
    {
        logger.LogInformation("No pending migrations.");
    }

    await Seeder.SeedAsync(context, soldProductManager, logger);
}

await app.RunAsync();