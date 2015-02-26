var mapStyles = [{ featureType: 'water', elementType: 'all', stylers: [{ hue: '#d7ebef' }, { saturation: -5 }, { lightness: 54 }, { visibility: 'on' }] }, { featureType: 'landscape', elementType: 'all', stylers: [{ hue: '#eceae6' }, { saturation: -49 }, { lightness: 22 }, { visibility: 'on' }] }, { featureType: 'poi.park', elementType: 'all', stylers: [{ hue: '#dddbd7' }, { saturation: -81 }, { lightness: 34 }, { visibility: 'on' }] }, { featureType: 'poi.medical', elementType: 'all', stylers: [{ hue: '#dddbd7' }, { saturation: -80 }, { lightness: -2 }, { visibility: 'on' }] }, { featureType: 'poi.school', elementType: 'all', stylers: [{ hue: '#c8c6c3' }, { saturation: -91 }, { lightness: -7 }, { visibility: 'on' }] }, { featureType: 'landscape.natural', elementType: 'all', stylers: [{ hue: '#c8c6c3' }, { saturation: -71 }, { lightness: -18 }, { visibility: 'on' }] }, { featureType: 'road.highway', elementType: 'all', stylers: [{ hue: '#dddbd7' }, { saturation: -92 }, { lightness: 60 }, { visibility: 'on' }] }, { featureType: 'poi', elementType: 'all', stylers: [{ hue: '#dddbd7' }, { saturation: -81 }, { lightness: 34 }, { visibility: 'on' }] }, { featureType: 'road.arterial', elementType: 'all', stylers: [{ hue: '#dddbd7' }, { saturation: -92 }, { lightness: 37 }, { visibility: 'on' }] }, { featureType: 'transit', elementType: 'geometry', stylers: [{ hue: '#c8c6c3' }, { saturation: 4 }, { lightness: 10 }, { visibility: 'on' }] }];


