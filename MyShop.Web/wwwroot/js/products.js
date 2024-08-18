var dtble;
$(document).ready(function () {
    loaddate();
    alert("son 7")
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
            { "data": "category.name", "title": "Category" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <a href="/Admin/Product/Edit/${data}" class="btn btn-success">Edit</a>
                        <a href="/Admin/Product/Delete/${data}" class="btn btn-danger">Delete</a>
                    `;
                }
            }
        ]
    });
}
