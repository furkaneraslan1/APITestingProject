using RestSharp;

namespace APITestingProject.Tests
{
    public abstract class ApiTestBase
    {
        protected RestClient Client { get; private set; }
        protected string BaseUrl { get; } = "https://jsonplaceholder.typicode.com";

        public ApiTestBase()
        {
            Client = new RestClient(BaseUrl);
        }
    }
}
