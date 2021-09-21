using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using poc.api.loadtest.Models;
using RestSharp;
using System;
using System.Net.Http;
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

        public LatencyInfoController(ILogger<LatencyInfoController> logger, IHttpClientFactory clientFactory)
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
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetProxy()
        {
            var uriLatencyInfo = new Uri("https://sb.openapis.itau.cl/public/sb/latencyinfo");
            var client = new RestClient(uriLatencyInfo);
            var request = new RestRequest("/", Method.GET, DataFormat.Json);
            var response = await client.ExecuteAsync(request);

            return new JsonResult(response.Content);            
        }

        [HttpGet("/proxy/clientfactory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        public async Task<latencyInfoAPIConnect> GetProxySingletonAsync()
        {
            var uriLatencyInfo = new Uri("https://sb.openapis.itau.cl/public/sb/latencyinfo");
            var request = new HttpRequestMessage(HttpMethod.Get, uriLatencyInfo);
            request.Headers.Add("Accept", "application/json");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            _logger.LogInformation("Get Proxy Singleton Async " + response);

            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                var resultOk = await JsonSerializer.DeserializeAsync<latencyInfoAPIConnect>(responseStream);

                return resultOk;
            }
            else
            {
                Response.StatusCode = 502;
                return null;
            }            
        }
    }

    public class latencyInfoRs
    {
        public string dateTimeServer { get; set; }
        public string ipClient { get; set; }
        public int randomInt { get; set; }
    }

    public class latencyInfoAPIConnect
    {
        public string server { get; set; }
        public DateTime dateTimeServer { get; set; }
    }
}
