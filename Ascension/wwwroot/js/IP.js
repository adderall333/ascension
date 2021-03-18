window.onload = function() {
    var x1 = document.getElementById("demo");

    document.getElementById("get-btn").addEventListener('click', getLocation);
    
    function getLocation() {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(showPosition);
        } else {
            x1.innerHTML = "Geolocation is not supported by this browser.";
        }
    }

    function showPosition(position) {
        fetch('/geoapi/get?lat=' + position.coords.latitude +'&lot='+ position.coords.longitude)
            .then(x => x.json())
            .then(x => x1.innerHTML = x.city)
    }
}