using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PromoRoboJob.Infrastructure.DI;
using PromoRoboJob.Application.DI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfra(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddControllers();

var app = builder.Build();

app.MapGet("/health", () => "ok");
app.MapControllers();

app.Run();
