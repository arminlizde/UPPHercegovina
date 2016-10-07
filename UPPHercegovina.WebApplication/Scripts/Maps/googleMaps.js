var initMap = function(location, markers) {
    console.log("location", location);
    var map = new google.maps.Map(document.getElementById('map'), {
        center: { lat: location.Latitude, lng: location.Longitude },
        zoom: 8
    });

    markers.foreach(function (marker) {
        var newMarker = new google.maps.Marker({
            position: { lat: marker.GeographicPosition.Latitude, lng: marker.GeographicPosition.Longitude },
            animation: google.maps.Animation.DROP,
            map: map,
            title: marker.Name
        });
        toggleBounce(marker);
        marker.addListener('click', toggleBounce);
        marker.setMap(map);
    });

    function toggleBounce(marker) {
        if (marker.getAnimation() !== null) {
            marker.setAnimation(null);
        } else {
            marker.setAnimation(google.maps.Animation.BOUNCE);
        }
    }
}