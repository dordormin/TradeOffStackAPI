using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace TradeOffStackAPI.Middleware;

/// <summary>
/// Middleware central de gestion des exceptions (pattern Chain of Responsibility du pipeline ASP.NET).
/// Convertit les exceptions métier/inattendues en réponses ProblemDetails homogènes,
/// journalise l'erreur et ne fuit jamais de stack trace au client.
/// </summary>
public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleAsync(context, ex);
        }
    }

    private async Task HandleAsync(HttpContext context, Exception exception)
    {
        var (status, title) = exception switch
        {
            ArgumentException => (StatusCodes.Status400BadRequest, "Requête invalide."),
            InvalidOperationException => (StatusCodes.Status409Conflict, "Conflit avec l'état actuel de la ressource."),
            KeyNotFoundException => (StatusCodes.Status404NotFound, "Ressource introuvable."),
            UnauthorizedAccessException => (StatusCodes.Status403Forbidden, "Accès refusé."),
            _ => (StatusCodes.Status500InternalServerError, "Une erreur interne est survenue.")
        };

        if (status == StatusCodes.Status500InternalServerError)
            _logger.LogError(exception, "Exception non gérée sur {Path}", context.Request.Path);
        else
            _logger.LogWarning(exception, "Exception métier ({Status}) sur {Path}", status, context.Request.Path);

        var problem = new ProblemDetails
        {
            Status = status,
            Title = title,
            // CORRECTION : On utilise exception.ToString() en développement pour avoir tous les détails,
            // y compris les exceptions internes, ce qui est crucial pour le débogage.
            Detail = _env.IsDevelopment() ? exception.ToString() : "An internal error occurred.",
            Instance = context.Request.Path
        };

        context.Response.StatusCode = status;
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }
}