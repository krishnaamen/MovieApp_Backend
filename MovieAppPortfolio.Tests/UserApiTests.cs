using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MovieAppPortfolio;
using Xunit;

namespace MovieAppPortfolio.Tests
{
    public class UserApiTests
    {
        [Fact]
        public async Task ShouldReturnTrue_WhenRegisterAndLoginWork()
        {
            // Fake server says everything is OK
            var fakeHandler = new FakeHandler(HttpStatusCode.OK);
            var client = new HttpClient(fakeHandler)
            {
                BaseAddress = new Uri("http://localhost:5210/")
            };

            var userTest = new UserTest(client);
            var result = await userTest.RegisterAndLoginAsync();

            Assert.True(result); // both worked
        }

        [Fact]
        public async Task ShouldReturnFalse_WhenRegisterFails()
        {
            // Fake server says Bad Request
            var fakeHandler = new FakeHandler(HttpStatusCode.BadRequest);
            var client = new HttpClient(fakeHandler)
            {
                BaseAddress = new Uri("http://localhost:5210/")
            };

            var userTest = new UserTest(client);
            var result = await userTest.RegisterAndLoginAsync();

            Assert.False(result); // register failed
        }
    }

    //   (OK or BadRequest)
    public class FakeHandler : HttpMessageHandler
    {
        private readonly HttpStatusCode _status;

        public FakeHandler(HttpStatusCode status)
        {
            _status = status;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage(_status));
        }
    }
}
