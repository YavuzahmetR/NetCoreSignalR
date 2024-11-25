﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NetCoreSignalR.API.Hubs;

namespace NetCoreSignalR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController(IHubContext<MyHub> hubContext) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get(string message)
        {
            await hubContext.Clients.All.SendAsync("ReceiveMessageForAllClients",message);
            return Ok();
        }

    }
}
