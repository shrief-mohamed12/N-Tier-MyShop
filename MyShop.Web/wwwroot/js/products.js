var dtble;
$(document).ready(function () {
    loaddate();
    alert("son")
});


function loaddate() {
    dtble = $("#mytable").DataTable({
        "ajax": {
            "url": "/Admin/Product/GetData",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "name", "title": "Name" },
            { "data": "description", "title": "Description" },
            { "data": "price", "title": "Price" },
            { "data": "category", "title": "Category" }
        ]
    });
}
