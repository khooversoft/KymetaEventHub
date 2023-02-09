using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.SOAP;

public static class OracleSoapTemplates
{
    #region Location
    /// <summary>
    ///  A template for finding Locations in Oracle based on Enterprise Id.
    /// </summary>
    /// <returns>TBD</returns>
    public static string FindLocations(List<Tuple<string, ulong?, ulong?>> addressIds)
    {
        if (addressIds == null || addressIds.Count == 0) return null;

        var findLocationsEnvelope =
            $@"<soap:Envelope
	            xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""
	            xmlns:find=""http://xmlns.oracle.com/adf/svc/types/""
	            xmlns:typ=""http://xmlns.oracle.com/apps/cdm/foundation/parties/locationService/applicationModule/types/"">
	            <soap:Body>
		            <typ:getLocationByOriginalSystemReference>
			            <typ:findCriteria>
				            <find:fetchStart>0</find:fetchStart>
				            <find:fetchSize>-1</find:fetchSize>
				            <find:filter>
					            <find:conjunction/>
					            <find:group>
						            <find:conjunction/>
						            <find:upperCaseCompare>false</find:upperCaseCompare>";
        foreach (var addressId in addressIds)
        {
            // find Location by LocationId for legacy objects or default to OrigSystemReference
            findLocationsEnvelope +=
                                    @$"<find:item>
							            <find:conjunction>Or</find:conjunction>
							            <find:upperCaseCompare>false</find:upperCaseCompare>
							            <find:attribute>{(addressId.Item2 != null ? "LocationId" : "OrigSystemReference")}</find:attribute>
							            <find:operator>=</find:operator>
							            <find:value>{(addressId.Item2 != null ? addressId.Item2 : addressId.Item1)}</find:value>
						            </find:item>";
        }

        findLocationsEnvelope +=
					            @$"</find:group>
				            </find:filter>
				            <find:findAttribute>LocationId</find:findAttribute>
				            <find:findAttribute>OrigSystemReference</find:findAttribute>
				            <find:findAttribute>Address1</find:findAttribute>
				            <find:findAttribute>Address2</find:findAttribute>
				            <find:findAttribute>City</find:findAttribute>
				            <find:findAttribute>State</find:findAttribute>
				            <find:findAttribute>PostalCode</find:findAttribute>
				            <find:findAttribute>Country</find:findAttribute>
				            <find:excludeAttribute>false</find:excludeAttribute>
			            </typ:findCriteria>
		            </typ:getLocationByOriginalSystemReference>
	            </soap:Body>
            </soap:Envelope>";
        return findLocationsEnvelope;
    }

    /// <summary>
    ///  A template for creating a Location object in Oracle
    /// </summary>
    /// <returns>TBD</returns>
    public static string CreateLocation(OracleLocationModel location)
    {
        var locationEnvelope =
            $@"<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                <soap:Body xmlns:typ=""http://xmlns.oracle.com/apps/cdm/foundation/parties/locationService/applicationModule/types/"">
                    <typ:createLocation>
                        <typ:location xmlns:loc=""http://xmlns.oracle.com/apps/cdm/foundation/parties/locationService/"">
                            <loc:CreatedByModule>HZ_WS</loc:CreatedByModule>
                            <loc:OrigSystem>SFDC</loc:OrigSystem>
                            <loc:OrigSystemReference>{location.OrigSystemReference}</loc:OrigSystemReference>
                            <loc:Address1>{location.Address1}</loc:Address1>
                            <loc:Address2>{location.Address2}</loc:Address2>
                            <loc:Address3> </loc:Address3>
                            <loc:City>{location.City}</loc:City>
                            <loc:State>{(string.IsNullOrEmpty(location.State) ? " " : location.State)}</loc:State>
                            <loc:Province>{(string.IsNullOrEmpty(location.State) ? " " : location.State)}</loc:Province>
                            <loc:PostalCode>{location.PostalCode}</loc:PostalCode>
                            <loc:Country>{location.Country}</loc:Country>
                        </typ:location>
                    </typ:createLocation>
                </soap:Body>
            </soap:Envelope>";
        return locationEnvelope;
    }

    /// <summary>
    ///  A template for updating an existing Location object in Oracle
    /// </summary>
    /// <returns>TBD</returns>
    public static string UpdateLocation(OracleLocationModel location)
    {
        var locationEnvelope =
            $@"<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
	            <soap:Body xmlns:typ=""http://xmlns.oracle.com/apps/cdm/foundation/parties/locationService/applicationModule/types/"">
		            <typ:updateLocation>
			            <typ:location xmlns:loc=""http://xmlns.oracle.com/apps/cdm/foundation/parties/locationService/"">
				            <loc:LocationId>{location.LocationId}</loc:LocationId>
				            <loc:Address1>{location.Address1}</loc:Address1>
				            <loc:Address2>{location.Address2}</loc:Address2>
				            <loc:Address3> </loc:Address3>
				            <loc:City>{location.City}</loc:City>
                            <loc:State>{(string.IsNullOrEmpty(location.State) ? " " : location.State)}</loc:State>
                            <loc:Province>{(string.IsNullOrEmpty(location.State) ? " " : location.State)}</loc:Province>
				            <loc:PostalCode>{location.PostalCode}</loc:PostalCode>
				            <loc:Country>{location.Country}</loc:Country>
			            </typ:location>
		            </typ:updateLocation>
	            </soap:Body>
            </soap:Envelope>";
        return locationEnvelope;
    }
    #endregion

    #region Organization & PartySite
    /// <summary>
    ///  A template for creating an Organization Party Site object in Oracle.
    /// </summary>
    /// <returns>TBD</returns>
    public static string FindOrganization(string originSystemReference, ulong? partyId = null)
    {
        var locationEnvelope = 
            $@"<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                <soap:Body>
                <typ:findOrganization xmlns:typ=""http://xmlns.oracle.com/apps/cdm/foundation/parties/organizationService/applicationModule/types/"">
                    <typ:findCriteria xmlns:find=""http://xmlns.oracle.com/adf/svc/types/"">
                    <find:fetchStart>0</find:fetchStart>
                    <find:fetchSize>-1</find:fetchSize>
                    <find:filter>
                        <find:conjunction/>
                        <find:group>
                        <find:conjunction/>
                        <find:upperCaseCompare>false</find:upperCaseCompare>
                        <find:item>
                            <find:conjunction/>
                            <find:upperCaseCompare>false</find:upperCaseCompare>
                            <find:attribute>{(partyId != null ? "PartyId" : "OrigSystemReference")}</find:attribute>
                            <find:operator>=</find:operator>
                            <find:value>{(partyId != null ? partyId : originSystemReference)}</find:value>
                        </find:item>
                        </find:group>
                    </find:filter>
                    <find:findAttribute>PartyId</find:findAttribute>
                    <find:findAttribute>PartyName</find:findAttribute>
                    <find:findAttribute>PartyNumber</find:findAttribute>
                    <find:findAttribute>OrigSystemReference</find:findAttribute>
				    <find:findAttribute>PartySite</find:findAttribute>
				    <find:findAttribute>Relationship</find:findAttribute>
                    <find:excludeAttribute>false</find:excludeAttribute>
				    <find:childFindCriteria>
                        <find:fetchStart>0</find:fetchStart>
                        <find:fetchSize>-1</find:fetchSize>
                        <find:childAttrName>PartySite</find:childAttrName>
                        <find:findAttribute>PartyId</find:findAttribute>
					    <find:findAttribute>PartySiteId</find:findAttribute>
                        <find:findAttribute>PartySiteNumber</find:findAttribute>
                        <find:findAttribute>PartySiteName</find:findAttribute>
					    <find:findAttribute>OrigSystemReference</find:findAttribute>
					    <find:findAttribute>LocationId</find:findAttribute>
                    </find:childFindCriteria>				
				     <find:childFindCriteria>
                        <find:fetchStart>0</find:fetchStart>
                        <find:fetchSize>-1</find:fetchSize>
                        <find:childAttrName>Relationship</find:childAttrName>					
					    <find:findAttribute>OrganizationContact</find:findAttribute>
					    <find:childFindCriteria>
						    <find:fetchStart>0</find:fetchStart>
						    <find:fetchSize>-1</find:fetchSize>
						    <find:childAttrName>OrganizationContact</find:childAttrName>
						    <find:findAttribute>ContactPartyId</find:findAttribute>
						    <find:findAttribute>ContactNumber</find:findAttribute>
						    <find:findAttribute>PersonFirstName</find:findAttribute>
						    <find:findAttribute>PersonLastName</find:findAttribute>
						    <find:findAttribute>OrigSystemReference</find:findAttribute>	
					    </find:childFindCriteria>
                    </find:childFindCriteria>
				
                    </typ:findCriteria>
                    <typ:findControl xmlns:find=""http://xmlns.oracle.com/adf/svc/types/"">
                    <find:retrieveAllTranslations>false</find:retrieveAllTranslations>
                    </typ:findControl>
                </typ:findOrganization>
                </soap:Body>
            </soap:Envelope>";
        return locationEnvelope;
    }

