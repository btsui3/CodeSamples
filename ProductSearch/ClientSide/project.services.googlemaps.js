// GoogleMaps module

/* --IMPLEMENTATION--
* This module provides basic tools for google maps display on an HTML page.
* When implementing this file, it must be followed by a googleMaps script file with the following form:
* 
* <script async defer
*        src="https://maps.googleapis.com/maps/api/js?key=[YOUR-GOOGLE-API-KEY]&callback=project.map.init">
* </script>
* 
* Where [YOUR-GOOGLE-API-KEY] is the access key provided by Google.
*
* 
* --LOCATION--
* WHILE this tool is bulit to accept plain-text addresses and simple lat&lng combinations, far more 
* sophisticated location plotting (customizable markers, infoWindows, etc) is also supported. 
* By passing this tool a 'location' object -- specified below -- you can better access the power 
* packaged in the Google Maps Api. 
*
* LocationObj = {
*
*   // Optional
*   settings = { 
*
*       [All Marker settings described in the GoogleMaps documentation.]
*       [Do not include POSITION or MAP.]
*       [To see how the Marker is ultimately built, see the 'buildMarker' function below.]
*   },
*
*   // Required*
*   latLng = {
*
*       // These values will build the POSITION property of your eventual Marker. 
*       lat: [LATITUDE],    
*       lng: [LONGITUDE]
*   }
* };
*/
project.services = project.services || {};
project.services.map = project.services.map || {};

// Attatch MAP tool to project namespace 
project.services.map.init = function () {

    // Master page map. HTML element must have id="map_display".
    var map;

    // Master collection of map markers.
    var markers = [];

    // project HQ
    var defaultAddress = "110 Newport Center Drive Newport Beach, Ca";


    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    function initMap(mapId) {
        //console.log("initMap works")
        // Page map element must have an id="map_display".
        mapId = mapId || 'map_display';

        //console.log('current mapId: ', mapId);

        map = map || new google.maps.Map(document.getElementById(mapId), {
           
            zoom: 13,

            // The map is set to default over project HQ
            center: { lat: 33.6189, lng: -117.9289 }
        });
        //console.log('map', map);
    }


    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    function geocodeLatLng(lat, lng) {

        var location = new google.maps.LatLng({ lat: lat, lng: lng });

        map.setCenter(location);

        var marker = new google.maps.Marker({
            map: map,
            position: location
        });
    }



    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    function geocodeAddress(geocoder, resultsMap) {

        geocoder.geocode({ 'address': defaultAddress }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                map.setCenter(results[0].geometry.location);
                var marker = new google.maps.Marker({
                    map: map,
                    position: results[0].geometry.location
                });
            } else {
                alert('Geocode was not successful for the following reason: ' + status);
                console.log('geocode did not pass');
            }
        });
    }



    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    function markAddress(address) {

        // Clear existing markers from map.
        deleteMarkers();
        var geocoder = new google.maps.Geocoder();

        // Set zoom level to 13
        map.setZoom(13);

        geocoder.geocode({ 'address': address }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                map.setCenter(results[0].geometry.location);
                var marker = new google.maps.Marker({
                    map: map,
                    position: results[0].geometry.location
                });

                // Add marker to array of markers.
                markers.push(marker);
            }
        });
    }



    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    function markLatLng(lat, lng) {

        // Clear existing markers from map.
        deleteMarkers();

        // Use lat & lng to create google maps coordinate
        var location = new google.maps.LatLng({ lat: lat, lng: lng });
        console.log('marklatlng has passed');

        map.setCenter(location);

        // Set zoom level to 13
        map.setZoom(13);

        var marker = new google.maps.Marker({
            map: map,
            position: location
        });

        // Add marker to array of markers.
        markers.push(marker);

        // Create infoWindow.
        var largeInfoWindow = new google.maps.InfoWindow();

        // Create an onclick event to open an info window at each marker.
        marker.addListener('click', function () {
            populateInfoWindow(this, largeInfoWindow);
        });

        plotLocation(location);
    }


    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    function plotLocation(location) {

        // Clear existing markers from map.
        deleteMarkers();

        // Set zoom level to 13
        map.setZoom(13);

        map.setCenter(location.latLng);

        var marker = buildMarker(location);

        markers.push(marker);


        console.log('plotLocation has passed');

    }



    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    function plotLocationArray(locations) {

        // Clear existing markers from map.
        deleteMarkers();

        // Set bounds object to set map size
        var bounds = new google.maps.LatLngBounds();

        // The following uses the location array to crate an array of markers on eecution.
        for (var i = 0; i < locations.length; i++) {
            // Create marker per location.
            var marker = buildMarker(locations[i]);

            // Push the marker to our array of markers.
            markers.push(marker);

            // Extends the boundaries of the map for each marker.
            bounds.extend(marker.position);

            // Create infoWindow.
            var largeInfoWindow = new google.maps.InfoWindow();

            // Create an onclick event to open an info window at each marker.
            marker.addListener('click', function () {
                populateInfoWindow(this, largeInfoWindow);
            });
        }
        map.fitBounds(bounds);
        console.log('plotLocationArray');
    }



    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    function populateInfoWindow(marker, infoWindow) {
        // Check to make sure the infoWindow is not already open on this wondow.
        if (infoWindow.marker != marker) {
            infoWindow.marker = marker;
            infoWindow.setContent('<div>Content to add (go to project.GoogleMaps.js)</div>');
            infoWindow.open(map, marker);

            // Make sure the property is cleared if the infoWindow is not open.
            infoWindow.addListener('closeclick', function () {
                infoWindow.setMarker(null);
            });
        }

        console.log('populateIfoWindow  has passed');
    }



    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    function buildMarker(location) {
        console.log("building marker...");
        // Marker settings can be defined in the 'location' object passed to this project.map service. 
        // The 'location' object is illustrated at the top of the page. 

        // Marker settings to be used if none provided
        var defaultSettings = {
            title: "Click Me!",
            //animation: google.maps.Animation.DROP
        };

        // If no settings supplied, use default
        var settings = location.settings != null ? location.settings : defaultSettings;

        settings.map = map;
        settings.position = location.latLng;

        var marker = new google.maps.Marker(settings);

        return marker;
    }



    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    function deleteMarkers() {
        for (var i = 0; i < markers.length; i++) {
            markers[i].setMap(null);
        }
        markers.length = 0;
        console.log('markers have been deleted');
    }



    // =============================================================================================
    // Methods and properties to make publicly available.
    // =============================================================================================
    return {


        init: initMap,

        markAddress: markAddress,

        geocodeLatLng: geocodeLatLng,

        markLatLng: markLatLng,

        plotLocation: plotLocation,

        plotLocationArray: plotLocationArray
    }
    // =============================================================================================
};
