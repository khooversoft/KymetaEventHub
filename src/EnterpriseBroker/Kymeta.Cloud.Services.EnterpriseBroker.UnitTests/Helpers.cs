using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.OSS;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Responses;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External;
using Kymeta.Cloud.Services.EnterpriseBroker.Repositories;
using Kymeta.Cloud.Services.EnterpriseBroker.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kymeta.Cloud.Services.EnterpriseBroker.UnitTests
{
    public class Helpers
    {
        public static void MockActionRepository(Mock<IActionsRepository> repository, SalesforceActionTransaction transaction)
        {
            // Mock successful addition of record to the actions repository
            repository
                .Setup(ar => ar.InsertActionRecord(It.IsAny<SalesforceActionTransaction>()))
                .ReturnsAsync(transaction);
            repository
                .Setup(ar => ar.UpdateActionRecord(It.IsAny<SalesforceActionTransaction>()))
                .ReturnsAsync(transaction);
        }

        public static SalesforceAccountModel BuildSalesforceAccountModel(bool syncOracle = true, bool syncOss = true, string name = "Trident Ltd")
        {
            return new SalesforceAccountModel
            {
                EnterpriseOriginUri = "https://salesforce.clouddev.test",
                SyncToOracle = syncOracle,
                SyncToOss = syncOss,
                UserName = "Unit McTester",
                ObjectId = "acc30001",
                BusinessUnit = "Commercial",
                AccountType = "Marine",
                Name = name,
                SubType = "Coast Goard",
                TaxId = "1004381",
                Addresses = new List<SalesforceAddressModel>
                {
                    new SalesforceAddressModel
                    {
                        ObjectId = "add30001",
                        Type = "Billing & Shipping",
                        ParentAccountId = "acc30001"
                    },
                    new SalesforceAddressModel
                    {
                        ObjectId = "add30002",
                        Type = "Shipping",
                        ParentAccountId = "acc30001"
                    }
                },
                Contacts = new List<SalesforceContactModel>
                {
                    new SalesforceContactModel
                    {
                        ObjectId = "con30001",
                        Email = "primary@company.com",
                        Name = "Primary Contact",
                        IsPrimary = true,
                        ParentAccountId = "acc30001",
                        Role = "Bill To Contact"
                    }
                },
                ChildAccounts = new List<SalesforceAccountModel>
                {
                    new SalesforceAccountModel
                    {
                        Name = "Child Account One",
                        ObjectId = "acc70001"
                    },
                    new SalesforceAccountModel
                    {
                        Name = "Child Account Two",
                        ObjectId = "acc70002"
                    },
                    new SalesforceAccountModel
                    {
                        Name = "Child Account Three",
                        ObjectId = "acc70003"
                    }
                }
            };
        }

        public static SalesforceAccountObjectModel BuildSalesforceObjectModel(string name = "Trident Ltd")
        {
            return new SalesforceAccountObjectModel
            {
                Id = "acc30001",
                KSN_Acct_ID__c = Guid.NewGuid().ToString(),
                Oracle_Acct__c = "or3001",
                ParentId = "acc20001",
                Type_of_Company__c = "Partner",
                Approved__c = true,
                Volume_Tier__c = "Tier 4(251-500)",
                Pricebook__c = "CPB & MPB & OW",
                Name = name
            };
        }

        public static SalesforceAddressModel BuildSalesforceAddressModel(bool syncOracle = true, bool syncOss = true)
        {
            return new SalesforceAddressModel
            {
                EnterpriseOriginUri = "https://salesforce.clouddev.test",
                SyncToOracle = syncOracle,
                SyncToOss = syncOss,
                UserName = "Unit McTester",
                ObjectId = "add30001",
                ParentAccountId = "acc30001",
                ParentAccountBusinessUnit = "Commercial",
                City = "Redmond",
                Country = "US",
                PostalCode = "98052",
                StateProvince = "WA",
                SiteName = "ShipStuffHereLol",
                Type = "Billing & Shipping",
                Address = $"12345 SE 100th Street{Environment.NewLine}Ste 100",
                Address1 = "12345 SE 100th Street"
            };
        }

        public static SalesforceContactModel BuildSalesforceContactModel(bool syncOracle = true, bool syncOss = true)
        {
            return new SalesforceContactModel
            {
                EnterpriseOriginUri = "https://salesforce.clouddev.test",
                SyncToOracle = syncOracle,
                SyncToOss = syncOss,
                UserName = "Unit McTester",
                ObjectId = "con30001",
                Email = "primary@company.com",
                Name = "Primary Contact",
                IsPrimary = true,
                ParentAccountId = "acc30001",
                Role = "Bill To Contact"
            };
        }

        public static SalesforceActionTransaction BuildSalesforceTransaction()
        {
            return new SalesforceActionTransaction
            {
                TransactionLog = new List<SalesforceActionRecord>()
            };
        }

        public static OracleOrganization BuildOracleOrganization()
        {
            return new OracleOrganization
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
                                PartySiteUseId = 20001,
                                SiteUseType = "Shipping"
                            }
                        }
                    },
                    new OraclePartySite
                    {
                        OrigSystemReference = "add30002",
                        LocationId = 30002
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
        }
        public static OracleCustomerAccount BuildOracleCustomerAccount()
        {
            return new OracleCustomerAccount
            {
                CustomerAccountId = 30001,
                PartyId = 30001,
                OrigSystemReference = "acc30001",
                Contacts = new List<OracleCustomerAccountContact>()
            };
        }

        public static IEnumerable<SalesOrderResponse> BuildSalesOrderResponse(string? salesOrder = null)
        {
            return new List<SalesOrderResponse>()
            {
                new SalesOrderResponse
                {
                    SalesOrder = null,
                    Terminals = new List<SalesOrderTerminal>
                    {
                        new SalesOrderTerminal
                        {
                            OracleSalesOrder = salesOrder,
                            TerminalSerial = "GRZ000C221221001",
                            OracleTerminalSerial = "GRZ000C221214001",
                            ProductCode = "U8999-00005-0",
                            TerminalKpn = "100-99999-101-A",
                            AntennaSerial = "ANT000C221221001",
                            SatModem = "90001",
                            HybridRouterSerial = null,
                            HybridRouterImei = null,
                            HybridRouterIccid = null,
                            BucSerial = "BUC9001",
                            LnbSerial = "LNB9001",
                            DiplexerSerial = "DI9001",
                            DescriptionFirstLine = "Unit Test Desc",
                            DescriptionSecondLine = null,
                            IpAddress = "192.168.0.101",
                            LinkTimestamp = DateTime.UtcNow
                        }
                    }
                }
            };
        }

        public static Tuple<bool, IEnumerable<SalesOrderReportItemModel>?, string> BuildOracleReportResponse()
        {
            var reportRecords = new List<SalesOrderReportItemModel>()
            {
                new SalesOrderReportItemModel
                {
                    AccountNumber = "001",
                    SalesOrderNumber = "8000001",
                    SerialNumbers = new List<string>
                    {
                        "GRZ000C221221001",
                        "GRZ000C221221002",
                        "GRZ000C221221003",
                        "GRZ000C221221004"
                    }
                },
                new SalesOrderReportItemModel
                {
                    AccountNumber = "002",
                    SalesOrderNumber = "8000002",
                    SerialNumbers = new List<string>
                    {
                        "GRZ001C221221001",
                        "GRZ001C221221002"
                    }
                },
                new SalesOrderReportItemModel
                {
                    AccountNumber = "003",
                    SalesOrderNumber = "8000003",
                    SerialNumbers = new List<string>() // empty list
                }
            };

            return new Tuple<bool, IEnumerable<SalesOrderReportItemModel>?, string>(true, reportRecords, null);
        }

        public static IEnumerable<SalesOrderTerminal> BuildUpdatedSalesOrderTerminalResponse()
        {
            return new List<SalesOrderTerminal>
            {
                new SalesOrderTerminal
                {
                    OracleSalesOrder = "8000001",
                    TerminalSerial = "GRZ000C221221001",
                    OracleTerminalSerial = "GRZ000C221214001",
                    ProductCode = "U8999-00005-0",
                    TerminalKpn = "100-99999-101-A",
                    AntennaSerial = "ANT000C221221001",
                    SatModem = "90001",
                    HybridRouterSerial = null,
                    HybridRouterImei = null,
                    HybridRouterIccid = null,
                    BucSerial = "BUC9001",
                    LnbSerial = "LNB9001",
                    DiplexerSerial = "DI9001",
                    DescriptionFirstLine = "Unit Test Desc",
                    DescriptionSecondLine = null,
                    IpAddress = "192.168.0.101",
                    LinkTimestamp = DateTime.UtcNow
                }
            };
        }
    }
}