    /// <summary>
    /// A template for creating an Organization object in Oracle (with PartySites)
    /// </summary>
    /// <param name="organization"></param>
    /// <param name="partySites"></param>
    /// <returns></returns>
    public static string CreateOrganization(CreateOracleOrganizationModel organization, List<OraclePartySite>? partySites)
    {
        var organizationEnvelope =
            $@"<soap:Envelope
	            xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""
	            xmlns:typ=""http://xmlns.oracle.com/apps/cdm/foundation/parties/organizationService/applicationModule/types/""
	            xmlns:org=""http://xmlns.oracle.com/apps/cdm/foundation/parties/organizationService/""
	            xmlns:par=""http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/"">
	            <soap:Body>
		            <typ:createOrganization>
			            <typ:organizationParty>
				            <org:CreatedByModule>HZ_WS</org:CreatedByModule>
				            <org:OrganizationProfile>
					            <org:OrganizationName>{organization.OrganizationName}</org:OrganizationName>
					            <org:OrigSystemReference>{organization.SourceSystemReferenceValue}</org:OrigSystemReference>
					            <org:TaxpayerIdentificationNumber>{organization.TaxpayerIdentificationNumber}</org:TaxpayerIdentificationNumber>
					            <org:CreatedByModule>HZ_WS</org:CreatedByModule>
				            </org:OrganizationProfile>
				            <org:PartyUsageAssignment>
					            <par:PartyUsageCode>CUSTOMER</par:PartyUsageCode>
					            <par:CreatedByModule>HZ_WS</par:CreatedByModule>
				            </org:PartyUsageAssignment>
				            <org:PartyUsageAssignment>
					            <par:PartyUsageCode>SALES_ACCOUNT</par:PartyUsageCode>
					            <par:CreatedByModule>HZ_WS</par:CreatedByModule>
				            </org:PartyUsageAssignment>";
        // check for PartySites
        if (partySites != null && partySites.Count > 0)
        {
            // include all the PartySite additions
            foreach (var ps in partySites)
            {
                organizationEnvelope +=
                                "<org:PartySite>" +
                                    $"<par:LocationId>{ps.LocationId}</par:LocationId>" +
                                    $"<par:OrigSystemReference>{ps.OrigSystemReference}</par:OrigSystemReference>" +
                                    $"<par:PartySiteName>{ps.PartySiteName}</par:PartySiteName>" +
                                    "<par:CreatedByModule>HZ_WS</par:CreatedByModule>";                
                // append the Site Uses (there can be one or many)
                if (ps.SiteUses != null)
                {
                    foreach (var siteUse in ps.SiteUses)
                    {
                        organizationEnvelope +=
                                        "<par:PartySiteUse>" +
                                            $"<par:SiteUseType>{siteUse.SiteUseType}</par:SiteUseType>" +
                                            "<par:CreatedByModule>HZ_WS</par:CreatedByModule>" +
                                        "</par:PartySiteUse>";
                    }
                }
                organizationEnvelope +=
                                "</org:PartySite>";
            }
        }

        organizationEnvelope +=
                        $@"</typ:organizationParty>
		            </typ:createOrganization>
	            </soap:Body>
            </soap:Envelope>";
        return organizationEnvelope;
    }

    /// <summary>
    ///  A template for creating an Organization Party Site object in Oracle.
    /// </summary>
    /// <returns>TBD</returns>
    public static string CreateOrganizationPartySites(ulong organizationPartyId, List<OraclePartySite> partySites)
    {
        var locationEnvelope =
            $"<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">" +
                "<soap:Body>" +
                    "<ns1:mergeOrganization xmlns:ns1=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/organizationService/applicationModule/types/\">" +
                        "<ns1:organizationParty xmlns:ns2=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/organizationService/\">" +
                            $"<ns2:PartyId>{organizationPartyId}</ns2:PartyId>";
        // include all the PartySite additions
        foreach (var ps in partySites)
        {
            locationEnvelope +=
                            "<ns2:PartySite xmlns:ns3=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/\">" +
                                $"<ns3:LocationId>{ps.LocationId}</ns3:LocationId>" +
                                $"<ns3:OrigSystemReference>{ps.OrigSystemReference}</ns3:OrigSystemReference>" +
                                $"<ns3:PartySiteName>{ps.PartySiteName}</ns3:PartySiteName>" +
                                "<ns3:CreatedByModule>HZ_WS</ns3:CreatedByModule>";
            if (ps.SiteUses != null)
            {
                foreach (var siteUse in ps.SiteUses)
                {
                    locationEnvelope +=
                                    "<ns3:PartySiteUse>" +
                                        $"<ns3:SiteUseType>{siteUse.SiteUseType}</ns3:SiteUseType>" +
                                        "<ns3:CreatedByModule>HZ_WS</ns3:CreatedByModule>" +
                                    "</ns3:PartySiteUse>";
                }
            }
            locationEnvelope +=
                            "</ns2:PartySite>";
        }

        locationEnvelope +=
                        "</ns1:organizationParty>" +
	                "</ns1:mergeOrganization>" +
                "</soap:Body>" +
                "</soap:Envelope>";
        return locationEnvelope;
    }

    /// <summary>
    ///  A template for creating an Organization Party Site object in Oracle.
    /// </summary>
    /// <returns>TBD</returns>
    public static string UpdateOrganizationPartySites(ulong organizationPartyId, List<OraclePartySite> partySites)
    {
        var locationEnvelope =
            $"<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">" +
                "<soap:Body>" +
                    "<ns1:mergeOrganization xmlns:ns1=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/organizationService/applicationModule/types/\">" +
                        "<ns1:organizationParty xmlns:ns2=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/organizationService/\">" +
                            $"<ns2:PartyId>{organizationPartyId}</ns2:PartyId>";
        // include all the PartySite additions
        foreach (var ps in partySites)
        {
            locationEnvelope +=
                            "<ns2:PartySite xmlns:ns3=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/\">" +
                                $"<ns3:PartyId>{ps.PartyId}</ns3:PartyId>" +
                                $"<ns3:PartySiteId>{ps.PartySiteId}</ns3:PartySiteId>" +
                                $"<ns3:LocationId>{ps.LocationId}</ns3:LocationId>" +
                                $"<ns3:OrigSystemReference>{ps.OrigSystemReference}</ns3:OrigSystemReference>" +
                                $"<ns3:PartySiteName>{ps.PartySiteName}</ns3:PartySiteName>";
            // TODO: figure out why Oracle prevents us from updating these PartySiteUse objects... current errors on conflicting Site Use values instead of accepting what is provided
            //if (ps.SiteUses != null)
            //{
            //    foreach (var siteUse in ps.SiteUses)
            //    {
            //        locationEnvelope +=
            //                        "<ns3:PartySiteUse>" +
            //                            $"<ns3:SiteUseType>{siteUse.SiteUseType}</ns3:SiteUseType>" +
            //                            "<ns3:CreatedByModule>HZ_WS</ns3:CreatedByModule>" +
            //                        "</ns3:PartySiteUse>";
            //    }
            //}
            locationEnvelope +=
                            "</ns2:PartySite>";
        }

        locationEnvelope +=
                        "</ns1:organizationParty>" +
                    "</ns1:mergeOrganization>" +
                "</soap:Body>" +
                "</soap:Envelope>";
        return locationEnvelope;
    }
    #endregion

