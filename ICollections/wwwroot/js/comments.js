let connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();

document.getElementById("sendButton").disabled = true;

connection.on("ReceiveComment", function (user, comment, time) {
    console.log("received");
    let div = document.getElementById("newComment");

    let ul = document.createElement("ul");
    ul.className = "list-group list-group-flush";
    ul.style.marginBottom = "12px";
    div.prepend(ul);

    let commentWhen = document.createElement("li");
    commentWhen.className = "list-group-item";
    commentWhen.textContent = `${time}`;
    ul.prepend(commentWhen);

    let commentText = document.createElement("li");
    commentText.className = "list-group-item";
    commentText.textContent = `${comment}`;
    ul.prepend(commentText);
    
    let userNickName = document.createElement("li");
    userNickName.className = "list-group-item";
    userNickName.textContent = `${user}`;
    ul.prepend(userNickName);
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    let comment = document.getElementById("commentToSend").value;
    let itemId = document.getElementById("itemId").value;
    connection.invoke("SendComment", comment, itemId).catch(function (err) {
        return console.error(err.toString());
    });
    document.getElementById("commentToSend").value = "";
    event.preventDefault();
});
