using Microsoft.AspNetCore.Mvc;

namespace poc.api.loadtest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FibonacciController : ControllerBase
    {
        [HttpGet]
        public int Get(int position)
        {

            return CalculateFibonacci(position);
        }

        private int CalculateFibonacci(int position)
        {
            if (position == 0 || position == 1)
            {
                return position;
            }           
            else
            {
                return CalculateFibonacci(position - 1) + CalculateFibonacci(position - 2);
            }
        }

    }
}