    #region Person
    /// <summary>
    /// A template for finding Persons in Oracle based on Enterprise Id.
    /// </summary>
    /// <returns>TBD</returns>
    public static string FindPersons(List<Tuple<string, ulong?>> contactIds)
    {
        if (contactIds == null || contactIds.Count == 0) return null;

        var findPersonsEnvelope =
            $@"<soap:Envelope
	            xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""
	            xmlns:find=""http://xmlns.oracle.com/adf/svc/types/""
	            xmlns:typ=""http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/applicationModule/types/"">
	            <soap:Body>
		            <typ:findPerson>
			            <typ:findCriteria>
				            <find:fetchStart>0</find:fetchStart>
				            <find:fetchSize>-1</find:fetchSize>
				            <find:filter>
					            <find:conjunction/>
					            <find:group>
						            <find:conjunction/>
						            <find:upperCaseCompare>false</find:upperCaseCompare>";
        foreach (var contactId in contactIds)
        {
            findPersonsEnvelope +=
                                    @$"<find:item>
							            <find:conjunction>Or</find:conjunction>
							            <find:upperCaseCompare>false</find:upperCaseCompare>
							            <find:attribute>{(contactId.Item2 != null ? "PartyId" : "OrigSystemReference")}</find:attribute>
							            <find:operator>=</find:operator>
							            <find:value>{(contactId.Item2 != null ? contactId.Item2 : contactId.Item1)}</find:value>
						            </find:item>";
        }

        findPersonsEnvelope +=
                                @$"</find:group>
				            </find:filter>
				            <find:findAttribute>PartyId</find:findAttribute>
				            <find:findAttribute>OrigSystemReference</find:findAttribute>
				            <find:findAttribute>PartyName</find:findAttribute>
				            <find:findAttribute>PersonFirstName</find:findAttribute>
				            <find:findAttribute>PersonLastName</find:findAttribute>				
				            <find:findAttribute>Email</find:findAttribute>
				            <find:childFindCriteria>
					            <find:fetchStart>0</find:fetchStart>
					            <find:fetchSize>-1</find:fetchSize>
					            <find:childAttrName>Email</find:childAttrName>
					            <find:findAttribute>ContactPointId</find:findAttribute>
					            <find:findAttribute>EmailAddress</find:findAttribute>
				            </find:childFindCriteria>				
				            <find:findAttribute>Phone</find:findAttribute>
				            <find:childFindCriteria>
					            <find:fetchStart>0</find:fetchStart>
					            <find:fetchSize>-1</find:fetchSize>
					            <find:childAttrName>Phone</find:childAttrName>
					            <find:findAttribute>ContactPointId</find:findAttribute>
					            <find:findAttribute>PhoneNumber</find:findAttribute>
                                <find:findAttribute>PhoneLineType</find:findAttribute>
				            </find:childFindCriteria>				
 				            <find:findAttribute>Relationship</find:findAttribute>
				            <find:childFindCriteria>
					            <find:fetchStart>0</find:fetchStart>
					            <find:fetchSize>-1</find:fetchSize>
					            <find:childAttrName>Relationship</find:childAttrName>
					            <find:findAttribute>RelationshipId</find:findAttribute>
					            <find:findAttribute>ObjectId</find:findAttribute>
					            <find:findAttribute>ObjectType</find:findAttribute>					
					            <find:findAttribute>OrganizationContact</find:findAttribute>
					            <find:childFindCriteria>
						            <find:fetchStart>0</find:fetchStart>
						            <find:fetchSize>-1</find:fetchSize>
						            <find:childAttrName>OrganizationContact</find:childAttrName>
						            <find:findAttribute>ContactNumber</find:findAttribute>
					            </find:childFindCriteria>
				            </find:childFindCriteria>
				            <find:excludeAttribute>false</find:excludeAttribute>
			            </typ:findCriteria>
		            </typ:findPerson>
	            </soap:Body>
            </soap:Envelope>";
        return findPersonsEnvelope;
    }

    /// <summary>
    ///  A template for creating a Person object in Oracle
    /// </summary>
    /// <returns>TBD</returns>
    public static string CreatePerson(OraclePersonObject person, ulong organizationPartyId)
    {
        var currentDate = $"{DateTime.UtcNow:yyyy-MM-dd}";
        var personEnvelope =
            $@"<soapenv:Envelope
                xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" 
		        xmlns:con=""http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/""
  	            xmlns:con1=""http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/contactPoint/""
                xmlns:org=""http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/orgContact/""
                xmlns:par=""http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/""
                xmlns:par1=""http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/partySite/""
                xmlns:per=""http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/""
                xmlns:per1=""http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/person/""
  	            xmlns:rel=""http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/""
		        xmlns:rel1=""http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/relationship/""
 		        xmlns:sour=""http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/sourceSystemRef/""
		        xmlns:typ=""http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/applicationModule/types/"">
                <soapenv:Header />
                <soapenv:Body>
                    <typ:createPerson>
                        <typ:personParty>
                            <per:CreatedByModule>HZ_WS</per:CreatedByModule>
                            <per:PersonProfile>
                                <per:PersonFirstName>{person.FirstName}</per:PersonFirstName>
                                <per:PersonLastName>{person.LastName}</per:PersonLastName>
                                <per:PersonTitle>{person.Title}</per:PersonTitle>
                                <per:CreatedByModule>HZ_WS</per:CreatedByModule>
                                <per:OrigSystemReference>{person.OrigSystemReference}</per:OrigSystemReference>
                            </per:PersonProfile>
                            <per:Relationship>
                                <rel:SubjectType>PERSON</rel:SubjectType>
                                <rel:SubjectTableName>HZ_PARTIES</rel:SubjectTableName>
                                <rel:ObjectId>{organizationPartyId}</rel:ObjectId>
                                <rel:ObjectType>ORGANIZATION</rel:ObjectType>
                                <rel:ObjectTableName>HZ_PARTIES</rel:ObjectTableName>
                                <rel:RelationshipCode>CONTACT_OF</rel:RelationshipCode>
                                <rel:RelationshipType>CONTACT</rel:RelationshipType>
                                <rel:StartDate>{currentDate}</rel:StartDate>
                                <rel:CreatedByModule>HZ_WS</rel:CreatedByModule>
                                <rel:OrganizationContact>
                                    <rel:CreatedByModule>HZ_WS</rel:CreatedByModule>
                                    <rel:OrganizationContactRole>
                                        <rel:RoleType>CONTACT</rel:RoleType>
                                        <rel:PrimaryFlag>{person.IsPrimary ?? false}</rel:PrimaryFlag>
                                        <rel:CreatedByModule>HZ_WS</rel:CreatedByModule>
                                    </rel:OrganizationContactRole>
                                </rel:OrganizationContact>";

        // verify we have Phone metadata
        if (person.PhoneNumbers != null && person.PhoneNumbers.Count > 0)
        {
            person.PhoneNumbers.ForEach(phone =>
            {
                personEnvelope +=
                                $@"<rel:Phone>
                                    <con:OwnerTableName>HZ_PARTIES</con:OwnerTableName>
                                    <con:CreatedByModule>HZ_WS</con:CreatedByModule>
                                    <con:PhoneNumber>{phone.PhoneNumber}</con:PhoneNumber>
                                    <con:PhoneLineType>{phone.PhoneLineType}</con:PhoneLineType> 
                                </rel:Phone>";
            });
        }

        // verify we have Email metadata
        if (person.EmailAddresses != null && person.EmailAddresses.Count > 0)
        {
            var email = person.EmailAddresses.FirstOrDefault();
            if (email != null)
            {
                personEnvelope +=
                                $@"<rel:Email>
                                    <con:OwnerTableName>HZ_PARTIES</con:OwnerTableName>
                                    <con:CreatedByModule>HZ_WS</con:CreatedByModule>
                                    <con:EmailAddress>{email.EmailAddress}</con:EmailAddress>
                                    <con:ContactPointPurpose>BUSINESS</con:ContactPointPurpose>
                                </rel:Email>";
            }
        }

        personEnvelope +=
                            $@"</per:Relationship>
                        </typ:personParty>
                    </typ:createPerson>
                </soapenv:Body>
            </soapenv:Envelope>";
        return personEnvelope;
    }

