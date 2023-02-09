using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.OSS;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External;
using Kymeta.Cloud.Services.EnterpriseBroker.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Kymeta.Cloud.Services.EnterpriseBroker.UnitTests;

public class AccountBrokerServiceTests : IClassFixture<TestFixture>
{
    private TestFixture _fixture;

    public AccountBrokerServiceTests(TestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    [Trait("Category", "AccountBrokerTests")]
    [Trait("Category", "AccountBrokerOssTests")]
    public async void PSA_SyncToOSS_ValidModel_Exists_ReturnsSuccess()
    {
        // Arrange
        var model = Helpers.BuildSalesforceAccountModel(false, true);
        var transaction = Helpers.BuildSalesforceTransaction();
        Helpers.MockActionRepository(_fixture.ActionsRepository, transaction);

        var accountFromOss = new AccountV2 { Id = Guid.NewGuid() };
        _fixture.OssService
            .Setup(oss => oss.GetAccountBySalesforceId(It.IsAny<string>()))
            .ReturnsAsync(accountFromOss);
        // Because the account is found above, we're doing an update
        _fixture.OssService
            .Setup(oss => oss.UpdateAccount(It.IsAny<SalesforceAccountModel>(), It.IsAny<SalesforceAccountObjectModel>(), It.IsAny<SalesforceActionTransaction>()))
            .ReturnsAsync(new Tuple<AccountV2, string>(accountFromOss, string.Empty));
        _fixture.OssService
            .Setup(oss => oss.UpdateChildAccounts(It.IsAny<AccountV2>(), It.IsAny<SalesforceAccountModel>(), It.IsAny<SalesforceActionTransaction>()))
            .ReturnsAsync(new Tuple<bool, List<AccountV2>?, string>(true, null, null));

        // Act
        var svc = new AccountBrokerService(_fixture.ActionsRepository.Object, _fixture.OracleService.Object, _fixture.OssService.Object, _fixture.SalesforceClient.Object);
        var result = await svc.ProcessAccountAction(model);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(model.ObjectId, result.SalesforceObjectId);
        Assert.Equal(StatusType.Skipped, result.OracleStatus);
        Assert.Equal(StatusType.Successful, result.OSSStatus);
        Assert.Null(result.OracleErrorMessage);
        Assert.Null(result.OSSErrorMessage);
        Assert.Equal(accountFromOss.Id?.ToString(), result.OssAccountId);
        Assert.Null(result.OracleCustomerAccountId);
        Assert.Null(result.OracleCustomerProfileId);
        Assert.Null(result.OracleOrganizationId);
    }

    [Fact]
    [Trait("Category", "AccountBrokerTests")]
    [Trait("Category", "AccountBrokerOssTests")]
    public async void PSA_SyncToOSS_ValidModel_NotExists_ReturnsSuccess()
    {
        // Arrange
        var model = Helpers.BuildSalesforceAccountModel(false, true);
        var transaction = Helpers.BuildSalesforceTransaction();
        Helpers.MockActionRepository(_fixture.ActionsRepository, transaction);
        
        var accountFromOss = new AccountV2 { Id = Guid.NewGuid() };
        _fixture.OssService
            .Setup(oss => oss.GetAccountBySalesforceId(It.IsAny<string>()))
            .ReturnsAsync((AccountV2?)null); // Mock a null return!
        // Because the account is not found above, it means we're adding, so mock that here
        _fixture.OssService
            .Setup(oss => oss.AddAccount(It.IsAny<SalesforceAccountModel>(), It.IsAny<SalesforceAccountObjectModel>(), It.IsAny<SalesforceActionTransaction>()))
            .ReturnsAsync(new Tuple<AccountV2, string>(accountFromOss, string.Empty));
        _fixture.OssService
            .Setup(oss => oss.UpdateChildAccounts(It.IsAny<AccountV2>(), It.IsAny<SalesforceAccountModel>(), It.IsAny<SalesforceActionTransaction>()))
            .ReturnsAsync(new Tuple<bool, List<AccountV2>?, string>(true, null, null));

        // Act
        var svc = new AccountBrokerService(_fixture.ActionsRepository.Object, _fixture.OracleService.Object, _fixture.OssService.Object, _fixture.SalesforceClient.Object);
        var result = await svc.ProcessAccountAction(model);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(model.ObjectId, result.SalesforceObjectId);
        Assert.Equal(StatusType.Skipped, result.OracleStatus);
        Assert.Equal(StatusType.Successful, result.OSSStatus);
        Assert.Null(result.OracleErrorMessage);
        Assert.Null(result.OSSErrorMessage);
        Assert.Equal(accountFromOss.Id?.ToString(), result.OssAccountId);
        Assert.Null(result.OracleCustomerAccountId);
        Assert.Null(result.OracleCustomerProfileId);
        Assert.Null(result.OracleOrganizationId);
    }

    [Fact]
    [Trait("Category", "AccountBrokerTests")]
    [Trait("Category", "AccountBrokerOracleTests")]
    public async void PSA_SyncToBoth_ValidModel_Exists_ReturnsSuccess()
    {
        // Arrange
        var model = Helpers.BuildSalesforceAccountModel(true, true);
        var transaction = Helpers.BuildSalesforceTransaction();
        Helpers.MockActionRepository(_fixture.ActionsRepository, transaction);
        
        // Mock OSS portion of the request
        var accountFromOss = new AccountV2 { Id = Guid.NewGuid() };
        _fixture.OssService
            .Setup(oss => oss.GetAccountBySalesforceId(It.IsAny<string>()))
            .ReturnsAsync(accountFromOss);
        _fixture.OssService
            .Setup(oss => oss.UpdateAccount(It.IsAny<SalesforceAccountModel>(), It.IsAny<SalesforceAccountObjectModel>(), It.IsAny<SalesforceActionTransaction>()))
            .ReturnsAsync(new Tuple<AccountV2, string>(accountFromOss, string.Empty));
        _fixture.OssService
            .Setup(oss => oss.UpdateChildAccounts(It.IsAny<AccountV2>(), It.IsAny<SalesforceAccountModel>(), It.IsAny<SalesforceActionTransaction>()))
            .ReturnsAsync(new Tuple<bool, List<AccountV2>?, string>(true, null, null));
        // Mock Oracle portion of the request
        var oracleOrg = Helpers.BuildOracleOrganization();
        _fixture.OracleService
            .Setup(ors => ors.RemapBusinessUnitToOracleSiteAddressSet(It.IsAny<string>(), transaction))
            .ReturnsAsync("300000001127004");
        _fixture.OracleService
            .Setup(ors => ors.GetOrganizationById(It.IsAny<string>(), transaction, null))
            .ReturnsAsync(new Tuple<bool, OracleOrganization, string>(true, oracleOrg, string.Empty));
        _fixture.OracleService
            .Setup(ors => ors.UpdateOrganization(It.IsAny<OracleOrganization>(), It.IsAny<SalesforceAccountModel>(), transaction))
            .ReturnsAsync(new Tuple<OracleOrganization, string>(oracleOrg, string.Empty));
        var locations = new List<OracleLocationModel>
        {
            new OracleLocationModel { LocationId = 30001, OrigSystemReference = "add30001" },
            new OracleLocationModel { LocationId = 30002, OrigSystemReference = "add30002" }
        };
        _fixture.OracleService
            .Setup(ors => ors.GetLocationsById(It.IsAny<List<Tuple<string, ulong?, ulong?>>>(), transaction))
            .ReturnsAsync(new Tuple<bool, IEnumerable<OracleLocationModel>, string>(true, locations, string.Empty));
        var persons = new List<OraclePersonObject>
        {
            new OraclePersonObject { PartyId = 30001, OrigSystemReference = "con30001" }
        };
        _fixture.OracleService
            .Setup(ors => ors.GetPersonsById(It.IsAny<List<Tuple<string, ulong?>>>(), transaction))
            .ReturnsAsync(new Tuple<bool, IEnumerable<OraclePersonObject>, string>(true, persons, string.Empty));
        var customerAccount = new OracleCustomerAccount
        {
            CustomerAccountId = 30001,
            AccountNumber = 30001,
            PartyId = 30001,
            OrigSystemReference = "acc30001"
        };
        _fixture.OracleService
            .Setup(ors => ors.GetCustomerAccountById(It.IsAny<string>(), transaction, null))
            .ReturnsAsync(new Tuple<bool, OracleCustomerAccount, string>(true, customerAccount, string.Empty));
        _fixture.OracleService
            .Setup(ors => ors.UpdateCustomerAccount(It.IsAny<OracleCustomerAccount>(), It.IsAny<SalesforceAccountModel>(), It.IsAny<List<OracleCustomerAccountSite>>(), It.IsAny<List<OracleCustomerAccountContact>>(), transaction))
            .ReturnsAsync(new Tuple<OracleCustomerAccount, string>(customerAccount, string.Empty));
        var customerProfile = new OracleCustomerAccountProfile
        {
            PartyId = 30001
        };
        _fixture.OracleService
            .Setup(ors => ors.GetCustomerProfileByAccountNumber(It.IsAny<string>(), transaction))
            .ReturnsAsync(new Tuple<bool, OracleCustomerAccountProfile, string>(true, customerProfile, string.Empty));
        _fixture.OssService
            .Setup(oss => oss.UpdateAccountOracleId(It.IsAny<SalesforceAccountModel>(), It.IsAny<string>(), transaction))
            .ReturnsAsync(new Tuple<AccountV2, string>(accountFromOss, string.Empty));

        // Act
        var svc = new AccountBrokerService(_fixture.ActionsRepository.Object, _fixture.OracleService.Object, _fixture.OssService.Object, _fixture.SalesforceClient.Object);
        var result = await svc.ProcessAccountAction(model);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(model.ObjectId, result.SalesforceObjectId);
        Assert.Equal(StatusType.Successful, result.OracleStatus);
        Assert.Equal(StatusType.Successful, result.OSSStatus);
        Assert.Null(result.OracleErrorMessage);
        Assert.Null(result.OSSErrorMessage);
        Assert.Equal(accountFromOss.Id?.ToString(), result.OssAccountId);
        Assert.Equal("30001", result.OracleOrganizationId);
        Assert.Equal("30001", result.OracleCustomerProfileId);
        Assert.Equal("30001", result.OracleCustomerAccountId);
    }

    [Fact]
    [Trait("Category", "AccountBrokerTests")]
    [Trait("Category", "AccountBrokerOracleTests")]
    public async void PSA_SyncToBoth_ValidModel_NotExists_ReturnsSuccess()
    {
        // Arrange
        var model = Helpers.BuildSalesforceAccountModel(true, true);
        var transaction = Helpers.BuildSalesforceTransaction();
        Helpers.MockActionRepository(_fixture.ActionsRepository, transaction);

        // Mock OSS portion of the request
        var accountFromOss = new AccountV2 { Id = Guid.NewGuid() };
        _fixture.OssService
            .Setup(oss => oss.GetAccountBySalesforceId(It.IsAny<string>()))
            .ReturnsAsync(accountFromOss);
        _fixture.OssService
            .Setup(oss => oss.UpdateAccount(It.IsAny<SalesforceAccountModel>(), It.IsAny<SalesforceAccountObjectModel>(), It.IsAny<SalesforceActionTransaction>()))
            .ReturnsAsync(new Tuple<AccountV2, string>(accountFromOss, string.Empty));
        _fixture.OssService
            .Setup(oss => oss.UpdateChildAccounts(It.IsAny<AccountV2>(), It.IsAny<SalesforceAccountModel>(), It.IsAny<SalesforceActionTransaction>()))
            .ReturnsAsync(new Tuple<bool, List<AccountV2>?, string>(true, null, null));
        // Mock Oracle portion of the request
        var oracleOrg = new OracleOrganization
        {
            PartyId = 001,
            PartyNumber = 30001,
            OrigSystemReference = "acc30001",
            PartySites = new List<OraclePartySite>
            {
                new OraclePartySite
                {
                    OrigSystemReference = "add30001",
                    LocationId = 30001,
                    PartySiteNumber = 30001
                },
                new OraclePartySite
                {
                    OrigSystemReference = "add30002",
                    LocationId = 30002,
                    PartySiteNumber = 30002
                }
            },
            Contacts = new List<OracleOrganizationContact>
            {
                new OracleOrganizationContact
                {
                    OrigSystemReference = "con30001",
                    ContactPartyId = 30001
                }
            }
        };
        _fixture.OracleService
            .Setup(ors => ors.RemapBusinessUnitToOracleSiteAddressSet(It.IsAny<string>(), transaction))
            .ReturnsAsync("300000001127004");
        // finding organization returns a null so...
        _fixture.OracleService
            .Setup(ors => ors.GetOrganizationById(It.IsAny<string>(), transaction, null))
            .ReturnsAsync(new Tuple<bool, OracleOrganization, string>(true, null, string.Empty));
        // we have to mock a create organization call
        _fixture.OracleService
            .Setup(ors => ors.CreateOrganization(It.IsAny<SalesforceAccountModel>(), It.IsAny<List<OraclePartySite>>(), transaction))
            .ReturnsAsync(new Tuple<OracleOrganization, string>(oracleOrg, string.Empty));
        // we have to mock a create organization call
        _fixture.OracleService
            .Setup(ors => ors.UpdateOrganization(It.IsAny<OracleOrganization>(), It.IsAny<SalesforceAccountModel>(), transaction))
            .ReturnsAsync(new Tuple<OracleOrganization, string>(oracleOrg, string.Empty));
        // locations return null, too
        _fixture.OracleService
            .Setup(ors => ors.GetLocationsById(It.IsAny<List<Tuple<string, ulong?, ulong?>>>(), transaction))
            .ReturnsAsync(new Tuple<bool, IEnumerable<OracleLocationModel>, string>(true, null, string.Empty));
        // we have to mock out create locations
        var locations = new List<OracleLocationModel>
        {
            new OracleLocationModel { LocationId = 30001, OrigSystemReference = "add30001" }
        };
        _fixture.OracleService
            .Setup(ors => ors.CreateLocation(It.IsAny<SalesforceAddressModel>(), transaction))
            .ReturnsAsync(new Tuple<OracleLocationModel, string>(locations.First(), string.Empty));
        // also have to mock out create party sites
        var partySites = new List<OraclePartySite>
        {
            new OraclePartySite { PartySiteId = 30001, OrigSystemReference = "add30001", LocationId = 30001, PartySiteNumber = 30001,
                SiteUses = new List<OraclePartySiteUse> 
                { 
                    new OraclePartySiteUse { PartySiteUseId = 20001, SiteUseType = "Billing" } 
                } 
            }
        };
        _fixture.OracleService
            .Setup(ors => ors.CreateOrganizationPartySites(It.IsAny<ulong>(), It.IsAny<List<OraclePartySite>>(), transaction))
            .ReturnsAsync(new Tuple<List<OraclePartySite>, string>(partySites, string.Empty));
        // persons returns null
        _fixture.OracleService
            .Setup(ors => ors.GetPersonsById(It.IsAny<List<Tuple<string, ulong?>>>(), transaction))
            .ReturnsAsync(new Tuple<bool, IEnumerable<OraclePersonObject>, string>(true, null, string.Empty));
        // we have to mock out CreatePerson
        var person = new OraclePersonObject
        {
            IsPrimary = true,
            PartyId = 30001,
            OrigSystemReference = "con30001"
        };
        _fixture.OracleService
            .Setup(ors => ors.CreatePerson(It.IsAny<SalesforceContactModel>(), It.IsAny<ulong>(), transaction))
            .ReturnsAsync(new Tuple<OraclePersonObject, string>(person, string.Empty));
        // customer account returns null
        _fixture.OracleService
            .Setup(ors => ors.GetCustomerAccountById(It.IsAny<string>(), transaction, null))
            .ReturnsAsync(new Tuple<bool, OracleCustomerAccount, string>(true, null, string.Empty));
        // we have to mock out customer account
        var customerAccount = new OracleCustomerAccount
        {
            CustomerAccountId = 30001,
            PartyId = 30001,
            OrigSystemReference = "acc30001",
            AccountNumber = 30001
        };
        _fixture.OracleService
            .Setup(ors => ors.CreateCustomerAccount(It.IsAny<ulong>(), It.IsAny<SalesforceAccountModel>(), It.IsAny<List<OracleCustomerAccountSite>>(), It.IsAny<List<OracleCustomerAccountContact>>(), transaction))
            .ReturnsAsync(new Tuple<OracleCustomerAccount, string>(customerAccount, string.Empty));
        // customer account profile returns null
        _fixture.OracleService
            .Setup(ors => ors.GetCustomerProfileByAccountNumber(It.IsAny<string>(), transaction))
            .ReturnsAsync(new Tuple<bool, OracleCustomerAccountProfile, string>(true, null, string.Empty));
        // we have to mock out customer profile
        var customerProfile = new OracleCustomerAccountProfile
        {
            PartyId = 30001
        };
        _fixture.OracleService
            .Setup(ors => ors.CreateCustomerAccountProfile(It.IsAny<ulong>(), It.IsAny<uint>(), transaction))
            .ReturnsAsync(new Tuple<OracleCustomerAccountProfile, string>(customerProfile, string.Empty));
        _fixture.OssService
            .Setup(oss => oss.UpdateAccountOracleId(It.IsAny<SalesforceAccountModel>(), It.IsAny<string>(), transaction))
            .ReturnsAsync(new Tuple<AccountV2, string>(accountFromOss, string.Empty));

        // Act
        var svc = new AccountBrokerService(_fixture.ActionsRepository.Object, _fixture.OracleService.Object, _fixture.OssService.Object, _fixture.SalesforceClient.Object);
        var result = await svc.ProcessAccountAction(model);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(model.ObjectId, result.SalesforceObjectId);
        Assert.Equal(StatusType.Successful, result.OracleStatus);
        Assert.Equal(StatusType.Successful, result.OSSStatus);
        Assert.Null(result.OracleErrorMessage);
        Assert.Null(result.OSSErrorMessage);
        Assert.Equal(accountFromOss.Id?.ToString(), result.OssAccountId);
        Assert.Equal("30001", result.OracleOrganizationId);
        Assert.Equal("30001", result.OracleCustomerProfileId);
        Assert.Equal("30001", result.OracleCustomerAccountId);
        Assert.Equal(2, result.Addresses?.Count());
        Assert.Contains("30001", result.Addresses?.Select(a => a.OracleId));
        Assert.Equal(1, result.Contacts?.Count());
    }

    [Fact]
    [Trait("Category", "AccountBrokerTests")]
    [Trait("Category", "AccountBrokerOracleTests")]
    [Trait("Category", "AccountBrokerOracleLocationsTests")]
    public async void PSA_SyncToOracle_ValidModel_ExistingLocations_NoMatchingPartySites_ReturnsSuccess()
    {
        // Arrange
        var model = Helpers.BuildSalesforceAccountModel(true, false);
        var transaction = Helpers.BuildSalesforceTransaction();
        Helpers.MockActionRepository(_fixture.ActionsRepository, transaction);

        // Mock Oracle portion of the request
        var oracleOrg = new OracleOrganization
        {
            PartyId = 001,
            PartyNumber = 30001,
            OrigSystemReference = "acc30001",
            PartySites = new List<OraclePartySite>
            {

            },
            Contacts = new List<OracleOrganizationContact>
                {
                    new OracleOrganizationContact
                    {
                        OrigSystemReference = "con30001",
                        ContactPartyId = 30001
                    }
                }
        };
        _fixture.OracleService
            .Setup(ors => ors.RemapBusinessUnitToOracleSiteAddressSet(It.IsAny<string>(), transaction))
            .ReturnsAsync("300000001127004");
        _fixture.OracleService
            .Setup(ors => ors.GetOrganizationById(It.IsAny<string>(), transaction, null))
            .ReturnsAsync(new Tuple<bool, OracleOrganization, string>(true, oracleOrg, string.Empty));
        _fixture.OracleService
            .Setup(ors => ors.UpdateOrganization(It.IsAny<OracleOrganization>(), It.IsAny<SalesforceAccountModel>(), transaction))
            .ReturnsAsync(new Tuple<OracleOrganization, string>(oracleOrg, string.Empty));
        var locations = new List<OracleLocationModel>
            {
                new OracleLocationModel { LocationId = 30001, OrigSystemReference = "add30001" },
                new OracleLocationModel { LocationId = 30002, OrigSystemReference = "add30002" }
            };
        _fixture.OracleService
            .Setup(ors => ors.GetLocationsById(It.IsAny<List<Tuple<string, ulong?, ulong?>>>(), transaction))
            .ReturnsAsync(new Tuple<bool, IEnumerable<OracleLocationModel>, string>(true, locations, string.Empty));
        // also have to mock out create party sites
        var partySites = new List<OraclePartySite>
            {
                new OraclePartySite {
                    PartySiteId = 30001,
                    OrigSystemReference = "add30001",
                    LocationId = 30001,
                    SiteUses = new List<OraclePartySiteUse>
                    {
                        new OraclePartySiteUse { PartySiteUseId = 20001, SiteUseType = "Billing" }
                    }
                }
            };
        _fixture.OracleService
            .Setup(ors => ors.CreateOrganizationPartySites(It.IsAny<ulong>(), It.IsAny<List<OraclePartySite>>(), transaction))
            .ReturnsAsync(new Tuple<List<OraclePartySite>, string>(partySites, string.Empty));
        // persons returns null
        _fixture.OracleService
            .Setup(ors => ors.GetPersonsById(It.IsAny<List<Tuple<string, ulong?>>>(), transaction))
            .ReturnsAsync(new Tuple<bool, IEnumerable<OraclePersonObject>, string>(true, null, string.Empty));
        // we have to mock out CreatePerson
        var person = new OraclePersonObject
        {
            IsPrimary = true,
            PartyId = 30001,
            OrigSystemReference = "con30001"
        };
        _fixture.OracleService
            .Setup(ors => ors.CreatePerson(It.IsAny<SalesforceContactModel>(), It.IsAny<ulong>(), transaction))
            .ReturnsAsync(new Tuple<OraclePersonObject, string>(person, string.Empty));
        // customer account returns null
        _fixture.OracleService
            .Setup(ors => ors.GetCustomerAccountById(It.IsAny<string>(), transaction, null))
            .ReturnsAsync(new Tuple<bool, OracleCustomerAccount, string>(true, null, string.Empty));
        // we have to mock out customer account
        var customerAccount = new OracleCustomerAccount
        {
            CustomerAccountId = 30001,
            PartyId = 30001,
            OrigSystemReference = "acc30001",
            AccountNumber = 30001
        };
        _fixture.OracleService
            .Setup(ors => ors.CreateCustomerAccount(It.IsAny<ulong>(), It.IsAny<SalesforceAccountModel>(), It.IsAny<List<OracleCustomerAccountSite>>(), It.IsAny<List<OracleCustomerAccountContact>>(), transaction))
            .ReturnsAsync(new Tuple<OracleCustomerAccount, string>(customerAccount, string.Empty));
        // customer account profile returns null
        _fixture.OracleService
            .Setup(ors => ors.GetCustomerProfileByAccountNumber(It.IsAny<string>(), transaction))
            .ReturnsAsync(new Tuple<bool, OracleCustomerAccountProfile, string>(true, null, string.Empty));
        // we have to mock out customer profile
        var customerProfile = new OracleCustomerAccountProfile
        {
            PartyId = 30001
        };
        _fixture.OracleService
            .Setup(ors => ors.CreateCustomerAccountProfile(It.IsAny<ulong>(), It.IsAny<uint>(), transaction))
            .ReturnsAsync(new Tuple<OracleCustomerAccountProfile, string>(customerProfile, string.Empty));

        // Act
        var svc = new AccountBrokerService(_fixture.ActionsRepository.Object, _fixture.OracleService.Object, _fixture.OssService.Object, _fixture.SalesforceClient.Object);
        var result = await svc.ProcessAccountAction(model);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(model.ObjectId, result.SalesforceObjectId);
        Assert.Equal(StatusType.Successful, result.OracleStatus);
        Assert.Equal(StatusType.Skipped, result.OSSStatus);
        Assert.Null(result.OracleErrorMessage);
        Assert.Null(result.OSSErrorMessage);
        Assert.Null(result.OssAccountId);
        Assert.Equal("30001", result.OracleOrganizationId);
        Assert.Equal("30001", result.OracleCustomerProfileId);
        Assert.Equal("30001", result.OracleCustomerAccountId);
    }

    [Fact]
    [Trait("Category", "AccountBrokerTests")]
    [Trait("Category", "AccountBrokerOracleTests")]
    [Trait("Category", "AccountBrokerOracleLocationsTests")]
    public async void PSA_SyncToOracle_ValidModel_PartySiteWithSiteUses_ReturnsSuccess()
    {
        // Arrange
        var model = Helpers.BuildSalesforceAccountModel(true, false);
        var transaction = Helpers.BuildSalesforceTransaction();
        Helpers.MockActionRepository(_fixture.ActionsRepository, transaction);

        // Mock Oracle portion of the request
        var oracleOrg = new OracleOrganization
        {
            PartyId = 001,
            PartyNumber = 30001,
            OrigSystemReference = "acc30001",
            PartySites = new List<OraclePartySite>
            {
                new OraclePartySite
                {
                    OrigSystemReference = "add30001",
                    LocationId = 30001,
                    SiteUses = new List<OraclePartySiteUse>
                    {
                        new OraclePartySiteUse
                        {
                            PartySiteUseId = 30001,
                            SiteUseType = "Shipping"
                        }
                    }
                },
                new OraclePartySite
                {
                    OrigSystemReference = "add30002",
                    LocationId = 30002,
                    SiteUses = null
                }
            },
            Contacts = new List<OracleOrganizationContact>
                {
                    new OracleOrganizationContact
                    {
                        OrigSystemReference = "con30001",
                        ContactPartyId = 30001
                    }
                }
        };
        _fixture.OracleService
            .Setup(ors => ors.RemapBusinessUnitToOracleSiteAddressSet(It.IsAny<string>(), transaction))
            .ReturnsAsync("300000001127004");
        _fixture.OracleService
            .Setup(ors => ors.GetOrganizationById(It.IsAny<string>(), transaction, null))
            .ReturnsAsync(new Tuple<bool, OracleOrganization, string>(true, oracleOrg, string.Empty));
        _fixture.OracleService
            .Setup(ors => ors.UpdateOrganization(It.IsAny<OracleOrganization>(), It.IsAny<SalesforceAccountModel>(), transaction))
            .ReturnsAsync(new Tuple<OracleOrganization, string>(oracleOrg, string.Empty));
        var locations = new List<OracleLocationModel>
            {
                new OracleLocationModel { LocationId = 30001, OrigSystemReference = "add30001" },
                new OracleLocationModel { LocationId = 30002, OrigSystemReference = "add30002" }
            };
        _fixture.OracleService
            .Setup(ors => ors.GetLocationsById(It.IsAny<List<Tuple<string, ulong?, ulong?>>>(), transaction))
            .ReturnsAsync(new Tuple<bool, IEnumerable<OracleLocationModel>, string>(true, locations, string.Empty));
        // also have to mock out create party sites
        var partySites = new List<OraclePartySite>
            {
                new OraclePartySite {
                    PartySiteId = 30001,
                    OrigSystemReference = "add30001",
                    LocationId = 30001,
                    SiteUses = new List<OraclePartySiteUse>
                    {
                        new OraclePartySiteUse { PartySiteUseId = 20001, SiteUseType = "Billing" }
                    }
                }
            };
        _fixture.OracleService
            .Setup(ors => ors.CreateOrganizationPartySites(It.IsAny<ulong>(), It.IsAny<List<OraclePartySite>>(), transaction))
            .ReturnsAsync(new Tuple<List<OraclePartySite>, string>(partySites, string.Empty));
        // persons returns null
        _fixture.OracleService
            .Setup(ors => ors.GetPersonsById(It.IsAny<List<Tuple<string, ulong?>>>(), transaction))
            .ReturnsAsync(new Tuple<bool, IEnumerable<OraclePersonObject>, string>(true, null, string.Empty));
        // we have to mock out CreatePerson
        var person = new OraclePersonObject
        {
            IsPrimary = true,
            PartyId = 30001,
            OrigSystemReference = "con30001"
        };
        _fixture.OracleService
            .Setup(ors => ors.CreatePerson(It.IsAny<SalesforceContactModel>(), It.IsAny<ulong>(), transaction))
            .ReturnsAsync(new Tuple<OraclePersonObject, string>(person, string.Empty));
        // customer account returns null
        _fixture.OracleService
            .Setup(ors => ors.GetCustomerAccountById(It.IsAny<string>(), transaction, null))
            .ReturnsAsync(new Tuple<bool, OracleCustomerAccount, string>(true, null, string.Empty));
        // we have to mock out customer account
        var customerAccount = new OracleCustomerAccount
        {
            CustomerAccountId = 30001,
            PartyId = 30001,
            OrigSystemReference = "acc30001",
            AccountNumber = 30001
        };
        _fixture.OracleService
            .Setup(ors => ors.CreateCustomerAccount(It.IsAny<ulong>(), It.IsAny<SalesforceAccountModel>(), It.IsAny<List<OracleCustomerAccountSite>>(), It.IsAny<List<OracleCustomerAccountContact>>(), transaction))
            .ReturnsAsync(new Tuple<OracleCustomerAccount, string>(customerAccount, string.Empty));
        // customer account profile returns null
        _fixture.OracleService
            .Setup(ors => ors.GetCustomerProfileByAccountNumber(It.IsAny<string>(), transaction))
            .ReturnsAsync(new Tuple<bool, OracleCustomerAccountProfile, string>(true, null, string.Empty));
        // we have to mock out customer profile
        var customerProfile = new OracleCustomerAccountProfile
        {
            PartyId = 30001
        };
        _fixture.OracleService
            .Setup(ors => ors.CreateCustomerAccountProfile(It.IsAny<ulong>(), It.IsAny<uint>(), transaction))
            .ReturnsAsync(new Tuple<OracleCustomerAccountProfile, string>(customerProfile, string.Empty));

        // Act
        var svc = new AccountBrokerService(_fixture.ActionsRepository.Object, _fixture.OracleService.Object, _fixture.OssService.Object, _fixture.SalesforceClient.Object);
        var result = await svc.ProcessAccountAction(model);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(model.ObjectId, result.SalesforceObjectId);
        Assert.Equal(StatusType.Successful, result.OracleStatus);
        Assert.Equal(StatusType.Skipped, result.OSSStatus);
        Assert.Null(result.OracleErrorMessage);
        Assert.Null(result.OSSErrorMessage);
        Assert.Null(result.OssAccountId);
        Assert.Equal("30001", result.OracleOrganizationId);
        Assert.Equal("30001", result.OracleCustomerProfileId);
        Assert.Equal("30001", result.OracleCustomerAccountId);
    }

    [Fact]
    [Trait("Category", "AccountBrokerTests")]
    public async void PSA_SyncToNone_ValidModel_ReturnsSuccess()
    {
        // Arrange
        var model = Helpers.BuildSalesforceAccountModel(false, false);
        var transaction = Helpers.BuildSalesforceTransaction();
        Helpers.MockActionRepository(_fixture.ActionsRepository, transaction);

        // Act
        var svc = new AccountBrokerService(_fixture.ActionsRepository.Object, _fixture.OracleService.Object, _fixture.OssService.Object, _fixture.SalesforceClient.Object);
        var result = await svc.ProcessAccountAction(model);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(model.ObjectId, result.SalesforceObjectId);
        Assert.Equal(StatusType.Skipped, result.OracleStatus);
        Assert.Equal(StatusType.Skipped, result.OSSStatus);
        Assert.Null(result.OracleErrorMessage);
        Assert.Null(result.OSSErrorMessage);
    }
}