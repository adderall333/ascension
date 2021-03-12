/*function signup() {
    
    let form = document.getElementById('register-form');
    let name = form.name.value;
    let surname = form.surname.value;
    let email = form.email.value;
    let pass = form.pass.value;
    let re_pass = form.re_pass.value;
    
    let checkName = /^[A-Z][a-zA-Z]+$/.test(name);
    let checkSurname = /^[A-Z][a-zA-Z]+$/.test(surname);
    let checkEmail = /^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,6}$/.test(email)
    let checkPassword = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,}$/.test(pass);
    
}
*/
let signup_button = document.getElementById('signup');

signup_button.addEventListener("click", () => {
    let form = document.getElementById('register-form');
    let name = form.name.value;
    let surname = form.surname.value;
    let email = form.email.value;
    let pass = form.pass.value;
    let re_pass = form.re_pass.value;

    let checkName = /^[A-Z][a-zA-Z]+$/.test(name);
    let checkSurname = /^[A-Z][a-zA-Z]+$/.test(surname);
    let checkEmail = /^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,6}$/.test(email)
    let checkPass = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,}$/.test(pass);
    let checkRePass = pass === re_pass;
    
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
    else {
        if (!checkName) {
            createErrorMessage('Invalid name')
        }
        if (!checkSurname) {
            createErrorMessage('Invalid surname')
        }
        if (!checkEmail) {
            createErrorMessage('Invalid email')
        }
        if (!checkPass) {
            createErrorMessage('Invalid password.\nThe password must be at least 6 characters long and contain at least one number, one uppercase and one lowercase letter.')
        }
        if (!checkRePass) {
            createErrorMessage('Password mismatch')
        }
    }
    
    // отправлять запрос на сервер
    
});