    /// <summary>
    ///  A template for updating a Person object in Oracle
    /// </summary>
    /// <returns>TBD</returns>
    public static string UpdatePerson(OraclePersonObject person)
    {
        var currentDate = $"{DateTime.UtcNow:yyyy-MM-dd}";
        // TODO: may need ContactPointId for Phone and Email
        // <con:ContactPointId>300000099945549</con:ContactPointId>
        var personEnvelope =
            $@"<soapenv:Envelope
	            xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/""
	            xmlns:con=""http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/""
	            xmlns:con1=""http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/contactPoint/""
	            xmlns:org=""http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/orgContact/""
	            xmlns:par=""http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/""
	            xmlns:par1=""http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/partySite/""
	            xmlns:per=""http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/""
	            xmlns:per1=""http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/person/""
	            xmlns:rel=""http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/""
	            xmlns:rel1=""http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/relationship/""
	            xmlns:sour=""http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/sourceSystemRef/""
	            xmlns:typ=""http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/applicationModule/types/"">
	            <soapenv:Header />
	            <soapenv:Body>
		            <typ:mergePerson>
			            <typ:personParty>
				            <per:PartyId>{person.PartyId}</per:PartyId>
				            <per:PersonProfile>
					            <per:PersonFirstName>{person.FirstName}</per:PersonFirstName>
					            <per:PersonLastName>{person.LastName}</per:PersonLastName>
                                <per:PersonTitle>{person.Title}</per:PersonTitle>
				            </per:PersonProfile>";
        
        // verify we have Phone metadata
        if (person.PhoneNumbers != null && person.PhoneNumbers.Count > 0)
        {
            person.PhoneNumbers.ForEach(phone =>
            {
                // if we have a ContactPointId, then we are updating an existing phone number
                if (phone.ContactPointId != null)
                {
                    personEnvelope +=
                                $@"<per:Phone>
                                    <con:ContactPointId>{phone.ContactPointId}</con:ContactPointId>
                                    <con:PhoneNumber>{phone.PhoneNumber}</con:PhoneNumber>
                                </per:Phone>";
                } else // create a new phone number
                {
                    personEnvelope +=
                                $@"<per:Phone>
                                    <con:OwnerTableName>HZ_PARTIES</con:OwnerTableName>
                                    <con:CreatedByModule>HZ_WS</con:CreatedByModule>
                                    <con:RelationshipId>{person.RelationshipId}</con:RelationshipId>
                                    <con:PhoneNumber>{phone.PhoneNumber}</con:PhoneNumber>
                                    <con:PhoneLineType>{phone.PhoneLineType}</con:PhoneLineType> 
                                </per:Phone>";
                }
            });
        }

        // verify we have Email metadata
        if (person.EmailAddresses != null && person.EmailAddresses.Count > 0)
        {
            var email = person.EmailAddresses.FirstOrDefault();
            if (email != null)
            {
                if (email.ContactPointId != null)
                {
                    personEnvelope +=
                                $@"<per:Email>
                                    <con:ContactPointId>{email.ContactPointId}</con:ContactPointId>
                                    <con:EmailAddress>{email.EmailAddress}</con:EmailAddress>
                                </per:Email>";
                }
                else
                {
                    personEnvelope +=
                                $@"<per:Email>
                                    <con:OwnerTableName>HZ_PARTIES</con:OwnerTableName>
					                <con:CreatedByModule>HZ_WS</con:CreatedByModule>
                                    <con:RelationshipId>{person.RelationshipId}</con:RelationshipId>
                                    <con:EmailAddress>{email.EmailAddress}</con:EmailAddress>
                                    <con:ContactPointPurpose>BUSINESS</con:ContactPointPurpose>
                                </per:Email>";
                }
            }
        }
        personEnvelope +=
			            $@"</typ:personParty>
		            </typ:mergePerson>
	            </soapenv:Body>
            </soapenv:Envelope>";
        return personEnvelope;
    }
    #endregion

    #region Customer Account
    /// <summary>
    /// Find an Oracle Customer Account by searching with the origin system reference (enterprise Id)
    /// </summary>
    /// <param name="enterpriseId">The Id of the originating object from Salesforce</param>
    /// <returns>A SOAP envelope XML payload to send as a request body.</returns>
    public static string FindCustomerAccount(string enterpriseId, ulong? oraclePartyId = null)
    {
        var findCustomerAccountEnvelope =
            $@"<soapenv:Envelope
	            xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/""
	            xmlns:typ=""http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/applicationModule/types/""
	            xmlns:find=""http://xmlns.oracle.com/adf/svc/types/"">
	            <soapenv:Header/>
	            <soapenv:Body>
		            <typ:findCustomerAccount>
			            <typ:findCriteria>
				            <find:fetchStart>0</find:fetchStart>
				            <find:fetchSize>1</find:fetchSize>
				            <find:filter>
					            <find:conjunction/>
					            <find:group>
						            <find:conjunction/>
						            <find:upperCaseCompare>false</find:upperCaseCompare>
						            <find:item>
							            <find:conjunction/>
							            <find:upperCaseCompare>false</find:upperCaseCompare>
							            <find:attribute>{(oraclePartyId != null ? "PartyId" : "OrigSystemReference")}</find:attribute>
							            <find:operator>=</find:operator>
							            <find:value>{(oraclePartyId != null ? oraclePartyId : enterpriseId)}</find:value>
						            </find:item>
					            </find:group>
					            <find:nested/>
				            </find:filter>
				            <find:findAttribute>PartyId</find:findAttribute>
				            <find:findAttribute>CustomerAccountId</find:findAttribute>
				            <find:findAttribute>AccountNumber</find:findAttribute>
				            <find:findAttribute>AccountName</find:findAttribute>
				            <find:findAttribute>OrigSystemReference</find:findAttribute>
				            <find:findAttribute>CustomerType</find:findAttribute>
				            <find:findAttribute>CustomerClassCode</find:findAttribute>
				            <find:findAttribute>CustomerAccountSite</find:findAttribute>
				            <find:findAttribute>CustomerAccountContact</find:findAttribute>
				            <find:excludeAttribute>false</find:excludeAttribute>
				            <!-- Customer Account Sites -->
				            <find:childFindCriteria>
					            <find:fetchStart>0</find:fetchStart>
					            <find:fetchSize>-1</find:fetchSize>
					            <find:childAttrName>CustomerAccountSite</find:childAttrName>
					            <find:findAttribute>OrigSystemReference</find:findAttribute>
					            <find:findAttribute>CustomerAccountSiteId</find:findAttribute>
					            <find:findAttribute>CustomerAccountId</find:findAttribute>
					            <find:findAttribute>PartySiteId</find:findAttribute>
					            <find:findAttribute>CustomerAccountSiteUse</find:findAttribute>
                                <!-- Original System Reference -->
					            <find:childFindCriteria>
						            <find:fetchStart>0</find:fetchStart>
						            <find:fetchSize>-1</find:fetchSize>
						            <find:childAttrName>CustomerAccountSiteUse</find:childAttrName>
						            <find:findAttribute>OriginalSystemReference</find:findAttribute>						
						            <find:childFindCriteria>
							            <find:fetchStart>0</find:fetchStart>
							            <find:fetchSize>-1</find:fetchSize>
							            <find:childAttrName>OriginalSystemReference</find:childAttrName>
							            <find:findAttribute>OrigSystemReference</find:findAttribute>
						            </find:childFindCriteria>
					            </find:childFindCriteria>
				            </find:childFindCriteria>
				            <!-- Customer Account Contacts -->
				            <find:childFindCriteria>
					            <find:fetchStart>0</find:fetchStart>
					            <find:fetchSize>-1</find:fetchSize>
					            <find:childAttrName>CustomerAccountContact</find:childAttrName>
					            <find:findAttribute>OrigSystemReference</find:findAttribute>
					            <find:findAttribute>ContactPersonId</find:findAttribute>
					            <find:findAttribute>CustomerAccountId</find:findAttribute>
					            <find:findAttribute>CustomerAccountRoleId</find:findAttribute>
					            <find:findAttribute>RelationshipId</find:findAttribute>
					            <find:findAttribute>PrimaryFlag</find:findAttribute>
				            </find:childFindCriteria>
			            </typ:findCriteria>
			            <typ:findControl>
				            <find:retrieveAllTranslations>false</find:retrieveAllTranslations>
			            </typ:findControl>
		            </typ:findCustomerAccount>
	            </soapenv:Body>
            </soapenv:Envelope>
            ";
        return findCustomerAccountEnvelope;
    }

