let editToggleBtn = document.getElementById('edit-toggle');
if(editToggleBtn != null) {
    editToggleBtn.addEventListener('click', () => {
        document.getElementById('haveReview').style.display = "none";
        document.getElementById('editReview').style.display = "block";
        
        let starsDiv = document.getElementsByClassName('add_stars')[0];
        starsCount = document.getElementById('review_rating').value;
        for (let i=1; i<6; i++) {
            let div = document.createElement('div');
            div.id = 'star' + i.toString();
            div.classList.add('star_btn');
            if (i <= starsCount)
                div.innerHTML = filledStar;
            else
                div.innerHTML = unfilledStar;
            starsDiv.append(div);
        }

        allStarts = document.querySelectorAll('.star_btn');
        for (let i = 0; i < allStarts.length; i++) {
            let star = allStarts[i];
            star.addEventListener('click', clickHandler);
        }
    });
}

let cancelToggleBtn = document.getElementById('cancel-toggle');
if(editToggleBtn != null) {
    cancelToggleBtn.addEventListener('click', () => {
        document.getElementById('haveReview').style.display = "block";
        document.getElementById('editReview').style.display = "none";

        let starsDiv = document.getElementsByClassName('add_stars')[0];
        while (starsDiv.firstChild) {
            starsDiv.removeChild(starsDiv.firstChild);
        }
    });
}