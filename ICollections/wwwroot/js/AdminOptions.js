function deleteCheckedContacts() {
    let objectsToDelete = document.getElementsByClassName("user-checkbox");
    let contactIds = [];
    for (let i = 0; i < objectsToDelete.length; i++) {
        let objectToDelete = objectsToDelete[i];
        if (objectToDelete.checked)
            contactIds.push(objectsToDelete[i].getAttribute("name"));
    }
    if (contactIds.length > 0) {
        let requestString = "";
        for (let i = 0; i < contactIds.length - 1; i++) {
            requestString += "Ids[" + i + "]=" + contactIds[i] + "&"
        }
        requestString += "Ids[" + (contactIds.length - 1) + "]=" + contactIds[contactIds.length - 1] + "";

        window.location.replace("/AdminPanel/delete?" + requestString);
    }

}

function promoteCheckedContacts() {
    let objectsToPromote = document.getElementsByClassName("user-checkbox")
    let contactIds = [];
    for (let i = 0; i < objectsToPromote.length; i++) {
        let objectToDelete = objectsToPromote[i];
        if (objectToDelete.checked)
            contactIds.push(objectsToPromote[i].getAttribute("name"));
    }
    if (contactIds.length > 0) {
        let requestString = "";
        for (let i = 0; i < contactIds.length - 1; i++) {
            requestString += "Ids[" + i + "]=" + contactIds[i] + "&"
        }
        requestString += "Ids[" + (contactIds.length - 1) + "]=" + contactIds[contactIds.length - 1] + "";

        window.location.replace("/AdminPanel/promote?" + requestString);
    }
}

function demoteCheckedContacts() {
    let objectsToDemote = document.getElementsByClassName("user-checkbox")
    let contactIds = [];
    for (let i = 0; i < objectsToDemote.length; i++) {
        let objectToDelete = objectsToDemote[i];
        if (objectToDelete.checked)
            contactIds.push(objectsToDemote[i].getAttribute("name"));
    }
    if (contactIds.length > 0) {
        let requestString = "";
        for (let i = 0; i < contactIds.length - 1; i++) {
            requestString += "Ids[" + i + "]=" + contactIds[i] + "&"
        }
        requestString += "Ids[" + (contactIds.length - 1) + "]=" + contactIds[contactIds.length - 1] + "";

        window.location.replace("/AdminPanel/demote?" + requestString);
    }
}

function blockCheckedContacts() {
    let objectsToBlock = document.getElementsByClassName("user-checkbox")
    let contactIds = [];
    for (let i = 0; i < objectsToBlock.length; i++) {
        let objectToDelete = objectsToBlock[i];
        if (objectToDelete.checked)
            contactIds.push(objectsToBlock[i].getAttribute("name"));
    }
    if (contactIds.length > 0) {
        let requestString = "";
        for (let i = 0; i < contactIds.length - 1; i++) {
            requestString += "Ids[" + i + "]=" + contactIds[i] + "&"
        }
        requestString += "Ids[" + (contactIds.length - 1) + "]=" + contactIds[contactIds.length - 1] + "";

        window.location.replace("/AdminPanel/block?" + requestString);
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

        window.location.replace("/AdminPanel/unblock?" + requestString);

    }

}
function logout() {
    window.location.replace("/Account/logout?");
}