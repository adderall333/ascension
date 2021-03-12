let signin_button = document.getElementById('signin');

signin_button.addEventListener("click", () => {
    let form = document.getElementById('login-form');
    let email = form.email.value;
    let pass = form.pass.value;

    function createErrorMessage(message) {
        let div = document.createElement('div');
        div.className = 'error-div';

        let i = document.createElement('i');
        i.classList.add('zmdi', 'zmdi-close', 'error-icon');

        let h6 = document.createElement('h6');
        h6.className = 'error-message';
        h6.innerHTML = message;

        div.appendChild(i);
        div.appendChild(h6);
        error_block.appendChild(div);
    }

    let error_block = document.getElementById('error-block');
    while (error_block.firstChild) {
        error_block.removeChild(error_block.lastChild);
    }

    if (name.length < 1 || surname.length < 1 || email.length < 1 || pass.length < 1 || re_pass.length < 1) {
        createErrorMessage('Fill in all the fields');
    }
    
    // отправлять запрос на сервер

});