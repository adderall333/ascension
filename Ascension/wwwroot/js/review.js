let starsCount = 0;
let unfilledStar = '☆';
let filledStar = '★';

let allStarts = document.querySelectorAll('.star_btn');

for (let i = 0; i < allStarts.length; i++) {
    let star = allStarts[i];
    star.addEventListener('click', clickHandler);
}

function clickHandler(e) {
    let starId = e.target.id;
    let n = parseInt(starId[starId.length-1]);
    starsCount = n;
    changeStars(n, filledStar, unfilledStar);
}

function changeStars(n, star1, star2) {
    for (let i = 0; i < allStarts.length; i++) {
        if (i < n)
            allStarts[i].innerHTML = star1;
        else
            allStarts[i].innerHTML = star2;
    }
}

let add_review_button = document.getElementById('add-review-btn');
if (add_review_button != null) {
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
        if (!check)
            return;

        let fD = new FormData()
        fD.append('text', text);
        fD.append('rating', starsCount);
        let prodId = document.getElementById('prod_id').value;
        fD.append('prodId', prodId);
        
        let ajaxUrl = '/Catalog/AddReview';
        if (document.getElementById('edit_review').checked)
            ajaxUrl = '/Catalog/EditReview';
            
        $.ajax({
            type: 'POST',
            url: ajaxUrl,
            data: fD,
            processData: false,
            contentType: false,
            success: function(res, status, xhr) {
                location.reload();
            }
        })
    });
}