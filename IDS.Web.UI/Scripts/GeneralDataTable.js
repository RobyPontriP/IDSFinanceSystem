

// Untuk Request data ke Controller
function httpGet(getOrpost, theUrl, param) {
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.open(getOrpost, theUrl, false); // false for synchronous request
    xmlHttp.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    xmlHttp.send(param);
    return JSON.parse(xmlHttp.responseText);
}

function modalProcess() {
    $('#loadingModal').modal({ backdrop: 'static', keyboard: false })
    $('#loadingModal').modal('show');
    setTimeout(function () {
        $('#loadingModal').modal('hide');
    }, 10);
}