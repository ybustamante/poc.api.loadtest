using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace poc.api.loadtest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FibonacciController : ControllerBase
    {
        private ILogger<FibonacciController> _logger;
        public FibonacciController(ILogger<FibonacciController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        public int Get(int position)
        {
            try
            {
                _logger.LogInformation($"Fibonacci GET {position}");
                return CalculateFibonacci(position);
            }
            catch (System.Exception ex)
            {
                _logger.LogError("exception", ex);
                throw;
            }
            
        }

        private int CalculateFibonacci(int position)
        {
            _logger.LogInformation($"CalculateFibonacci= {position}");
            if (position == 0 || position == 1)
            {
                return position;
            }
            else if (position <0)
            {
                throw new System.Exception("The number must be greater than 1");
            }
            else
            {
                return CalculateFibonacci(position - 1) + CalculateFibonacci(position - 2);
            }
        }

    }
}
