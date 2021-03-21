const CITY_KEY = 'x-city';
const setCity = city => localStorage.setItem(CITY_KEY, city);
const getCity = () => localStorage.getItem(CITY_KEY);

const updateCityElem = (x) => {
    const city = getCity();
    if (city) {
        x.innerHTML = city;
    }
}

window.onload = function() {
    checkCity();
    const x1 = document.getElementById("demo");

    document.getElementById("get-btn").addEventListener('click', getLocation);
    
    
    updateCityElem(x1);
    
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
            .then(({ city }) => {
                setCity(city)
                updateCityElem(x1)
            })
            
    }
}

function checkCity() {
    const city = getCity();
    if (city) {
        $('.pop-up').hide();
    }
     const cityEl = document.querySelector('#demo');
     updateCityElem(cityEl);
}

$(function() {
    $('.pop-up').hide();
    const city = getCity();
    if (city) {
        return;
    }
    $('.pop-up').fadeIn(1200);
    $('.close-button').click(function (e) {
        $('.pop-up').fadeOut(700);
        $('#overlay').removeClass('blur-in');
        $('#overlay').addClass('blur-out');
        e.stopPropagation();
    });
});