    /// <summary>
    ///  A template for creating a Customer Account object in Oracle
    /// </summary>
    /// <returns>SOAP Envelope (payload) for creating a Customer Account in Oracle</returns>
    public static string CreateCustomerAccount(OracleCustomerAccount model, ulong organizationPartyId, List<OracleCustomerAccountSite> customerSites, List<OracleCustomerAccountContact> contacts)
    {
        // create the SOAP envelope with a beefy string
        var customerAccountEnvelope =
            $"<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" " +
                $"xmlns:info=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccount/\" " +
                $"xmlns:par=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/\">" +
                "<soap:Body xmlns:typ=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/applicationModule/types/\">" +
                  "<typ:createCustomerAccount>" +
                     "<typ:customerAccount xmlns:cus=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/\">" +
                        $"<cus:PartyId>{organizationPartyId}</cus:PartyId>" + // acquired from the create Organization response (via REST)
                        $"<cus:AccountName>{model.AccountName}</cus:AccountName>" + // description for the Customer Account
                        $"<cus:CustomerType>{model.AccountType}</cus:CustomerType>" +
                        $"<cus:CustomerClassCode>{model.AccountSubType}</cus:CustomerClassCode>" +
                        $"<cus:OrigSystemReference>{model.SalesforceId}</cus:OrigSystemReference>" +
                        "<cus:CreatedByModule>HZ_WS</cus:CreatedByModule>" +
                        "<cus:CustAcctInformation>" +
                            $"<info:salesforceId>{model.SalesforceId}</info:salesforceId>" +
                            $"<info:ksnId>{model.OssId}</info:ksnId>" +
                        "</cus:CustAcctInformation>";

        // check for the existence of any Sites
        if (customerSites != null && customerSites.Count > 0)
        {
            // include each Site
            foreach (var site in customerSites)
            {
                customerAccountEnvelope +=
                        "<cus:CustomerAccountSite>" +
                            $"<cus:PartySiteId>{site.PartySiteId}</cus:PartySiteId>" +
                            "<cus:CreatedByModule>HZ_WS</cus:CreatedByModule>" +
                            $"<cus:SetId>{site.SetId}</cus:SetId>" +
                            "<cus:OrigSystem>SFDC</cus:OrigSystem>" +
                            $"<cus:OrigSystemReference>{site.OrigSystemReference}</cus:OrigSystemReference>";
                if (site.SiteUses != null)
                {
                    foreach (var siteUse in site.SiteUses)
                    {
                        customerAccountEnvelope +=
                            @$"<cus:CustomerAccountSiteUse>
								<cus:SiteUseCode>{siteUse.SiteUseCode}</cus:SiteUseCode>
								<cus:CreatedByModule>HZ_WS</cus:CreatedByModule>
							</cus:CustomerAccountSiteUse>";
                    }
                }
                customerAccountEnvelope +=
                        "</cus:CustomerAccountSite>";
            }
        }

        // check for the existence of any Persons
        if (contacts != null && contacts.Count > 0)
        {
            // include each Person
            foreach (var contact in contacts)
            {
                customerAccountEnvelope +=
                        "<cus:CustomerAccountContact>" +
                            "<cus:RoleType>CONTACT</cus:RoleType>" +
                            "<cus:CreatedByModule>HZ_WS</cus:CreatedByModule>" +
                            $"<cus:RelationshipId>{contact.RelationshipId}</cus:RelationshipId>" +
                            "<cus:OrigSystem>SFDC</cus:OrigSystem>" +
                            $"<cus:OrigSystemReference>{contact.OrigSystemReference}</cus:OrigSystemReference>";
                if (contact.ResponsibilityTypes != null && contact.ResponsibilityTypes.Count > 0)
                {
                    foreach (var rType in contact.ResponsibilityTypes)
                    {
                        customerAccountEnvelope +=
                                "<cus:CustomerAccountContactRole>" +
                                    $"<cus:ResponsibilityType>{rType}</cus:ResponsibilityType>" +
                                "</cus:CustomerAccountContactRole>";
                    }
                }
                customerAccountEnvelope +=
                        "</cus:CustomerAccountContact>";
            }
        }

        // close the envelope
        customerAccountEnvelope +=
                     "</typ:customerAccount>" +
                  "</typ:createCustomerAccount>" +
               "</soap:Body>" +
            "</soap:Envelope>";
        return customerAccountEnvelope;
    }

