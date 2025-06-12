function AddUser() {
    var Name = $("#txtNameAdd").val();
    var User = $("#txtUserAdd").val(); 
    var Password = $("#txtPassAdd").val();
    var Id_Role = $("#ddlAddUserRoles").val();
    var Status = $("#chkAddUser").prop("checked");
    var Email = $("#txtEmailAdd").val();
    var PhoneNumber = $("#txtPhoneAdd").val();
    var DocumentID = $("#txtDocumentIdAdd").val();
    var IdIdentificationType = $("#ddlAddIdentificationType").val();

    var Opcion = 0;

    var UserRequest = { Opcion: Opcion, Id_Role: Id_Role, User: User, Password: Password, Status: Status, Name: Name, Email: Email, PhoneNumber: PhoneNumber, DocumentID: DocumentID, IdIdentificationType: IdIdentificationType };
    if (Validate('txtNameAdd', 'txtPassAdd', 'lblConfirmPassAdd', 'ddlAddUserRoles', 'txtEmailAdd', 'txtPhoneAdd', 'txtDocumentIdAdd', 'ddlAddIdentificationType', false)) {

        $.ajax({
            url: '/User/AddUser',
            type: 'POST',
            data: UserRequest,
        }).done(function (response) {
            if (response.includes("exitosamente")) {
                Swal.fire({
                    text: response,
                    icon: 'success',
                    didClose: () => {
                        window.location.href = '/User/Index';
                    }
                });
            } else {
                Swal.fire('', response, 'error');
            }
        });
    } else {
        return;
    }
}

function ModifyUser() {
    var Name = $("#txtNameModify").val();
    var User = $("#txtUserModify").val();
    var Password = $("#txtPassModify").val();
    var Id_Role = $("#ddlModifyRoles").val();
    var Status = $("#chkModifyUser").prop("checked");
    var Email = $("#txtEmailModify").val();
    var PhoneNumber = $("#txtPhoneModify").val();
    var DocumentID = $("#txtDocumentIdModify").val();
    var IdIdentificationType = $("#ddlModifyIdentificationType").val();
    var ID = $("#txtIdUserModify").val();

    var Opcion = 2;

    var UserRequest = { Opcion: Opcion, Id_Role: Id_Role, User: User, Password: Password, Status: Status, Name: Name, Email: Email, PhoneNumber: PhoneNumber, DocumentID: DocumentID, IdIdentificationType: IdIdentificationType, ID: ID };
    if (Validate('txtNameModify', 'txtPassModify', 'txtConfirmPassModify', 'ddlModifyRoles', 'txtEmailModify', 'txtPhoneModify', 'txtDocumentIdModify', 'ddlModifyIdentificationType', true)) {

        $.ajax({
            url: '/User/UpdateUser',
            type: 'POST',
            data: UserRequest,
        }).done(function (response) {

            if (response.includes("exitosamente")) {
                Swal.fire({
                    text: response,
                    icon: 'success',
                    didClose: () => {
                        window.location.href = '/User/Index';
                    }
                });

            }
            else {
                Swal.fire('', response, 'error');
            }
        });
    }
    else {
        return;
    }



}

