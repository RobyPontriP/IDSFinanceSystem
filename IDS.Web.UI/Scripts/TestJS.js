
        
function LoadCreateNewForm() {
    $.ajax({
        type: "GET",
        url: 'http://localhost/datindo2021/GeneralTable/Bank/Create',
        contentType: 'application/html; charset=utf-8',
        dataType: 'html',
        success: function (result) {
            $('#PartialSection').empty();
            $("#PartialSection").html(result);
            //$("form#formCreateEdit")[0].reset();
            $("#modalTitle").html("Create Data");
            $("#CreateEditForm").modal('show');
        },
        error: function (status, h, k, l) {
            
        }
    });
}

function LoadEditForm() {
    var form = this;
    var selRow = table.rows('.selected').data();
    var data = null;

    if (selRow.length > 0) {
        data = selRow[0]["BankCode"];
        $.ajax({
            type: "GET",
            url: '@Url.Action("Edit", "Bank", new { Area = "GeneralTable" })',
            data: {
                'BankCode': data
            },
            dataType: 'html',
            success: function (result) {
                $('#PartialSection').empty();
                $("#PartialSection").html(result);
                $("#modalTitle").html("Edit Data");
                $("#BankCode").attr('readonly', 'true');
                $("#CreateEditForm").modal('show');
            },
            error: function (status) {
                alert('');
            }
        });
    }
    else {
        alert("Please select data to be edit");
    }
}

function Delete() {
    var form = this;
    var selRow = table.rows('.selected').data();

    if (selRow == null || selRow == 'undefined' || selRow.length == 0) {
        alert("Please select the data you want to delete");
        return false;
    }

    var dataCollection = '';
    var data = '';

    $.each(selRow, function (index, row) {
        data += row["BankCode"] + ',';
        dataCollection += '[' + row["BankCode"] + '] - ' + row["BankName"] + "\n";
    })

    var answer = confirm("You are about to delete data:\n" + dataCollection + "\nAre you sure want to delete the data ?");

    //var form = this;
    //var selRow = table.rows('.selected').data();
    //var dataCollectionMessage = '';
    //var dataCollection = '';

    //$.each(selRow, function (index, rowId) {
    //    dataCollectionMessage += rowId["BrandID"] + ' | ';
    //})

    //if (selRow.length > 0) {
    //    var dataCollectionMessageStr = dataCollectionMessage.substring(0, dataCollectionMessage.length - 3);
    //    var answer = confirm("Are you sure want to delete Brand ID : " + dataCollectionMessageStr + " ?");
    //}
    //else {
    //    alert('Please select the data you want to delete');
    //}

    if (answer) {
        if (dataCollection.length > 0) {
            $.ajax({
                type: "POST",
                url: '@Url.Action("Delete", "Bank", new { Area = "GeneralTable" })',
                data: { "banksCodeList": data },
                dataType: 'JSON',
                success: function (respond) {
                    switch (respond) {
                        case "NoAccess":
                            window.location.href = '@Server.MapPath("~/Views/Shared/View403.cshtml")'
                            break;
                        case "Success":
                            alert(respond);
                            window.location.href = '@Url.Action("index", "Bank", new { Area = "GeneralTable" })'
                            break;
                        default:
                            alert(respond);
                            window.location.href = '@Url.Action("index", "Bank", new { Area = "GeneralTable" })'
                            break;
                    }

                },
                error: function (respond) {
                    alert(respond);
                }
            })
        }
    }
    else {
        return false;
    }

}