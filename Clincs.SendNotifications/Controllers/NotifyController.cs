using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Clincs.SendNotifications.Controllers
{
    [Route("api/notify")]
    [ApiController]
    public class NotifyController : ControllerBase
    {
        private HttpClient _httpClient;


        public NotifyController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Get()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "");

            // Call the todolist service.
            var response = await _httpClient.GetAsync("http://localhost:44385/api/notify");
            return Ok(response);
        }

       
    }
}
