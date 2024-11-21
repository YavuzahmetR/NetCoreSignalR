var toastSetTimeOut;

$(document).ready(function () {

    const connection = new window.signalR.HubConnectionBuilder().withUrl("/hub").build();

    connection.start().then(() => { console.log("Connection Established") });

    connection.on("AlertCompleteDownloadFile", (downloadPath) => {

        clearTimeout(toastSetTimeOut);

        $(".toast-body").html(`<p>Your Excel creation process is completed, please download the file from the link below.</p>
        <a href="${downloadPath}">Download</a>
        `);
        $("#liveToast").show();
    })

})