let signup_button = document.getElementById('signup');

signup_button.addEventListener("click", () => {
    let form = document.getElementById('register-form');
    let name = form.name.value;
    let surname = form.surname.value;
    let email = form.email.value;
    let pass = form.pass.value;
    let re_pass = form.re_pass.value;

    for(let i=0; i<form.elements.length; i++) {
        let input = form.elements[i];
        input.classList.remove('error');
    }

    let checkName = /^[A-Z][a-zA-Z]+$/.test(name);
    let checkSurname = /^[A-Z][a-zA-Z]+$/.test(surname);
    let checkEmail = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/.test(email)
    let checkPass = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,}$/.test(pass);
    let checkRePass = pass === re_pass;
    
    function createErrorMessage(zmdi, message) {
        let div = document.createElement('div');
        div.className = 'error-div';
        
        let i = document.createElement('i');
        i.classList.add('zmdi', zmdi, 'error-icon');
        
        let h6 = document.createElement('h6');
        h6.className = 'error-message';
        h6.innerHTML = message;
        
        div.appendChild(i);
        div.appendChild(h6);
        error_block.appendChild(div);
    }
    
    function checkInputs() {
        let check = true;
        
        for(let i=0; i<form.elements.length; i++) {
            let input = form.elements[i];
            if (input.type !== 'submit' && input.value.length < 1) {
                check = false;
                input.classList.add('error');
            }
        }
        
        if (!check) {
            createErrorMessage('zmdi-close','Fill in all the fields');
            return false;
        }
        
        if (!checkName) {
            check = false;
            createErrorMessage('zmdi-close','Invalid name');
            form.name.classList.add('error');
        }
        if (!checkSurname) {
            check = false;
            createErrorMessage('zmdi-close','Invalid surname');
            form.surname.classList.add('error');
        }
        if (!checkEmail) {
            check = false;
            createErrorMessage('zmdi-close','Invalid email');
            form.email.classList.add('error');
        }
        if (!checkPass) {
            check = false;
            createErrorMessage('zmdi-close','Invalid password' +
                '</br>The password must be at least 6 characters long and contain at least one number, one uppercase and one lowercase letter');
            form.pass.classList.add('error');
        }
        if (!checkRePass) {
            check = false;
            createErrorMessage('zmdi-close','Password mismatch');
            form.re_pass.classList.add('error');
        }
        
        return check;
    }
    
    let error_block = document.getElementById('error-block');
    while (error_block.firstChild) {
        error_block.removeChild(error_block.lastChild);
    }

    let check = checkInputs();
    if (!check)
        return;

    let fD = new FormData();
    fD.append('name', name);
    fD.append('surname', surname);
    fD.append('email', email);
    fD.append('pass', pass);
    
    $.ajax({
        type: 'POST',
        url: '/Authentication/TryRegister',
        data: fD,
        processData: false,
        contentType: false,
        success: function(res, status, xhr) {
            let result = xhr.getResponseHeader("registration_result")
            if (result === "ok") {
                createErrorMessage('zmdi-check','You have successfully registered in our site');
                Timer();
            }
            else if (result === "failed") {
                createErrorMessage('zmdi-close','This email address already exists</br>Please choose a unique one');
                form.email.classList.add('error');
            }
            else // "error"
                createErrorMessage('zmdi-close','An error occurred while registering</br>Please try again')
        }
    })
    
    let timer = 4;
    function Timer() {
        timer--;
        document.getElementById("timer").innerHTML = 'Redirecting to SignIn: ' + timer;
        if (timer === 0) {
            location.replace('/Authentication/Signin')
        }
        setTimeout(() => Timer(), 1000);
    }
});