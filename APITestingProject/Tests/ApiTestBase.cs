using RestSharp;
using Serilog;


namespace APITestingProject.Tests
{
    public abstract class ApiTestBase
    {
        protected RestClient Client { get; private set; }
        protected string BaseUrl { get; } = "https://jsonplaceholder.typicode.com";
        protected ILogger Logger { get; private set; }

        public ApiTestBase()
        {
            Client = new RestClient(BaseUrl);

            // Set up the logger
            Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/testlog.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Logger.Information("Starting test setup");
        }

        protected async Task<RestResponse> ExecuteWithLoggingAsync(RestRequest request)
        {
            // Log the request
            Logger.Information("Executing request: {Method} {Resource}", request.Method, request.Resource);

            // Execute the request
            var response = await Client.ExecuteAsync(request);

            // Log the response
            Logger.Information("Response received: {StatusCode} - {Content}", response.StatusCode, response.Content);

            return response;
        }

        public void Dispose()
        {
            Logger.Information("Test execution finished");
        }
    }
}
