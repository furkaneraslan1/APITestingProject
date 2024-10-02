using FluentAssertions;
using Moq;
using RestSharp;
using System.Net;
using Xunit;

namespace APITestingProject.Tests
{
    public class ApiTestsWithMocking
    {
        [Fact]
        public async Task GetPosts_ShouldReturnMockedResponse()
        {
            // Arrange
            var mockClient = new Mock<IRestClient>();
            var mockRequest = new RestRequest("posts", Method.Get);

            var mockResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = "[{\"id\":1, \"title\":\"Mocked Post\", \"body\":\"This is a mocked response.\"}]"
            };

            mockClient.Setup(client => client.ExecuteAsync(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(mockResponse);

            // Act
            var response = await mockClient.Object.ExecuteAsync(mockRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Should().Contain("Mocked Post");
        }
    }
}
