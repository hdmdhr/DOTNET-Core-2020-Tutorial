var h2 = document.createElement("h2");
h2.innerHTML = "Hello from CustomScript";
document.body.appendChild(h2);


function confirmDelete(userId, isDeleteClicked) {
    var deleteId = '#delete_' + userId;
    var confirmDeleteId = '#confirmDelete_' + userId;

    if (isDeleteClicked) {
        console.log(deleteId);
        $(deleteId).hide();
        $(confirmDeleteId).show();
    } else {
        $(deleteId).show();
        $(confirmDeleteId).hide();
    }
}