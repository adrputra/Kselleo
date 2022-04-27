$(document).ready(function () {
    var table = $('.tableUser').DataTable({
        "filter": true,
        "orderMulti": false,
        "ajax": {
            "url": "https://localhost:44308/api/users",
            "datatype": "json",
            "dataSrc": "result",
        },
        "columns": [
            {
                "data": "id"
            },
            {
                "data": "image"
            },
            {
                "data": "fullName"
            },
            {
                "data": "email"
            },
            {
                "data": null
            }
        ]
    })
});