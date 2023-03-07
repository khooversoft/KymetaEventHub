using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;

namespace Kymeta.Cloud.Services.Toolbox.Serialization;

public static class CsvSerializer
{
    private static CsvConfiguration _config = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        //PrepareHeaderForMatch = args => args.Header.ToLower(),
        Delimiter = ",",
    };

    public static IReadOnlyList<T> DeserializeCsv<T>(this byte[]? bytes)
    {
        return bytes switch
        {
            null => Array.Empty<T>(),
            var v => read(v),
        };


        static IReadOnlyList<T> read(byte[] data)
        {
            try
            {
                using var memory = new MemoryStream(data);
                using var reader = new StreamReader(memory);
                using CsvReader csv = new(reader, _config);

                return csv.GetRecords<T>().ToArray();
            }
            catch
            {
                return Array.Empty<T>();
            }
        }
    }

    public static async Task<IReadOnlyList<T>> DeserializeCsv<T>(this Task<byte[]?> bytes)
    {
        return (await bytes) switch
        {
            null => Array.Empty<T>(),
            byte[] v => DeserializeCsv<T>(v),
        };
    }
}
