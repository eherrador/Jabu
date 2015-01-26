function fn_LoadUsuariosInitialize() {

    $.gritter.removeAll();

    $.ajax({
        type: "GET",
        url: '/Account/Register',
        success: function (registerForm, status, xhr) {

            $("#user-register").html(registerForm);
            fn_FormSubmitRegister();
            
        },
        error: function (xhr, status, err) {
            if (xhr.status == 401) {
                window.location.replace('/Account/Login');
            }
            else {
                alert("Error: " + err);
            }
        }
    });

    fn_ObtenerUsuariosRoles();

}

function fn_FormSubmitRegister() {
    $('#frmRegister').submit(function () {

        $.validator.unobtrusive.parse($('#frmRegister'));

        if ($(this).valid()) {
            $.ajax({
                url: this.action,
                type: this.method,
                data: $(this).serialize(),
                success: function (result, status, xhr) {

                    $("#user-register").html(result);
                    fn_ObtenerUsuariosRoles();
                    alert("El Usuario se registro con exito.");
                    fn_FormSubmitRegister();
                },
                error: function (xhr, status, err) {
                    if (xhr.status == 401) {
                        window.location.replace('/Account/Login');
                    }
                    else {
                        alert("Error: " + err);
                    }
                }
            });
        }
        return false;
    });
}

function fn_ObtenerUsuariosRoles() {

    $.ajax({
        type: "GET",
        url: '/Account/GetUsersAndRoles',
        success: function (result, status, xhr) {
            $("#users-admin").html(result);
        },
        error: function (xhr, status, err) {
            if (xhr.status == 401) {
                window.location.replace('/Account/Login');
            }
            else {
                alert("Error: " + err);
            }
        }
    });

}

function fn_EditarUsuario(idUser) {

    $.ajax({
        type: "GET",
        url: '/Account/EditUser',
        data: { id: idUser },
        success: function (result, status, xhr) {

            $("#user-register").html(result);
            fn_FormSubmitRegister();

        },
        error: function (xhr, status, err) {
            if (xhr.status == 401) {
                window.location.replace('/Account/Login');
            }
            else {
                alert("Error: " + err);
            }
        }
    });

}

function fn_EliminarUsuario(idUser, nameUser) {

    var res = confirm("El usuario " + nameUser + " ser\u00e1 eliminado favor de confirmar operaci\u00f3.");

    if (res == true) {
        $.ajax({
            type: "POST",
            url: '/Account/DeleteUser',
            data: { id: idUser },
            success: function (result, status, xhr) {
                $("#users-admin").html(result);
            },
            error: function (xhr, status, err) {
                if (xhr.status == 401) {
                    window.location.replace('/Account/Login');
                }
                else {
                    alert("Error: " + err);
                }
            }
        });
    }

}

function fn_CancelarRegistro() {

    $.ajax({
        type: "GET",
        url: '/Account/Register',
        success: function (registerForm, status, xhr) {

            $("#user-register").html(registerForm);
            fn_FormSubmitRegister();

        },
        error: function (xhr, status, err) {
            if (xhr.status == 401) {
                window.location.replace('/Account/Login');
            }
            else {
                alert("Error: " + err);
            }
        }
    });

}