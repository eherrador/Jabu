var _InitIds = "";
var _isFirstLoad = true;

function fn_LoadListInitialize() {

    $.gritter.removeAll();

    var _ids = $("#filtro-ids").val();

    if (_ids != _InitIds) {
        $("#items-compared").val("");
        $("#tbl-container").empty();
    }
    else {
        $("#grd-desarrollos tr").removeClass("tr-list-selected");
        
        var _idSelectedOnList = $("#item-details").val();
        if (_idSelectedOnList != "") {
            $("#grd-desarrollos tr").find(".grd-cell-id").filter(function () {
                if ($(this).text() == _idSelectedOnList)
                    return $(this).text() == _idSelectedOnList;
            }).parent().addClass("tr-list-selected");
        }
    }

    if (_isFirstLoad == true || _ids != _InitIds) {
        $.ajax({
            type: "POST",
            url: '/Dashboard/Listado',
            data: {
                ids: _ids,
                pagina: 1,
                ordenacion: 'desarrollo',
                direccion: 'ASC'
            },
            success: function (gridResultList, status, xhr) {
                _InitIds = _ids;
                _isFirstLoad = false;

                $("#grd-container").html(gridResultList);
                fn_DragAndDropInDropZone();
                fn_SelectedRowInTable();
            },
            error: function (xhr, status, err) {
                if(xhr.status == 401)
                {
                    window.location.replace('/Account/Login');
                }
                else
                {
                    alert("Error: " + err);
                }
            }
        });
    }
}

function fn_TableSorting(sortname) {
    var _columnSort = $("#column-sort").val();
    var _sortDirection = $("#sort-direction").val();

    if (sortname == _columnSort) {
        if (_sortDirection == "ASC") {
            $("#sort-direction").val("DESC");
            _sortDirection = "DESC";
        }
        else if (_sortDirection == "DESC") {
            $("#sort-direction").val("ASC");
            _sortDirection = "ASC";
        }
    }
    else {
        $("#column-sort").val(sortname);
        $("#sort-direction").val("ASC");

        _columnSort = sortname;
        _sortDirection = "ASC";
    }

    var _ids = $("#filtro-ids").val();

    $.ajax({
        type: "POST",
        url: '/Dashboard/Listado',
        data: {
            ids: _ids,
            pagina: 1,
            ordenacion: _columnSort,
            direccion: _sortDirection
        },
        success: function (gridResultList, status, xhr) {
            $("#grd-container").html(gridResultList);
            fn_DragAndDropInDropZone();
            fn_SelectedRowInTable();
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

function fn_TablePaging(_page) {

    var _ids = $("#filtro-ids").val();
    $.ajax({
        type: "POST",
        url: '/Dashboard/Listado',
        data: {
            ids: _ids,
            pagina: _page,
            ordenacion: $("#column-sort").val(),
            direccion: $("#sort-direction").val()
        },
        success: function (gridResultList, status, xhr) {
            $("#grd-container").html(gridResultList);
            fn_DragAndDropInDropZone();
            fn_SelectedRowInTable();
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

function fn_DragAndDropInDropZone() {
    var c = {};
    $("#grd-desarrollos tr").draggable({
        helper: "clone",
        start: function (event, ui) {
            c.helper = ui.helper;

            $("#grd-desarrollos tr").removeClass("tr-list-selected");
            $("#item-details").val("");

            var _selected = $(this).hasClass("tr-list-selected");
            if (!_selected) {
                $(this).addClass("tr-list-selected");

                var _idSelected = $(this).find(".grd-cell-id").text();
                $("#item-details").val(_idSelected);
            }
        }
    });

    $("#tbl-container").droppable({
        hoverClass: "hover-zone-drop",
        tolerance: "pointer",
        drop: function (event, ui) {
            var _rowdiv = "<div id='id-" + $(c.helper).find(".grd-cell-id").text() + "' class='drop-zone-group'><ul><li><span>" + $(c.helper).find(".grd-cell-nombre").text() + "</span></li>";
            _rowdiv = _rowdiv + "<li>Segmento: " + $(c.helper).find(".grd-cell-segmento").text() + "</li>";
            _rowdiv = _rowdiv + "<li>Superficie: " + $(c.helper).find(".grd-cell-superficie").text() + "</li></ul>";
            _rowdiv = _rowdiv + "<a href='javascript:fn_CloseItemInDropZone(\"id-" + $(c.helper).find(".grd-cell-id").text() + "\",\"" + $(c.helper).find(".grd-cell-id").text() + "\")'>Cerrar</a></div>";

            var _items = $("#items-compared").val();
            if (_items != "" && _items.split(",").length < 3) {
                var _isExists = false;
                var _ids = _items.split(",");
                for (var i in _ids) {
                    if (_ids[i].toString() == $(c.helper).find(".grd-cell-id").text())
                        _isExists = true;
                }

                if (!_isExists) {
                    _items = _items + "," + $(c.helper).find(".grd-cell-id").text();
                    $("#items-compared").val(_items);
                    $(this).append(_rowdiv);
                }
                else
                    alert("El elemento ya existe en la lista comparativa.");
            }
            else if (_items == "") {
                $("#items-compared").val($(c.helper).find(".grd-cell-id").text());
                $(this).append(_rowdiv);
            }
            else {
                alert("Solo se puede realizar la comparación con 3 elementos.");
            }
        }
    });
}

function fn_CloseItemInDropZone(itemId, id) {
    var _itemId = "#" + itemId;
    $(_itemId).remove();

    var _ids = $("#items-compared").val().split(",");
    for (var i in _ids) {
        if (_ids[i].toString() == id.toString()) {
            _ids.splice(i, 1);
        }
    }
    $("#items-compared").val(_ids.toString());
}

function fn_SelectedRowInTable() {
    $("#grd-desarrollos tr").click(function () {
        $("#grd-desarrollos tr").removeClass("tr-list-selected");
        $("#item-details").val("");

        var _selected = $(this).hasClass("tr-list-selected");
        if (!_selected) {
            $(this).addClass("tr-list-selected");

            var _idSelected = $(this).find(".grd-cell-id").text();
            $("#item-details").val(_idSelected);
        }
    });

    var _idSelectedOnList = $("#item-details").val();
    if (_idSelectedOnList != "") {
        $("#grd-desarrollos tr").find(".grd-cell-id").filter(function () {
            if ($(this).text() == _idSelectedOnList)
                return $(this).text() == _idSelectedOnList;
        }).parent().addClass("tr-list-selected");
    }
}