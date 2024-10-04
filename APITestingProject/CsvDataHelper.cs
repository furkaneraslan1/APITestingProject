using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace APITestingProject
{
    public static class CsvDataHelper
    {
        public static IEnumerable<object[]> GetTestDataFromCsv(string filePath)
        {
            //var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            //{
            //    Delimiter = ";", // Specify semicolon as the delimiter
            //};

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<TestData>().ToList();
                foreach (var record in records)
                {
                    yield return new object[] { record.Title, record.Body, record.UserId };
                }
            }
        }
    }

    public class TestData
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public int UserId { get; set; }
    }
}