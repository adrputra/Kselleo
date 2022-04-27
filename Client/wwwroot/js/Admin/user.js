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
                "data": "id",
            },
            {
                "data": "gender",
            },
            {
                "data": "fullName",
            },
            {
                "data": "email",
            },
            {
                "className": "dt-center", "targets": "_all",
                "orderable": false,
                "data": null,
                "render": function (data, type, row) {
                    return `<button class="bi bi-trash-fill btn-secondary btn-sm" onclick='deletedata("${row["nik"]}")'></button>
                            <button class='bi bi-pencil-square btn-secondary btn-sm' data-toggle="modal" onclick='GetDataUpdate("${row["nik"]}")' data-target='#UpdateModal'></button>`;
                }
            }
        ]
    })
});