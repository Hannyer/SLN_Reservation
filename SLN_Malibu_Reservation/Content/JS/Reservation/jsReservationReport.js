function ExportDataReservationReport() {
    $("#btnExportData").click(function () {
        var checkIn = $('#txtCheckIn').val();
        var tmpCheckOut = $('#txtCheckOut').val();

        var parameters = { checkIn: checkIn, tmpCheckOut: tmpCheckOut };
        $.ajax({
            url: '/ReservationReport/ExportData',
            type: 'POST',
            data: parameters,
        }).done(function (response) {
            $("#contenedorVistaParcial").html(response);
            var table =  $('#tblReservationReport').DataTable({
                "paging": true,
                "lengthChange": true,
                "searching": true,
                "ordering": true,
                "responsive": true,             
                buttons: [
                    'copy', 'excel', 'pdf'
                ],
                "lengthMenu": [[5, 10, 25, 50, 100], [5, 10, 25, 50, "Todo"]],
                "language": {
                    "lengthMenu": "Mostrar _MENU_ registros por página",
                    "zeroRecords": "No se encontraron registros",
                    "info": "Mostrando _START_ a _END_ de _TOTAL_ registros",
                    "infoEmpty": "No hay registros disponibles",
                    "infoFiltered": "(filtrados de _MAX_ registros en total)",
                    "search": "Buscar:",
                    "paginate": {
                        "first": "Primero",
                        "last": "Último",
                        "next": "Siguiente",
                        "previous": "Anterior"
                    }
                }
            });
           
        });
        $("#divBtnDownload").fadeIn();
    });
}

