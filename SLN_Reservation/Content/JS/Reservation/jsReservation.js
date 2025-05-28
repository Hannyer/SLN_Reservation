function NewReservation() {
    try {
        var IdCard_Client = GetDropDownValueSelected('ddlAddReservationModal', 'AddReservationList');
        var Reservation_Description = $("#txtDescription").val();
        var CheckIn = $("#txtCheckIn").val();
        var CheckOut = $("#txtCheckOut").val();
        var Status = 1;
        var Days = tmpDays(CheckIn, CheckOut);
        var ID_Rate = GetDropDownValueSelected('ddlAddTarifaModal', 'AddTarifaList');
        var ID_ROOM = GetDropDownValueSelected('ddlAddRoomModal', 'AddRoomList');

        if (Validate('ddlAddReservationModal', 'AddReservationList', 'ddlAddTarifaModal', 'AddTarifaList', 'txtCheckIn', 'txtCheckOut', 'ddlAddRoomModal', 'AddRoomList')) {
            mostrarSpinner();

            var reservation = {
                IdCard_Client: IdCard_Client,
                Reservation_Description: Reservation_Description,
                CheckIn: CheckIn,
                CheckOut: CheckOut,
                Status: Status,
                Days: Days,
                ID_Rate: ID_Rate,
                ID_ROOM: ID_ROOM
            };

            $.ajax({
                url: '/Reservation/NewReservation',
                type: 'POST',
                data: reservation,
            }).done(function (response) {
                ocultarSpinner();
                if (response.includes("exitosamente")) {
                    Swal.fire({
                        text: response,
                        icon: 'success',
                        didClose: () => {
                            $("#txtDescription").text('');
                            $("#txtModifyDescription").val('');
                            window.location.href = '/Reservation/Index';
                        }
                    });
                } else {
                    Swal.fire('', response, 'warning');
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                ocultarSpinner();
                console.error("Error en la solicitud AJAX:", textStatus, errorThrown);
                Swal.fire({
                    text: "Ocurrió un error al procesar la reservación. Por favor, inténtelo de nuevo más tarde.",
                    icon: 'error'
                });
            }).always(function () {
                ocultarSpinner();
            });

        }
    } catch (error) {
        ocultarSpinner();
        console.error("Error en la función NewReservation:", error);
        Swal.fire({
            text: "Ocurrió un error inesperado. Por favor, inténtelo de nuevo más tarde.",
            icon: 'error'
        });
    }
}



function ModifyReservation() {
  
    var IdCard_Client = GetDropDownValueSelected('ddlModifyReservationModal', 'ModifyReservationList');
    var Reservation_Description = $("#txtModifyDescription").val();
    var CheckIn = $("#txtModifyCheckIn").val();
    var CheckOut = $("#txtModifyCheckOut").val();
    var Status = 0;
    var Days = tmpDays(CheckIn, CheckOut);
    var ID_Rate = GetDropDownValueSelected('ddlModifyTarifaModal', 'ModifyTarifaList');
    var Id = $('#hdfIdReservationModify').val();
    if (Validate('ddlModifyReservationModal', 'ModifyReservationList', 'ddlModifyTarifaModal', 'ModifyTarifaList', 'txtModifyCheckIn', 'txtModifyCheckOut')) {
        mostrarSpinner();
        var parameters = { IdCard_Client: IdCard_Client, Reservation_Description: Reservation_Description, CheckIn: CheckIn, CheckOut: CheckOut, Status: Status, Days: Days, ID_Rate: ID_Rate, Id: Id };
            $.ajax({
                url: '/Reservation/ModifyReservation',
                type: 'POST',
                data: parameters,
            }).done(function (response) {
                ocultarSpinner();
                if (response.includes("exitosamente")) {
                    Swal.fire({
                        text: response,
                        icon: 'success',
                        didClose: () => {
                            $("#txtDescription").text('');
                            $("#txtModifyDescription").val('');
                            window.location.href = '/Reservation/Index';
                        }
                    });
                }
                else {
                    Swal.fire('', response, 'warning');
                }
            });
       
    } 
}

// Función para calcular la diferencia de días entre dos fechas
function tmpDays(P_CHECKIN, P_CHECKOUT) {
    // Convertir las fechas a objetos de fecha
    const tmpP_CHECKIN = new Date(P_CHECKIN);
    const tmpP_CHECKOUT = new Date(P_CHECKOUT);

    // Calcular la diferencia en milisegundos
    const diferenciaMs = tmpP_CHECKOUT - tmpP_CHECKIN;

    // Convertir la diferencia a días
    const diferenciaDias = Math.floor(diferenciaMs / (1000 * 60 * 60 * 24));

    return diferenciaDias;
}



function DeleteReservation(Id) {

    
 
    console.log(Id);
       
        Swal.fire({
            title: '¿Está seguro?',
            text: '¡Que desea borrar este dato!',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#3085d6',
            confirmButtonText: 'Sí, borrarlo',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
             
                var parameters = { Id: Id };
                $.ajax({
                    url: '/Reservation/DeleteReservation',
                    type: 'POST',
                    data: parameters,
                }).done(function (response) {

                    if (response.includes("exitosamente")) {
                        Swal.fire({
                            text: response,
                            icon: 'success',
                            didClose: () => {
                                window.location.href = '/Reservation/Index';
                            }
                        });

                    }
                    else {
                        Swal.fire('', response, 'warning');
                    }
                });
            }
        });
   

}


