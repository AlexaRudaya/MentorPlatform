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

const joinUser = async () => {
    const name = window.prompt('Enter the name: ');
    if(name)
    {
        sessionStorage.setItem('user', name);

        await joinChat(name);
    }
}

const joinChat = async (user) => {
    if(!user)
       return;
    try { 
        const message = `${user} joined`;
        await connection.invoke("JoinChat", user, message);
        console.log('Joined the chat');
        alert(message);
    } catch (error) {
        console.log(error);
    }
}

const getUser = () => sessionStorage.getItem('user')

const startApp = async () => {
    await start();
    await joinUser();
}

startApp();