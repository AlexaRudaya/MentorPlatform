const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7006/chathub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

const start = async () => {
    try {
        await connection.start();
        console.log("SignalR is connected.");
    } catch (error) {
        console.log(error);
    }
}

const startApp = async () => {
    await start();
}

startApp();