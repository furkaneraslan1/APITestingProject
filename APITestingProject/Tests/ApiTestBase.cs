using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using RestSharp;
using System.Diagnostics;
using System.Runtime.CompilerServices;


namespace APITestingProject.Tests
{
    public abstract class ApiTestBase
    {
        protected RestClient Client { get; private set; }
        protected string BaseUrl { get; } = "https://jsonplaceholder.typicode.com";

        protected ExtentReports Extent;
        protected ExtentTest Test;

        public ApiTestBase(ExtentReportsFixture fixture)
        {
            // Initialize RestClient with base URL
            Client = new RestClient(BaseUrl);
            Extent = fixture.Extent;
        }

        protected async Task<RestResponse> ExecuteWithLoggingAsync(RestRequest request, [CallerMemberName] string testName = null)
        {
            // Automatically create a new ExtentTest for the calling test method
            Test = Extent.CreateTest(testName);

            // Log request details
            Test.Log(Status.Info, $"Sending {request.Method} request to {request.Resource}");
            Test.Log(Status.Info, $"Request Body: {request.Parameters.FirstOrDefault(p => p.Type == ParameterType.RequestBody)?.Value}");

            var stopwatch = Stopwatch.StartNew();
            RestResponse response;

            try
            {
                // Execute the API request
                response = await Client.ExecuteAsync(request);
                stopwatch.Stop();

                // Log performance and response details
                Test.Log(Status.Info, $"Response Time: {stopwatch.ElapsedMilliseconds} ms");
                Test.Log(Status.Info, $"Response Status: {response.StatusCode}");
                Test.Log(response.IsSuccessful ? Status.Pass : Status.Fail, $"Response: {response.StatusCode} - {response.Content}");
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                // Log any exceptions that occur
                Test.Log(Status.Error, $"Request error after {stopwatch.ElapsedMilliseconds} ms with Exception: {ex.Message}");
                throw;
            }

            return response;
        }
    }
}
