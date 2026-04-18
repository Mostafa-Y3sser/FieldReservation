using FieldReservation.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace FieldReservation.API.Middlewares
{
    public class GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger,
        IHostEnvironment env)
    {

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex, default!);
            }
        }

        public async Task HandleExceptionAsync(HttpContext context, Exception exception,
            CancellationToken cancellationToken)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode =StatusCodes.Status500InternalServerError;

            string detail = env.IsDevelopment()
                ? exception.Message : "An unexpected error occurred";

            var problem = new ProblemDetails
            {
                Title = "Internal Server Error",
                Status = StatusCodes.Status500InternalServerError,
                Detail =detail,
                Instance = context.Request.Path
            };

            await context.Response.WriteAsJsonAsync(problem, cancellationToken);
        }
    }
}