function Validate(ddlIDClientSelected, dataClientSelected, ddlRateSelected, dataRateSelected, Checkin, Checkout,ddlAddHotelRoomSelected,dataAddHotelRoomselected) {
    if (GetDropDownValueSelected(ddlIDClientSelected, dataClientSelected) === null) {
        Swal.fire('', 'Debe seleccionar un cliente', 'error');
        return false;
    }
    if (GetDropDownValueSelected(ddlRateSelected, dataRateSelected) === null) {
        Swal.fire('', 'Debe seleccionar una tarifa', 'error');
        return false;
    }
    if ($("#" + Checkin).val() === '') {
        Swal.fire('', 'Debe seleccionar la fecha de entrada .', 'error');
        return false;
    }
    if ($("#" + Checkout).val() === '') {
        Swal.fire('', 'Debe seleccionar la fecha de salida.', 'error');
        return false;
    }

    if (tmpDays($("#" + Checkin).val(), $("#" + Checkout).val()) < 1) {
        Swal.fire('', 'La fecha de entrada y de salida no puede ser iguales.', 'error');
        return false;
    }
    console.log(ddlAddHotelRoomSelected);
    console.log(dataAddHotelRoomselected);
    if (GetDropDownValueSelected(ddlAddHotelRoomSelected, dataAddHotelRoomselected) === null) {
        Swal.fire('', 'Debe seleccionar una habitación', 'error');
        return false;
    }
    if (SeachExistsReservacionClient(GetDropDownValueSelected(ddlIDClientSelected, dataClientSelected))) {
        Swal.fire('', 'Ya el cliente posee una reservación.', 'error');
        return false;
    }

    return true;
}
function SeachExistsReservacionClient(IdCar_clientReservation) {

    return reservationList.find(function (reservation) {

  
        return (reservation.IdCard_Client.toLowerCase() === IdCar_clientReservation.toLowerCase() && reservation.Status.toLowerCase()==='reservado' ) ;
    });
}

