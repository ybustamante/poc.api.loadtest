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
            var calculatePosition = FibonacciController.CalculateFibonacci(2);
            Assert.Pass();
        }
    }
}