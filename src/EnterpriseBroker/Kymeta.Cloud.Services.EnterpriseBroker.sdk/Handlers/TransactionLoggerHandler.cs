using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services.TransactionLog;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows;
using Kymeta.Cloud.Services.Toolbox.Tools;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Handlers;

public class TransactionLoggerHandler : DelegatingHandler
{
    private readonly ITransactionLoggingService _transactionLoggingService;
    public TransactionLoggerHandler(ITransactionLoggingService transactionLoggingService) => _transactionLoggingService = transactionLoggingService.NotNull();

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var requestLog = await LogRequest(request);

        HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

        await LogResponse(response, requestLog);

        return response;
    }

    private async Task<TransLog> LogRequest(HttpRequestMessage request)
    {
        var transLog = new TransLog
        {
            Method = request.Method.ToString(),
            Uri = request.RequestUri?.ToString() ?? "<empty>",
            SendContent = request.Content switch
            {
                null => "(no content)",
                not null => Fixup(await request.Content.ReadAsStringAsync()),
            }
        };

        var logItem = new TransLogItemBuilder().SetSubject(transLog).Build();
        _transactionLoggingService.Add("Send", transLog.InstanceId, logItem);

        return transLog;
    }

    private async Task LogResponse(HttpResponseMessage response, TransLog transLog)
    {
        transLog.StatusCode = response.StatusCode.ToString();
        transLog.ReceiveContent = response.Content switch
        {
            null => "(no content)",
            not null => Fixup(await response.Content.ReadAsStringAsync()),
        };

        var logItem = new TransLogItemBuilder().SetSubject(transLog).Build();

        _transactionLoggingService.Add("Send", transLog.InstanceId, logItem);
    }

    private string? Fixup(string subject) => subject?.Replace("\"", "^");

    private record TransLog
    {
        public string InstanceId { get; } = Guid.NewGuid().ToString();
        public string Method { get; init; } = null!;
        public string Uri { get; init; } = null!;
        public object? SendContent { get; init; }
        public string StatusCode { get; set; } = null!;
        public object? ReceiveContent { get; set; }
    }
}
