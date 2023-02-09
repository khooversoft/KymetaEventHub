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
    public class AddressBrokerServiceTests : IClassFixture<TestFixture>
    {
        private TestFixture _fixture;

        public AddressBrokerServiceTests(TestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        [Trait("Category", "AddressBrokerTests")]
        [Trait("Category", "AddressBrokerOracleTests")]
        public async void SyncToOracle_LocationAndSiteNotExists_ReturnsSuccess()
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
                .ReturnsAsync(new Tuple<List<OraclePartySite>, string>(partySites, string.Empty));
            // Lastly, updating the customer account children
            // We're not using an customer account properties, so it's okay
            _fixture.OracleService
                .Setup(ors => ors.UpdateCustomerAccountChildren(It.IsAny<OracleCustomerAccount>(), transaction, It.IsAny<List<OracleCustomerAccountSite>>(), It.IsAny<List<OracleCustomerAccountContact>>()))
                .ReturnsAsync(new Tuple<OracleCustomerAccount, string>(new OracleCustomerAccount(), string.Empty));

            // Act
            var svc = new AddressBrokerService(_fixture.ActionsRepository.Object, _fixture.OracleService.Object);
            var result = await svc.ProcessAddressAction(model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(model.ObjectId, result.SalesforceObjectId);
            Assert.Equal(StatusType.Successful, result.OracleStatus);
            Assert.Equal(StatusType.Skipped, result.OSSStatus);
            Assert.Null(result.OracleErrorMessage);
            Assert.Null(result.OSSErrorMessage);
        }

        [Fact]
        [Trait("Category", "AddressBrokerTests")]
        [Trait("Category", "AddressBrokerOracleTests")]
        public async void SyncToOracle_LocationExistsSiteNotExists_ReturnsSuccess()
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
                .ReturnsAsync(new Tuple<OracleLocationModel, string>(locations.First(), string.Empty));
            // party sites don't exist
            var partySites = new List<OraclePartySite>
            {
                new OraclePartySite { PartySiteId = 30003, OrigSystemReference = "add30003", LocationId = 30003,
                    SiteUses = new List<OraclePartySiteUse>
                    {
                        new OraclePartySiteUse { PartySiteUseId = 20001, SiteUseType = "Billing" }
                    }
                }
            };
            _fixture.OracleService
                .Setup(ors => ors.CreateOrganizationPartySites(It.IsAny<ulong>(), It.IsAny<List<OraclePartySite>>(), transaction))
                .ReturnsAsync(new Tuple<List<OraclePartySite>, string>(partySites, string.Empty));
            // Lastly, updating the customer account children
            // We're not using an customer account properties, so it's okay
            _fixture.OracleService
                .Setup(ors => ors.UpdateCustomerAccountChildren(It.IsAny<OracleCustomerAccount>(), transaction, It.IsAny<List<OracleCustomerAccountSite>>(), It.IsAny<List<OracleCustomerAccountContact>>()))
                .ReturnsAsync(new Tuple<OracleCustomerAccount, string>(new OracleCustomerAccount(), string.Empty));
            _fixture.OracleService
                .Setup(ors => ors.UpdateOrganizationPartySites(It.IsAny<ulong>(), It.IsAny<List<OraclePartySite>>(), transaction))
                .ReturnsAsync(new Tuple<List<OraclePartySite>, string>(partySites, string.Empty));

            // Act
            var svc = new AddressBrokerService(_fixture.ActionsRepository.Object, _fixture.OracleService.Object);
            var result = await svc.ProcessAddressAction(model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(model.ObjectId, result.SalesforceObjectId);
            Assert.Equal(StatusType.Successful, result.OracleStatus);
            Assert.Equal(StatusType.Skipped, result.OSSStatus);
            Assert.Null(result.OracleErrorMessage);
            Assert.Null(result.OSSErrorMessage);
        }

        [Fact]
        [Trait("Category", "AddressBrokerTests")]
        [Trait("Category", "AddressBrokerOracleTests")]
        public async void SyncToOracle_LocationExistsSiteExists_ReturnsSuccess()
        {
            // Arrange
            var model = Helpers.BuildSalesforceAddressModel(true, false);
            model.ObjectId = "add30003";
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
                .ReturnsAsync(new Tuple<OracleLocationModel, string>(locations.First(), string.Empty));
            // party sites don't exist
            var partySites = new List<OraclePartySite>
            {
                new OraclePartySite { PartySiteId = 30003, OrigSystemReference = "add30003", LocationId = 30003,
                    SiteUses = new List<OraclePartySiteUse>
                    {
                        new OraclePartySiteUse { PartySiteUseId = 20001, SiteUseType = "Billing" }
                    }
                }
            };
            _fixture.OracleService
                .Setup(ors => ors.CreateOrganizationPartySites(It.IsAny<ulong>(), It.IsAny<List<OraclePartySite>>(), transaction))
                .ReturnsAsync(new Tuple<List<OraclePartySite>, string>(partySites, string.Empty));
            _fixture.OracleService
                .Setup(ors => ors.UpdateOrganizationPartySites(It.IsAny<ulong>(), It.IsAny<List<OraclePartySite>>(), transaction))
                .ReturnsAsync(new Tuple<List<OraclePartySite>, string>(partySites, string.Empty));
            // Lastly, updating the customer account children
            // We're not using an customer account properties, so it's okay
            _fixture.OracleService
                .Setup(ors => ors.UpdateCustomerAccountChildren(It.IsAny<OracleCustomerAccount>(), transaction, It.IsAny<List<OracleCustomerAccountSite>>(), It.IsAny<List<OracleCustomerAccountContact>>()))
                .ReturnsAsync(new Tuple<OracleCustomerAccount, string>(new OracleCustomerAccount(), string.Empty));

            // Act
            var svc = new AddressBrokerService(_fixture.ActionsRepository.Object, _fixture.OracleService.Object);
            var result = await svc.ProcessAddressAction(model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(model.ObjectId, result.SalesforceObjectId);
            Assert.Equal(StatusType.Successful, result.OracleStatus);
            Assert.Equal(StatusType.Skipped, result.OSSStatus);
            Assert.Null(result.OracleErrorMessage);
            Assert.Null(result.OSSErrorMessage);
        }
    }
}
