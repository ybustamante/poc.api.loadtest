using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace poc.api.loadtest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LatencyInfoController : ControllerBase
    {
        private ILogger<LatencyInfoController> _logger;
        private IHttpClientFactory _clientFactory;

        public LatencyInfoController(ILogger<LatencyInfoController> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
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

        [HttpGet("/proxy/stateless")]
        public ActionResult GetProxy()
        {
            var uriLatencyInfo = new Uri("https://sb.openapis.itau.cl/public/sb/latencyinfo");
            var client = new RestClient(uriLatencyInfo);
            var request = new RestRequest("/", Method.GET, DataFormat.Json);
            var response = client.Get(request);

            return new JsonResult(response.Content);            
        }

        [HttpGet("/proxy/clientfactory")]
        public async Task<ActionResult> GetProxySingletonAsync()
        {
            var uriLatencyInfo = new System.Uri("https://sb.openapis.itau.cl/public/sb/latencyinfo");
            var request = new HttpRequestMessage(HttpMethod.Get, uriLatencyInfo);
            request.Headers.Add("Accept", "application/json");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            _logger.LogInformation("Get Proxy Singleton Async " + response);
            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStringAsync();

                return Ok(responseStream);
            }
            else
            {
                return new StatusCodeResult(502);
            }            
        }
    }

    public class latencyInfoRs
    {
        public string dateTimeServer { get; set; }
        public string ipClient { get; set; }
        public int randomInt { get; set; }
    }


}
