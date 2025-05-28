function NewRole() {

    mostrarSpinner();
    var Description = $("#txtDescription").val();
   
    var Opcion = 0;
    if (Description.length > 0) {
        var role = { Opcion: Opcion, Description: Description, Status:true};
        $.ajax({
            url: '/Security/NewRole',
            type: 'POST',
            data: role,
        }).done(function (response) {

            if (response.includes("exitosamente")) {
                Swal.fire({
                    text: response,
                    icon: 'success',
                    didClose: () => {
                        window.location.href = '/Security/RoleIndex';
                    }
                });
            }
            else {
                Swal.fire('', response, 'error');
            }
        });
    } else {
        Swal.fire('', "Debe digitar una descripción", 'error');
    }

    ocultarSpinner();

}


function ModifyRole() {
    var ID_Role = $("#hdfID_Role").val();
    var Description = $("#txtModifyDescription").val();
    var Status = $("#chkModifyRole").prop("checked");
    var Opcion = 0;
    if (Description.length > 0 ) {
        var role = { Opcion: Opcion, ID_Role: ID_Role, Description: Description, Status: Status };
        $.ajax({
            url: '/Security/UpdateRole',
            type: 'POST',
            data: role,
        }).done(function (response) {

            if (response.includes("exitosamente")) {
                Swal.fire({
                    text: response,
                    icon: 'success',
                    didClose: () => {
                        window.location.href = '/Security/RoleIndex';
                    }
                });

            }
            else {
                Swal.fire('', response, 'error');
            }
        });
    } else {
        Swal.fire('', "Debe digitar una descripción", 'error');
    }

}


function DeleteRole(button) {
    var ID_Role = $(button).data('idrole');

   
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
            // Aquí puedes realizar la acción de borrado
           

            var Opcion = 1;

            var role = { Opcion: Opcion, ID_Role: ID_Role };
            $.ajax({
                url: '/Security/DeleteRole',
                type: 'POST',
                data: role,
            }).done(function (response) {

                if (response.includes("exitosamente")) {
                    Swal.fire({
                        text: response,
                        icon: 'success',
                        didClose: () => {
                            window.location.href = '/Security/RoleIndex';
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