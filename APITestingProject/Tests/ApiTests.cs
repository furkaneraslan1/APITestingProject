using FluentAssertions;
using RestSharp;
using System.Net;
using Xunit;

namespace APITestingProject.Tests
{
    public class ApiTests : ApiTestBase
    {
        [Fact]
        public async Task GetPosts_ShouldReturnPosts()
        {
            var request = new RestRequest("posts", Method.Get);

            // Act
            RestResponse response = await Client.ExecuteAsync(request);
            
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Should().NotBeNull();
        }

        [Fact]
        public async Task CreatePost_ShouldReturnCreatedPost()
        {
            var request = new RestRequest("posts", Method.Post);
            request.AddJsonBody(new
            {
                title = "myfirsttitle",
                body = "myfirstbar",
                userId = 1
            });

            RestResponse response = await Client.ExecuteAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Content.Should().Contain("\"title\": \"myfirsttitle\"");
        }

        [Fact]
        public async Task UpdatePost_ShouldReturnUpdatedPost()
        {
            var request = new RestRequest("posts/1", Method.Put);
            request.AddJsonBody(new
            {
                id = 1,
                title = "updated title",
                body = "updated body",
                userId = 1
            });

            RestResponse response = await Client.ExecuteAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Should().Contain("\"title\": \"updated title\"");
        }

        [Fact]
        public async Task DeletePost_ShouldReturnNoContent()
        {
            var request = new RestRequest("posts/1", Method.Delete);

            RestResponse response = await Client.ExecuteAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK); // JSONPlaceholder returns 200 OK, doesn't actually delete the post
        }

        [Fact]
        public async Task GetNonExistingPost_ShouldReturnNotFound()
        {
            var request = new RestRequest("posts/9999", Method.Get);

            RestResponse response = await Client.ExecuteAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound); 
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetPostById_ShouldReturnCorrectPost(int postId)
        {
            var request = new RestRequest($"posts/{postId}", Method.Get);

            RestResponse response = await Client.ExecuteAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Should().Contain($"\"id\": {postId}");
        }

        [Theory]
        [InlineData("Title 1", "Body 1", 1)]
        [InlineData("Title 2", "Body 2", 2)]
        [InlineData("Title 3", "Body 3", 3)]
        public async Task CreatePosts_ShouldReturnCreatedPosts(string title, string body, int userId)
        {
            var request = new RestRequest("posts", Method.Post);
            request.AddJsonBody(new
            {
                title,
                body,
                userId
            });

            RestResponse response = await Client.ExecuteAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Content.Should().Contain($"\"title\": \"{title}\"");
        }
    }
}
