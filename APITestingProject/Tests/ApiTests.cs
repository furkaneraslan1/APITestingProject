using FluentAssertions;
using RestSharp;
using System.Net;
using Xunit;

namespace APITestingProject.Tests
{
    public class ApiTests
    {
        private readonly string _baseUrl = "https://jsonplaceholder.typicode.com";
        private RestClient GetClient()
        {
            return new RestClient(_baseUrl);
        }

        [Fact]
        public async Task GetPosts_ShouldReturnPosts()
        {
            // Arrange
            var client = GetClient();
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
            var client = GetClient();
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

        [Fact]
        public async Task UpdatePost_ShouldReturnUpdatedPost()
        {
            var client = GetClient();
            var request = new RestRequest("posts/1", Method.Put);
            request.AddJsonBody(new
            {
                id = 1,
                title = "updated title",
                body = "updated body",
                userId = 1
            });

            RestResponse response = await client.ExecuteAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Should().Contain("\"title\": \"updated title\"");
        }

        [Fact]
        public async Task DeletePost_ShouldReturnNoContent()
        {
            var client = GetClient();
            var request = new RestRequest("posts/1", Method.Delete);

            RestResponse response = await client.ExecuteAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK); // JSONPlaceholder returns 200 OK, doesn't actually delete the post
        }

        [Fact]
        public async Task GetNonExistingPost_ShouldReturnNotFound()
        {
            var client = GetClient();
            var request = new RestRequest("posts/9999", Method.Get);

            RestResponse response = await client.ExecuteAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound); 
        }



    }
}
