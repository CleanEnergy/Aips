var ids = [];

function markPresence(checkbox, id) {
    
    if ($(checkbox).prop('checked')) {
        ids.push(id);
    } else {
        ids.splice(ids.indexOf(id), 1);
    }
}

function savePresence() {

    $('#presentIds').val(JSON.stringify(ids));
    document.forms["idsForm"].submit();

}