function DeleteUser(user) {
    var User =JSON.parse(user);

    console.log(User);
    console.log(User.User);
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
           


            var Opcion = 1;

            var UserRequest = { Opcion: Opcion, User: User.User, Password: User.Password, ID: User.ID };
            $.ajax({
                url: '/User/DeleteUser',
                type: 'POST',
                data: UserRequest,
            }).done(function (response) {

                if (response.includes("exitosamente")) {
                    Swal.fire({
                        text: response,
                        icon: 'success',
                        didClose: () => {
                            window.location.href = '/User/Index';
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

function Validate(txtName, txtPassw, txtConfirmPass, ddlDropDown, txtEmail, txtPhone, txtDocumentId, ddlIdentificationType, EsEditar) {

    if ($("#" + txtName).val() === '') {
        Swal.fire('', 'Debe digitar el nombre de quien pertenece el usuario.', 'error');
        return false;
    }

    // ELIMINADA la validación de txtUser, ya que no es un parámetro

    if ($("#" + txtEmail).val() === '') {
        Swal.fire('', 'Debe digitar un correo electrónico.', 'error');
        return false;
    }

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test($("#" + txtEmail).val())) {
        Swal.fire('', 'El formato del correo electrónico no es válido.', 'error');
        return false;
    }


    if ($("#" + txtPhone).val() === '') {
        Swal.fire('', 'Debe digitar un número de teléfono de 8 dígitos.', 'error');
        return false;
    }
    const phoneRegex = /^[0-9]{8}$/;
    if (!phoneRegex.test($("#" + txtPhone).val())) {
        Swal.fire('', 'El número de teléfono debe contener exactamente 8 dígitos numéricos.', 'error');
        return false;
    }

    if ($("#" + txtDocumentId).val() === '') {
        Swal.fire('', 'Debe digitar la cédula o pasaporte.', 'error');
        return false;
    }

    var documentID = $("#" + txtDocumentId).val();
    // Esta línea ahora debería ser correcta porque ddlIdentificationType recibe el ID correcto.
    var selectedIdTypeId = $("#" + ddlIdentificationType).val();

    if (!isValidDocumentID(documentID, selectedIdTypeId)) {
        Swal.fire('', 'El formato del documento de identificación no es válido para el tipo seleccionado.', 'error');
        return false;
    }

    if ($("#" + txtPassw).val() === '') {
        Swal.fire('', 'Debe digitar una contraseña.', 'error');
        return false;
    }
    else {
        if (!isValidPassword($("#" + txtPassw).val())) {
            Swal.fire('', 'El formato de la contraseña no es válido (Debe contener al menos 8 caracteres, una mayúscula, una minúscula y un número).', 'error');
            return false;
        }
    }
    if ($("#" + txtConfirmPass).val() === '') {
        Swal.fire('', 'Debe confirmar la contraseña.', 'error');
        return false;
    }
    else {
        if ($("#" + txtPassw).val() != $("#" + txtConfirmPass).val()) {
            Swal.fire('', 'Las contraseñas no coinciden, favor validar nuevamente.', 'error');
            $("#" + txtConfirmPass).val('');
            return false;
        }
    }
    if ($("#" + ddlDropDown)[0].selectedIndex < 1) {
        Swal.fire('', 'Debe seleccionar un rol para el usuario.', 'error');
        return false;
    }
    if (!EsEditar) {
        var EmailFound = SeachExistsEmail($("#" + txtEmail).val());
        if (EmailFound) {
            Swal.fire('', 'Ya se encuentra registrado un usuario con el correo electrónico: ' + EmailFound.Email, 'error');
            return false;
        }
    }

    return true;
}
function isValidPassword(password) {
    const regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[A-Za-z\d]{8,}$/;
    return regex.test(password);
}
function SeachExistsUser(user) { 

    return userList.find(function (usuario) {
        return usuario.User.toLowerCase() === user.toLowerCase();
    });
}
function SeachExistsEmail(email) {
    return userList.find(function (usuario) {
        return usuario.Email.toLowerCase() === email.toLowerCase();
    });
}

var identificationTypeDescriptions = {
    "1": "cedula fisica",
    "2": "cedula juridica",
    "3": "dimex/pasaporte",
    "4": "nite"

};

function isValidDocumentID(docId, identificationTypeID) {
    if (!docId || docId.trim() === '') return false;
    if (!identificationTypeID) return false;

    const cedulaFisicaRegex = /^(\d-\d{4}-\d{4}|\d{9})$/;
    const cedulaJuridicaRegex = /^\d-\d{3}-\d{6}$/;
    const dimexPasaporteRegex = /^[A-Z0-9]{6,15}$/i;
    const niteRegex = /^\d{10}$/;

    const normalizedDescription = identificationTypeDescriptions[identificationTypeID];

    if (!normalizedDescription) return false;

    if (normalizedDescription === "cedula fisica") {
        return cedulaFisicaRegex.test(docId);
    } else if (normalizedDescription === "cedula juridica") {
        return cedulaJuridicaRegex.test(docId);
    } else if (normalizedDescription === "dimex/pasaporte") {
        return dimexPasaporteRegex.test(docId);
    } else if (normalizedDescription === "nite") {
        return niteRegex.test(docId);
    }

    return false;
}

