using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using poc.api.loadtest.Controllers;

namespace poc.api.loadtest.test
{
    public class FibonacciControllerTest
    {
        private FibonacciController _fibonacciController;

        [SetUp]
        public void Setup()
        {
            var mocklog = new Mock<ILogger>();
            var mockConfig = new Mock<IConfiguration>();
            _fibonacciController = new FibonacciController(mocklog.Object, mockConfig.Object);
        }

        [Test]
        public void Fibonacci()
        {            
            var response = _fibonacciController.Get(10);
            Assert.AreEqual(55, response);
        }
    }
}