const storageKeys = {
    CITY: 'xcity',
};

function showPosition(position) {
    fetch(
        '/geoapi/get?lat=' +
        position.coords.latitude +
        '&lot=' +
        position.coords.longitude
    )
        .then((x) => x.json())
        .then(({ city }) => {
            setCity(city);
        });
}
    
const refreshCityTag = () => {
    const city = getCity();

    if (city) {
        document.querySelector('.home').innerText = city;
    }
};

const setCity = (city, reload) => {
    localStorage.setItem(storageKeys.CITY, city);

    refreshCityTag();

    if (reload) return location.reload();

    document.querySelector('.suggested-city').innerText = `${city}?`;
};

const getCity = () => localStorage.getItem(storageKeys.CITY);

const getGeolocation = () =>
    new Promise((rs, rj) => navigator.geolocation.getCurrentPosition(rs, rj));

const getGeoSuggestion = async () => {
    if (navigator.geolocation) {
        try {
            const location = await getGeolocation();
            if (location) showPosition(location);
        } catch (error) {}
    }
};

$(async () => {
    refreshCityTag();
    if (getCity()) return;

    $('.pop-up').hide();

    await getGeoSuggestion();
    $('.pop-up').fadeIn(1200);

    $('.detect-cd').click(function (e) {
        $('.pop-up').fadeOut(700);
        $('#overlay').removeClass('blur-in');

        $('#overlay').addClass('blur-out');

        e.stopPropagation();
    });

    $('.choose-btn').click(() => {
        const city = $('#slc-city option:selected').text();
        if (city) {
            setCity(city, true);
        }
    });
});

function city() {
    $('.pop-up').hide();

    getGeoSuggestion();
    $('.pop-up').fadeIn(1200);

    $('.detect-cd').click(function (e) {
        $('.pop-up').fadeOut(700);
        $('#overlay').removeClass('blur-in');

        $('#overlay').addClass('blur-out');

        e.stopPropagation();
    });
    

    $('.choose-btn').click(() => {
        const city = $('#slc-city option:selected').text();
        if (city) {
            setCity(city, true);
        }
    });
}
$(function() {
    $('.close-button').click(function (e) {
        $('.pop-up').fadeOut(700);
        $('#overlay').removeClass('blur-in');
        $('#overlay').addClass('blur-out');
        e.stopPropagation();
        location.reload()
    });
});



