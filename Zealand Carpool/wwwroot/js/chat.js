"use strict";  
  
var connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();  
  
//Disable send button until connection is established  
document.getElementById("sendBtn").disabled = true;  
  
connection.on("ReceiveMessage", function (user, message) {  
    var msg = message.replace(/&/g, "&").replace(/</g, "<").replace(/>/g, ">");  
    var encodedMsg = user + " says " + msg;  
    var li = document.createElement("li");  
    li.textContent = encodedMsg;  
    document.getElementById("ulmessages").appendChild(li);  
});  
  
connection.start().then(function () {  
    document.getElementById("sendBtn").disabled = false;  
}).catch(function (err) {  
    return console.error(err.toString());  
});  
  
document.getElementById("sendBtn").addEventListener("click", function (event) {      
    var message = document.getElementById("txtmessage").value;  
    connection.invoke("SendMessage", message).catch(function (err) {  
        return console.error(err.toString());  
    });  
    event.preventDefault();  
});