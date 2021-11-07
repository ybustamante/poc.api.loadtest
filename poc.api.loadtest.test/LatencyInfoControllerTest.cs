using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using poc.api.loadtest.Controllers;
using System.Net.Http;
using poc.api.loadtest.Models;
using Moq.Protected;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace poc.api.loadtest.test
{
    public class LatencyInfoControllerTest
    {
        private IConfiguration _mockIConfiguration;
        private ILogger<LatencyInfoController> _mockILogger;

        [SetUp]
        public void Setup()
        {
            var mocklog = new Mock<ILogger<LatencyInfoController>>();
            var mockConfig = new Mock<IConfiguration>();
            _mockIConfiguration = mockConfig.Object;
            _mockILogger = mocklog.Object;
        }

        //[Test]
        //public void Get()
        //{
        //    var response = _latencyInfoController.Get();
        //    Assert.IsInstanceOf(typeof(latencyInfoRs), response);
        //    Assert.Pass();
        //}

        [Test]
        public async Task Should_Return_Ok()
        {
            var expectedMessage = "Hello world";
            var mockFactory = new Mock<IHttpClientFactory>();

            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(expectedMessage)
                });

            var httpClient = new HttpClient(mockMessageHandler.Object);

            mockFactory.Setup(o => o.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var controller = new LatencyInfoController(_mockILogger, _mockIConfiguration, mockFactory.Object);
            
            var actionResult = await controller.GetProxy();

            var result = actionResult.Result as ObjectResult;
            Assert.IsTrue(result.StatusCode ==  200);            
        }
    }
}