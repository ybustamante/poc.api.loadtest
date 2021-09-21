using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace poc.api.loadtest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FibonacciController : ControllerBase
    {
        private ILogger<FibonacciController> _logger;
        private Dictionary<int, int> _dictionary = new Dictionary<int, int>();
        public static IConfiguration Configuration { get; private set; }
        public FibonacciController(ILogger<FibonacciController> logger, IConfiguration configuration)
        {
            _logger = logger;
            Configuration = configuration;
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
            return Configuration["enable_hack"] == "true" ? CalculateFibonacciHack(position) : CalculateFibonacciRecursive(position);
        }

        private int CalculateFibonacciRecursive(int position)
        {            
            if (position == 0 || position == 1)
            {
                return position;
            }
            else if (position < 0)
            {                
                throw new System.Exception("The number must be greater than 1");
            }
            else
            {
                var result = CalculateFibonacciRecursive(position - 1) + CalculateFibonacciRecursive(position - 2);                
                return result;
            }
        }

        private int CalculateFibonacciHack(int position)
        {            
            if (position == 0 || position == 1)
            {
                return position;
            }
            else if (position < 0)
            {
                throw new System.Exception("The number must be greater than 1");
            }
            else
            {
                if (_dictionary.ContainsKey(position))
                {
                    return _dictionary.GetValueOrDefault(position);
                }
                else
                {
                    var result = CalculateFibonacciHack(position - 1) + CalculateFibonacciHack(position - 2);
                    _dictionary.Add(position, result);
                    return result;
                }
            }
        }

    }
}
