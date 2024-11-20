namespace NetCoreSignalR.API.Hubs
{
    public interface IMyHub
    {
        public Task ReceiveMessageForAllClients(string message);
    }
}
