function fn_LoadDetailsInitialize() {

    $.gritter.removeAll();

    var _id = $("#item-details").val();
    
    $.ajax({
        type: "POST",
        url: '/Dashboard/Detalles',
        data: { id: _id },
        success: function (detallesResult, status, xhr) {
            $(".panel-detalles").html(detallesResult);
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