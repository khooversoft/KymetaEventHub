using Microsoft.Extensions.DependencyInjection;

namespace Kymeta.Cloud.Services.EnterpriseBroker.IntegrationTests;

[CollectionDefinition("Integration")]
public class TestApplicationFixture : IClassFixture<EnterpriseBrokerFactory>
{
    private readonly EnterpriseBrokerFactory _factory;

    protected TestApplicationFixture(EnterpriseBrokerFactory factory)
    {
        _factory = factory;
        Client = factory.Client;
    }

    public HttpClient Client { get; }

    protected T Resolve<T>() where T : notnull
    {
        return default!;
        //return _factory.Services.GetRequiredService<T>();
    }
}