    /// <summary>
    ///  A template for adding a Contact to a Customer Account in Oracle
    /// </summary>
    /// <returns>SOAP Envelope (payload) for creating updating a Customer Account in Oracle with a new Contact</returns>
    public static string UpsertCustomerAccount(OracleCustomerAccount account, List<OracleCustomerAccountSite> accountSites, List<OracleCustomerAccountContact> accountContacts)
    {
        // validate the inputs
        if (account.CustomerAccountId == null)
        {
            throw new ArgumentException($"'{nameof(account.CustomerAccountId)}' cannot be null.", nameof(account.CustomerAccountId));
        }
        if (account.PartyId == null)
        {
            throw new ArgumentException($"'{nameof(account.PartyId)}' cannot be null.", nameof(account.PartyId));
        }

        // create the SOAP envelope with a beefy string
        var customerAccountEnvelope =
            $"<soapenv:Envelope " +
                "xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" " +
                "xmlns:cus=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/\" " +
                "xmlns:cus1=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccountContactRole/\" " +
                "xmlns:cus2=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccountContact/\" " +
                "xmlns:cus3=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccountRel/\" " +
                "xmlns:cus4=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccountSiteUse/\" " +
                "xmlns:cus5=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccountSite/\" " +
                "xmlns:cus6=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccount/\" " +
                "xmlns:par=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/\" " +
                "xmlns:sour=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/sourceSystemRef/\" " +
                "xmlns:typ=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/applicationModule/types/\">" +
                "<soapenv:Header />" +
                "<soapenv:Body>" +
                    "<typ:mergeCustomerAccount>" +
                        "<typ:customerAccount>" +
                            $"<cus:CustomerAccountId>{account.CustomerAccountId}</cus:CustomerAccountId>" +
                            $"<cus:PartyId>{account.PartyId}</cus:PartyId>" +
                            $"<cus:AccountName>{account.AccountName}</cus:AccountName>" +
                            $"<cus:CustomerType>{account.AccountType}</cus:CustomerType>" +
                            "<cus:CustAcctInformation>" +
                                $"<cus6:salesforceId>{account.SalesforceId}</cus6:salesforceId>" +
                                $"<cus6:ksnId>{account.OssId}</cus6:ksnId>" +
                            "</cus:CustAcctInformation>";

        // update Customer Account Contacts
        if (accountContacts != null && accountContacts.Count > 0)
        {
            foreach (var person in accountContacts)
            {
                if (person.RelationshipId != null)
                {
                    customerAccountEnvelope +=
                            "<cus:CustomerAccountContact>" +
                                $"<cus:CustomerAccountId>{account.CustomerAccountId}</cus:CustomerAccountId>" +
                                $"<cus:RelationshipId>{person.RelationshipId}</cus:RelationshipId>" +
                                $"<cus:OrigSystemReference>{person.OrigSystemReference}</cus:OrigSystemReference>" +
                                $"<cus:PrimaryFlag>{person.IsPrimary}</cus:PrimaryFlag>" +
                                "<cus:CreatedByModule>HZ_WS</cus:CreatedByModule>" +
                                "<cus:RoleType>CONTACT</cus:RoleType>";
                    if (person.ResponsibilityTypes != null && person.ResponsibilityTypes.Count > 0)
                    {
                        foreach (var rType in person.ResponsibilityTypes)
                        {
                            customerAccountEnvelope +=
                                "<cus:CustomerAccountContactRole>" +
                                    $"<cus:ResponsibilityType>{rType}</cus:ResponsibilityType>" +
                                "</cus:CustomerAccountContactRole>";
                        }
                    }
                    customerAccountEnvelope +=
                            "</cus:CustomerAccountContact>";
                }
            }
        }

        // update Customer Account Sites
        if (accountSites != null && accountSites.Count > 0)
        {
            foreach (var site in accountSites)
            {
                customerAccountEnvelope +=
                            $@"<cus:CustomerAccountSite>
								<cus:PartySiteId>{site.PartySiteId}</cus:PartySiteId>
								<cus:CustomerAccountId>{account.CustomerAccountId}</cus:CustomerAccountId>
								<cus:CreatedByModule>HZ_WS</cus:CreatedByModule>
								<cus:SetId>{site.SetId}</cus:SetId>
								<cus:OrigSystemReference>{site.OrigSystemReference}</cus:OrigSystemReference>";

                if (site.SiteUses != null)
                {
                    foreach (var siteUse in site.SiteUses)
                    {
                        customerAccountEnvelope +=
                                        @$"<cus:CustomerAccountSiteUse>
									        <cus:SiteUseCode>{siteUse.SiteUseCode}</cus:SiteUseCode>
									        <cus:CreatedByModule>HZ_WS</cus:CreatedByModule>
								        </cus:CustomerAccountSiteUse>";
                    }
                }
                customerAccountEnvelope +=
                            "</cus:CustomerAccountSite>";
            }
        }
        customerAccountEnvelope +=
                        "</typ:customerAccount>" +
                    "</typ:mergeCustomerAccount>" +
                "</soapenv:Body>" +
            "</soapenv:Envelope>";
        return customerAccountEnvelope;
    }

    /// <summary>
    ///  A template for adding a Contact to a Customer Account in Oracle
    /// </summary>
    /// <returns>SOAP Envelope (payload) for creating updating a Customer Account in Oracle with a new Contact</returns>
    public static string UpdateCustomerAccountChildren(OracleCustomerAccount account, List<OracleCustomerAccountSite>? accountSites = null, List<OracleCustomerAccountContact>? accountContacts = null)
    {
        // validate the inputs
        if (account.CustomerAccountId == null)
        {
            throw new ArgumentException($"'{nameof(account.CustomerAccountId)}' cannot be null.", nameof(account.CustomerAccountId));
        }
        if (account.PartyId == null)
        {
            throw new ArgumentException($"'{nameof(account.PartyId)}' cannot be null.", nameof(account.PartyId));
        }

        // create the SOAP envelope with a beefy string
        var customerAccountEnvelope =
            $"<soapenv:Envelope " +
                "xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" " +
                "xmlns:cus=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/\" " +
                "xmlns:cus1=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccountContactRole/\" " +
                "xmlns:cus2=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccountContact/\" " +
                "xmlns:cus3=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccountRel/\" " +
                "xmlns:cus4=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccountSiteUse/\" " +
                "xmlns:cus5=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccountSite/\" " +
                "xmlns:cus6=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccount/\" " +
                "xmlns:par=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/\" " +
                "xmlns:sour=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/sourceSystemRef/\" " +
                "xmlns:typ=\"http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/applicationModule/types/\">" +
                "<soapenv:Header />" +
                "<soapenv:Body>" +
                    "<typ:mergeCustomerAccount>" +
                        "<typ:customerAccount>" +
                            $"<cus:PartyId>{account.PartyId}</cus:PartyId>" +
                            $"<cus:CustomerAccountId>{account.CustomerAccountId}</cus:CustomerAccountId>" +
                            $"<cus:OrigSystemReference>{account.OrigSystemReference}</cus:OrigSystemReference>";

        // update Customer Account Contacts
        if (accountContacts != null && accountContacts.Count > 0)
        {
            foreach (var person in accountContacts)
            {
                if (person.RelationshipId != null)
                {
                    customerAccountEnvelope +=
                            "<cus:CustomerAccountContact>" +
                                "<cus:CreatedByModule>HZ_WS</cus:CreatedByModule>" +
                                $"<cus:RelationshipId>{person.RelationshipId}</cus:RelationshipId>" + // RelationshipId from the Person response
                                $"<cus:OrigSystemReference>{person.OrigSystemReference}</cus:OrigSystemReference>" +
                                "<cus:RoleType>CONTACT</cus:RoleType>";

                    if (person.ResponsibilityTypes != null && person.ResponsibilityTypes.Count > 0)
                    {
                        foreach (var rType in person.ResponsibilityTypes)
                        {
                            customerAccountEnvelope +=
                                "<cus:CustomerAccountContactRole>" +
                                    $"<cus:ResponsibilityType>{rType}</cus:ResponsibilityType>" +
                                "</cus:CustomerAccountContactRole>";
                        }
                    }
                    customerAccountEnvelope +=
                            "</cus:CustomerAccountContact>";
                }
            }
        }

        // update Customer Account Sites
        if (accountSites != null && accountSites.Count > 0)
        {
            foreach (var site in accountSites)
            {
                customerAccountEnvelope +=
                            $@"<cus:CustomerAccountSite>
								<cus:PartySiteId>{site.PartySiteId}</cus:PartySiteId>
								<cus:CustomerAccountId>{account.CustomerAccountId}</cus:CustomerAccountId>
								<cus:CreatedByModule>HZ_WS</cus:CreatedByModule>
								<cus:SetId>{site.SetId}</cus:SetId>
								<cus:OrigSystemReference>{site.OrigSystemReference}</cus:OrigSystemReference>";

                // account for multiple site uses (purposes)
                if (site.SiteUses != null)
                {
                    foreach (var siteUse in site.SiteUses)
                    {
                        customerAccountEnvelope +=
                                        @$"<cus:CustomerAccountSiteUse>
									        <cus:SiteUseCode>{siteUse.SiteUseCode}</cus:SiteUseCode>
									        <cus:CreatedByModule>HZ_WS</cus:CreatedByModule>
								        </cus:CustomerAccountSiteUse>";
                    }
                }
                customerAccountEnvelope +=
                            "</cus:CustomerAccountSite>";
            }
        }
        customerAccountEnvelope +=
                        "</typ:customerAccount>" +
                    "</typ:mergeCustomerAccount>" +
                "</soapenv:Body>" +
            "</soapenv:Envelope>";
        return customerAccountEnvelope;
    }
    #endregion

