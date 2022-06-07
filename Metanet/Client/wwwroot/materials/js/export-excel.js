function TestDataTablesAdd() {
    var table = arguments[0];
    $(table).DataTable({
        dom: 'Bfrtip',
        buttons: [
            'excelHtml5',
            'csvHtml5'
        ],
        bStateSave: true,
        bDestroy: true,
        extend: "excelHtml5",
        autoFilter: true
    });
}

function TestDataTablesRemove(table) {
    $(document).ready(function () {
        $(table).DataTable().destroy();
    });
}