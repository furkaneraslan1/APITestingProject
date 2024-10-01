using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace APITestingProject
{
    public static class TestDataHelper
    {
        public class TestData
        {
            public string Title { get; set; }
            public string Body { get; set; }
            public int UserId { get; set; }
        }

        public static IEnumerable<object[]> GetTestData(string filePath)
        {
            var jsonData = File.ReadAllText(filePath);
            var testData = JsonConvert.DeserializeObject<List<TestData>>(jsonData);
            foreach (var data in testData)
            {
                yield return new object[] { data.Title, data.Body, data.UserId };
            }
        }


    }
}
