using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using poc.api.loadtest.Controllers;
using System.Net.Http;
using poc.api.loadtest.Models;

namespace poc.api.loadtest.test
{
    public class Tests
    {
        private LatencyInfoController _latencyInfoController;

        [SetUp]
        public void Setup()
        {
            var mocklog = new Mock<ILogger>();
            var mockConfig = new Mock<IConfiguration>();
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _latencyInfoController = new LatencyInfoController(mocklog.Object, mockConfig.Object, mockHttpClientFactory.Object);
        }

        [Test]
        public void Get()
        {
            var response = _latencyInfoController.Get();
            Assert.IsInstanceOf(typeof(latencyInfoRs), response);
            Assert.Pass();
        }
    }
}