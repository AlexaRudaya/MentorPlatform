"use strict";

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

        const userDto = {
            Name: name
        }

        await joinChat(userDto);
    }
}

const joinChat = async (userDto) => {
    if(!userDto)
       return;
    try { 
        const messageDto = {
            User: userDto,
            Content: `${userDto.Name} joined`
        };
        await connection.invoke("JoinChat", messageDto);
    } catch (error) {
        console.log(error);
    }
}

const getUser = () => sessionStorage.getItem('user')

const receiveMessage = async () => {
    const currentUser = getUser();
    if(!currentUser)
        return;
    try {
        await connection.on("ReceiveMessage", (user, message) => {
            const messageClass = currentUser === user ? "send" : "received";
        appendMessage(message, messageClass);
        })
    } catch (error) {
        console.log(error);
    }
}

const appendMessage = (message, messageClass) => {
    const messageSectionElement = document.getElementById('messageSection');
    const messageBoxElement = document.createElement("div");
    messageBoxElement.classList.add("msg-box");
    messageBoxElement.classList.add(messageClass);
    messageBoxElement.innerHTML = message;
    messageSectionElement.appendChild(messageBoxElement);
}

document.getElementById('btnSend').addEventListener('click', async (e) => {
    e.preventDefault();
    const user = getUser();
    if(!user)
        return;
    const txtMessageElement = document.getElementById('txtMessage');
    const message = txtMessageElement.value;
    if(message) {
        const userDto = {
            Name: user
        };
        const messageDto = {
            User: userDto,
            Content: `${user}: ${message}`
        };
        await sendMessage(messageDto);
        txtMessageElement.value = "";
    }
})

const sendMessage = async (messageDto) => {
    try {
        await connection.invoke('SendMessage', messageDto);
    } catch (error) {
        console.log(error);
    }
}

const startApp = async () => {
    await start();
    await joinUser();
    await receiveMessage();
}

startApp();