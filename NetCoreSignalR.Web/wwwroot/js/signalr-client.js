document.addEventListener("DOMContentLoaded", function () {

    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/exampleTypeSafeHub")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    const broadcastMessageToAllClientsHubMehodCall = "BroadcastMessageToAllClients";
    const receiveMessageForAllClientsClientMethodCall = "ReceiveMessageForAllClients";

    const broadcastTypedMessageToAllClientsHubMehodCall = "BroadcastTypedMessageToAllClients";
    const receiveTypedMessageForAllClientsClientMethodCall = "ReceiveTypedMessageForAllClients";

    const broadcastMessageToSpecifiedClientHubMehodCall = "BroadcastMessageToSpecifiedClient";
    const receiveMessageSpecifiedClientClientMethodCall = "ReceiveMessageForSpecifiedClient";

    const broadcastMessageToOtherClientsHubMehodCall = "BroadcastMessageToOtherClients";
    const receiveMessageForOtherClientsClientMethodCall = "ReceiveMessageForOtherClients";

    const broadcastMessageToIndivualClientHubMehodCall = "BroadcastMessageToIndivualClient";
    const receiveMessageForIndivualClientClientMethodCall = "ReceiveMessageForIndivualClient";

    const broadcastMessageToGroupClientsHubMehodCall = "BroadcastMessageToGroupClients"
    const receiveMessageForGroupClientsClientMethodCall = "ReceiveMessageForGroupClients"

    const connectedClientCountAllClients = "ConnectedClientCountAllClients";

    
    const groupA = "GroupA";
    const groupB = "GroupB";
    let currentGroupList = [];


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

    

    function refreshGroupList() {
        $("#groupList").empty();

        currentGroupList.forEach(x => {
            $("#groupList").append(`<p>${x}</p>`);
        });
    }


    const connected_client_count = document.getElementById("span-connected-client-count");
    connection.on(connectedClientCountAllClients, (clientCount) => {
        connected_client_count.textContent = clientCount;
        console.log("Connected Client Count:", clientCount);
    });

    connection.on(receiveMessageForAllClientsClientMethodCall, (message) => {
        console.log("Received Message : ", message);
    });

    connection.on(receiveMessageForOtherClientsClientMethodCall, (message) => {
        console.log("Received Others Message : ", message);
    });

    connection.on(receiveMessageSpecifiedClientClientMethodCall, (message) => {
        console.log("Received Specified Message : ", message);
    });


    connection.on(receiveMessageForIndivualClientClientMethodCall, (message) => {
        console.log("Received Indivual Message : ", message);
    });

    connection.on(receiveMessageForGroupClientsClientMethodCall, (message) => {
        console.log("Group Clients : ", message)
    })

    connection.on(receiveTypedMessageForAllClientsClientMethodCall, (product) => {
        console.log("Received Product Type : ", product);
    });
   

    // Send message to all clients
    document.getElementById("btn-send-message-all-client").addEventListener("click", function () {
        const message = "Hello World!";
        connection.invoke(broadcastMessageToAllClientsHubMehodCall, message)
            .catch(err => {
                console.error("Error: ", err);
            });
        console.log("Message sent successfully");
    });

    // Send message to specified client
    document.getElementById("btn-send-message-specified-client").addEventListener("click", function () {
        const message = "Hello Specified User";
        connection.invoke(broadcastMessageToSpecifiedClientHubMehodCall, message)
            .catch(err => {
                console.error("Error : ", err);
            });
        console.log("Message sent successfully");
    });

    // Send message to other clients
    document.getElementById("btn-send-message-other-client").addEventListener("click", function () {
        const message = "Hello Other Users";
        connection.invoke(broadcastMessageToOtherClientsHubMehodCall, message)
            .catch(err => {
                console.error("Error : ", err);
            });
        console.log("Message sent successfully");
    });

    // Send message to indivual client
    document.getElementById("btn-send-message-indivual-client").addEventListener("click", function () {
        const message = "Hello My Friend";
        const connectionId = $("#text-connectionId").val();

        connection.invoke(broadcastMessageToIndivualClientHubMehodCall, connectionId, message)
            .catch(err => {
                console.error("Error : ", err);
            });
        console.log("Message sent successfully");
    });

    document.getElementById("btn-add-groupA").addEventListener("click", function () {

        if (currentGroupList.includes(groupA)) return;

        connection.invoke("AddGroup", groupA).then(() => {
            currentGroupList.push(groupA);
            refreshGroupList();
        })
    })
    document.getElementById("btn-remove-groupA").addEventListener("click", function () {

        if (!currentGroupList.includes(groupA)) return;

        connection.invoke("RemoveGroup", groupA).then(() => {
            currentGroupList = currentGroupList.filter(x => x !== groupA)
            refreshGroupList();
        })
    })
    document.getElementById("btn-add-groupB").addEventListener("click", function () {

        if (currentGroupList.includes(groupB)) return;

        connection.invoke("AddGroup", groupB).then(() => {
            currentGroupList.push(groupB);
            refreshGroupList();
        })
    })
    document.getElementById("btn-remove-groupB").addEventListener("click", function () {

        if (!currentGroupList.includes(groupB)) return;

        connection.invoke("RemoveGroup", groupB).then(() => {
            currentGroupList = currentGroupList.filter(x => x !== groupB)
            refreshGroupList();
        })
    })

    document.getElementById("btn-send-message-groupA").addEventListener("click", function () {

        if (!currentGroupList.includes(groupA)) {
            console.error("You are not a member of Group A.");
            return;
        }

        const message = "Hello GroupA";

        connection.invoke(broadcastMessageToGroupClientsHubMehodCall, groupA, message)
            .catch(err => {
                console.error("Error : ", err);
            });
        console.log("Message sent successfully");
    })

    document.getElementById("btn-send-message-groupB").addEventListener("click", function () {

        if (!currentGroupList.includes(groupB)) {
            console.error("You are not a member of Group B.");
            return;
        }
        
        const message = "Hello GroupB";

        connection.invoke(broadcastMessageToGroupClientsHubMehodCall, groupB, message)
            .catch(err => {
                console.error("Error : ", err);
            });
        console.log("Message sent successfully");
    })

    document.getElementById("btn-send-product-all-client").addEventListener("click", function () {


        const product = { id: 1, name: "Product Pencil", price: 200 };

        connection.invoke(broadcastTypedMessageToAllClientsHubMehodCall, product)
            .catch(err => {
                console.error("Error : ", err);
            });
        console.log("Product sent successfully");
    })
    

   

});

