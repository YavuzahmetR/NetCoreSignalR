using Microsoft.AspNetCore.SignalR.Client;

namespace NetCoreSignalRClient.WorkerServiceApp
{
    public class Worker(ILogger<Worker> _logger, IConfiguration configuration) : BackgroundService
    {
        private HubConnection? _connection;
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _connection = new HubConnectionBuilder().WithUrl(configuration.GetSection("SignalR")["Hub"]!).Build();

            _connection.StartAsync().ContinueWith((result) =>
            {
                _logger.LogInformation(result.IsCompletedSuccessfully ? "Connected" : "Connection Failed");
            });

            return base.StartAsync(cancellationToken);
        }
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _connection!.StopAsync(cancellationToken);
            await _connection!.DisposeAsync();
            await base.StopAsync(cancellationToken);
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _connection!.On<Product>("ReceiveTypedMessageForAllClients", (product) =>
            {
                _logger.LogInformation($"Received Product : {product.Id} - {product.Name} - {product.Price}");
            });
            return Task.CompletedTask;
        }
    }
}
