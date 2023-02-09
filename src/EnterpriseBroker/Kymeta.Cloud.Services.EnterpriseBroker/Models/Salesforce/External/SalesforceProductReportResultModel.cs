using Newtonsoft.Json;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External
{
    public class SalesforceProductReportResultModel
    {

        public Attributes attributes { get; set; }
        public bool allData { get; set; }
        public Factmap factMap { get; set; }
        public Groupingsacross groupingsAcross { get; set; }
        public Groupingsdown groupingsDown { get; set; }
        public bool hasDetailRows { get; set; }
        public Picklistcolors picklistColors { get; set; }
        public Reportextendedmetadata reportExtendedMetadata { get; set; }
        public Reportmetadata reportMetadata { get; set; }
        public class Attributes
        {
            public string describeUrl { get; set; }
            public string instancesUrl { get; set; }
            public string reportId { get; set; }
            public string reportName { get; set; }
            public string type { get; set; }
        }

        public class Factmap
        {
            [JsonProperty(PropertyName = "T!T")]
            public TT TT { get; set; }
        }

        public class TT
        {
            public Row[] rows { get; set; }
        }

        public class Row
        {
            public Datacell[] dataCells { get; set; }
        }

        public class Datacell
        {
            public string label { get; set; }
            public string recordId { get; set; }
            public object value { get; set; }
            public string escapedLabel { get; set; }
        }

        public class Groupingsacross
        {
            public object[] groupings { get; set; }
        }

        public class Groupingsdown
        {
            public object[] groupings { get; set; }
        }

        public class Picklistcolors
        {
        }

        public class Reportextendedmetadata
        {
            public Aggregatecolumninfo aggregateColumnInfo { get; set; }
            public object availableDashboardSettings { get; set; }
            public Detailcolumninfo detailColumnInfo { get; set; }
            public Groupingcolumninfo groupingColumnInfo { get; set; }
        }

        public class Aggregatecolumninfo
        {
            public Rowcount RowCount { get; set; }
        }

        public class Rowcount
        {
            public string dataType { get; set; }
            public string label { get; set; }
        }

        public class Detailcolumninfo
        {
            public CUSTOMER_PRODUCT_ID CUSTOMER_PRODUCT_ID { get; set; }
            public Product2Stage__C Product2Stage__c { get; set; }
            public PRODUCT_NAME PRODUCT_NAME { get; set; }
            public Product2Product_Gen__C Product2Product_Gen__c { get; set; }
            public Product2Producttype__C Product2ProductType__c { get; set; }
            public FAMILY FAMILY { get; set; }
            public Product2Terminal_Category__C Product2Terminal_Category__c { get; set; }
            public Product2Target_Markets__C Product2Target_Markets__c { get; set; }
            public NAME NAME { get; set; }
            public UNIT_PRICE UNIT_PRICE { get; set; }
            public Product2Itemdetails__C Product2ItemDetails__c { get; set; }
            public Product2Cpqproductdescription__C Product2cpqProductDescription__c { get; set; }
        }

        public class CUSTOMER_PRODUCT_ID
        {
            public string dataType { get; set; }
            public string entityColumnName { get; set; }
            public Fieldinfo fieldInfo { get; set; }
            public object[] filterValues { get; set; }
            public bool filterable { get; set; }
            public string fullyQualifiedName { get; set; }
            public object[] inactiveFilterValues { get; set; }
            public bool isLookup { get; set; }
            public string label { get; set; }
            public bool uniqueCountable { get; set; }
        }

        public class Fieldinfo
        {
            public string entityName { get; set; }
            public string fieldName { get; set; }
            public string lookupEntityName { get; set; }
        }

        public class Product2Stage__C
        {
            public string dataType { get; set; }
            public string entityColumnName { get; set; }
            public Fieldinfo1 fieldInfo { get; set; }
            public Filtervalue[] filterValues { get; set; }
            public bool filterable { get; set; }
            public string fullyQualifiedName { get; set; }
            public object[] inactiveFilterValues { get; set; }
            public bool isLookup { get; set; }
            public string label { get; set; }
            public bool uniqueCountable { get; set; }
        }

        public class Fieldinfo1
        {
            public string entityName { get; set; }
            public string fieldName { get; set; }
        }

        public class Filtervalue
        {
            public string apiName { get; set; }
            public string label { get; set; }
            public string name { get; set; }
        }

        public class PRODUCT_NAME
        {
            public string dataType { get; set; }
            public string entityColumnName { get; set; }
            public Fieldinfo2 fieldInfo { get; set; }
            public object[] filterValues { get; set; }
            public bool filterable { get; set; }
            public string fullyQualifiedName { get; set; }
            public object[] inactiveFilterValues { get; set; }
            public bool isLookup { get; set; }
            public string label { get; set; }
            public bool uniqueCountable { get; set; }
        }

        public class Fieldinfo2
        {
            public string entityName { get; set; }
            public string fieldName { get; set; }
            public string lookupEntityName { get; set; }
        }

        public class Product2Product_Gen__C
        {
            public string dataType { get; set; }
            public string entityColumnName { get; set; }
            public Fieldinfo3 fieldInfo { get; set; }
            public Filtervalue1[] filterValues { get; set; }
            public bool filterable { get; set; }
            public string fullyQualifiedName { get; set; }
            public object[] inactiveFilterValues { get; set; }
            public bool isLookup { get; set; }
            public string label { get; set; }
            public bool uniqueCountable { get; set; }
        }

        public class Fieldinfo3
        {
            public string entityName { get; set; }
            public string fieldName { get; set; }
        }

        public class Filtervalue1
        {
            public string apiName { get; set; }
            public string label { get; set; }
            public string name { get; set; }
        }

        public class Product2Producttype__C
        {
            public string dataType { get; set; }
            public string entityColumnName { get; set; }
            public Fieldinfo4 fieldInfo { get; set; }
            public Filtervalue2[] filterValues { get; set; }
            public bool filterable { get; set; }
            public string fullyQualifiedName { get; set; }
            public object[] inactiveFilterValues { get; set; }
            public bool isLookup { get; set; }
            public string label { get; set; }
            public bool uniqueCountable { get; set; }
        }

        public class Product2Target_Markets__C
        {
            public string dataType { get; set; }
            public string entityColumnName { get; set; }
            public Fieldinfo fieldInfo { get; set; }
            public Filtervalue[] filterValues { get; set; }
            public bool filterable { get; set; }
            public object[] inactiveFilterValues { get; set; }
            public bool isLookup { get; set; }
            public string label { get; set; }
            public bool uniqueCountable { get; set; }
        }

        public class Fieldinfo4
        {
            public string entityName { get; set; }
            public string fieldName { get; set; }
        }

        public class Filtervalue2
        {
            public string apiName { get; set; }
            public string label { get; set; }
            public string name { get; set; }
        }

        public class FAMILY
        {
            public string dataType { get; set; }
            public string entityColumnName { get; set; }
            public Fieldinfo5 fieldInfo { get; set; }
            public Filtervalue3[] filterValues { get; set; }
            public bool filterable { get; set; }
            public string fullyQualifiedName { get; set; }
            public object[] inactiveFilterValues { get; set; }
            public bool isLookup { get; set; }
            public string label { get; set; }
            public bool uniqueCountable { get; set; }
        }

        public class Fieldinfo5
        {
            public string entityName { get; set; }
            public string fieldName { get; set; }
        }

        public class Filtervalue3
        {
            public string apiName { get; set; }
            public string label { get; set; }
            public string name { get; set; }
        }

        public class Product2Terminal_Category__C
        {
            public string dataType { get; set; }
            public string entityColumnName { get; set; }
            public Fieldinfo6 fieldInfo { get; set; }
            public Filtervalue4[] filterValues { get; set; }
            public bool filterable { get; set; }
            public string fullyQualifiedName { get; set; }
            public object[] inactiveFilterValues { get; set; }
            public bool isLookup { get; set; }
            public string label { get; set; }
            public bool uniqueCountable { get; set; }
        }

        public class Fieldinfo6
        {
            public string entityName { get; set; }
            public string fieldName { get; set; }
        }

        public class Filtervalue4
        {
            public string apiName { get; set; }
            public string label { get; set; }
            public string name { get; set; }
        }

        public class NAME
        {
            public string dataType { get; set; }
            public string entityColumnName { get; set; }
            public Fieldinfo7 fieldInfo { get; set; }
            public object[] filterValues { get; set; }
            public bool filterable { get; set; }
            public string fullyQualifiedName { get; set; }
            public object[] inactiveFilterValues { get; set; }
            public bool isLookup { get; set; }
            public string label { get; set; }
            public bool uniqueCountable { get; set; }
        }

        public class Fieldinfo7
        {
            public string entityName { get; set; }
            public string fieldName { get; set; }
            public string lookupEntityName { get; set; }
        }

        public class UNIT_PRICE
        {
            public string dataType { get; set; }
            public string entityColumnName { get; set; }
            public object[] filterValues { get; set; }
            public bool filterable { get; set; }
            public string fullyQualifiedName { get; set; }
            public object[] inactiveFilterValues { get; set; }
            public bool isLookup { get; set; }
            public string label { get; set; }
            public bool uniqueCountable { get; set; }
        }

        public class Product2Itemdetails__C
        {
            public string dataType { get; set; }
            public string entityColumnName { get; set; }
            public object[] filterValues { get; set; }
            public bool filterable { get; set; }
            public string fullyQualifiedName { get; set; }
            public object[] inactiveFilterValues { get; set; }
            public bool isLookup { get; set; }
            public string label { get; set; }
            public bool uniqueCountable { get; set; }
        }

        public class Product2Cpqproductdescription__C
        {
            public string dataType { get; set; }
            public string entityColumnName { get; set; }
            public object[] filterValues { get; set; }
            public bool filterable { get; set; }
            public string fullyQualifiedName { get; set; }
            public object[] inactiveFilterValues { get; set; }
            public bool isLookup { get; set; }
            public string label { get; set; }
            public bool uniqueCountable { get; set; }
        }

        public class Groupingcolumninfo
        {
        }

        public class Reportmetadata
        {
            public string[] aggregates { get; set; }
            public object chart { get; set; }
            public object[] crossFilters { get; set; }
            public object currency { get; set; }
            public object dashboardSetting { get; set; }
            public string description { get; set; }
            public string[] detailColumns { get; set; }
            public string developerName { get; set; }
            public object division { get; set; }
            public string folderId { get; set; }
            public object[] groupingsAcross { get; set; }
            public object[] groupingsDown { get; set; }
            public bool hasDetailRows { get; set; }
            public bool hasRecordCount { get; set; }
            public object[] historicalSnapshotDates { get; set; }
            public string id { get; set; }
            public string name { get; set; }
            public Presentationoptions presentationOptions { get; set; }
            public object reportBooleanFilter { get; set; }
            public Reportfilter[] reportFilters { get; set; }
            public string reportFormat { get; set; }
            public Reporttype reportType { get; set; }
            public object scope { get; set; }
            public bool showGrandTotal { get; set; }
            public bool showSubtotals { get; set; }
            public Sortby[] sortBy { get; set; }
            public Standarddatefilter standardDateFilter { get; set; }
            public object standardFilters { get; set; }
            public bool supportsRoleHierarchy { get; set; }
            public object userOrHierarchyFilterId { get; set; }
        }

        public class Presentationoptions
        {
            public bool hasStackedSummaries { get; set; }
        }

        public class Reporttype
        {
            public string label { get; set; }
            public string type { get; set; }
        }

        public class Standarddatefilter
        {
            public string column { get; set; }
            public string durationValue { get; set; }
            public object endDate { get; set; }
            public object startDate { get; set; }
        }

        public class Reportfilter
        {
            public string column { get; set; }
            public string filterType { get; set; }
            public bool isRunPageEditable { get; set; }
            public string _operator { get; set; }
            public string value { get; set; }
        }

        public class Sortby
        {
            public string sortColumn { get; set; }
            public string sortOrder { get; set; }
        }
    }
}
