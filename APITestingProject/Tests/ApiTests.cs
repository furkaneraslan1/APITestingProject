using FluentAssertions;
using RestSharp;
using System.Net;
using Xunit;

namespace APITestingProject.Tests
{
    public class ApiTests
    {
        private readonly string _baseUrl = "https://jsonplaceholder.typicode.com";

        [Fact]
        public async Task GetPosts_ShouldReturnPosts()
        {
            // Arrange
            var client = new RestClient(_baseUrl);
            var request = new RestRequest("posts", Method.Get);

            // Act
            RestResponse response = await client.ExecuteAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Should().NotBeNull();
        }

        [Fact]
        public async Task CreatePost_ShouldReturnCreatedPost()
        {
            // Arrange
            var client = new RestClient(_baseUrl);
            var request = new RestRequest("posts", Method.Post);
            request.AddJsonBody(new
            {
                title = "myfirsttitle",
                body = "myfirstbar",
                userId = 1
            });

            RestResponse response = await client.ExecuteAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Content.Should().Contain("\"title\": \"myfirsttitle\"");
        }
    }
}
