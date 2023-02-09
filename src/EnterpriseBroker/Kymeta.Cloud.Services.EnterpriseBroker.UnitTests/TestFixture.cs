using Kymeta.Cloud.Logging.Activity;
using Kymeta.Cloud.Services.EnterpriseBroker.HttpClients;
using Kymeta.Cloud.Services.EnterpriseBroker.Repositories;
using Kymeta.Cloud.Services.EnterpriseBroker.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kymeta.Cloud.Services.EnterpriseBroker.UnitTests
{
    public class TestFixture : IDisposable
    {
        public Mock<ISalesforceClient> SalesforceClient = new();
        public Mock<IAccountsClient> AccountsClient = new();
        public Mock<IUsersClient> UsersClient = new();
        public Mock<IOracleClient> OracleClient = new();
        public Mock<IActionsRepository> ActionsRepository = new();
        public Mock<IActivityLoggerClient> ActivityLoggerClient = new();
        public Mock<IActivityLogger> ActivityLogger = new();
        public Mock<IOssService> OssService = new();
        public Mock<IOracleService> OracleService = new();
        public Mock<ITerminalSerialCacheRepository> TerminalSerialCacheRepository = new();
        public Mock<IManufacturingProxyClient> ManufacturingProxyClient = new();
        public IConfiguration Configuration;

        public TestFixture()
        {
            var configValues = new Dictionary<string, string>
            {
                { "SystemUserId", Guid.NewGuid().ToString() },
                { "KymetaAccountId", Guid.NewGuid().ToString() }
            };
            Configuration = new ConfigurationBuilder().AddInMemoryCollection(configValues).Build();
        }

        public void Dispose()
        {
            // Cleanup here
        }
    }
}
