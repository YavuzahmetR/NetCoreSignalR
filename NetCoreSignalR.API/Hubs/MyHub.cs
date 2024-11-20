using Microsoft.AspNetCore.SignalR;

namespace NetCoreSignalR.API.Hubs
{
    public class MyHub : Hub<IMyHub>
    {

        public async Task BroadcastMessageToAllClients(string message)
        {
            await Clients.All.ReceiveMessageForAllClients(message);
        }

    }
}
