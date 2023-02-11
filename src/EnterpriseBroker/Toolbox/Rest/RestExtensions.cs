using System.Net;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.Toolbox.Rest;

public static class RestExtensions
{
    public static async Task<T?> GetContent<T>(this Task<RestResponse> httpResponse)
    {
        var response = await httpResponse;
        return response.GetContent<T>();
    }

    public static T? GetContent<T>(this RestResponse response)
    {
        return response.Content switch
        {
            null => default,
            var v => tryDeserialize(v),
        };

        T? tryDeserialize(string value)
        {
            try
            {
                return Json.Default.Deserialize<T>(value).NotNull();
            }
            catch (Exception ex)
            {
                response.Logger?.LogError(ex, "Failed deserialization into type={type}", typeof(T).Name);
                return default;
            }
        };
    }

    public static async Task<T> GetRequiredContent<T>(this Task<RestResponse> httpResponse)
    {
        return await httpResponse.GetContent<T>() switch
        {
            null => throw new ArgumentException("No content"),
            var v => v,
        };
    }

    public static async Task<string> GetContent(this HttpRequestMessage subject)
    {
        return subject.Content switch
        {
            null => "<no content>",
            not null => await subject.Content.ReadAsStringAsync(),
        };
    }

    public static bool IsSuccess(this HttpStatusCode subject) =>
        subject == HttpStatusCode.OK || subject == HttpStatusCode.NoContent || subject == HttpStatusCode.Created;
}
