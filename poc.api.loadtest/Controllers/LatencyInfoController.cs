using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestSharp;
using System;



namespace poc.api.loadtest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LatencyInfoController : ControllerBase
    {
        private ILogger<LatencyInfoController> _logger;
        public LatencyInfoController(ILogger<LatencyInfoController> logger)
        {
            _logger = logger;
        }

        // GET: LatencyInfoController
        [HttpGet("/simple")]
        public ActionResult Get()
        {
            var response = new latencyInfoRs()
            {                
                dateTimeServer = DateTime.Now.ToString(),
                ipClient = this.Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                randomInt = new Random().Next(100)
            };
            _logger.LogInformation($"Get LatencyInfo > { response }");
            return Ok(response);
        }

        [HttpGet("/proxy")]
        public ActionResult GetProxy()
        {
            var uriAuthorizationStatus = new System.Uri("https://sb.openapis.itau.cl/public/sb/latencyinfo");
            var client = new RestClient(uriAuthorizationStatus);
            var request = new RestRequest("/", Method.GET, DataFormat.Json);
            var response = client.Get(request);

            return new JsonResult(response.Content);            
        }
    }

    public class latencyInfoRs
    {
        public string dateTimeServer { get; set; }
        public string ipClient { get; set; }
        public int randomInt { get; set; }
    }


}
