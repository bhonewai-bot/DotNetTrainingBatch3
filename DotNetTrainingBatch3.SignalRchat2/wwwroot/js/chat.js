"use strict"

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();

document.getElementById("sendButton").disabled = true;

connection.on("ClientReceiveMessage", (user, message) => {
    const li = document.createElement("li");
    li.textContent = `${user} says: ${message}`;
    document.getElementById("messagesList").appendChild(li);
});

connection.start()
    .then(() => {
        document.getElementById("sendButton").disabled = false;
    })
    .catch(err => console.error(err.toString()));

document.getElementById("sendButton")
    .addEventListener("click", event => {

        const user = document.getElementById("userInput").value;
        const message = document.getElementById("messageInput").value;

        connection.invoke("ServerReceiveMessage", user, message)
            .catch(err => console.error(err.toString()));

        event.preventDefault();
    });
