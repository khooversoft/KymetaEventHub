# Kymeta Enterprise Broker


This service is responsible for routing requests between Salesforce, Oracle and OSS.


The following base hosts are available:
- http://localhost:5098
- https://kymetacloudservicesenterprisebroker-integration.azurewebsites.net
- https://kymetacloudservicesenterprisebroker-validation.azurewebsites.net
- https://kymetacloudservicesenterprisebroker-staging.azurewebsites.net
- https://kymetacloudservicesenterprisebroker.azurewebsites.net

The API is hosted at /api/v1/broker/* where * refers to the entity type such as _account_ or _contact_.