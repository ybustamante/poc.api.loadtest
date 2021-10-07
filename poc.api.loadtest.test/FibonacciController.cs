using NUnit.Framework;

namespace poc.api.loadtest.test
{
    public class FibonacciController
    {

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Fibonacci()
        {

            var service = new Mock<IService>();
            controller = new MyController(service.Object);

            service.Setup(service => service.GetAsync(1)).ReturnsAsync((MyType)null);

            var result = await controller.Get(1);

            Assert.That(result.Result, Is.TypeOf<NotFoundResult>());


            var calculatePosition = FibonacciController.CalculateFibonacci(2);
            Assert.Pass();
        }
    }
}