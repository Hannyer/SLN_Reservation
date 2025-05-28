
function NewClient() {
 
    var Full_Name = $("#txtName").val();
    var IdCard = $("#txtIdCard").val();
    var Phone_number1 = iti1.getNumber();
    var Phone_number2 = iti2.getNumber();
    var Mail = $("#txtMail").val();
    var Detail = $("#txtDetail").val();
    var Id_Identificationtype = GetDropDownValueSelected('ddlClient', 'clientList');
    var RateType_Id = GetDropDownValueSelected('ddlAddClientRateType', 'AddRateTypeList');
    var Opcion = 0;
    if (Validate('txtName', 'ddlAddClientRateType', 'AddRateTypeList', 'txtIdCard', iti1, iti2, 'txtMail', 'txtDetail', false, 'ddlClient', 'clientList')) {
        var parameters = { Opcion: Opcion, Full_Name: Full_Name, IdCard: IdCard, Phone_number1: Phone_number1, Phone_number2, Phone_number2, Mail: Mail, Detail: Detail, Id_Identificationtype: Id_Identificationtype, RateType_Id: RateType_Id };
        $.ajax({
            url: '/Client/NewClient',
            type: 'POST',
            data: parameters,
        }).done(function (response) {

            if (response.includes("exitosamente")) {
                Swal.fire({
                    text: response,
                    icon: 'success',
                    didClose: () => {
                        window.location.href = '/Client/Index';
                    }
                });
            }
            else {
                Swal.fire('', response, 'warning');
            }
        });
    }
   
}

function ModifyClient() {
    var Full_Name = $("#txtModifyName").val();
    var IdCard = $("#txtModifyIdCard").val();
    var Phone_number1 = Modifyiti1.getNumber();
    var Phone_number2 = Modifyiti2.getNumber();
    var Mail = $("#txModifytMail").val();
    var Detail = $("#txtModifyDetail").val();
    var Id_Identificationtype = GetDropDownValueSelected('ddlModifyClient', 'ModifyclientList');
    var RateType_Id = GetDropDownValueSelected('ddlModifyClientRateType', 'ModifyRateTypeList');

    var Opcion = 0;
    if (Validate('txtModifyName', 'ddlModifyClientRateType', 'ModifyRateTypeList', 'txtModifyIdCard', Modifyiti1, Modifyiti2, 'txModifytMail', 'txtModifyDetail', true, 'ddlModifyClient', 'ModifyclientList')) {
        var parameters = { Opcion: Opcion, Full_Name: Full_Name, IdCard: IdCard, Phone_number1: Phone_number1, Phone_number2, Phone_number2, Mail: Mail, Detail: Detail, Id_Identificationtype: Id_Identificationtype, RateType_Id: RateType_Id };
        $.ajax({
            url: '/Client/ModifyClient',
            type: 'POST',
            data: parameters,
        }).done(function (response) {

            if (response.includes("éxito")) {
                Swal.fire({
                    text: response,
                    icon: 'success',
                    didClose: () => {
                        window.location.href = '/Client/Index';
                    }
                });

            }
            else {
                Swal.fire('', response, 'warning');
            }
        });
    } 
   
}

function DeleteClient(client) {
    var Client = JSON.parse(client);
    Swal.fire({
        title: '¿Está seguro?',
        text: '¡Que desea borrar este registro!',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Sí, borrarlo',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {



            
            Client.Opcion = 1;

            var ClientRequest = Client;
            $.ajax({
                url: '/Client/DeletClient',
                type: 'POST',
                data: ClientRequest,
            }).done(function (response) {

                if (response.includes("exitosamente")) {
                    Swal.fire({
                        text: response,
                        icon: 'success',
                        didClose: () => {
                            window.location.href = '/Client/Index';
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


function Validate(txtFull_Name, ddlDropDownRateType, dataListRateType, txtIdCard, iti1, iti2, Mail, Detail, EsEditar, ddlDropDownIDType, dataListIDType) {
    if (GetDropDownValueSelected(ddlDropDownRateType, dataListRateType) === null) {
        Swal.fire('', 'Debe seleccionar un tipo de tarifa para el cliente', 'error');
        return false;
    }
    if ($("#" + txtFull_Name).val() === '') {
        Swal.fire('', 'Debe digitar el nombre del cliente.', 'error');
        return false;
    }
    if (GetDropDownValueSelected(ddlDropDownIDType, dataListIDType) === null) {
        Swal.fire('', 'Debe seleccionar el tipo de identificación', 'error');
        return false;
    }

    if ($("#" + txtIdCard).val() === '') {
        Swal.fire('', 'Debe digitar la identificación del cliente.', 'error');
        return false;
    }
    if ($("#" + Mail).val() === '') {
        Swal.fire('', 'Debe digitar un correo electrónico', 'error');
        return false;
    }
    else {
        if (!ValidateEmail($("#" + Mail).val())) {
            Swal.fire('', 'El correo digitado no es válido', 'error');
            return false;
        }
    }
    if (iti1.getNumber()==='') {
        Swal.fire('', 'Debe digitar un número de teléfono.', 'error');
        return false;
    }
    else {
        if (!iti1.isValidNumber()) {
            Swal.fire('', 'El formato del número 1 digitado no es válido.', 'error');
            return false;
        }
    }
    if (iti2.getNumber() != '') {
        if (!iti2.isValidNumber()) {
            Swal.fire('', 'El formato del número 2 digitado no es válido.', 'error');
            return false;
        }
    }
  
    if (!EsEditar) {
        var ClientFound = SeachExistsClient($("#" + txtIdCard).val());
        if (ClientFound) {
            Swal.fire('', 'Ya se encuentra registrado el cliente con la identificación: ' + ClientFound.IdCard, 'error');
            return false;
        }
    }

    return true;
} 

function SeachExistsClient(clientIdCard) {

    return clientList.find(function (client) {
        return client.IdCard.toLowerCase() === clientIdCard.toLowerCase();
    });
}