function ExportDataTotalReport() {
    $("#btnExportDataTotalReport").click(function () {
        var checkIn = $('#txtCheckInTotalReport').val();
        var tmpCheckOut = $('#txtCheckOutTotalReport').val();

        var parameters = { checkIn: checkIn, tmpCheckOut: tmpCheckOut };
        $.ajax({
            url: '/ReservationReport/ExportDataTotalReport',
            type: 'POST',
            data: parameters,
        }).done(function (response) {
            $("#contenedorVistaParcialTotalReport").html(response);
            var table = $('#tblTotalReport').DataTable({
                "paging": true,
                "lengthChange": true,
                "searching": true,
                "ordering": true,
                "responsive": true,
                buttons: [
                    'copy', 'excel', 'pdf'
                ],
                "lengthMenu": [[5, 10, 25, 50, 100], [5, 10, 25, 50, "Todo"]],
                "language": {
                    "lengthMenu": "Mostrar _MENU_ registros por página",
                    "zeroRecords": "No se encontraron registros",
                    "info": "Mostrando _START_ a _END_ de _TOTAL_ registros",
                    "infoEmpty": "No hay registros disponibles",
                    "infoFiltered": "(filtrados de _MAX_ registros en total)",
                    "search": "Buscar:",
                    "paginate": {
                        "first": "Primero",
                        "last": "Último",
                        "next": "Siguiente",
                        "previous": "Anterior"
                    }
                }
            });

        });
        $("#divBtnDownloadTotalReport").fadeIn();
    });
}
function ExportDataAvailabilityReport() {
    $("#btnExportDataAvailabilityReport").click(function () {
        mostrarSpinner();
        var checkIn = $('#txtCheckInAvailability').val();
        var tmpCheckOut = $('#txtCheckOutAvailability').val();

        var parameters = { checkIn: checkIn, tmpCheckOut: tmpCheckOut };
        $.ajax({
            url: '/ReservationReport/ExportDataAvailability',
            type: 'POST',
            data: parameters,
        }).done(function (response) {
            $("#contenedorVistaAvailabilityReport").html(response);
            var table = $('#tblAvailabilityReport').DataTable({
                "paging": true,
                "lengthChange": true,
                "searching": true,
                "ordering": true,
                "responsive": true,
                buttons: [
                    'copy', 'excel', 'pdf'
                ],
                "lengthMenu": [[5, 10, 25, 50, 100], [5, 10, 25, 50, "Todo"]],
                "language": {
                    "lengthMenu": "Mostrar _MENU_ registros por página",
                    "zeroRecords": "No se encontraron registros",
                    "info": "Mostrando _START_ a _END_ de _TOTAL_ registros",
                    "infoEmpty": "No hay registros disponibles",
                    "infoFiltered": "(filtrados de _MAX_ registros en total)",
                    "search": "Buscar:",
                    "paginate": {
                        "first": "Primero",
                        "last": "Último",
                        "next": "Siguiente",
                        "previous": "Anterior"
                    }
                }
            });

        });
        $("#divBtnDownload").fadeIn();

        ocultarSpinner();
    });
}
function ExportDataAvailabilityReportToExcel() {
    $("#btnExportToExcel").click(function () {
        mostrarSpinner();
        var checkIn = $('#txtCheckInAvailability').val();
        var tmpCheckOut = $('#txtCheckOutAvailability').val();

        var parameters = { CheckIn: checkIn, CheckOut: tmpCheckOut };
        $.ajax({
            url: '/ReservationReport/ExportReservationAvailabilityReportEToExcel',
            type: 'POST',
            data: parameters,
        }).done(function (response) {
          

        });
        $("#divBtnDownload").fadeIn();

        ocultarSpinner();
    });
}
function ExportDataReservationReportStart() {
    var fechaActual = new Date();
    var fechaFormateada = fechaActual.toISOString().split('T')[0];
    $('#txtCheckIn').val(fechaFormateada);
    $('#txtCheckOut').val(fechaFormateada);
    $("#divBtnDownload").hide();

    var table = $('#tblReservationReport').DataTable({
        "paging": true,
        "lengthChange": true,
        "searching": true,
        "ordering": true,
        "responsive": true,
        "lengthMenu": [[5, 10, 25, 50, 100], [5, 10, 25, 50, "Todo"]],
        "language": {
            "lengthMenu": "Mostrar _MENU_ registros por página",
            "zeroRecords": "No se encontraron registros",
            "info": "Mostrando _START_ a _END_ de _TOTAL_ registros",
            "infoEmpty": "No hay registros disponibles",
            "infoFiltered": "(filtrados de _MAX_ registros en total)",
            "search": "Buscar:",
            "paginate": {
                "first": "Primero",
                "last": "Último",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        }
    });

}

function ExportDataTotalReportEStart() {
    var fechaActual = new Date();
    var fechaFormateada = fechaActual.toISOString().split('T')[0];
    $('#txtCheckInTotalReport').val(fechaFormateada);
    $('#txtCheckOutTotalReport').val(fechaFormateada);
    $("#divBtnDownloadTotalReport").hide();

    var table = $('#tblTotalReport').DataTable({
        "paging": true,
        "lengthChange": true,
        "searching": true,
        "ordering": true,
        "responsive": true,
        "lengthMenu": [[5, 10, 25, 50, 100], [5, 10, 25, 50, "Todo"]],
        "language": {
            "lengthMenu": "Mostrar _MENU_ registros por página",
            "zeroRecords": "No se encontraron registros",
            "info": "Mostrando _START_ a _END_ de _TOTAL_ registros",
            "infoEmpty": "No hay registros disponibles",
            "infoFiltered": "(filtrados de _MAX_ registros en total)",
            "search": "Buscar:",
            "paginate": {
                "first": "Primero",
                "last": "Último",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        }
    });

}

function DownloadReservationReport() {
    mostrarSpinner()
    $("#btnReservationReporDownload").click(function () {
        $.ajax({
            url: '/ReservationReport/ExportReservationReportEToExcel',
            type: 'POST',
        }).done(function (response) {
         

        });
    });
    ocultarSpinner();
}

function DownloadTotalReport() {
    $("#btnTotalReportDownload").click(function () {
       
    });

  
}