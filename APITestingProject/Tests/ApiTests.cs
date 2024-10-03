using FluentAssertions;
using RestSharp;
using System.Net;
using Xunit;


namespace APITestingProject.Tests
{
    public class ApiTests : ApiTestBase, IClassFixture<ExtentReportsFixture>
    {

        public ApiTests(ExtentReportsFixture fixture) : base(fixture)
        {
        }

        public static IEnumerable<object[]> GetPostTestData()
        {
            return TestDataHelper.GetTestData("TestData/testdata.json");
        }

        [Fact]
        public async Task GetPosts_ShouldReturnPosts()
        {
                // Arrange
                var request = new RestRequest("posts", Method.Get);

                // Act
                RestResponse response = await ExecuteWithLoggingAsync(request);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                response.Content.Should().NotBeNull();
        }

        [Theory]
        [MemberData(nameof(GetPostTestData))]
        public async Task CreatePost_ShouldReturnCreatedPost(string title, string body, string userId)
        {
            var request = new RestRequest("posts", Method.Post);
            request.AddJsonBody(new
            {
                title,
                body,
                userId
            });

            RestResponse response = await ExecuteWithLoggingAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Content.Should().Contain($"\"title\": \"{title}\"");
            response.Content.Should().Contain($"\"body\": \"{body}\"");
            response.Content.Should().Contain($"\"userId\": \"{userId}\"");
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

            RestResponse response = await ExecuteWithLoggingAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Should().Contain("\"title\": \"updated title\"");
        }

        [Fact]
        public async Task DeletePost_ShouldReturnNoContent()
        {
            var request = new RestRequest("posts/1", Method.Delete);

            RestResponse response = await ExecuteWithLoggingAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK); // JSONPlaceholder returns 200 OK, doesn't actually delete the post
        }

        [Fact]
        public async Task GetNonExistingPost_ShouldReturnNotFound()
        {
            var request = new RestRequest("posts/9999", Method.Get);

            RestResponse response = await ExecuteWithLoggingAsync(request);

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

            RestResponse response = await ExecuteWithLoggingAsync(request);

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

            RestResponse response = await ExecuteWithLoggingAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Content.Should().Contain($"\"title\": \"{title}\"");
        }

        [Fact]
        public async Task GetProtectedResource_ShouldReturnOkWithValidToken()
        {
            var token = "valid_token";
            var request = new RestRequest("protected/resource", Method.Get);
            request.AddHeader("Authorization", $"Bearer {token}");

            // Act
            RestResponse response = await ExecuteWithLoggingAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetProtectedResource_ShouldReturnUnauthorizedWithInvalidToken()
        {
            var invalidToken = "invalid_token";
            var request = new RestRequest("protected/resource", Method.Get);
            request.AddHeader("Authorization", $"Bearer {invalidToken}");

            // Act
            RestResponse response = await ExecuteWithLoggingAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetProtectedResource_ShouldReturnUnauthorizedWithoutToken()
        {
            var request = new RestRequest("protected/resource", Method.Get);

            // Act
            RestResponse response = await ExecuteWithLoggingAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task CreatePost_InvalidData_ShouldReturnBadRequest()
        {
            var request = new RestRequest("posts", Method.Post);
            request.AddJsonBody(new
            {
                title = "",
                body = "",
                userId = 0
            });

            RestResponse response = await ExecuteWithLoggingAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetPosts_ServerError_ShouldReturnInternalServerError()
        {
            var request = new RestRequest("posts", Method.Get);

            RestResponse response = await ExecuteWithLoggingAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Fact]
       public async Task GetPosts_ShouldRespondWithinTimeLimit()
        {
            // Arrange
            var request = new RestRequest("posts", Method.Get);
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // Act
            var response = await Client.ExecuteAsync(request);
            stopwatch.Stop();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(100);
        }
    }
}
