// See https://aka.ms/new-console-template for more information
using Microsoft.AspNetCore.SignalR.Client;
using NetSignalRClient.ConsoleApp;

Console.WriteLine("Hello, World!");
var connection = new HubConnectionBuilder().WithUrl("https://localhost:7094/exampleTypeSafeHub").Build();

connection.StartAsync().ContinueWith((result) =>
{
    Console.WriteLine(result.IsCompletedSuccessfully ? "Connected" : "Connection Failed");
});

connection.On<Product>("ReceiveTypedMessageForAllClients", (product) =>
{
    Console.WriteLine($"Received Product : {product.Id} - {product.Name} - {product.Price}");
});


while (true)
{
    var key = Console.ReadLine();
    if (key == "exit") break;

    var newProduct = new Product(20, "Pen 20", 300);
    await connection.InvokeAsync("BroadcastTypedMessageToAllClients", newProduct);
}

Console.ReadKey(); 