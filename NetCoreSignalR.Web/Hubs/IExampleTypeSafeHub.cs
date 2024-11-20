using NetCoreSignalR.Web.Models;

namespace NetCoreSignalR.Web.Hubs
{
    public interface IExampleTypeSafeHub
    {
        //js client call methods labels
        Task ReceiveMessageForAllClients(string message);
        Task ReceiveTypedMessageForAllClients(Product product);
        Task ReceiveMessageAsStreamForAllClients(string names);
        Task ReceiveProductAsStreamForAllClients(Product product);
        Task ReceiveMessageForSpecifiedClient(string message);
        Task ReceiveMessageForOtherClients(string message);
        Task ReceiveMessageForIndivualClient(string message);
        Task ReceiveMessageForGroupClients(string message);
        Task ConnectedClientCountAllClients(int clientCount);

    }
}
