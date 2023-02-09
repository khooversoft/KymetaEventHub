namespace Kymeta.Cloud.Services.EnterpriseBroker;

public class AuthKeyMiddleware
{
    private readonly IConfiguration _config;
    private readonly RequestDelegate _next;

    private const string SHARED_KEY_HEADER = "ebKey";
    private const int UNAUTHORIZED_RESPONSE = 401;

    public AuthKeyMiddleware(RequestDelegate next, IConfiguration config)
    {
        _next = next;
        _config = config;
    }

    public async Task Invoke(HttpContext context)
    {
        // If it's not an API call, means it's a RazorPages call, so skip the auth key validation
        if (context.Request.Path.HasValue && !context.Request.Path.Value.Contains("api/"))
        {
            await _next.Invoke(context);
            return;
        }

        var keyHeader = context.Request.Headers.FirstOrDefault(o => o.Key.ToLower() == SHARED_KEY_HEADER.ToLower());

        if (string.IsNullOrEmpty(keyHeader.Value) || keyHeader.Value != _config["EnterpriseBrokerKey"])
        {
            context.Response.StatusCode = UNAUTHORIZED_RESPONSE;
            await context.Response.WriteAsync("Unauthorized.");
            return;
        }

        await _next.Invoke(context);
    }
}

public static class SharedKeyMiddlewareExtension
{
    public static IApplicationBuilder UseAuthKeyMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuthKeyMiddleware>();
    }
}

