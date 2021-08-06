using Microsoft.AspNetCore.Mvc;

namespace poc.api.loadtest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NumberController : Controller
    {
        [HttpGet]
        public RsIntegerTest Get(long position)
        {
            return new RsIntegerTest()
            {
                integer32 = 132132,
                integer64 = position
            };
        }
    }


    public class RsIntegerTest
    {
        public int integer32 { get; set; }

        public long integer64 { get; set; }
    }
}
