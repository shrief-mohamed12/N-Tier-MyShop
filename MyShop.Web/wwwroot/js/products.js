var dtble;
$(document).ready(function () {
    loaddate();
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
                        <a onClick=DeleteItem("/Admin/Product/ConfirmDelete/${data}") class="btn btn-danger">Delete</a>
                    `;
                }
            }
        ]
    });
}


function DeleteItem(URL) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: URL,
                type: "DELETE",
                success: function (data) {
                    alert("jdfjfsd");
                    if (data.success) {
                        dtble.ajax.reload();
                        toastr.error(data.message);
                    } else {
                        toastr.success(data.message);
                    }
                }
            });
            //Swal.fire({
            //    title: "Deleted!",
            //    text: "Your file has been deleted.",
            //    icon: "danger"
            //});
        }
    });
}

