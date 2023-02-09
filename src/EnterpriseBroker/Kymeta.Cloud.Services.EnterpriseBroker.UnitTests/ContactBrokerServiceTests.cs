using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.OSS;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce;
using Kymeta.Cloud.Services.EnterpriseBroker.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Kymeta.Cloud.Services.EnterpriseBroker.UnitTests
{
    public class ContactBrokerServiceTests : IClassFixture<TestFixture>
    {
        private TestFixture _fixture;

        public ContactBrokerServiceTests(TestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        [Trait("Category", "ContactBrokerTests")]
        [Trait("Category", "ContactBrokerOracleTests")]
        public async void SyncToOracle_CreateContact_ReturnsSuccess()
        {
            // Arrange
            var model = Helpers.BuildSalesforceContactModel(true, false);
            var transaction = Helpers.BuildSalesforceTransaction();
            Helpers.MockActionRepository(_fixture.ActionsRepository, transaction);

            // Get Org
            var oracleOrg = Helpers.BuildOracleOrganization();
            _fixture.OracleService
                .Setup(ors => ors.GetOrganizationById(It.IsAny<string>(), transaction, null))
                .ReturnsAsync(new Tuple<bool, OracleOrganization, string>(true, oracleOrg, "Explosions"));
            // Get Customer Account
            var customerAccount = Helpers.BuildOracleCustomerAccount();
            _fixture.OracleService
                .Setup(ors => ors.GetCustomerAccountById(It.IsAny<string>(), transaction, null))
                .ReturnsAsync(new Tuple<bool, OracleCustomerAccount, string>(true, customerAccount, "Explosions"));
            // Get list of persons
            var persons = new List<OraclePersonObject>
            {
                new OraclePersonObject
                {
                    IsPrimary = true,
                    PartyId = 30001,
                    OrigSystemReference = "con30001",
                    RelationshipId = 40001
                }
            };
            // Empty list
            _fixture.OracleService
                .Setup(ors => ors.GetPersonsById(It.IsAny<List<Tuple<string, ulong?>>>(), transaction))
                .ReturnsAsync(new Tuple<bool, IEnumerable<OraclePersonObject>, string>(true, null, string.Empty));
            // Create the person
            _fixture.OracleService
                .Setup(ors => ors.CreatePerson(It.IsAny<SalesforceContactModel>(), It.IsAny<ulong>(), transaction))
                .ReturnsAsync(new Tuple<OraclePersonObject, string>(persons.First(), string.Empty));
            // Update the customer profile children
            _fixture.OracleService
                .Setup(ors => ors.UpdateCustomerAccountChildren(It.IsAny<OracleCustomerAccount>(), transaction, It.IsAny<List<OracleCustomerAccountSite>>(), It.IsAny<List<OracleCustomerAccountContact>>()))
                .ReturnsAsync(new Tuple<OracleCustomerAccount, string>(customerAccount, string.Empty));

            // Act
            var svc = new ContactBrokerService(_fixture.ActionsRepository.Object, _fixture.OracleService.Object, _fixture.SalesforceClient.Object);
            var result = await svc.ProcessContactAction(model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(model.ObjectId, result.SalesforceObjectId);
            Assert.Equal(StatusType.Successful, result.OracleStatus);
            Assert.Equal(StatusType.Skipped, result.OSSStatus);
            Assert.Null(result.OSSErrorMessage);
            Assert.Null(result.OracleErrorMessage);
        }
    }
}
