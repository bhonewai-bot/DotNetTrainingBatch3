"use strict"

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .build();

let count = 0;
let announcements = [];

connection.on("ReceiveAnnouncement", (title, content) => {
    count++;
    document.getElementById("notificationCount").innerText = count;
    announcements.push({ title, content });
});

connection.start()
    .catch(err => console.error(err.toString()));

document.getElementById("showListBtn")
    .addEventListener("click", () => {

        const list = document.getElementById("announcementList");
        list.innerHTML = "";

        announcements.forEach(a => {
            const li = document.createElement("li");
            li.className = "list-group-item";
            li.innerHTML = `<strong>${a.title}</strong> : ${a.content}`;
            list.appendChild(li);
        });
    });