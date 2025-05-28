function AddUser() {
    var Name = $("#txtNameAdd").val();
    var User = $("#txtUserAdd").val();
    var Password = $("#txtPassAdd").val();
    var Id_Role = $("#ddlAddUserRoles").val();
    var Status = $("#chkAddUser").prop("checked");
  
    var Opcion = 0;

    var UserRequest = { Opcion: Opcion, Id_Role: Id_Role, User: User, Password: Password, Status: Status, Name: Name };
    if (Validate('txtNameAdd', 'txtUserAdd', 'txtPassAdd','lblConfirmPassAdd', 'ddlAddUserRoles',false)) {

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

function ModifyUser() {
    var Name = $("#txtNameModify").val();
    var User = $("#txtUserModify").val();
    var Password = $("#txtPassModify").val();
    var Id_Role = $("#ddlModifyRoles").val();
    var Status = $("#chkModifyUser").prop("checked");
   
    var Opcion = 2;

    var UserRequest = { Opcion: Opcion, Id_Role: Id_Role, User: User, Password: Password, Status: Status, Name: Name };
    if (Validate('txtNameModify', 'txtUserModify', 'txtPassModify','txtConfirmPassModify','ddlModifyRoles',true)) {

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

            var UserRequest = { Opcion: Opcion, User: User.User, Password: User.Password };
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

function Validate(txtName,txtuser,txtPassw,txtConfirmPass,ddlDropDown,EsEditar) {

    if ($("#" + txtName).val() === '') {
        Swal.fire('', 'Debe digitar el nombre de quien pertenece el usuario.', 'error');
        return false;
    }
    if ($("#" + txtuser).val() === '') {
        Swal.fire('', 'Debe digitar un usuario.', 'error');
        return false;
    }
    if ($("#" + txtPassw).val() === '') {
        Swal.fire('', 'Debe digitar una contraseña.', 'error');
        return false;
    }
    else {
        
        if (!isValidPassword($("#" + txtPassw).val())) {
            Swal.fire('', 'El formato de la contraseña no es válido', 'error');
            return false;

        }
    }
    if ($("#" + txtConfirmPass).val() === '') {
        Swal.fire('', 'Debe confirmar la contraseña contraseña.', 'error');
        return false;
    }
    else {
        if ($("#" + txtPassw).val() != $("#" + txtConfirmPass).val()) {
            Swal.fire('', 'Las contraseñas no coinciden, favor validar nuevamente.', 'error');
            $("#" + txtConfirmPass).val('');
            return false;
        }
    }
    if ($("#"+ddlDropDown)[0].selectedIndex<1) {
        Swal.fire('', 'Debe seleccionar un rol para el usuario.', 'error');
        return false;
    }
    if (!EsEditar) {
        var UserFound = SeachExistsUser($("#" + txtuser).val());
        if (UserFound) {
            Swal.fire('', 'Ya se encuentra registrado el usuario: ' + UserFound.User, 'error');
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

