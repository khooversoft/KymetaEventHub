using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.REST;
using Newtonsoft.Json;
using System.Net;

public class OracleResponse<T> where T : IOracleResponsePayload
{
    public OracleResponse(HttpStatusCode status, string? message, string? conent)
    {
        StatusCode = status;
        Message = message;
        Content = conent;
        Payload = GetPayload();
    }

    public string? Message { get; init; }
    public string? Content { get; init; }
    public T? Payload { get; init; }
    public HttpStatusCode StatusCode { get; init; }

    public bool IsSuccessStatusCode() => Payload switch
    {
        null => false,
        _ => Payload.IsSuccessfulResponse()
    };

    private T? GetPayload()
    {
        if (string.IsNullOrEmpty(Content)) { return default; }
        return JsonConvert.DeserializeObject<T>(Content);
    }

}
