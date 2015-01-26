function fn_LoadComparativoInitialize() {

    $.gritter.removeAll();

    var _idsComparativo = $("#items-compared").val();
    var _idsPromedio = $("#filtro-ids").val();

    $.ajax({
        type: "POST",
        url: '/Dashboard/Comparativo',
        data: {
            IdsPromedio: _idsPromedio,
            IdsComparativo: _idsComparativo
        },
        success: function (detallesResult, status, xhr) {
            $(".panel-comparativo").html(detallesResult);
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