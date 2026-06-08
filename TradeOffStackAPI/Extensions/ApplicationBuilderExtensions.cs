using TradeOffStackAPI.Middleware;

namespace TradeOffStackAPI.Extensions;

public static class ApplicationBuilderExtensions
{
    /// <summary>Branche le middleware central de gestion des exceptions.</summary>
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
        => app.UseMiddleware<ExceptionHandlingMiddleware>();
}
