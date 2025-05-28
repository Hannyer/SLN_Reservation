function NewJobs() {
    var TaskTypeName = GetDropDownTextContentSelected('ddlAddTaskTypeModal', 'AddTaskTypeList')
    var Type = GetDropDownValueSelected('ddlAddTaskTypeModal', 'AddTaskTypeList')
    var Description = $("#txtDescription").val();
    var Date = $("#dtDate").val();
    var Status = 0;
    var Collaborator = GetDropDownValueSelected('ddlAddCollaboratorModal','AddCollaboratorList');
    var Frequency = 0;
    if (TaskTypeName.toLowerCase() === "especial") {
        Frequency = $("#txtFrequency").val();
    }
   
    var Opcion = 0;
    if (Validate('ddlAddTaskTypeModal', 'AddTaskTypeList', 'txtDescription', 'dtDate', 'txtCollaborator','txtFrequency')) {
        var dailyJob = { Type: Type, Description: Description, Date: [Date], Status: Status, Collaborator: Collaborator, Frequency: Frequency, Opcion: Opcion };
        $.ajax({
            url: '/DailyJobs/NewJobs',
            type: 'POST',
            data: dailyJob,
        }).done(function (response) {

            if (response.includes("éxito")) {
                Swal.fire({
                    text: response,
                    icon: 'success',
                    didClose: () => {
                        window.location.href = '/DailyJobs/Index';
                    }
                });
            }
            else {
                Swal.fire('', response, 'warning');
            }
        });
    } else {
        Swal.fire('', "Debe digitar todos los elementos solicitados", 'warning');
    }

}

function ModifyJobs() {
    var TaskTypeName = GetDropDownTextContentSelected('ddlModifyTaskTypeModal', 'ModifyTaskTypeList')
   
    var Id = $("#hdfIdModifyDailyJob").val();
    var Type = GetDropDownValueSelected('ddlModifyTaskTypeModal', 'ModifyTaskTypeList')
    var Description = $("#txtDescriptionModify").val();
    var Date = $("#dtDateModify").val();
    var Status = 0;
    var Collaborator = GetDropDownValueSelected('ddlModifyCollaboratorModal','ModifyCollaboratorList');
    var Frequency = 0;
    
    if (TaskTypeName.toLowerCase() === "especial") {
        Frequency = $("#txtFrequencyModify").val();
    }
    var Opcion = 2;
   
    if (Validate('ddlModifyTaskTypeModal', 'ModifyTaskTypeList', 'txtDescriptionModify', 'dtDateModify', 'txtCollaboratorModify','txtFrequencyModify')) {
        var dailyJob = { Opcion: Opcion, Id: Id, Type: Type, Description: Description, Date, Date, Status: Status, Collaborator: Collaborator, Frequency: Frequency };
        $.ajax({
            url: '/DailyJobs/ModifyJobs',
            type: 'POST',
            data: dailyJob,
        }).done(function (response) {

            if (response.includes("éxito")) {
                Swal.fire({
                    text: response,
                    icon: 'success',
                    didClose: () => {
                        window.location.href = '/DailyJobs/Index';
                      
                    }
                });

            }
            else {
                Swal.fire('', response, 'warning');
            }
        });
    } 
   
}


function ToEndJobs() {
    var P_TYPE = "";
    var P_DESCRIPTION = "";
    var P_DATE = "";
    var P_STATUS = 1;
    var P_COLLABORATOR = "";
    var P_FREQUENCY = "";
    var P_OPCION = 3;
    var P_ID = $("#txtTmpID").val();
    if (P_ID.length > 0) {
        var parameters = { P_OPCION: P_OPCION, P_ID: P_ID, P_TYPE: P_TYPE, P_DESCRIPTION: P_DESCRIPTION, P_DATE, P_DATE, P_STATUS: P_STATUS, P_COLLABORATOR: P_COLLABORATOR, P_FREQUENCY: P_FREQUENCY };
        $.ajax({
            url: '/DailyJobs/ToEndJobs',
            type: 'POST',
            data: parameters,
        }).done(function (response) {

            if (response.includes("éxito")) {
                Swal.fire({
                    text: response,
                    icon: 'success',
                    didClose: () => {
                        window.location.href = '/DailyJobs/Index';
                    }
                });

            }
            else {
                Swal.fire('', response, 'warning');
            }
        });
    } else {
        Swal.fire('', "Debe Seleccionar una Tarea para Finalizar", 'warning');
    }

}


function DeleteJobs(dailyJob) {
    console.log(dailyJob);

   
        // Utiliza SweetAlert para mostrar una alerta de confirmación
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
                var Parameters = { dailyJob: dailyJob }
                $.ajax({
                    url: '/DailyJobs/DeleteJobs',
                    type: 'POST',
                    data: Parameters,
                }).done(function (response) {

                    if (response.includes("éxito")) {
                        Swal.fire({
                            text: response,
                            icon: 'success',
                            didClose: () => {
                                window.location.href = '/DailyJobs/Index';
                            }
                        });

                    }
                    else {
                        Swal.fire('', response, 'warning');
                    }
                });
            }
        });// Abre el modal con el id "myModal"
  

}



function Validate(dropdownList,dataSelected, txtDescription, dtDate, txtCollaborator, txtFrequency) {
    if (GetDropDownValueSelected(dropdownList, dataSelected) === null) {
        Swal.fire('', 'Debe seleccionar un tipo de tarea', 'error');
        return false;
    }
    if ($("#" + txtCollaborator).val() === '') {
        Swal.fire('', 'Debe el nombre del colaborador.', 'error');
        return false;
    }
  
    if ($("#" + dtDate).val() === '') {
        Swal.fire('', 'Debe seleccionar la fecha de la primer ejecución.', 'error');
        return false;
    }

   
    
    if (GetDropDownTextContentSelected(dropdownList, dataSelected) === 'Especial') {
        if ($("#" + txtFrequency).val() === '') {
            Swal.fire('', 'Debe digitar la frecuencia con la que se ejecutará la tarea.', 'error');
            return false;
        }
    }
    if ($("#" + txtDescription).val() === '') {
        Swal.fire('', 'Debe digitar la descripción de la tarea .', 'error');
        return false;
    }
   

    return true;
}







