using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MovieAppPortfolio;
using Xunit;

namespace MovieAppPortfolio.Tests
{
    public class UserApiTests
    {
        [Fact]
        public async Task RegisterAndLoginAsync_ShouldReturnTrue_WhenBothRequestsSucceed()
        {
            // Arrange
            var handler = new FakeHttpMessageHandler(HttpStatusCode.OK);
            var client = new HttpClient(handler)
            {
                BaseAddress = new Uri("http://localhost:5210/")
            };

            var apiClient = new UserTest(client);

            // Act
            var result = await apiClient.RegisterAndLoginAsync();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task RegisterAndLoginAsync_ShouldReturnFalse_WhenRegisterFails()
        {
            // Arrange
            var handler = new FakeHttpMessageHandler(HttpStatusCode.BadRequest);
            var client = new HttpClient(handler)
            {
                BaseAddress = new Uri("http://localhost:5210/")
            };

            var apiClient = new UserTest(client);

            // Act
            var result = await apiClient.RegisterAndLoginAsync();

            // Assert
            Assert.False(result);
        }
    }

    //  HTTP responses
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpStatusCode _statusCode;

        public FakeHttpMessageHandler(HttpStatusCode statusCode)
        {
            _statusCode = statusCode;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage(_statusCode));
        }
    }
}
