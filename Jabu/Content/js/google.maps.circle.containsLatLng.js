// Añade el metodo containsLatLng a la clase google.maps.Circle. 
// En primer lugar, utiliza el recuadro de delimitación de excluir a un punto si 
// ni siquiera está en el cuadro de límite. Si está en el cuadro de límite, 
// entonces se compara la distancia desde el punto del centro, con la radio, 
// y devuelve true sólo si la distancia es más corta que la del radio.

// Una vez añadido el javascript abajo, usted puede llamar al método containsLatLng() en el objeto círculo.

google.maps.Circle.prototype.containsLatLng = function (latLng) {
    return this.getBounds().contains(latLng) && google.maps.geometry.spherical.computeDistanceBetween(this.getCenter(), latLng) <= this.getRadius();
}