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
    public class AddressBrokerNegativeTests : IClassFixture<TestFixture>
    {
        private TestFixture _fixture;

        public AddressBrokerNegativeTests(TestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        [Trait("Category", "AddressBrokerTests")]
        [Trait("Category", "AddressBrokerNegativeTests")]
        [Trait("Category", "AddressBrokerOracleTests")]
        public async void SyncToOracle_LocationAndSiteNotExists_FailsToGetOrganization_ReturnsError()
        {
            // Arrange
            var model = Helpers.BuildSalesforceAddressModel(true, false);
            var transaction = Helpers.BuildSalesforceTransaction();
            Helpers.MockActionRepository(_fixture.ActionsRepository, transaction);

            _fixture.OracleService
                .Setup(ors => ors.GetOrganizationById(It.IsAny<string>(), transaction, null))
                .ReturnsAsync(new Tuple<bool, OracleOrganization, string>(false, null, string.Empty));

            // Act
            var svc = new AddressBrokerService(_fixture.ActionsRepository.Object, _fixture.OracleService.Object);
            var result = await svc.ProcessAddressAction(model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(model.ObjectId, result.SalesforceObjectId);
            Assert.Equal(StatusType.Error, result.OracleStatus);
            Assert.Equal(StatusType.Skipped, result.OSSStatus);
            Assert.Equal($"Error syncing Address to Oracle: Organization object with SF reference Id {model.ParentAccountId} was not found.", result.OracleErrorMessage);
            Assert.Null(result.OSSErrorMessage);
        }

        [Fact]
        [Trait("Category", "AddressBrokerTests")]
        [Trait("Category", "AddressBrokerNegativeTests")]
        [Trait("Category", "AddressBrokerOracleTests")]
        public async void SyncToOracle_LocationAndSiteNotExists_FailsToGetCustomerAccount_ReturnsError()
        {
            // Arrange
            var model = Helpers.BuildSalesforceAddressModel(true, false);
            var transaction = Helpers.BuildSalesforceTransaction();
            Helpers.MockActionRepository(_fixture.ActionsRepository, transaction);

            // The organization exists
            var oracleOrg = Helpers.BuildOracleOrganization();
            _fixture.OracleService
                .Setup(ors => ors.GetOrganizationById(It.IsAny<string>(), transaction, null))
                .ReturnsAsync(new Tuple<bool, OracleOrganization, string>(true, oracleOrg, string.Empty));
            // The customer account fails to come back
            _fixture.OracleService
                .Setup(ors => ors.GetCustomerAccountById(It.IsAny<string>(), transaction, null))
                .ReturnsAsync(new Tuple<bool, OracleCustomerAccount, string>(false, null, string.Empty));

            // Act
            var svc = new AddressBrokerService(_fixture.ActionsRepository.Object, _fixture.OracleService.Object);
            var result = await svc.ProcessAddressAction(model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(model.ObjectId, result.SalesforceObjectId);
            Assert.Equal(StatusType.Error, result.OracleStatus);
            Assert.Equal(StatusType.Skipped, result.OSSStatus);
            Assert.Equal($"Error syncing Address to Oracle: Customer Account object with SF reference Id {model.ParentAccountId} was not found.", result.OracleErrorMessage);
            Assert.Null(result.OSSErrorMessage);
        }

        [Fact]
        [Trait("Category", "AddressBrokerTests")]
        [Trait("Category", "AddressBrokerNegativeTests")]
        [Trait("Category", "AddressBrokerOracleTests")]
        public async void SyncToOracle_LocationAndSiteNotExists_FailsToGetLocations_ReturnsError()
        {
            // Arrange
            var model = Helpers.BuildSalesforceAddressModel(true, false);
            var transaction = Helpers.BuildSalesforceTransaction();
            Helpers.MockActionRepository(_fixture.ActionsRepository, transaction);

            // valid business unit
            _fixture.OracleService
                .Setup(ors => ors.RemapBusinessUnitToOracleSiteAddressSet(It.IsAny<string>(), transaction))
                .ReturnsAsync("300000001127004");
            // The organization exists
            var oracleOrg = Helpers.BuildOracleOrganization();
            _fixture.OracleService
                .Setup(ors => ors.GetOrganizationById(It.IsAny<string>(), transaction, null))
                .ReturnsAsync(new Tuple<bool, OracleOrganization, string>(true, oracleOrg, string.Empty));
            // The customer account exists
            var customerAccount = Helpers.BuildOracleCustomerAccount();
            _fixture.OracleService
                .Setup(ors => ors.GetCustomerAccountById(It.IsAny<string>(), transaction, null))
                .ReturnsAsync(new Tuple<bool, OracleCustomerAccount, string>(true, customerAccount, string.Empty));
            // locations are an empty list, which means we're creating
            _fixture.OracleService
                .Setup(ors => ors.GetLocationsById(It.IsAny<List<Tuple<string, ulong?, ulong?>>>(), transaction))
                .ReturnsAsync(new Tuple<bool, IEnumerable<OracleLocationModel>, string>(false, null, "Explosions"));

            // Act
            var svc = new AddressBrokerService(_fixture.ActionsRepository.Object, _fixture.OracleService.Object);
            var result = await svc.ProcessAddressAction(model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(model.ObjectId, result.SalesforceObjectId);
            Assert.Equal(StatusType.Error, result.OracleStatus);
            Assert.Equal(StatusType.Skipped, result.OSSStatus);
            Assert.Equal("Error syncing Address to Oracle: Explosions", result.OracleErrorMessage);
            Assert.Null(result.OSSErrorMessage);
        }

        [Fact]
        [Trait("Category", "AddressBrokerTests")]
        [Trait("Category", "AddressBrokerNegativeTests")]
        [Trait("Category", "AddressBrokerOracleTests")]
        public async void SyncToOracle_LocationAndSiteNotExists_FailsToCreateLocations_ReturnsError()
        {
            // Arrange
            var model = Helpers.BuildSalesforceAddressModel(true, false);
            var transaction = Helpers.BuildSalesforceTransaction();
            Helpers.MockActionRepository(_fixture.ActionsRepository, transaction);

            // valid business unit
            _fixture.OracleService
                .Setup(ors => ors.RemapBusinessUnitToOracleSiteAddressSet(It.IsAny<string>(), transaction))
                .ReturnsAsync("300000001127004");
            // The organization exists
            var oracleOrg = Helpers.BuildOracleOrganization();
            _fixture.OracleService
                .Setup(ors => ors.GetOrganizationById(It.IsAny<string>(), transaction, null))
                .ReturnsAsync(new Tuple<bool, OracleOrganization, string>(true, oracleOrg, string.Empty));
            // The customer account exists
            var customerAccount = Helpers.BuildOracleCustomerAccount();
            _fixture.OracleService
                .Setup(ors => ors.GetCustomerAccountById(It.IsAny<string>(), transaction, null))
                .ReturnsAsync(new Tuple<bool, OracleCustomerAccount, string>(true, customerAccount, string.Empty));
            // locations are an empty list, which means we're creating
            _fixture.OracleService
                .Setup(ors => ors.GetLocationsById(It.IsAny<List<Tuple<string, ulong?, ulong?>>>(), transaction))
                .ReturnsAsync(new Tuple<bool, IEnumerable<OracleLocationModel>, string>(true, new List<OracleLocationModel>(), string.Empty));
            // Fails to create
            _fixture.OracleService
                .Setup(ors => ors.CreateLocation(It.IsAny<SalesforceAddressModel>(), transaction))
                .ReturnsAsync(new Tuple<OracleLocationModel, string>(null, "Explosions"));

            // Act
            var svc = new AddressBrokerService(_fixture.ActionsRepository.Object, _fixture.OracleService.Object);
            var result = await svc.ProcessAddressAction(model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(model.ObjectId, result.SalesforceObjectId);
            Assert.Equal(StatusType.Error, result.OracleStatus);
            Assert.Equal(StatusType.Skipped, result.OSSStatus);
            Assert.Equal($"Error syncing Address to Oracle: Error creating Location: Explosions.", result.OracleErrorMessage);
            Assert.Null(result.OSSErrorMessage);
        }

        [Fact]
        [Trait("Category", "AddressBrokerTests")]
        [Trait("Category", "AddressBrokerNegativeTests")]
        [Trait("Category", "AddressBrokerOracleTests")]
        public async void SyncToOracle_LocationAndSiteNotExists_FailsToCreatePartySites_ReturnsError()
        {
            // Arrange
            var model = Helpers.BuildSalesforceAddressModel(true, false);
            var transaction = Helpers.BuildSalesforceTransaction();
            Helpers.MockActionRepository(_fixture.ActionsRepository, transaction);

            // valid business unit
            _fixture.OracleService
                .Setup(ors => ors.RemapBusinessUnitToOracleSiteAddressSet(It.IsAny<string>(), transaction))
                .ReturnsAsync("300000001127004");
            // The organization exists
            var oracleOrg = Helpers.BuildOracleOrganization();
            _fixture.OracleService
                .Setup(ors => ors.GetOrganizationById(It.IsAny<string>(), transaction, null))
                .ReturnsAsync(new Tuple<bool, OracleOrganization, string>(true, oracleOrg, string.Empty));
            // The customer account exists
            var customerAccount = Helpers.BuildOracleCustomerAccount();
            _fixture.OracleService
                .Setup(ors => ors.GetCustomerAccountById(It.IsAny<string>(), transaction, null))
                .ReturnsAsync(new Tuple<bool, OracleCustomerAccount, string>(true, customerAccount, string.Empty));
            // locations are an empty list, which means we're creating
            _fixture.OracleService
                .Setup(ors => ors.GetLocationsById(It.IsAny<List<Tuple<string, ulong?, ulong?>>>(), transaction))
                .ReturnsAsync(new Tuple<bool, IEnumerable<OracleLocationModel>, string>(true, new List<OracleLocationModel>(), string.Empty));
            // creates new locations
            var locations = new List<OracleLocationModel>
            {
                new OracleLocationModel { LocationId = 30001, OrigSystemReference = "add30001" }
            };
            _fixture.OracleService
                .Setup(ors => ors.CreateLocation(It.IsAny<SalesforceAddressModel>(), transaction))
                .ReturnsAsync(new Tuple<OracleLocationModel, string>(locations.First(), string.Empty));
            // fails to create party sites
            _fixture.OracleService
                .Setup(ors => ors.CreateOrganizationPartySites(It.IsAny<ulong>(), It.IsAny<List<OraclePartySite>>(), transaction))
                .ReturnsAsync(new Tuple<List<OraclePartySite>, string>(null, "Explosions"));

            // Act
            var svc = new AddressBrokerService(_fixture.ActionsRepository.Object, _fixture.OracleService.Object);
            var result = await svc.ProcessAddressAction(model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(model.ObjectId, result.SalesforceObjectId);
            Assert.Equal(StatusType.Error, result.OracleStatus);
            Assert.Equal(StatusType.Skipped, result.OSSStatus);
            Assert.Equal($"Error syncing Address to Oracle: Failed to create Organization Party Sites: Explosions.", result.OracleErrorMessage);
            Assert.Null(result.OSSErrorMessage);
        }

        [Fact]
        [Trait("Category", "AddressBrokerTests")]
        [Trait("Category", "AddressBrokerNegativeTests")]
        [Trait("Category", "AddressBrokerOracleTests")]
        public async void SyncToOracle_LocationAndSiteNotExists_FailsToUpdateCustomerAccount_ReturnsError()
        {
            // Arrange
            var model = Helpers.BuildSalesforceAddressModel(true, false);
            var transaction = Helpers.BuildSalesforceTransaction();
            Helpers.MockActionRepository(_fixture.ActionsRepository, transaction);

            // valid business unit
            _fixture.OracleService
                .Setup(ors => ors.RemapBusinessUnitToOracleSiteAddressSet(It.IsAny<string>(), transaction))
                .ReturnsAsync("300000001127004");
            // The organization exists
            var oracleOrg = Helpers.BuildOracleOrganization();
            _fixture.OracleService
                .Setup(ors => ors.GetOrganizationById(It.IsAny<string>(), transaction, null))
                .ReturnsAsync(new Tuple<bool, OracleOrganization, string>(true, oracleOrg, string.Empty));
            // The customer account exists
            var customerAccount = Helpers.BuildOracleCustomerAccount();
            _fixture.OracleService
                .Setup(ors => ors.GetCustomerAccountById(It.IsAny<string>(), transaction, null))
                .ReturnsAsync(new Tuple<bool, OracleCustomerAccount, string>(true, customerAccount, string.Empty));
            // locations are an empty list, which means we're creating
            _fixture.OracleService
                .Setup(ors => ors.GetLocationsById(It.IsAny<List<Tuple<string, ulong?, ulong?>>>(), transaction))
                .ReturnsAsync(new Tuple<bool, IEnumerable<OracleLocationModel>, string>(true, new List<OracleLocationModel>(), string.Empty));
            var locations = new List<OracleLocationModel>
            {
                new OracleLocationModel { LocationId = 30001, OrigSystemReference = "add30001" }
            };
            _fixture.OracleService
                .Setup(ors => ors.CreateLocation(It.IsAny<SalesforceAddressModel>(), transaction))
                .ReturnsAsync(new Tuple<OracleLocationModel, string>(locations.First(), string.Empty));
            // party sites are also an empty list, so we're creating
            var partySites = new List<OraclePartySite>
            {
                new OraclePartySite { PartySiteId = 30001, OrigSystemReference = "add30001", LocationId = 30001,
                    SiteUses = new List<OraclePartySiteUse>
                    {
                        new OraclePartySiteUse { PartySiteUseId = 20001, SiteUseType = "Billing" }
                    }
                }
            };
            _fixture.OracleService
                .Setup(ors => ors.CreateOrganizationPartySites(It.IsAny<ulong>(), It.IsAny<List<OraclePartySite>>(), transaction))
                .ReturnsAsync(new Tuple<List<OraclePartySite>, string>(new List<OraclePartySite>(), string.Empty));
            // Lastly, updating the customer account children
            // We're not using an customer account properties, so it's okay
            _fixture.OracleService
                .Setup(ors => ors.UpdateCustomerAccountChildren(It.IsAny<OracleCustomerAccount>(), transaction, It.IsAny<List<OracleCustomerAccountSite>>(), It.IsAny<List<OracleCustomerAccountContact>>()))
                .ReturnsAsync(new Tuple<OracleCustomerAccount, string>(null, "Explosions"));

            // Act
            var svc = new AddressBrokerService(_fixture.ActionsRepository.Object, _fixture.OracleService.Object);
            var result = await svc.ProcessAddressAction(model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(model.ObjectId, result.SalesforceObjectId);
            Assert.Equal(StatusType.Error, result.OracleStatus);
            Assert.Equal(StatusType.Skipped, result.OSSStatus);
            Assert.Equal("Error syncing Address to Oracle: Error adding CustomerAccountSite: Explosions.", result.OracleErrorMessage);
            Assert.Null(result.OSSErrorMessage);
        }

        [Fact]
        [Trait("Category", "AddressBrokerTests")]
        [Trait("Category", "AddressBrokerNegativeTests")]
        [Trait("Category", "AddressBrokerOracleTests")]
        public async void SyncToOracle_LocationExistsSiteNotExists_UpdateLocationFailes_ReturnsError()
        {
            // Arrange
            var model = Helpers.BuildSalesforceAddressModel(true, false);
            var transaction = Helpers.BuildSalesforceTransaction();
            Helpers.MockActionRepository(_fixture.ActionsRepository, transaction);

            // valid business unit
            _fixture.OracleService
                .Setup(ors => ors.RemapBusinessUnitToOracleSiteAddressSet(It.IsAny<string>(), transaction))
                .ReturnsAsync("300000001127004");
            // The organization exists
            var oracleOrg = Helpers.BuildOracleOrganization();
            _fixture.OracleService
                .Setup(ors => ors.GetOrganizationById(It.IsAny<string>(), transaction, null))
                .ReturnsAsync(new Tuple<bool, OracleOrganization, string>(true, oracleOrg, string.Empty));
            // The customer account exists
            var customerAccount = Helpers.BuildOracleCustomerAccount();
            _fixture.OracleService
                .Setup(ors => ors.GetCustomerAccountById(It.IsAny<string>(), transaction, null))
                .ReturnsAsync(new Tuple<bool, OracleCustomerAccount, string>(true, customerAccount, string.Empty));
            // locations exist, which means we're updating
            // this location has a different id than the organization's partysites model, so it will not exist on the organization
            var locations = new List<OracleLocationModel>
            {
                new OracleLocationModel { LocationId = 30003, OrigSystemReference = "add30003" }
            };
            _fixture.OracleService
                .Setup(ors => ors.GetLocationsById(It.IsAny<List<Tuple<string, ulong?, ulong?>>>(), transaction))
                .ReturnsAsync(new Tuple<bool, IEnumerable<OracleLocationModel>, string>(true, locations, string.Empty));

            _fixture.OracleService
                .Setup(ors => ors.UpdateLocation(It.IsAny<SalesforceAddressModel>(), It.IsAny<OracleLocationModel>(), transaction))
                .ReturnsAsync(new Tuple<OracleLocationModel, string>(null, "Explosions"));

            // Act
            var svc = new AddressBrokerService(_fixture.ActionsRepository.Object, _fixture.OracleService.Object);
            var result = await svc.ProcessAddressAction(model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(model.ObjectId, result.SalesforceObjectId);
            Assert.Equal(StatusType.Error, result.OracleStatus);
            Assert.Equal(StatusType.Skipped, result.OSSStatus);
            Assert.Equal($"Error syncing Address to Oracle: Error updating Location: Explosions.", result.OracleErrorMessage);
            Assert.Null(result.OSSErrorMessage);
        }
    }
}