$.ajaxSetup({
    cache: true
});

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Google Map - Homepage
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function createHomepageGoogleMap(_latitude, _longitude, _desarrollos, _filtro) {

    setMapHeight();

    if (document.getElementById('map') != null) {

        var _lat = 19.432601800;
        var _long = -99.133204900;

        if (_desarrollos.length > 0) {
            _lat = _desarrollos[0].Latitud;
            _long = _desarrollos[0].Longitud;
        }

        var map = new google.maps.Map(document.getElementById('map'), {
            zoom: 13,
            scrollwheel: false,
            center: new google.maps.LatLng(_lat, _long),
            mapTypeId: google.maps.MapTypeId.ROADMAP,
            styles: mapStyles
        });

        map.overlayMapTypes.insertAt(0, new jabuWaterMark(new google.maps.Size(256, 256)));

        var _markers = [];
        for (var i = 0; i < _desarrollos.length; i++) {

            var info =
                '<div style="width:200px; height:auto; font-size:10px;">' +
                    '<table>' +
                        '<tr>' +
                            '<td style="font-weight:bold;"> Nombre:</td>' +
                            '<td style="padding-left:10px;">' + _desarrollos[i].Nombre + '</td>' +
                        '</tr>' +
                        '<tr>' +
                            '<td style="font-weight:bold;"> Segmento:</td>' +
                            '<td style="padding-left:10px;">' + _desarrollos[i].TipoSegmento + '</td>' +
                        '</tr>' +
                        '<tr>' +
                            '<td style="font-weight:bold;"> Tipo:</td>' +
                            '<td style="padding-left:10px;">' + _desarrollos[i].TipoConstruccion + '</td>' +
                        '</tr>' +
                        '<tr>' +
                            '<td style="font-weight:bold;"> Superficie:</td>' +
                            '<td style="padding-left:10px;">' + _desarrollos[i].SuperficieConstruccion + '</td>' +
                        '</tr>' +
                        //'<tr>' +
                        //    '<td style="font-weight:bold;"> Inicio:</td>' +
                        //    '<td style="padding-left:10px;">' + _desarrollos[i].FechaInicioVentas + '</td>' +
                        //'</tr>' +
                        '<tr>' +
                            '<td style="font-weight:bold;"> Unidades Totales:</td>' +
                            '<td style="padding-left:10px;">' + _desarrollos[i].UnidadesTotales + '</td>' +
                        '</tr>' +
                        '<tr>' +
                            '<td style="font-weight:bold;"> Unidades Vendidas:</td>' +
                            '<td style="padding-left:10px;">' + _desarrollos[i].UnidadesVendidas + '</td>' +
                        '</tr>' +
                        '<tr>' +
                            '<td style="font-weight:bold;"> Absorci&oacute;n:</td>' +
                            '<td style="padding-left:10px;">' + _desarrollos[i].Absorcion + '</td>' +
                        '</tr>' +
                    '</table>' +
                    '<input type="hidden" value="$' + _desarrollos[i].Id + '$" />' +
                '</div>';

            var strUrlImage = "";
            if (_filtro == "segmento") {
                switch (_desarrollos[i].IdSegmento) {
                    case 1:
                        strUrlImage = "/Content/img/segmento-markers/economica.png";
                        break;
                    case 2:
                        strUrlImage = "/Content/img/segmento-markers/popular.png";
                        break;
                    case 3:
                        strUrlImage = "/Content/img/segmento-markers/tradicional.png";
                        break;
                    case 4:
                        strUrlImage = "/Content/img/segmento-markers/media.png";
                        break;
                    case 5:
                        strUrlImage = "/Content/img/segmento-markers/residencial.png";
                        break;
                    case 6:
                        strUrlImage = "/Content/img/segmento-markers/residencial-plus.png";
                        break;
                }
            }
            else if (_filtro == "tipo") {
                switch (_desarrollos[i].IdTipoConstruccion) {
                    case 1:
                        strUrlImage = "/Content/img/tipo-markers/departamento.png";
                        break;
                    case 2:
                        strUrlImage = "/Content/img/tipo-markers/loft.png";
                        break;
                    case 3:
                        strUrlImage = "/Content/img/tipo-markers/penthouse.png";
                        break;
                    case 4:
                        strUrlImage = "/Content/img/tipo-markers/town-house.png";
                        break;
                    case 5:
                        strUrlImage = "/Content/img/tipo-markers/casa.png";
                        break;
                    case 6:
                        strUrlImage = "/Content/img/tipo-markers/duplex.png";
                        break;
                    case 7:
                        strUrlImage = "/Content/img/tipo-markers/cuadruplex.png";
                        break;
                    case 8:
                        strUrlImage = "/Content/img/tipo-markers/condominio-horizontal.png";
                        break;
                    case 9:
                        strUrlImage = "/Content/img/tipo-markers/villa.png";
                        break;
                }
            }
            else if (_filtro == "superficie") {
                if (_desarrollos[i].SuperficieConstruccion > 0 && _desarrollos[i].SuperficieConstruccion < 101)
                    strUrlImage = "/Content/img/superficie-markers/100.png";
                else if (_desarrollos[i].SuperficieConstruccion >= 101 && _desarrollos[i].SuperficieConstruccion < 151)
                    strUrlImage = "/Content/img/superficie-markers/150.png";
                else if (_desarrollos[i].SuperficieConstruccion >= 151 && _desarrollos[i].SuperficieConstruccion < 201)
                    strUrlImage = "/Content/img/superficie-markers/200.png";
                else if (_desarrollos[i].SuperficieConstruccion >= 201 && _desarrollos[i].SuperficieConstruccion < 301)
                    strUrlImage = "/Content/img/superficie-markers/300.png";
                else if (_desarrollos[i].SuperficieConstruccion >= 301 && _desarrollos[i].SuperficieConstruccion < 501)
                    strUrlImage = "/Content/img/superficie-markers/500.png";
                else if (_desarrollos[i].SuperficieConstruccion >= 501)
                    strUrlImage = "/Content/img/superficie-markers/501.png";
            }

            var image = {
                url: strUrlImage,
                size: new google.maps.Size(22, 27),
                origin: new google.maps.Point(0, 0),
                anchor: new google.maps.Point(0, 32)
            };

            var marker = new google.maps.Marker({
                position: new google.maps.LatLng(_desarrollos[i].Latitud, _desarrollos[i].Longitud),
                map: map,
                icon: image,
                title: _desarrollos[i].Nombre
            });

            var infoWindow = new google.maps.InfoWindow({
                content: info
            });

            var infoWindowsOpened = [];
            google.maps.event.addListener(marker, 'click', (function (marker, infoWindow) {
                return function () {
                    if (infoWindowsOpened.length > 0) {
                        for (var j = 0; j < infoWindowsOpened.length; j++) {
                            infoWindowsOpened[j].close();
                        }

                        infoWindowsOpened = [];
                        infoWindowsOpened.push(infoWindow);
                        infoWindow.open(map, marker);
                    }
                    else {
                        infoWindowsOpened.push(infoWindow);
                        infoWindow.open(map, marker);
                    }

                    var a = infoWindow.getContent();
                    var b = a.substring(a.indexOf("$") + 1);
                    var selectedId = b.substring(0, b.indexOf("$"))

                    $("#item-details").val(selectedId);

                    var selectGritter = $.gritter.add({
                        // (string | mandatory) the heading of the notification
                        title: 'Desarrollo seleccionado.',
                        // (string | mandatory) the text inside the notification
                        text: 'Para ver la informaci&oacute;n espec&iacute;fica del desarrollo seleccionado da cl&iacute;ck a la pesta&ntilde;a de <b>Detalles.</b>',
                        // (bool | optional) if you want it to fade out on its own or just sit there
                        sticky: true,
                        // (int | optional) the time you want it to be alive for before fading out
                        time: '',
                        // (string | optional) the class name you want to apply to that specific message
                        class_name: 'my-sticky-class'
                    });

                    // You can have it return a unique id, this can be used to manually remove it later using
                    setTimeout(function () {

                        $.gritter.remove(selectGritter, {
                            fade: true,
                            speed: 'slow'
                        });

                    }, 6000);

                    $.post("/Dashboard/RegisterEventLogMessage", { message: "Jabu Mapa. Selecciono un marcador en el mapa." });
                    
                };
            })(marker, infoWindow));

            _markers[i] = new Array();
            _markers[i][0] = marker;
            _markers[i][1] = _desarrollos[i].Id;
            _markers[i][2] = _desarrollos[i].IdSegmento;
            _markers[i][3] = _desarrollos[i].IdTipoConstruccion;
            _markers[i][4] = _desarrollos[i].SuperficieConstruccion;
            _markers[i][5] = _desarrollos[i].UnidadesTotales;
            _markers[i][6] = _desarrollos[i].UnidadesVendidas;
            _markers[i][7] = _desarrollos[i].Absorcion;
            _markers[i][8] = _desarrollos[i].PrecioInicial;
            _markers[i][9] = _desarrollos[i].PrecioActualizado;
            _markers[i][10] = _desarrollos[i].PrecioMetroCuadrado;
        }

        var drawingManager = new google.maps.drawing.DrawingManager({
            drawingMode: null,
            drawingControl: true,
            drawingControlOptions: {
                position: google.maps.ControlPosition.TOP_CENTER,
                drawingModes: [
                    google.maps.drawing.OverlayType.CIRCLE,
                    google.maps.drawing.OverlayType.POLYGON
                ]
            }
        });

        drawingManager.setMap(map);

        var isCircle = false;
        var isPolygon = false;
        var POL = [];
        var shape = null;
        var latt = null;
        var lonn = null;
        var circleLtLg;
        var circleRad;

        google.maps.event.addListener(drawingManager, 'overlaycomplete', function (event) {

            if (shape != null) {
                shape.setMap(null);

                shape = event.overlay;
                shape.type = event.type;
            }
            else {
                shape = event.overlay;
                shape.type = event.type;
            }

            switch (shape.type) {
                case google.maps.drawing.OverlayType.CIRCLE:

                    isCircle = true;
                    isPolygon = false;

                    latt = shape.getCenter().lat();
                    lonn = shape.getCenter().lng();

                    circleLtLg = latt + "," + lonn;
                    circleRad = parseFloat(shape.getRadius() / 1000).toFixed(4);
                    
                    //
                    //
                    // Funcion de consulta de desarrollos
                    //
                    //
                    updatePoints(shape, _markers, 'circle');

                    $.post("/Dashboard/RegisterEventLogMessage", { message: "Jabu Mapa. Se creo forma cilcular de selecci\u00f3n de marcadores dentro del mapa." });

                    var infoWindowCircle = new google.maps.InfoWindow({
                        content: "<input style=\"height:40px; width:200px;\" id=\"btnDeleteCircle\" class=\"ui-state-default ui-corner-all\" type=\"button\" value=\"Eliminar\" />"
                    });

                    google.maps.event.addListener(shape, 'click', function (e) {
                        infoWindowCircle.setPosition(e.latLng);
                        infoWindowCircle.open(map);

                        var contenido = document.getElementById('btnDeleteCircle');

                        contenido.addEventListener('click', function (e) {
                            shape.setMap(null);
                            updatePoints(null, _markers, '');
                            infoWindowCircle.close();

                            $.post("/Dashboard/RegisterEventLogMessage", { message: "Jabu Mapa. Se elimino forma cilcular de selecci\u00f3n de marcadores dentro del mapa." });
                        });
                    });

                    google.maps.event.addListener(shape, 'mouseover', function () {
                        shape.setEditable(true);
                    });

                    google.maps.event.addListener(shape, 'mouseout', function () {
                        shape.setEditable(false);
                    });

                    google.maps.event.addListener(shape, 'center_changed', function () {

                        latt = shape.getCenter().lat();
                        lonn = shape.getCenter().lng();

                        circleLtLg = latt + "," + lonn;
                        circleRad = parseFloat(shape.getRadius() / 1000).toFixed(4);

                        //
                        //
                        // Funcion de consulta de desarrollos
                        //
                        //
                        updatePoints(shape, _markers, 'circle');

                        $.post("/Dashboard/RegisterEventLogMessage", { message: "Jabu Mapa. Se modifico la posici\u00f3n de la forma cilcular de selecci\u00f3n de marcadores dentro del mapa." });
                    });

                    google.maps.event.addListener(shape, 'radius_changed', function () {

                        latt = shape.getCenter().lat();
                        lonn = shape.getCenter().lng();

                        circleLtLg = latt + "," + lonn;
                        circleRad = parseFloat(shape.getRadius() / 1000).toFixed(4);

                        //
                        //
                        // Funcion de consulta de desarrollos
                        //
                        //
                        updatePoints(shape, _markers, 'circle');

                        $.post("/Dashboard/RegisterEventLogMessage", { message: "Jabu Mapa. Se modifico el radio de la forma cilcular de selecci\u00f3n de marcadores dentro del mapa." });
                    });

                    break;
                case google.maps.drawing.OverlayType.POLYGON:

                    isPolygon = true;
                    isCircle = false;

                    POL = [];
                    for (var j = 0; j < shape.getPath().getArray().length; j++) {
                        POL.push(shape.getPath().getAt(j).lat().toFixed(5) + "," + shape.getPath().getAt(j).lng().toFixed(5));
                    }

                    //
                    //
                    // Funcion de consulta de desarrollos
                    //
                    //
                    updatePoints(shape, _markers, 'polygon');

                    $.post("/Dashboard/RegisterEventLogMessage", { message: "Jabu Mapa. Se creo forma de pol\u00edgono de selecci\u00f3n de marcadores dentro del mapa." });

                    var infoWindowPolygon = new google.maps.InfoWindow({
                        content: "<input style=\"height:40px; width:200px;\" id=\"btlDeletePolygon\" class=\"ui-state-default ui-corner-all\" type=\"button\" value=\"Eliminar\" />"
                    });
                    google.maps.event.addListener(shape, 'click', function (e) {

                        infoWindowPolygon.setPosition(e.latLng);
                        infoWindowPolygon.open(map);

                        var contenido = document.getElementById('btlDeletePolygon');

                        contenido.addEventListener('click', function (e) {
                            shape.setMap(null);
                            updatePoints(null, _markers, '');
                            infoWindowPolygon.close();

                            $.post("/Dashboard/RegisterEventLogMessage", { message: "Jabu Mapa. Se elimino forma de pol\u00edgono de selecci\u00f3n de marcadores dentro del mapa." });
                        });

                    });

                    google.maps.event.addListener(shape, 'mouseover', function () {
                        shape.setEditable(true);
                    });

                    google.maps.event.addListener(shape, 'mouseout', function () {
                        shape.setEditable(false);
                    });

                    google.maps.event.addListener(shape.getPath(), 'set_at', function () {

                        POL = [];
                        for (var j = 0; j < shape.getPath().getArray().length; j++) {
                            POL.push(shape.getPath().getAt(j).lat().toFixed(5) + "," + shape.getPath().getAt(j).lng().toFixed(5));
                        }

                        //
                        //
                        // Funcion de consulta de desarrollos
                        //
                        //
                        updatePoints(shape, _markers, 'polygon');

                        $.post("/Dashboard/RegisterEventLogMessage", { message: "Jabu Mapa. Se modifico la forma de pol\u00edgono de selecci\u00f3n de marcadores dentro del mapa." });
                    });

                    google.maps.event.addListener(shape.getPath(), 'insert_at', function () {

                        POL = [];
                        for (var j = 0; j < shape.getPath().getArray().length; j++) {
                            POL.push(shape.getPath().getAt(j).lat().toFixed(5) + "," + shape.getPath().getAt(j).lng().toFixed(5));
                        }

                        //
                        //
                        // Funcion de consulta de desarrollos
                        //
                        //
                        updatePoints(shape, _markers, 'polygon');

                        $.post("/Dashboard/RegisterEventLogMessage", { message: "Jabu Mapa. Se modifico forma de pol\u00edgono de selecci\u00f3n de marcadores dentro del mapa." });
                    });

                    break;
            }
        });
    }
}

