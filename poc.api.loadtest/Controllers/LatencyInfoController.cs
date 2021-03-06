using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using poc.api.loadtest.Models;
using RestSharp;
using System;
using System.Net.Http;
using System.Net.Mime;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace poc.api.loadtest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LatencyInfoController : ControllerBase
    {
        private ILogger<LatencyInfoController> _logger;
        private IHttpClientFactory _clientFactory;

        public LatencyInfoController(ILogger<LatencyInfoController> logger, IConfiguration configuration, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        // GET: LatencyInfoController
        [HttpGet("/simple")]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<string>> GetProxy()
        {
            var uriLatencyInfo = new Uri("https://sb.openapis.itau.cl/public/sb/latencyinfo");
            var client = new RestClient(uriLatencyInfo);
            var request = new RestRequest("/", Method.GET, DataFormat.Json);
            var response = await client.ExecuteAsync(request);

            return Ok(response.Content);
        }
                     
        [HttpPost("/proxy/clientfactory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        public async Task<ActionResult<string>> GetProxySingletonAsync(GetProxySingletonAsyncRs uri)
        {
            _logger.LogInformation("Get Proxy Singleton Async to URI " + uri);            
            var uriLatencyInfo = new Uri(uri.uri);
            var request = new HttpRequestMessage(HttpMethod.Get, uriLatencyInfo);
            request.Headers.Add("Accept", "application/json");

            var client = _clientFactory.CreateClient();

            var response = await client.GetAsync(uri.uri);

            _logger.LogInformation("Get Proxy Singleton Async " + response);

            if (response.IsSuccessStatusCode)
            {
                return Ok(response.Content);
            }
            else
            {
                Response.StatusCode = 502;
                return null;
            }
        }
    }

    public class latencyInfoAPIConnect
    {
        public string server { get; set; }
        public DateTime dateTimeServer { get; set; }
    }

    public class GetProxySingletonAsyncRs
    {
        public string uri { get; set; }
    }
}
