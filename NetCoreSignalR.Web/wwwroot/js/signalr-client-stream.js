document.addEventListener("DOMContentLoaded", function () {

    const broadcastStreamDataToAllClients = "BroadcastStreamDataToAllClients";
    const receiveMessageAsStreamForAllClients = "ReceiveMessageAsStreamForAllClients";

    const broadcastStreamProductToAllClients = "BroadcastStreamProductToAllClients";
    const receiveProductAsStreamForAllClients = "ReceiveProductAsStreamForAllClients";

    const broadcastFromHubToClientStream = "BroadcastFromHubToClientStream";

    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/exampleTypeSafeHub")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    async function start() {
        try {
            await connection.start();
            console.log("Connected To Hub");
            $("#connectionId").html(`Connection Id : ${connection.connectionId}`);
        } catch (err) {
            console.error("Error while starting connection: ", err);
            setTimeout(start, 3000); // 3 saniye sonra tekrar dene
        }
    }

    connection.onclose(async () => {
        await start(); // Bağlantı kapanırsa yeniden başlat
    });

    start();

    connection.on(receiveMessageAsStreamForAllClients, (names) => {
        $("#streamBox").append(`<p>${names}</p>`)
    })

    connection.on(receiveProductAsStreamForAllClients, (product) => {
        $("#streamBox").append(`<p>${product.id} - ${product.name} - ${product.price}</p>`)
    })

    document.getElementById("btn-FromClient-ToHub").addEventListener("click", function () {

        const names = $("#txt_stream").val();
        const namesAsChunck = names.split(",");

        const subject = new signalR.Subject();

        connection.send(broadcastStreamDataToAllClients, subject).catch(err => console.error(err));

        namesAsChunck.forEach(name => {
            subject.next(name);
        });

        subject.complete();

    })

    document.getElementById("btn-FromClient-ToHub2").addEventListener("click", function () {

        const productList = [
            {id: 1, name:"Pen1", price:100},
            {id: 2, name:"Pen2", price:200},
            {id: 3, name:"Pen3", price:300}
        ]

        const subject = new signalR.Subject();

        connection.send(broadcastStreamProductToAllClients, subject).catch(err => console.error(err));

        productList.forEach(product => {
            subject.next(product);
        });

        subject.complete();

    })

    document.getElementById("btn-FromHub-ToClient").addEventListener("click", function () {

        connection.stream(broadcastFromHubToClientStream, 5).subscribe(
            {
                next: (message) => $("#streamBox").append(`<p>${message}</p>`)
            }
        )

    });

    // Bağlantıyı başlat
     
});