function jabuWaterMark(tileSize) {
    this.tileSize = tileSize;
}

jabuWaterMark.prototype.getTile = function (coord, zoom, ownerDocument) {
    var img = ownerDocument.createElement('img');
    img.src = '/Content/images/jabu-logo-watermark.png';
    img.style.width = this.tileSize.width + 'px';
    img.style.height = this.tileSize.height + 'px';
    return img;
};

var first_gritter;
var second_gritter;

function updatePoints(shape, markers, shapeType) {
    var _filterIds = [];

    var _acumSuperficie = 0;
    var _acumUnidadesTotales = 0;
    var _acumUnidadesVendidas = 0;
    var _acumUnidadesDisponibles = 0;
    var _acumAbsorcion = 0;
    var _acumPrecioInicial = 0;
    var _acumPrecioActualizado = 0;
    var _acumPrecioMetroCuadrado = 0;

    var _absorcionPromedio = 0;
    var _superficiePromedio = 0;
    var _precioInicialPromedio = 0;
    var _precioActualizadoPromedio = 0;
    var _precioMetroCuadradoPromedio = 0;

    var _menorPrecioInicial = 0;
    var _menorAbsorcion = 0;
    var _menorUnidadesTotales = 0;
    var _menorSuperficie = 0;

    var _mayorPrecioInicial = 0;
    var _mayorAbsorcion = 0;
    var _mayorUnidadesTotales = 0;
    var _mayorSuperficie = 0;

    if (first_gritter) {
        $.gritter.remove(first_gritter, {
            fade: false,
            speed: 'fast'
        });
    }

    if (second_gritter) {
        $.gritter.remove(second_gritter, {
            fade: false,
            speed: 'fast'
        });
    }

    if (shape) {
        var load_gritter = $.gritter.add({
            sticky: true,
            //title: 'Cargando Sumario y Tabla Comparativa',
            title: 'Cargando resum&eacute;n del &aacute;rea',
            text: '<table style="width: 100%;"><tr><td style="text-align: center;"><img src="/Content/images/loading.gif" /></td></tr></table>'
        });
    }

    var countDesarrollosInShape = 0;
    //Ciclo que saca todos los marker en markers[] para verificar cual esta y cual no en un Radio
    for (var i in markers) {
        var _marker = markers[i][0];
        var _id = markers[i][1];
        var _segmento = markers[i][2];
        var _tipo = markers[i][3];
        var _superficie = markers[i][4];
        //var _unidadesTotales = markers[i][5];
        //var _unidadesVendidas = markers[i][6];
        //var _absorsion = markers[i][7];
        //var _precioInicial = markers[i][8];
        //var _precioActualizado = markers[i][9];
        //var _precioMetroCuadrado = markers[i][10];

        if (shape) {
            var _position = _marker.getPosition();
            var _ContainInPoly = shape.containsLatLng(_position); //Variable que regresa un booleano que determina si esta o no

            if (_ContainInPoly) {//Si el marker esta en el Radio
                _acumUnidadesTotales += parseInt(markers[i][5]);
                _acumUnidadesVendidas += parseInt(markers[i][6]);
                _acumAbsorcion += parseInt(markers[i][7]);
                _acumSuperficie += parseFloat(markers[i][4]);
                _acumPrecioInicial += parseFloat(markers[i][8]);
                _acumPrecioActualizado += parseFloat(markers[i][9]);
                _acumPrecioMetroCuadrado += parseFloat(markers[i][10]);

                if (countDesarrollosInShape == 0) {
                    _menorPrecioInicial = parseFloat(markers[i][8]);
                    _menorAbsorcion = parseInt(markers[i][7]);
                    _menorUnidadesTotales = parseInt(markers[i][5]);
                    _menorSuperficie = parseFloat(markers[i][4]);

                    _mayorPrecioInicial = parseFloat(markers[i][8]);
                    _mayorAbsorcion = parseInt(markers[i][7]);
                    _mayorUnidadesTotales = parseInt(markers[i][5]);
                    _mayorSuperficie = parseFloat(markers[i][4]);
                }
                else {
                    _menorPrecioInicial = _menorPrecioInicial < parseFloat(markers[i][8]) ? _menorPrecioInicial : parseFloat(markers[i][8]);
                    _menorAbsorcion = _menorAbsorcion < parseInt(markers[i][7]) ? _menorAbsorcion : parseInt(markers[i][7]);
                    _menorUnidadesTotales = _menorUnidadesTotales < parseInt(markers[i][5]) ? _menorUnidadesTotales : parseInt(markers[i][5]);
                    _menorSuperficie = _menorSuperficie < parseFloat(markers[i][4]) ? _menorSuperficie : parseFloat(markers[i][4]);

                    _mayorPrecioInicial = _mayorPrecioInicial > parseFloat(markers[i][8]) ? _mayorPrecioInicial : parseFloat(markers[i][8]);
                    _mayorAbsorcion = _mayorAbsorcion > parseInt(markers[i][7]) ? _mayorAbsorcion : parseInt(markers[i][7]);
                    _mayorUnidadesTotales = _mayorUnidadesTotales > parseInt(markers[i][5]) ? _mayorUnidadesTotales : parseInt(markers[i][5]);
                    _mayorSuperficie = _mayorSuperficie > parseFloat(markers[i][4]) ? _mayorSuperficie : parseFloat(markers[i][4]);
                }

                setMarkersImages(_marker, _segmento, _tipo, _superficie, true);
                _filterIds.push(_id); //Agrega a la lista que ira al Grid1

                countDesarrollosInShape++;
            }
            else {
                if (shape)
                    setMarkersImages(_marker, _segmento, _tipo, _superficie, false);
            }
        }
        else {
            setMarkersImages(_marker, _segmento, _tipo, _superficie, true);
        }
    }

    if (shape) {

        var titleGritter = '';
        switch (shapeType) {
            case 'circle':
                //titleGritter = 'Sumario de promedios y totales en un radio de ' + String(parseFloat(shape.getRadius() / 1000).toFixed(4)) + ' Km';
                titleGritter = 'Resum&eacute;n del &aacute;rea en un radio de ' + String(parseFloat(shape.getRadius() / 1000).toFixed(4)) + ' Km';
                break;
            case 'polygon':
                //titleGritter = 'Sumario de promedios y totales en un &aacute;rea de ' + String(parseFloat(google.maps.geometry.spherical.computeArea(shape.getPath().getArray()) / (1000 * 1000)).toFixed(2)) + 'Km&sup2';
                titleGritter = 'Resum&eacute;n del &aacute;rea ' + String(parseFloat(google.maps.geometry.spherical.computeArea(shape.getPath().getArray()) / (1000 * 1000)).toFixed(2)) + 'Km&sup2';
                break;
        }

        _acumUnidadesDisponibles = _acumUnidadesTotales - _acumUnidadesVendidas;
        _absorcionPromedio = _acumAbsorcion / countDesarrollosInShape;
        _superficiePromedio = parseFloat(_acumSuperficie / countDesarrollosInShape);
        _precioInicialPromedio = parseFloat(_acumPrecioInicial / countDesarrollosInShape);
        _precioActualizadoPromedio = parseFloat(_acumPrecioActualizado / countDesarrollosInShape);
        _precioMetroCuadradoPromedio = parseFloat(_acumPrecioMetroCuadrado / countDesarrollosInShape);

        //var precioInicialPromedioCurrency = $.formatNumber(String(_precioInicialPromedio), { format: "#,###.00", locale: "us" });
        //var precioActualizadoPromedioCurrency = $.formatNumber(String(_precioActualizadoPromedio), { format: "#,###.00", locale: "us" });
        //var precioMetroCuadradoPromedioCurrency = $.formatNumber(String(_precioMetroCuadradoPromedio), { format: "#,###.00", locale: "us" });
        //var menorPrecioInicialCurrency = $.formatNumber(String(_menorPrecioInicial), { format: "#,###.00", locale: "us" });
        //var mayorPrecioInicialCurrency = $.formatNumber(String(_mayorPrecioInicial), { format: "#,###.00", locale: "us" });
        var precioInicialPromedioCurrency = $.formatNumber(String(_precioInicialPromedio), { format: "#,###", locale: "us" });
        var precioActualizadoPromedioCurrency = $.formatNumber(String(_precioActualizadoPromedio), { format: "#,###", locale: "us" });
        var precioMetroCuadradoPromedioCurrency = $.formatNumber(String(_precioMetroCuadradoPromedio), { format: "#,###", locale: "us" });
        var menorPrecioInicialCurrency = $.formatNumber(String(_menorPrecioInicial), { format: "#,###", locale: "us" });
        var mayorPrecioInicialCurrency = $.formatNumber(String(_mayorPrecioInicial), { format: "#,###", locale: "us" });

        var textGritterTableFirst = '<table>'
                                  + '<tr><th style="text-align:right;">Unidades</th><th style="text-align:right; width:120px;">Total</th></tr>'
                                  + '<tr><td style="text-align:right;">Totales: </td><td style="text-align:right;">' + String(_acumUnidadesTotales) + '</td></tr>'
                                  + '<tr><td style="text-align:right;">Vendidas: </td><td style="text-align:right;">' + String(_acumUnidadesVendidas) + '</td></tr>'
                                  + '<tr><td style="text-align:right;">En Inventario: </td><td style="text-align:right;">' + String(_acumUnidadesDisponibles) + '</td></tr>'
                                  + '<tr><th style="text-align:right;">Promedio</th><th></th></tr>'
                                  + '<tr><td style="text-align:right;">Absorci&oacute;n:</td><td style="text-align:right;">' + String(_absorcionPromedio.toFixed(0)) + '</td></tr>'
                                  + '<tr><td style="text-align:right;">Superficie:</td><td style="text-align:right;">' + String(_superficiePromedio.toFixed(0)) + ' m&sup2;</td></tr>'
                                  + '<tr><td style="text-align:right;">Precio Por Unidad:</td><td style="text-align:right;">$' + String(precioInicialPromedioCurrency) + '</td></tr>'
                                  + '<tr><td style="text-align:right;">Precio Por m&sup2:</td><td style="text-align:right;">$' + String(precioMetroCuadradoPromedioCurrency) + '</td></tr>'
                                  + '</table>';

        var textGritterTableSecond = '<table ><tr><th></th><th style="text-align:right;width:120px;">Menor</th><th style="text-align:right;width:120px;">Mayor</th></tr>'
                                   + '<tr><td style="text-align:right;">Precio: </td><td style="text-align:right;">$' + String(menorPrecioInicialCurrency) + ' </td><td style="text-align:right;">$' + String(mayorPrecioInicialCurrency) + '</td></tr>'
                                   + '<tr><td style="text-align:right;">Absorci&oacute;n: </td><td style="text-align:right;">' + String(_menorAbsorcion) + ' </td><td style="text-align:right;">' + String(_mayorAbsorcion) + ' </td></tr>'
                                   + '<tr><td style="text-align:right;">Unidades: </td><td style="text-align:right;">' + String(_menorUnidadesTotales) + ' </td><td style="text-align:right;">' + String(_mayorUnidadesTotales) + '</td></tr>'
                                   + '<tr><td style="text-align:right;">Superficie: </td><td style="text-align:right;">' + String(_menorSuperficie.toFixed(0)) + ' m&sup2</td><td style="text-align:right;">' + String(_mayorSuperficie.toFixed(0)) + ' m&sup2</td></tr>'
                                   + '</table>';

        setTimeout(function () {


            first_gritter = $.gritter.add({
                // (string | mandatory) the heading of the notification
                title: titleGritter,
                // (string | mandatory) the text inside the notification
                text: textGritterTableFirst,
                // (string | optional) the image to display on the left
                //image: 'http://s3.amazonaws.com/twitter_production/profile_images/132499022/myface_bigger.jpg',
                // (bool | optional) if you want it to fade out on its own or just sit there
                sticky: true,
                // (int | optional) the time you want it to be alive for before fading out
                time: '',
                // (string | optional) the class name you want to apply to that specific message
                class_name: 'my-sticky-class'
            });

            // You can have it return a unique id, this can be used to manually remove it later using
            /*
            setTimeout(function(){
        
                $.gritter.remove(unique_id, {
                    fade: true,
                    speed: 'slow'
                });
        
            }, 6000)
            */

            second_gritter = $.gritter.add({
                // (string | mandatory) the heading of the notification
                title: 'Tabla de Comparaci&oacute;n.',
                // (string | mandatory) the text inside the notification
                text: textGritterTableSecond,
                // (string | optional) the image to display on the left
                //image: 'http://s3.amazonaws.com/twitter_production/profile_images/132499022/myface_bigger.jpg',
                // (bool | optional) if you want it to fade out on its own or just sit there
                sticky: true,
                // (int | optional) the time you want it to be alive for before fading out
                time: '',
                // (string | optional) the class name you want to apply to that specific message
                class_name: 'my-sticky-class'
            });

            // You can have it return a unique id, this can be used to manually remove it later using
            /*
            setTimeout(function(){
        
                $.gritter.remove(unique_id, {
                    fade: true,
                    speed: 'slow'
                });
        
            }, 6000)
            */

            setTimeout(function () {
                if (load_gritter) {
                    $.gritter.remove(load_gritter, {
                        fade: false,
                        speed: 'slow'
                    });
                }
            }, 1000);
            
        }, 3000);
    }

    $("#filtro-ids").val(_filterIds.toString());
}

