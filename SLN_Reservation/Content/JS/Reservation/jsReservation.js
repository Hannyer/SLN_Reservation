function NewReservation() {
    try {
        if (!ValidateReservation(
            '#ddlAddReservationModal',
            '#txtCheckIn',
            '#txtCheckOut',
            '#txtGuestCount',
            '#ddlAddRoomModal',
            true
        )) return;
        const days = tmpDays($('#txtCheckIn').val(), $('#txtCheckOut').val());
        const description = $('#txtDescription').val().trim();
        var reservation = {
            ClientId: $('#ddlAddReservationModal').val(),
            CheckIn: $('#txtCheckIn').val(),
            CheckOut: $('#txtCheckOut').val(),
            GuestCount: parseInt($('#txtGuestCount').val(), 10),
            RoomId: $('#ddlAddRoomModal').val(),
            Nights: days
        };
        if (description.length > 0) {
            reservation.Description = description;
        }
        console.log(reservation.Description);
        mostrarSpinner();
        $.post('/Reservation/NewReservation', reservation)
            .done(function (response) {
                ocultarSpinner();
                if (response.includes('exitosamente')) {
                    Swal.fire({ text: response, icon: 'success' }).then(() => location.reload());
                } else {
                    Swal.fire('', response, 'warning');
                }
            })
            .fail(function () {
                ocultarSpinner();
                Swal.fire('', 'Error al procesar la reservación. Intente de nuevo.', 'error');
            });
    } catch (e) {
        ocultarSpinner();
        console.error(e);
        Swal.fire('', 'Ocurrió un error inesperado.', 'error');
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

/**
 * Valida los datos mínimos de una reservación: cliente, fechas, huéspedes y habitación.
 *
 * @param {string} clientSel   Selector jQuery para el dropdown de cliente (p. ej. '#ddlAddReservationModal')
 * @param {string} checkInSel  Selector jQuery para la fecha de entrada (p. ej. '#txtCheckIn')
 * @param {string} checkOutSel Selector jQuery para la fecha de salida (p. ej. '#txtCheckOut')
 * @param {string} guestSel    Selector jQuery para la cantidad de huéspedes (p. ej. '#txtGuestCount')
 * @param {string} roomSel     Selector jQuery para el campo oculto de habitación (p. ej. '#ddlAddRoomModal')
 * @param {boolean} checkExisting (opcional) Si es true, también valida que el cliente no tenga ya una reserva activa
 * @returns {boolean} true si todo es válido; false en otro caso (muestra un Swal.error)
 */
function ValidateReservation(clientSel, checkInSel, checkOutSel, guestSel, roomSel, checkExisting = false) {
    var clientId = $(clientSel).val();
    if (!clientId) {
        Swal.fire('', 'Debe seleccionar un cliente.', 'error');
        return false;
    }

    var checkIn = $(checkInSel).val();
    var checkOut = $(checkOutSel).val();
    if (!checkIn) {
        Swal.fire('', 'Debe seleccionar la fecha de entrada.', 'error');
        return false;
    }
    if (!checkOut) {
        Swal.fire('', 'Debe seleccionar la fecha de salida.', 'error');
        return false;
    }
    var days = tmpDays(checkIn, checkOut);
    if (days < 1) {
        Swal.fire('', 'La fecha de salida debe ser posterior a la de entrada.', 'error');
        return false;
    }

    var guestCount = parseInt($(guestSel).val(), 10) || 0;
    if (guestCount < 1) {
        Swal.fire('', 'La cantidad de huéspedes debe ser al menos 1.', 'error');
        return false;
    }

    var roomId = $(roomSel).val();
    if (!roomId) {
        Swal.fire('', 'Debe seleccionar una habitación.', 'error');
        return false;
    }

    //if (checkExisting && SeachExistsReservacionClient(clientId)) {
    //    Swal.fire('', 'El cliente ya tiene una reservación activa.', 'error');
    //    return false;
    //}

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
    Console.log(reservation);
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

