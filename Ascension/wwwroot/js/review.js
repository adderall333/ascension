let starsCount = 0;
let unfilledStar = '☆';
//let pointedStar = '✭';
let filledStar = '★';

let allStarts = document.querySelectorAll('.star_btn');

for (let i = 0; i < allStarts.length; i++) {
    let star = allStarts[i];
    star.addEventListener('click', clickHandler);
    //star.addEventListener('mouseover', mouseOverHandler);
    //star.addEventListener('mouseout', mouseOutHandler)
}

function clickHandler(e) {
    let starId = e.target.id;
    let n = parseInt(starId[starId.length-1]);
    starsCount = n;
    changeStars(n, filledStar, unfilledStar);
}
/*
function mouseOverHandler(e) {
    let starId = e.target.id;
    let n = parseInt(starId[starId.length-1]);
    changeStars(n, filledStar, pointedStar, unfilledStar);
}

function mouseOutHandler(e) {
    changeStars(starsCount, filledStar, '', unfilledStar);
}
*/
function changeStars(n, star1, star2) {
    for (let i = 0; i < allStarts.length; i++) {
        if (i < n)
            allStarts[i].innerHTML = star1;
        else
            allStarts[i].innerHTML = star2;
    }
}



let add_review_button = document.getElementById('add-review-btn');

add_review_button.addEventListener('click', () => {
    let textArea = document.getElementById('add-comment');
    let text = textArea.value;
    let arrow = document.getElementById('stars_arrow');
    
    textArea.classList.remove('textarea_error');
    arrow.classList.remove('arrow_error');
    
    function checkInputs() {
        let check = true;
        if (text.length < 1) {
            check = false;
            textArea.classList.add('textarea_error');
        }
        if (starsCount < 1) {
            check = false;
            arrow.classList.add('arrow_error');
        }
        return check;
    }
    
    let check = checkInputs();
    alert(check);
});