function setMarkersImages(marker, segmento, tipo, superficie, inPolygon) {
    if ($("input[name=filter]:checked").val() == "segmento" && inPolygon == true) {
        switch (segmento) {
            case 1:
                marker.setIcon("/Content/img/segmento-markers/economica.png");
                break;
            case 2:
                marker.setIcon("/Content/img/segmento-markers/popular.png");
                break;
            case 3:
                marker.setIcon("/Content/img/segmento-markers/tradicional.png");
                break;
            case 4:
                marker.setIcon("/Content/img/segmento-markers/media.png");
                break;
            case 5:
                marker.setIcon("/Content/img/segmento-markers/residencial.png");
                break;
            case 6:
                marker.setIcon("/Content/img/segmento-markers/residencial-plus.png");
                break;
        }
    }
    else if ($("input[name=filter]:checked").val() == "segmento" && inPolygon == false) {
        switch (segmento) {
            case 1:
                marker.setIcon("/Content/img/segmento-markers/economicaTr.png");
                break;
            case 2:
                marker.setIcon("/Content/img/segmento-markers/popularTr.png");
                break;
            case 3:
                marker.setIcon("/Content/img/segmento-markers/tradicionalTr.png");
                break;
            case 4:
                marker.setIcon("/Content/img/segmento-markers/mediaTr.png");
                break;
            case 5:
                marker.setIcon("/Content/img/segmento-markers/residencialTr.png");
                break;
            case 6:
                marker.setIcon("/Content/img/segmento-markers/residencial-plusTr.png");
                break;
        }
    }
    else if ($("input[name=filter]:checked").val() == "tipo" && inPolygon == true) {
        switch (tipo) {
            case 1:
                marker.setIcon("/Content/img/tipo-markers/departamento.png");
                break;
            case 2:
                marker.setIcon("/Content/img/tipo-markers/loft.png");
                break;
            case 3:
                marker.setIcon("/Content/img/tipo-markers/penthouse.png");
                break;
            case 4:
                marker.setIcon("/Content/img/tipo-markers/town-house.png");
                break;
            case 5:
                marker.setIcon("/Content/img/tipo-markers/casa.png");
                break;
            case 6:
                marker.setIcon("/Content/img/tipo-markers/duplex.png");
                break;
            case 7:
                marker.setIcon("/Content/img/tipo-markers/cuadruplex.png");
                break;
            case 8:
                marker.setIcon("/Content/img/tipo-markers/condominio-horizontal.png");
                break;
            case 9:
                marker.setIcon("/Content/img/tipo-markers/villa.png");
                break;
        }
    }
    else if ($("input[name=filter]:checked").val() == "tipo" && inPolygon == false) {
        switch (tipo) {
            case 1:
                marker.setIcon("/Content/img/tipo-markers/departamentoTr.png");
                break;
            case 2:
                marker.setIcon("/Content/img/tipo-markers/loftTr.png");
                break;
            case 3:
                marker.setIcon("/Content/img/tipo-markers/penthouseTr.png");
                break;
            case 4:
                marker.setIcon("/Content/img/tipo-markers/town-houseTr.png");
                break;
            case 5:
                marker.setIcon("/Content/img/tipo-markers/casaTr.png");
                break;
            case 6:
                marker.setIcon("/Content/img/tipo-markers/duplexTr.png");
                break;
            case 7:
                marker.setIcon("/Content/img/tipo-markers/cuadruplexTr.png");
                break;
            case 8:
                marker.setIcon("/Content/img/tipo-markers/condominio-horizontalTr.png");
                break;
            case 9:
                marker.setIcon("/Content/img/tipo-markers/villaTr.png");
                break;
        }
    }
    else if ($("input[name=filter]:checked").val() == "superficie" && inPolygon == true) {
        var _SuperficieConstruccion = superficie;

        if (_SuperficieConstruccion > 0 && _SuperficieConstruccion < 101)
            marker.setIcon("/Content/img/superficie-markers/100.png");
        else if (_SuperficieConstruccion >= 101 && _SuperficieConstruccion < 151)
            marker.setIcon("/Content/img/superficie-markers/150.png");
        else if (_SuperficieConstruccion >= 151 && _SuperficieConstruccion < 201)
            marker.setIcon("/Content/img/superficie-markers/200.png");
        else if (_SuperficieConstruccion >= 201 && _SuperficieConstruccion < 301)
            marker.setIcon("/Content/img/superficie-markers/300.png");
        else if (_SuperficieConstruccion >= 301 && _SuperficieConstruccion < 501)
            marker.setIcon("/Content/img/superficie-markers/500.png");
        else if (_SuperficieConstruccion >= 501)
            marker.setIcon("/Content/img/superficie-markers/501.png");
    }
    else if ($("input[name=filter]:checked").val() == "superficie" && inPolygon == false) {
        var _SuperficieConstruccion = superficie;

        if (_SuperficieConstruccion > 0 && _SuperficieConstruccion < 101)
            marker.setIcon("/Content/img/superficie-markers/100Tr.png");
        else if (_SuperficieConstruccion >= 101 && _SuperficieConstruccion < 151)
            marker.setIcon("/Content/img/superficie-markers/150Tr.png");
        else if (_SuperficieConstruccion >= 151 && _SuperficieConstruccion < 201)
            marker.setIcon("/Content/img/superficie-markers/200Tr.png");
        else if (_SuperficieConstruccion >= 201 && _SuperficieConstruccion < 301)
            marker.setIcon("/Content/img/superficie-markers/300Tr.png");
        else if (_SuperficieConstruccion >= 301 && _SuperficieConstruccion < 501)
            marker.setIcon("/Content/img/superficie-markers/500Tr.png");
        else if (_SuperficieConstruccion >= 501)
            marker.setIcon("/Content/img/superficie-markers/501Tr.png");
    }
}