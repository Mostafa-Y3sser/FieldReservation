using Microsoft.AspNetCore.RateLimiting;
using Asp.Versioning;
using Microsoft.OpenApi;
using FieldReservation.API.Middlewares;
using FieldReservation.Application.Extentions;
using FieldReservation.Infrastructure.Extensions;
using FieldReservation.Infrastructure.Persistence.Seeding;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FieldReservation API", Version = "v1" });
});

builder.Services.AddProblemDetails();

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddFixedWindowLimiter("auth", options =>
    {
        options.AutoReplenishment = true;
        options.PermitLimit = 5;
        options.QueueLimit = 0;
        options.Window = TimeSpan.FromMinutes(15);
    });
});

var app = builder.Build();

// Seed Database
await DbInitializer.SeedAsync(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FieldReservation API v1");
    });
}

app.UseDeveloperExceptionPage();

app.UseMiddleware<GlobalExceptionHandler>();

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();