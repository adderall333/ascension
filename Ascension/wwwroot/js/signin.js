let signin_button = document.getElementById('signin');

signin_button.addEventListener("click", () => {
    let form = document.getElementById('login-form');
    let email = form.email.value;
    let pass = form.pass.value;
    let remember = form.remember.checked;
    
    form.email.classList.remove('error');
    form.pass.classList.remove('error');

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

    if (email.length < 1 || pass.length < 1) {
        createErrorMessage('Fill in all the fields');
        form.email.classList.add('error');
        form.pass.classList.add('error');
        return;
    }

    let fD = new FormData();
    fD.append('email', email);
    fD.append('pass', pass);
    fD.append('remember', remember);

    $('#loading').addClass('processing');
    $.ajax({
        type: 'POST',
        url: '/Authentication/TryLogin',
        data: fD,
        processData: false,
        contentType: false,
        success: function(res, status, xhr) {
            $('#loading').removeClass('processing');
            let result = xhr.getResponseHeader("login_result")
            if (result === "ok") {
                let redirectUrl = localStorage.getItem('redirectUrl');
                if (typeof redirectUrl !== 'undefined' && redirectUrl != null) {
                    window.location.href = redirectUrl;
                    localStorage.removeItem('redirectUrl');
                }
                else
                    window.location.href = "/Account";
            }
            else {
                createErrorMessage('Invalid email or password');
                form.email.classList.add('error');
                form.pass.classList.add('error');
            }
        },
        error: function(res, status, xhr) {
            $('#loading').removeClass('processing');
            createErrorMessage('Something went wrong. Please try again later.');
        }
    })

});