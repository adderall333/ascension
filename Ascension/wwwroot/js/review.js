let starsCount = 0;

let allStarts = document.querySelectorAll('.star_btn');

for (let i = 0; i < allStarts.length; i++) {
    let star = allStarts[i];
    star.addEventListener('click', clickHandler);
}

function clickHandler(e) {
    let starId = e.target.id;
    let n = parseInt(starId[starId.length-1]);
    starsCount = n;
    changeStars();
}

function changeStars() {
    for (let i = 0; i < allStarts.length; i++) {
        if (i < starsCount)
            allStarts[i].innerHTML = '★';
        else
            allStarts[i].innerHTML = '☆';
    }
}