function GenerateInvoceReservation(button) {
    var ReservationJson = button.getAttribute("data-reservation");
    var reservation = JSON.parse(ReservationJson);
    
    Swal.fire({
        title: '¿Está seguro?',
        text: '¡Que desea facturar esta reservación!',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Facturar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        
        if (result.isConfirmed) {
            mostrarSpinner();
            var parameters = { reservation: reservation };
            $.ajax({
                url: '/Reservation/GenerateInvoceReservation',
                type: 'POST',
                data: parameters,
            }).done(function (response) {
                ocultarSpinner();
                if (response.includes("exitosamente")) {
                    Swal.fire({
                        text: response,
                        icon: 'success',
                        didClose: () => {
                           
                            window.location.href = '/Reservation/Index';
                        }
                    });

                }
                else {
                    Swal.fire('', response, 'warning');
                }
            });
        }

    });

    ocultarSpinner();
}
function GenerateInvoceReservation(button) {
    var ReservationJson = button.getAttribute("data-reservation");
    var reservation = JSON.parse(ReservationJson);
    console.log(reservation);
    Swal.fire({
        title: '¿Está seguro?',
        text: '¡Que desea facturar esta reservación!',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Facturar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {

        if (result.isConfirmed) {
            mostrarSpinner();
            var parameters = { reservation: reservation };
            $.ajax({
                url: '/Reservation/GenerateInvoceReservation',
                type: 'POST',
                data: parameters,
            }).done(function (response) {
                ocultarSpinner();
                if (response.includes("exitosamente")) {
                    Swal.fire({
                        text: response,
                        icon: 'success',
                        didClose: () => {

                            window.location.href = '/Reservation/Index';
                        }
                    });

                }
                else {
                    Swal.fire('', response, 'warning');
                }
            });
        }

    });

    ocultarSpinner();
}
function EndProcess(button) {
    var ReservationJson = button.getAttribute("data-reservation");
    var reservation = JSON.parse(ReservationJson);

    Swal.fire({
        title: '¿Está seguro?',
        text: '¡Que desea finalizar esta reservación!',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Finalizar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {

        if (result.isConfirmed) {
            mostrarSpinner();
            var parameters = { reservation: reservation };
            $.ajax({
                url: '/Reservation/EndProccess',
                type: 'POST',
                data: parameters,
            }).done(function (response) {
                ocultarSpinner();
                if (response.includes("exitosamente")) {
                    Swal.fire({
                        text: response,
                        icon: 'success',
                        didClose: () => {

                            window.location.href = '/Reservation/Index';
                        }
                    });

                }
                else {
                    Swal.fire('', response, 'warning');
                }
            });
        }

    });

    ocultarSpinner();
}

function AddDeposit(button) {


    var ReservationJson = button.getAttribute("data-reservation");
    var reservation = JSON.parse(ReservationJson);
    console.log(reservation);

    localStorage.setItem('currentReservation', JSON.stringify(reservation));



    $('#mdlDeposit').modal('show');


}

function SendDeposit(){
    var DepositAmmount = $('#txtDepositAmmount').val();
    var storedReservation = JSON.parse(localStorage.getItem('currentReservation'));
    if (storedReservation.Currency == "CRC") {
        if (DepositAmmount < 5000) {

            Swal.fire('', 'El valor del depósito debe ser mayor a ₡5.000', 'error');
            return;
        }
        else {
           // if(storedReservation.)
        }

    }
    else {
        if (DepositAmmount < 20) {

            Swal.fire('', 'El valor del depósito debe ser mayor a $20.00', 'error');
            return;
        }

    }

    Swal.fire({
        title: '¿Está seguro?',
        text: '¡Que desea agregar el deposito a la reservación!',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Aceptar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {

        if (result.isConfirmed) {
            mostrarSpinner();
            var parameters = { reservation: storedReservation, Ammount: DepositAmmount };
            $.ajax({
                url: '/Reservation/AddDeposit',
                type: 'POST',
                data: parameters,
            }).done(function (response) {
                ocultarSpinner();
                if (response.includes("exitosamente")) {
                    Swal.fire({
                        text: response,
                        icon: 'success',
                        didClose: () => {

                            window.location.href = '/Reservation/Index';
                        }
                    });

                }
                else {
                    Swal.fire('', response, 'error');
                }
            });
        }

    });

    ocultarSpinner();

}

