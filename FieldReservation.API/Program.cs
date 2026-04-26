using Microsoft.OpenApi;
using FieldReservation.API.Middlewares;
using FieldReservation.Application.Extentions;
using FieldReservation.Infrastructure.Extensions;
using FieldReservation.Infrastructure.Persistence.Seeding;
using FieldReservation.Application.Common.Interfaces;
using FieldReservation.API.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);


builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService,CurrentUserService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FieldReservation API", Version = "v1" });
});

builder.Services.AddProblemDetails();



var app = builder.Build();

// Seed Database
await DbInitializer.SeedAsync(app.Services);

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FieldReservation API v1");
    });

    app.UseDeveloperExceptionPage();


app.UseMiddleware<GlobalExceptionHandler>();

app.UseExceptionHandler();

app.UseHttpsRedirection();



app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();