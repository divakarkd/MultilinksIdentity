﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;


namespace Multilinks.ApiService.Hubs
{
    [Authorize]
    public class MainHub : Hub
    {
        /* Dummy example method */
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}