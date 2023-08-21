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

        await joinChat(name);
    }
}

const joinChat = async (user) => {
    if(!user)
       return;
    try { 
        const message = `${user} joined`;
        await connection.invoke("JoinChat", user, message);
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
        await sendMessage(user,`${user}: ${message}`);
        txtMessageElement.value = "";
    }
})

const sendMessage = async (user, message) => {
    try {
        await connection.invoke('SendMessage', user, message);
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