    #region Customer Profile
    /// <summary>
    ///  A template for creating a Customer Account Profile object in Oracle
    /// </summary>
    /// <returns>TBD</returns>
    public static string CreateCustomerProfile(ulong? customerAccountPartyId, uint customerAccountNumber)
    {
        var locationEnvelope =
            $@"<soapenv:Envelope
	            xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/""
	            xmlns:typ=""http://xmlns.oracle.com/apps/financials/receivables/customers/customerProfileService/types/""
	            xmlns:cus=""http://xmlns.oracle.com/apps/financials/receivables/customers/customerProfileService/""
	            xmlns:cus1=""http://xmlns.oracle.com/apps/financials/receivables/customerSetup/customerProfiles/model/flex/CustomerProfileDff/""
	            xmlns:cus2=""http://xmlns.oracle.com/apps/financials/receivables/customerSetup/customerProfiles/model/flex/CustomerProfileGdf/""
	            xmlns:xsi=""xsi"">
	            <soapenv:Header/>
	            <soapenv:Body>
		            <typ:createCustomerProfile>
			            <typ:customerProfile>
				            <cus:PartyId>{customerAccountPartyId}</cus:PartyId> <!-- You will get Customer PartyId in response of Customer creation -->
				            <cus:AccountNumber>{customerAccountNumber}</cus:AccountNumber>
				            <cus:ProfileClassName>DEFAULT</cus:ProfileClassName>
				            <cus:EffectiveStartDate>{DateTime.UtcNow:yyyy-MM-dd}</cus:EffectiveStartDate>
			            </typ:customerProfile>
		            </typ:createCustomerProfile>
	            </soapenv:Body>
            </soapenv:Envelope>";
        return locationEnvelope;
    }

    public static string GetActiveCustomerProfile(string customerAccountNumber)
    {
        var customerProfileEnvelope =
            $@"<soapenv:Envelope
	            xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/""
	            xmlns:typ=""http://xmlns.oracle.com/apps/financials/receivables/customers/customerProfileService/types/""
	            xmlns:cus=""http://xmlns.oracle.com/apps/financials/receivables/customers/customerProfileService/""
	            xmlns:cus1=""http://xmlns.oracle.com/apps/financials/receivables/customerSetup/customerProfiles/model/flex/CustomerProfileDff/""
	            xmlns:cus2=""http://xmlns.oracle.com/apps/financials/receivables/customerSetup/customerProfiles/model/flex/CustomerProfileGdf/""
	            xmlns:xsi=""xsi"">
	            <soapenv:Header/>
	            <soapenv:Body>
		            <typ:getActiveCustomerProfile>
			            <typ:customerProfile>
				            <cus:AccountNumber>{customerAccountNumber}</cus:AccountNumber>
			            </typ:customerProfile>
		            </typ:getActiveCustomerProfile>
	            </soapenv:Body>
            </soapenv:Envelope>";
        return customerProfileEnvelope;
    }
    #endregion

    #region Reports
    /// <summary>
    ///  A template for fetching a Report from Oracle based on Absolute Path.
    /// </summary>
    /// <returns>Returns the XML SOAP payload for Oracle HTTP request.</returns>
    public static string FetchReport(string reportAbsolutePath)
    {
        if (string.IsNullOrEmpty(reportAbsolutePath)) return null;

        var fetchReportEnvelope =
            $@"<soap:Envelope 
	            xmlns:soap=""http://www.w3.org/2003/05/soap-envelope""
	            xmlns:pub=""http://xmlns.oracle.com/oxp/service/PublicReportService"">
               <soap:Header/>
               <soap:Body>
                  <pub:runReport>
                     <pub:reportRequest>
                        <pub:parameterNameValues>
                           <!--Zero or more repetitions:-->
                           <pub:item>
                              <pub:name>p_report_type</pub:name>
                              <pub:values>
                                 <!--Zero or more repetitions:-->
                                 <pub:item>SHIPMENT</pub:item>
                              </pub:values>
                           </pub:item>
                        </pub:parameterNameValues>
                        <pub:reportAbsolutePath>{reportAbsolutePath}</pub:reportAbsolutePath>
                        <pub:sizeOfDataChunkDownload>-1</pub:sizeOfDataChunkDownload>
                     </pub:reportRequest>
                     <pub:appParams></pub:appParams>
                  </pub:runReport>
               </soap:Body>
            </soap:Envelope>";
        return fetchReportEnvelope;
    }
    #endregion

