using Kymeta.Cloud.Services.EnterpriseBroker.Models;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.REST;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.SOAP.ResponseModels;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Kymeta.Cloud.Services.EnterpriseBroker.HttpClients;

public interface IOracleClient
{
    Task<Tuple<OracleOrganizationResponse, string>> CreateOrganization(CreateOracleOrganizationModel model);
    Task<Tuple<OracleOrganizationResponse, string>> UpdateOrganization(UpdateOracleOrganizationModel model, ulong partyNumber);
    Task<Tuple<OracleAddressObject, string>> CreateAddress(string accountNumber, CreateOracleAddressViewModel model);
    Task<Tuple<OracleAddressObject, string>> UpdateAddress(string accountNumber, CreateOracleAddressViewModel model, string partyNumber);
    Task<Tuple<XDocument, string, string>> SendSoapRequest(string soapEnvelope, string oracleServiceUrl, string? contentType = null);
}

public class OracleClient : IOracleClient
{
    private HttpClient _client;
    private ILogger<OracleClient> _logger;
    private IConfiguration _config;

    public OracleClient(HttpClient client, IConfiguration configuration, ILogger<OracleClient> logger)
    {
        client.BaseAddress = new Uri(configuration.GetValue<string>("Oracle:Endpoint"));
        // custom BasicAuthenticationHeaderValue class to wrap the encoding 
        client.DefaultRequestHeaders.Authorization = new BasicAuthenticationHeaderValue(configuration.GetValue<string>("Oracle:Username"), configuration.GetValue<string>("Oracle:Password"));
        _client = client;
        _config = configuration;

        _logger = logger;
    }

    /// <summary>
    /// Marshal a SOAP request to the designated Oracle URL.
    /// </summary>
    /// <param name="soapEnvelope"></param>
    /// <param name="oracleServiceUrl"></param>
    /// <returns></returns>
    public async Task<Tuple<XDocument, string, string>> SendSoapRequest(string soapEnvelope, string oracleServiceUrl, string? contentType = null)
    {
        try
        {
            // check for empty content
            if (string.IsNullOrEmpty(soapEnvelope)) return new Tuple<XDocument, string, string>(null, $"soapEnvelope is required.", null);
            if (string.IsNullOrEmpty(oracleServiceUrl)) return new Tuple<XDocument, string, string>(null, $"oracleServiceUrl is required.", null);

            // encode the XML envelope (payload)
            byte[] byteArray = Encoding.UTF8.GetBytes(soapEnvelope);

            // construct the base 64 encoded string used as credentials for the service call
            byte[] toEncodeAsBytes = Encoding.ASCII.GetBytes(_config["Oracle:Username"] + ":" + _config["Oracle:Password"]);
            string credentials = Convert.ToBase64String(toEncodeAsBytes);

            // TODO: check to see if we can re-use BasicAuthenticationHeaderValue (below) instead of encoding above
            //var creds = new BasicAuthenticationHeaderValue(_config.GetValue<string>("Oracle:Username"), _config.GetValue<string>("Oracle:Password"));

            // create HttpWebRequest connection to the service
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(oracleServiceUrl);

            // configure request headers
            request.Method = "POST";
            request.ContentType = contentType ?? "text/xml;charset=UTF-8";
            request.ContentLength = byteArray.Length;

            // configure the request to use basic authentication, with base64 encoded user name and password
            request.Headers.Add("Authorization", $"Basic {credentials}");

            // write the xml payload to the request
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            var doc = new XDocument();
            // get the response and process it (print out the response XDocument doc)
            using (WebResponse response = await request.GetResponseAsync())
            {
                using Stream stream = response.GetResponseStream();
                doc = XDocument.Load(stream);
            }
            Console.WriteLine(doc);
            return new Tuple<XDocument, string, string>(doc, null, null);
        }
        catch (WebException wex)
        {
            using var stream = wex.Response?.GetResponseStream();
            using var reader = new StreamReader(stream);

            try
            {
                // deserialize the xml response envelope
                XmlSerializer serializer = new(typeof(FaultEnvelope));
                var result = (FaultEnvelope)serializer.Deserialize(reader);
                var faultMessage = result?.Body.Fault.faultstring;

                return new Tuple<XDocument, string, string>(null, wex.Message, faultMessage);
            }
            catch (Exception ex)
            {
                // additional catch-all in case the fault cannot be deserialized
                return new Tuple<XDocument, string, string>(null, ex.Message, null);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new Tuple<XDocument, string, string>(null, ex.Message, null);
        }
    }

    public async Task<Tuple<OracleOrganizationResponse, string>> CreateOrganization(CreateOracleOrganizationModel model)
    {
        var response = await _client.PostAsJsonAsync($"crmRestApi/resources/latest/accounts", model, new JsonSerializerOptions { PropertyNameCaseInsensitive = false });
        string data = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogCritical($"Failed CreateOrganization Oracle HTTP call: {(int)response.StatusCode} | {data} | Model sent: {JsonSerializer.Serialize(model)}");
            return new Tuple<OracleOrganizationResponse, string>(null, data);
        }

        var deserializedObject = JsonSerializer.Deserialize<OracleOrganizationResponse>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return new Tuple<OracleOrganizationResponse, string>(deserializedObject, null);
    }

