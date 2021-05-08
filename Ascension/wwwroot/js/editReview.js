let editToggleBtn = document.getElementById('edit-toggle');
if(editToggleBtn != null) {
    editToggleBtn.addEventListener('click', () => {
        document.getElementById('haveReview').style.display = "none";
        document.getElementById('editReview').style.display = "block";
    });
}

let cancelToggleBtn = document.getElementById('cancel-toggle');
if(editToggleBtn != null) {
    cancelToggleBtn.addEventListener('click', () => {
        document.getElementById('haveReview').style.display = "block";
        document.getElementById('editReview').style.display = "none";
    });
}