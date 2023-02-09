using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.SOAP;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Services;

public static class Helpers
{
    // fetch a value from a Dictionary based on the key
    public static TV GetValue<TK, TV>(this IDictionary<TK, TV> dict, TK key, TV defaultValue = default(TV))
    {
        return dict.TryGetValue(key, out TV value) ? value : defaultValue;
    }

    public static List<OraclePartySiteUse> RemapAddressTypeToOracleSiteUse(SalesforceAddressModel address)
    {
        // if no Address, then return empty list
        if (address == null) return new List<OraclePartySiteUse>();

        // calculate SiteUseTypes (there can be multiple purposes (billing & shipping)
        var decodedType = OracleSoapTemplates.DecodeEncodedNonAsciiCharacters(address.Type);
        var siteUseTypes = new List<OraclePartySiteUse>();
        switch (decodedType.ToLower())
        {
            case "billing & shipping":
                siteUseTypes.Add(new OraclePartySiteUse { SiteUseType = OracleSoapTemplates.AddressType.BILL_TO.ToString() });
                siteUseTypes.Add(new OraclePartySiteUse { SiteUseType = OracleSoapTemplates.AddressType.SHIP_TO.ToString() });
                break;
            case "shipping":
                siteUseTypes.Add(new OraclePartySiteUse { SiteUseType = OracleSoapTemplates.AddressType.SHIP_TO.ToString() });
                break;
            default:
                break;
        }
        return siteUseTypes;
    }

    /// <summary>
    /// Find all indexes in a string.
    /// </summary>
    /// <param name="str">The string to process</param>
    /// <param name="value">The value to match within the string.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static List<int> AllIndexesOf(this string str, string value)
    {
        if (string.IsNullOrEmpty(value)) throw new ArgumentException("The string to find may not be empty", nameof(value));
        List<int> indexes = new();
        for (int index = 0; ; index += value.Length)
        {
            index = str.IndexOf(value, index);
            if (index == -1) return indexes;
            indexes.Add(index);
        }
    }
}