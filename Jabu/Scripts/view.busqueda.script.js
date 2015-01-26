$(document).ready(function () {
    createHomepageGoogleMap(0, 0, _desarrollos, $("input[name=filter]:checked").val());

    $("#ddl-estado").change(function () {
        $("#ddl-estado option:selected").each(function () {
            if ($(this).val() == "") {
                $("#ddl-demarcacion").selectpicker('val', '');
                $("#ddl-demarcacion").attr("disabled", "disabled").selectpicker('refresh');
            }
            else {
                var _options = '';
                var _id = parseInt($(this).val());
                $.ajax({
                    type: "POST",
                    url: '/Dashboard/ObtenerDemarcacionesPorEstado',
                    data: { id: _id },
                    success: function (demarcaciones, status, xhr) {
                        $.each(demarcaciones, function (i, item) {
                            _options = _options + "<option value='" + demarcaciones[i].Id + "'>" + demarcaciones[i].Nombre + "</option>";
                        });

                        $("#ddl-demarcacion").html(_options).selectpicker('refresh');
                        $("#ddl-demarcacion").removeAttr("disabled").selectpicker('refresh');
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
        });
    });

    $("#btn-buscar").click(function () {

        var id_estado = 0;
        if ($("#ddl-estado option:selected").val() != "")
            id_estado = $("#ddl-estado option:selected").val();

        var ids_demarcaciones = [];
        if ($("#ddl-demarcacion").val() != null)
            ids_demarcaciones = $("#ddl-demarcacion").val();

        var ids_segmentos = [];
        if ($("#ddl-segmento").val() != null)
            ids_segmentos = $("#ddl-segmento").val();

        var ids_tipos = [];
        if ($("#ddl-tipo").val() != null)
            ids_tipos = $("#ddl-tipo").val();

        var ids_superficies = [];
        if ($("#ddl-superficie").val() != null)
            ids_superficies = $("#ddl-superficie").val();

        $.ajax({
            type: "POST",
            url: '/Dashboard/ObtenerDesarrollos',
            data: {
                id_estado: id_estado,
                ids_demarcaciones: ids_demarcaciones,
                ids_segmentos: ids_segmentos,
                ids_tipos: ids_tipos,
                ids_superficies: ids_superficies
            },
            success: function (desarrollos, status, xhr) {
                _desarrollos = desarrollos;
                createHomepageGoogleMap(0, 0, _desarrollos, $("input[name=filter]:checked").val());
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
    });

    $("input[name=filter]").change(function () {

        if ($("input[name=filter]:checked").val() == "segmento") {
            createHomepageGoogleMap(0, 0, _desarrollos, $("input[name=filter]:checked").val());

            $(".segmento").css("display", "block");
            $(".tipo").css("display", "none");
            $(".superficie").css("display", "none");
        }
        else if ($("input[name=filter]:checked").val() == "tipo") {
            createHomepageGoogleMap(0, 0, _desarrollos, $("input[name=filter]:checked").val());

            $(".segmento").css("display", "none");
            $(".tipo").css("display", "block");
            $(".superficie").css("display", "none");
        }
        else if ($("input[name=filter]:checked").val() == "superficie") {
            createHomepageGoogleMap(0, 0, _desarrollos, $("input[name=filter]:checked").val());

            $(".segmento").css("display", "none");
            $(".tipo").css("display", "none");
            $(".superficie").css("display", "block");
        }

    });

    $(".find-filters").draggable({
        containment: "#map",
        handle: ".filter-move"
    });
});