    public async Task<Tuple<OracleAddressObject, string>> CreateAddress(string accountNumber, CreateOracleAddressViewModel model)
    {
        var response = await _client.PostAsJsonAsync($"crmRestApi/resources/latest/accounts/{accountNumber}/child/Address", model, new JsonSerializerOptions { PropertyNameCaseInsensitive = false });
        string data = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogCritical($"Failed AddAddress Oracle HTTP call: {(int)response.StatusCode} | {data} | Model sent: {JsonSerializer.Serialize(model)}");
            return new Tuple<OracleAddressObject, string>(null, data);
        }

        var deserializedObject = JsonSerializer.Deserialize<OracleAddressObject>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return new Tuple<OracleAddressObject, string>(deserializedObject, null);
    }

    public async Task<Tuple<OracleOrganizationResponse, string>> UpdateOrganization(UpdateOracleOrganizationModel model, ulong partyNumber)
    {
        var serialized = JsonSerializer.Serialize(model, new JsonSerializerOptions { PropertyNameCaseInsensitive = false });
        var content = new StringContent(serialized, Encoding.UTF8, "application/json");

        var response = await _client.PatchAsync($"crmRestApi/resources/latest/accounts/{partyNumber}", content);

        string data = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogCritical($"Failed UpdateAccount Oracle HTTP call: {(int)response.StatusCode} | {data} | Model sent: {JsonSerializer.Serialize(model)}");
            return new Tuple<OracleOrganizationResponse, string>(null, $"{(int)response.StatusCode} | {data}");
        }

        var deserializedObject = JsonSerializer.Deserialize<OracleOrganizationResponse>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return new Tuple<OracleOrganizationResponse, string>(deserializedObject, null);
    }

    public async Task<Tuple<OracleAddressObject, string>> UpdateAddress(string accountNumber, CreateOracleAddressViewModel model, string partyNumber)
    {
        var serialized = JsonSerializer.Serialize(model, new JsonSerializerOptions { PropertyNameCaseInsensitive = false });
        var content = new StringContent(serialized, Encoding.UTF8, "application/json");

        var response = await _client.PatchAsync($"crmRestApi/resources/latest/accounts/{accountNumber}/child/Address/{partyNumber}", content);

        string data = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogCritical($"Failed UpdateAddress Oracle HTTP call: {(int)response.StatusCode} | {data} | Model sent: {JsonSerializer.Serialize(model)}");
            return new Tuple<OracleAddressObject, string>(null, data);
        }

        var deserializedObject = JsonSerializer.Deserialize<OracleAddressObject>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return new Tuple<OracleAddressObject, string>(deserializedObject, null);
    }
}