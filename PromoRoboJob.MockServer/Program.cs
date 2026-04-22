using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapPost("/message/sendText/{instanceId}", async (HttpRequest req, string instanceId) =>
{
    // read body for logging
    var body = await new System.IO.StreamReader(req.Body).ReadToEndAsync();
    Console.WriteLine($"MockServer received for instance={instanceId} body={body}");
    return Results.Ok(new { ok = true, instance = instanceId });
});

app.Run("http://localhost:3000");
