﻿using Microsoft.AspNetCore.SignalR;

namespace IMS.WebUI.Hubs
{
    public class CommunicationHub : Hub
    {
        public async Task Notification(string clientId)
        {
            await Clients.All.SendAsync("Notify", clientId);
        }
    }
}
