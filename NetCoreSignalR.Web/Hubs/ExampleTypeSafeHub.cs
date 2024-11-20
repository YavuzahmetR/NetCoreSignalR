using Microsoft.AspNetCore.SignalR;
using NetCoreSignalR.Web.Models;

namespace NetCoreSignalR.Web.Hubs
{
    public class ExampleTypeSafeHub:Hub<IExampleTypeSafeHub>
    {
        private static int ConnectedClientCount = 0;
        public async Task BroadcastMessageToAllClients(string message)
        {
            await Clients.All.ReceiveMessageForAllClients(message);
        }

        public async Task BroadcastTypedMessageToAllClients(Product product)
        {
            await Clients.All.ReceiveTypedMessageForAllClients(product);
        }

        public async Task BroadcastStreamDataToAllClients(IAsyncEnumerable<string> namesAsChunk)
        {
            await foreach (var name in namesAsChunk)
            {
                await Task.Delay(500);
                await Clients.All.ReceiveMessageAsStreamForAllClients(name);
            }
        }

        public async Task BroadcastStreamProductToAllClients(IAsyncEnumerable<Product> productAsChunks)
        {
            await foreach (var product in productAsChunks)
            {
                await Task.Delay(500);
                await Clients.All.ReceiveProductAsStreamForAllClients(product);
            }
        }

        public async IAsyncEnumerable<string> BroadcastFromHubToClientStream(int count)
        {
            foreach (var item in Enumerable.Range(1, count).ToList())
            {
                await Task.Delay(500);
                yield return $"{item}. data";
            }
        }

        public async Task BroadcastMessageToSpecifiedClient(string message)
        {
            await Clients.Caller.ReceiveMessageForSpecifiedClient(message);
        }

        public async Task BroadcastMessageToOtherClients(string message)
        {
            await Clients.Others.ReceiveMessageForOtherClients(message);
        }

        public async Task BroadcastMessageToIndivualClient(string connectionId, string message)
        {
            await Clients.Client(connectionId).ReceiveMessageForIndivualClient(message);
        }

        public async Task BroadcastMessageToGroupClients(string groupName, string message)
        {
            await Clients.Group(groupName).ReceiveMessageForGroupClients(message);
        }

        public async Task AddGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Caller.ReceiveMessageForSpecifiedClient($"Joined The Group {groupName}");
            await Clients.Group(groupName).ReceiveMessageForGroupClients
                ($"{Context.ConnectionId} Has Joined The Group {groupName} ");
        }

        public async Task RemoveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Caller.ReceiveMessageForSpecifiedClient($"Left The Group {groupName}");
            await Clients.Group(groupName).ReceiveMessageForGroupClients
                ($"{Context.ConnectionId} Has Left The Group {groupName} ");
        }


        public override async Task OnConnectedAsync()
        {
            ConnectedClientCount++;
            await Clients.All.ConnectedClientCountAllClients(ConnectedClientCount);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            ConnectedClientCount--;
            await Clients.All.ConnectedClientCountAllClients(ConnectedClientCount);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
