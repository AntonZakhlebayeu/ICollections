function deleteCheckedContacts() {
    var objectsToDelete = document.getElementsByClassName("user-checkbox");
    var contactIds = [];
    for (var i = 0; i < objectsToDelete.length; i++) {
        var objectToDelete = objectsToDelete[i];
        if (objectToDelete.checked)
            contactIds.push(objectsToDelete[i].getAttribute("name"));
    }
    if (contactIds.length > 0) {
        var requestString = "";
        for (var i = 0; i < contactIds.length - 1; i++) {
            requestString += "Ids[" + i + "]=" + contactIds[i] + "&"
        }
        requestString += "Ids[" + (contactIds.length - 1) + "]=" + contactIds[contactIds.length - 1] + "";

        window.location.replace("/home/delete?" + requestString);
    }

}

function promoteCheckedContacts() {
    var objectsToBlock = document.getElementsByClassName("user-checkbox")
    var contactIds = [];
    for (var i = 0; i < objectsToBlock.length; i++) {
        var objectToDelete = objectsToBlock[i];
        if (objectToDelete.checked)
            contactIds.push(objectsToBlock[i].getAttribute("name"));
    }
    if (contactIds.length > 0) {
        var requestString = "";
        for (var i = 0; i < contactIds.length - 1; i++) {
            requestString += "Ids[" + i + "]=" + contactIds[i] + "&"
        }
        requestString += "Ids[" + (contactIds.length - 1) + "]=" + contactIds[contactIds.length - 1] + "";

        window.location.replace("/home/promote?" + requestString);
    }
}

function demoteCheckedContacts() {
    var objectsToPromote = document.getElementsByClassName("user-checkbox")
    var contactIds = [];
    for (var i = 0; i < objectsToPromote.length; i++) {
        var objectToDelete = objectsToPromote[i];
        if (objectToDelete.checked)
            contactIds.push(objectsToPromote[i].getAttribute("name"));
    }
    if (contactIds.length > 0) {
        var requestString = "";
        for (var i = 0; i < contactIds.length - 1; i++) {
            requestString += "Ids[" + i + "]=" + contactIds[i] + "&"
        }
        requestString += "Ids[" + (contactIds.length - 1) + "]=" + contactIds[contactIds.length - 1] + "";

        window.location.replace("/home/demote?" + requestString);
    }
}

function blockCheckedContacts() {
    var objectsToDemote = document.getElementsByClassName("user-checkbox")
    var contactIds = [];
    for (var i = 0; i < objectsToDemote.length; i++) {
        var objectToDelete = objectsToDemote[i];
        if (objectToDelete.checked)
            contactIds.push(objectsToDemote[i].getAttribute("name"));
    }
    if (contactIds.length > 0) {
        var requestString = "";
        for (var i = 0; i < contactIds.length - 1; i++) {
            requestString += "Ids[" + i + "]=" + contactIds[i] + "&"
        }
        requestString += "Ids[" + (contactIds.length - 1) + "]=" + contactIds[contactIds.length - 1] + "";

        window.location.replace("/home/block?" + requestString);
    }
}

function unblockCheckedContacts() {
    var objectsToUnblock = document.getElementsByClassName("user-checkbox")
    var contactIds = [];
    for (var i = 0; i < objectsToUnblock.length; i++) {
        var objectToDelete = objectsToUnblock[i];
        if (objectToDelete.checked)
            contactIds.push(objectsToUnblock[i].getAttribute("name"));
    }

    if (contactIds.length > 0) {
        var requestString = "";
        for (var i = 0; i < contactIds.length - 1; i++) {
            requestString += "Ids[" + i + "]=" + contactIds[i] + "&"
        }
        requestString += "Ids[" + (contactIds.length - 1) + "]=" + contactIds[contactIds.length - 1] + "";

        window.location.replace("/home/unblock?" + requestString);

    }

}
function logout() {
    window.location.replace("/Account/logout?");
}