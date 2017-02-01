using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;

namespace CheckoutApp
{
    internal static class AppUtils
    {
        public static IList<T> ParseCsv<T>(Stream inputStream, Type csvClassMap)
        {
            var csv = new CsvReader(new StreamReader(inputStream));
            csv.Configuration.TrimFields = true;
            csv.Configuration.TrimHeaders = true;
            csv.Configuration.WillThrowOnMissingField = false;
            csv.Configuration.RegisterClassMap(csvClassMap);
            return csv.GetRecords<T>().ToList();
        }
    }
}