    #region Helpers
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AddressType
    {
        BILL_TO,
        SHIP_TO
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum BusinessUnit
    {
        KYMETA,
        KGS,
        KYMETAKGS
    }

    public static readonly Dictionary<string, string> AddressSetIds = new()
    {
        { "kymeta", "300000001127004" },
        { "kgs", "300000001127005" },
        { "kymetakgs", "300000104202523" }
    };

    public static readonly Dictionary<string, string> CountryShortcodes = new()
    {
        { "Afghanistan", "AF" },
        { "Aland Islands", "AX" },
        { "Albania", "AL" },
        { "Algeria", "DZ" },
        //{ "", "AS" },
        { "Andorra", "AD" },
        { "Angola", "AO" },
        { "Anguilla", "AI" },
        { "Antarctica", "AQ" },
        { "Antigua and Barbuda", "AG" },
        { "Argentina", "AR" },
        { "Armenia", "AM" },
        { "Aruba", "AW" },
        { "Australia", "AU" },
        { "Austria", "AT" },
        { "Azerbaijan", "AZ" },
        { "Bahamas", "BS" },
        { "Bahrain", "BH" },
        { "Bangladesh", "BD" },
        { "Barbados", "BB" },
        { "Belarus", "BY" },
        { "Belgium", "BE" },
        { "Belize", "BZ" },
        { "Benin", "BJ" },
        { "Bermuda", "BM" },
        { "Bhutan", "BT" },
        { "Bolivia, Plurinational State of", "BO" },
        //{ "Bonaire, Sint Eustatius and Saba", "BQ" },
        { "Bosnia and Herzegovina", "BA" },
        { "Botswana", "BW" },
        { "Bouvet Island", "BV" },
        { "Brazil", "BR" },
        { "British Indian Ocean Territory", "IO" },
        { "Brunei Darussalam", "BN" },
        { "Bulgaria", "BG" },
        { "Burkina Faso", "BF" },
        { "Burundi", "BI" },
        { "Cabo Verde", "CV" },
        { "Cambodia", "KH" },
        { "Cameroon", "CM" },
        { "Canada", "CA" },
        //{ "Cape Verde", "CV" },
        { "Caribbean Netherlands", "BQ" },
        { "Cayman Islands", "KY" },
        { "Central African Republic", "CF" },
        { "Chad", "TD" },
        { "Chile", "CL" },
        { "China", "CN" },
        { "Christmas Island", "CX" },
        { "Cocos (Keeling) Islands", "CC" },
        { "Colombia", "CO" },
        { "Comoros", "KM" },
        { "Congo", "CG" },
        { "Congo, the Democratic Republic of the", "CD" },
        { "Cook Islands", "CK" },
        { "Costa Rica", "CR" },
        { "Cote d’Ivoire", "CI" },
        { "Croatia", "HR" },
        { "Cuba", "CU" },
        { "Curaçao", "CW" },
        { "Cyprus", "CY" },
        { "Czech Republic", "CZ" },
        { "Denmark", "DK" },
        { "Djibouti", "DJ" },
        { "Dominica", "DM" },
        { "Dominican Republic", "DO" },
        { "Ecuador", "EC" },
        { "Egypt", "EG" },
        { "El Salvador", "SV" },
        { "Equatorial Guinea", "GQ" },
        { "Eritrea", "ER" },
        { "Estonia", "EE" },
        { "Eswatini", "SZ" },
        { "Ethiopia", "ET" },
        //{ "", "GB" },
        { "Falkland Islands (Malvinas)", "FK" },
        { "Faroe Islands", "FO" },
        { "Fiji", "FJ" },
        { "Finland", "FI" },
        { "France", "FR" },
        { "French Guiana", "GF" },
        { "French Polynesia", "PF" },
        { "French Southern Territories", "TF" },
        { "Gabon", "GA" },
        { "Gambia", "GM" },
        { "Georgia", "GE" },
        { "Germany", "DE" },
        { "Ghana", "GH" },
        { "Gibraltar", "GI" },
        { "Greece", "GR" },
        { "Greenland", "GL" },
        { "Grenada", "GD" },
        { "Guadeloupe", "GP" },
        //{ "", "GU" },
        { "Guatemala", "GT" },
        { "Guernsey", "GG" },
        { "Guinea", "GN" },
        { "Guinea-Bissau", "GW" },
        { "Guyana", "GY" },
        { "Haiti", "HT" },
        { "Heard Island and McDonald Islands", "HM" },
        { "Holy See (Vatican City State)", "VA" },
        { "Honduras", "HN" },
        { "Hong Kong", "HK" },
        { "Hungary", "HU" },
        { "Iceland", "IS" },
        { "India", "IN" },
        { "Indonesia", "ID" },
        { "Iran, Islamic Republic of", "IR" },
        { "Iraq", "IQ" },
        { "Ireland", "IE" },
        { "Isle of Man", "IM" },
        { "Israel", "IL" },
        { "Italy", "IT" },
        { "Jamaica", "JM" },
        { "Japan", "JP" },
        { "Jersey", "JE" },
        { "Jordan", "JO" },
        { "Kazakhstan", "KZ" },
        { "Kenya", "KE" },
        { "Kiribati", "KI" },
        { "Korea, Democratic People’s Republic of", "KP" },
        { "Korea, Republic of", "KR" },
        { "Kosovo", "XK" },
        { "Kuwait", "KW" },
        { "Kyrgyzstan", "KG" },
        { "Lao People’s Democratic Republic", "LA" },
        { "Latvia", "LV" },
        { "Lebanon", "LB" },
        { "Lesotho", "LS" },
        { "Liberia", "LR" },
        { "Libya", "LY" },
        { "Liechtenstein", "LI" },
        { "Lithuania", "LT" },
        { "Luxembourg", "LU" },
        { "Macao", "MO" },
        { "Macedonia, the former Yugoslav Republic of", "MK" },
        { "Madagascar", "MG" },
        { "Malawi", "MW" },
        { "Malaysia", "MY" },
        { "Maldives", "MV" },
        { "Mali", "ML" },
        { "Malta", "MT" },
        //{ "", "MH" },
        { "Martinique", "MQ" },
        { "Mauritania", "MR" },
        { "Mauritius", "MU" },
        { "Mayotte", "YT" },
        { "Mexico", "MX" },
        //{ "", "FM" },
        { "Moldova, Republic of", "MD" },
        { "Monaco", "MC" },
        { "Mongolia", "MN" },
        { "Montenegro", "ME" },
        { "Montserrat", "MS" },
        { "Morocco", "MA" },
        { "Mozambique", "MZ" },
        { "Myanmar", "MM" },
        { "Namibia", "NA" },
        { "Nauru", "NR" },
        { "Nepal", "NP" },
        { "Netherlands", "NL" },
        //{ "", "AN" },
        { "New Caledonia", "NC" },
        { "New Zealand", "NZ" },
        { "Nicaragua", "NI" },
        { "Niger", "NE" },
        { "Nigeria", "NG" },
        { "Niue", "NU" },
        { "Norfolk Island", "NF" },
        { "North Macedonia", "MK" },
        //{ "", "MP" },
        { "Norway", "NO" },
        { "Oman", "OM" },
        { "Pakistan", "PK" },
        //{ "", "PW" },
        { "Palestine", "PS" },
        { "Panama", "PA" },
        { "Papua New Guinea", "PG" },
        { "Paraguay", "PY" },
        { "Peru", "PE" },
        { "Philippines", "PH" },
        { "Pitcairn", "PN" },
        { "Poland", "PL" },
        { "Portugal", "PT" },
        //{ "", "PR" },
        { "Qatar", "QA" },
        { "Reunion", "RE" },
        { "Romania", "RO" },
        { "Russian Federation", "RU" },
        { "Rwanda", "RW" },
        { "Saint Barthélemy", "BL" },
        { "Saint Helena, Ascension and Tristan da Cunha", "SH" },
        { "Saint Kitts and Nevis", "KN" },
        { "Saint Lucia", "LC" },
        { "Saint Martin (French part)", "MF" },
        { "Saint Pierre and Miquelon", "PM" },
        { "Saint Vincent and the Grenadines", "VC" },
        { "Samoa", "WS" },
        { "San Marino", "SM" },
        { "Sao Tome and Principe", "ST" },
        { "Saudi Arabia", "SA" },
        { "Senegal", "SN" },
        { "Serbia", "RS" },
        { "Seychelles", "SC" },
        { "Sierra Leone", "SL" },
        { "Singapore", "SG" },
        { "Sint Maarten (Dutch part)", "SX" },
        { "Slovakia", "SK" },
        { "Slovenia", "SI" },
        { "Solomon Islands", "SB" },
        { "Somalia", "SO" },
        { "South Africa", "ZA" },
        { "South Georgia and the South Sandwich Islands", "GS" },
        //{ "South Sudan", "" },
        { "Spain", "ES" },
        { "Sri Lanka", "LK" },
        { "Sudan", "SD" },
        { "Suriname", "SR" },
        { "Svalbard and Jan Mayen", "SJ" },
        //{ "Swaziland", "SZ" },
        { "Sweden", "SE" },
        { "Switzerland", "CH" },
        { "Syrian Arab Republic", "SY" },
        { "Taiwan", "TW" },
        { "Tajikistan", "TJ" },
        { "Tanzania, United Republic of", "TZ" },
        { "Thailand", "TH" },
        { "Timor-Leste", "TL" },
        { "Togo", "TG" },
        { "Tokelau", "TK" },
        { "Tonga", "TO" },
        { "Trinidad and Tobago", "TT" },
        { "Tunisia", "TN" },
        { "Turkey", "TR" },
        { "Turkmenistan", "TM" },
        { "Turks and Caicos Islands", "TC" },
        { "Tuvalu", "TV" },
        { "Uganda", "UG" },
        { "Ukraine", "UA" },
        { "United Arab Emirates", "AE" },
        { "United Kingdom", "GB" },
        { "United States", "US" },
        //{ "", "UM" },
        { "Uruguay", "UY" },
        { "Uzbekistan", "UZ" },
        { "Vanuatu", "VU" },
        { "Venezuela, Bolivarian Republic of", "VE" },
        { "Vietnam", "VN" },
        { "Virgin Islands, British", "VG" },
        //{ "", "VI" },
        { "Wallis and Futuna", "WF" },
        { "Western Sahara", "EH" },
        { "Yemen", "YE" },
        { "Zambia", "ZM" },
        { "Zimbabwe", "ZW" },

    };

    // acceptable value map for Account Type
    public static readonly Dictionary<string, string> CustomerTypeMap = new()
    {
        { "Consultant", "CONSULTANT" },
        { "End Customer", "ENDCUSTOMER" },
        { "External", "R" },
        { "Internal", "I" },
        { "Internal Dept", "INTERNALDEPT" },
        { "Partner", "PARTNER" },
        { "Partner-Terminated", "PARTNERTERMED" },
        { "Partner Affiliate", "PARTNERAFFILIATE" },
        { "Prospect", "PROSPECT" },
        { "Regulatory Organization", "REGULATORY" },
        { "SubDistributor", "SUBDISTRIBUTOR" },
        { "Supplier", "SUPPLIER" },
        { "Other", "OTHER" }
    };

    // must create a full dictionary to match values from Salesforce
    public static List<string> GetResponsibilityType(string role)
    {
        List<string> responsibilityTypes = new();
        if (string.IsNullOrEmpty(role)) return responsibilityTypes;
        role = role.ToLower();
        if (role.Contains("primary")) responsibilityTypes.Add("SFDCPRIMARY");
        if (role.Contains("bill")) responsibilityTypes.Add("BILL_TO");
        if (role.Contains("ship")) responsibilityTypes.Add("SHIP_TO");
        return responsibilityTypes;
    }

    public static string DecodeEncodedNonAsciiCharacters(string value)
    {
        return Regex.Replace(
            value,
            @"\\u(?<Value>[a-zA-Z0-9]{4})",
            m => {
                return ((char)int.Parse(m.Groups["Value"].Value, NumberStyles.HexNumber)).ToString();
            });
    }
    #endregion
}
