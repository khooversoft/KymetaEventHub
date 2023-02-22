using Kymeta.Cloud.Services.Toolbox.Extensions;
using Kymeta.Cloud.Services.Toolbox.Tools;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows;

public record TransLogItem
{
    public bool IsReplay { get; init; }
    public string TypeName { get; init; } = null!;
    public object Subject { get; init; } = default!;
}


public class TransLogItemBuilder
{
    public bool IsReplay { get; set; }
    public string? TypeName { get; set; }
    public object? Subject { get; set; }

    public TransLogItemBuilder SetIsReplay(bool isReplay) => this.Action(x => x.IsReplay = isReplay);
    public TransLogItemBuilder SetTypeName(string typeName) => this.Action(x => x.TypeName = typeName);
    public TransLogItemBuilder SetSubject(object? subject, string typeName) => this.Action(x => { x.Subject = subject; x.TypeName = typeName; });
    public TransLogItemBuilder SetSubject<T>(T subject) => this.Action(x => x.SetSubject(subject, typeof(T).GetTypeName()));

    public TransLogItem Build()
    {
        TypeName.NotEmpty(message: "required");
        Subject.NotNull(message: "required");

        return new TransLogItem
        {
            IsReplay = IsReplay,
            TypeName = TypeName,
            Subject = Subject,
        };
    }
}

