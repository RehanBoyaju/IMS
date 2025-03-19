using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace IMS.WebUI.Hubs
{
    public class HubConnectionService
    {
        private readonly HubConnection hubConnection;
        public HubConnectionService(NavigationManager navigationManager)
        {
            hubConnection = new HubConnectionBuilder().WithUrl(navigationManager.ToAbsoluteUri("/communicationHub")).Build();
            hubConnection.StartAsync();
        }

        public HubConnection GetHubConnection() => hubConnection;
    }
}
