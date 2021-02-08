
$('#btnAddCustomer').on("click", function () {
    window.location.href = baseUrl + "Customer/Detail";
});

$('#btnSaveCustomer').on('click', function () {
    var customer = {};
    customer.FirstName = $('#firstName').val().trim();
    customer.LastName = $('#lastName').val().trim();
    customer.Address = $('#address').val().trim();
    customer.Email = $('#email').val().trim();
    customer.Contact = $('#contact').val().trim();
    customer.Gender = $('#gender').val().trim();
    customer.CustomerId = $('#hdCustomerId').val();

    $.ajax({
        url: baseUrl + "Customer/Save",
        type: "POST",
        dataType: "json",
        data: JSON.stringify(customer),
        contentType: "application/json; charset=utf-8",
        traditional: true,
        success: function (data) {
            if (data && data.CustomerId) {
                window.location.href = baseUrl + "Customer/Detail?Id=" + data.CustomerId;
            }

        },
        error: function () {

        }
    })
});

$('#btnDeleteCustomers').on('click', function () {
    if ($('.chkCustomer:checked').length > 0) {
        var ids = [];
        if (confirm("Are you sure you want to delete the selected customers ?")) {
            $('.chkCustomer:checked').each(function () {
                ids.push($(this).closest('tr').attr('id'));
            });

            if (ids.length > 0) {
                DeleteCustomer(ids.toString());
            }
        }
    }
});

$('#btnDeleteCustomer').on('click', function () {
    var id = $('#hdCustomerId').val();
    if (id && confirm("Are you sure you want to delete the current customer ?")) {
        DeleteCustomer(id);
    }
})

function DeleteCustomer(ids) {
    $.ajax({
        url: baseUrl + "Customer/Delete",
        type: "POST",
        data: { 'ids': ids },
        dataType: "json",
        success: function (message) {
            if (message = "Success") {
                window.location.href = baseUrl + "Customer";
            }
            else {
                alert("Internal Error Occured. Error message :" + message);
            }
        },
        error: function (err) {
            alert("Network Error Occured. Error message :" + err.message);
        }
    });
}

function ArchiveCustomer(ids) {
    $.ajax({
        url: baseUrl + "Customer/Archive",
        type: "POST",
        data: { 'ids': ids },
        dataType: "json",
        success: function (message) {
            if (message = "Success") {
                window.location.href = baseUrl + "Customer";
            }
            else {
                alert("Internal Error Occured. Error message :" + message);
            }
        },
        error: function (err) {
            alert("Network Error Occured. Error message :" + err.message);
        }
    });
}

$('#btnArchiveCustomers').on('click', function () {
    if ($('.chkCustomer:checked').length > 0) {
        var ids = [];

        $('.chkCustomer:checked').each(function () {
            ids.push($(this).closest('tr').attr('id'));
        });

        if (ids.length > 0) {
            ArchiveCustomer(ids.toString());
        }
    }
});

$('#btnArchiveCustomer').on('click', function () {
    var id = $('#hdCustomerId').val();
    ArchiveCustomer(id);
})

$('#btnPrint').on('click', function () {
    var printContents = $('#printCustomerDetail').html();
    var popupWin = window.open('', '_blank', 'width=800,height=600');
    popupWin.document.open();
    popupWin.document.write('<html><head></head><body onload="window.print()">' + printContents + '</body></html>');
    popupWin.document.close();
});

function requestPage(page) {
    if (page == -4) {
        page = 1;
    }
    else if (page == -3) {
        page = currentPage - 1;
    }
    else if (page == -2) {
        page = currentPage + 1;
    }
    else if (page == -1) {
        page = totalPage;
    }

    console.log(page);
    if (page != currentPage && page != 0 && page <= totalPage) {
        var key = $("#txtSearch").val().trim();
        $("#txtSearch").val(key);
        loadPartialPage(page, 7, "");
    }

}

function loadPartialPage(page, pageSize, search) {
    $.ajax({
        url: baseUrl + "Customer/CustomerList",
        type: "POST",
        data: { "search": search, "page": page, "pageSize": pageSize, },
        traditional: true,
        success: function (data) {
            $("#partialPage").html(data);
        },
        error: function (err) {
            alert("Error Occurred : " + err);
        }
    })
}

$("#btnSearch").on("click", function () {
    var key = $("#txtSearch").val().trim();
    $("#txtSearch").val(key);
    loadPartialPage(1, 7, key);
});

$('#txtSearch').keypress(function (e) {
    var code = e.keyCode || e.which;
    if (code === 13) {
        $("#btnSearch").click();
    }
});
