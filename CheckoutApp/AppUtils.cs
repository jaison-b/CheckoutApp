using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;

namespace CheckoutApp
{
    internal static class AppUtils
    {
        /// <summary>
        ///     Parses csv file to return list of required types
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inputStream"></param>
        /// <param name="csvClassMap"></param>
        /// <returns></returns>
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