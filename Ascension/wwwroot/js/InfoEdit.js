let edit_button = document.getElementById('edit-personal');

edit_button.addEventListener("click", () => {
    let form = document.getElementById('edit-form');
    let name = form.name.value;
    let surname = form.surname.value;
    let email = form.email.value;
    let pass = form.pass.value;
    let old_pass = form.old_pass.value;
    let  repass = form.repass.value;

    for(let i=0; i<form.elements.length; i++) {
        let input = form.elements[i];
        input.classList.remove('error');
    }
    let checkName = /^[A-Z][a-zA-Z]+$/.test(name);
    let checkSurname = /^[A-Z][a-zA-Z]+$/.test(surname);
    let checkEmail = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/.test(email)
    let checkPass = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,}$/.test(pass);
    function createMessage(zmdi, message) {
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

        for(let i=0; i<form.elements.length - 1; i++) {
            let input = form.elements[i];
            if(input.type !== 'password')
            if (input.type !== 'submit' && input.value.length < 1) {
                check = false;
                input.classList.add('error');
            }
            
        }

        if (!check) {
            createMessage('zmdi-close','Fill in all the fields');
            return false;
        }
        if (pass === "" && (repass !== "" || old_pass !== ""))
        {
            check = false;
            createMessage('zdmi-close', 'Type your new password');
            form.pass.classList.add('error');
        }
        if (repass === "" && (pass !== "" || old_pass !== ""))
        {
            check = false;
            createMessage('zdmi-close', 'Type your new password again');
            form.repass.classList.add('error');
        }
        if (old_pass === "" && (pass !== "" || repass !== ""))
        {
            check = false;
            createMessage('zdmi-close', 'Type your old password');
            form.old_pass.classList.add('error');
        }

        if (!checkName) {
            check = false;
            createMessage('zmdi-close','Invalid name');
            form.name.classList.add('error');
        }
        if (!checkSurname) {
            check = false;
            createMessage('zmdi-close','Invalid surname');
            form.surname.classList.add('error');
        }
        if (!checkEmail) {
            check = false;
            createMessage('zmdi-close','Invalid email');
            form.email.classList.add('error');
        }
        if (!checkPass) {
            if(form.pass.value !== "") {
                check = false;
                createMessage('zmdi-close', 'Invalid password' +
                    '</br>The password must be at least 6 characters long and contain at least one number, one uppercase and one lowercase letter');
                form.pass.classList.add('error');
            }
        }
        if(old_pass === pass){
            if(pass !== "" && old_pass !== ""){
                check = false;
                createMessage('zdmi-close', 'Old and new passwords must not match');
                form.pass.classList.add('error');
                form.old_pass.classList.add('error');
            }
        }
        if(repass !== pass){
            if(pass !== "" && repass !== ""){
                check = false;
                createMessage('zdmi-close', 'The passwords doesn\'t match');
                form.pass.classList.add('error');
                form.repass.classList.add('error');
            }
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
    fD.append('old_pass', old_pass);

    $('#loading').addClass('processing');
    $.ajax({
        type: 'POST',
        url: '/Profile/TryEdit',
        data: fD,
        processData: false,
        contentType: false,
        success: function(res, status, xhr) {
            $('#loading').removeClass('processing');
            let result = xhr.getResponseHeader("edit_result")
            if (result === "ok") {
                createMessage('zmdi-check','You have successfully edit information');
            }
            else if (result === "failedEmail") {
                createMessage('zmdi-close','This email address already exists</br>Please choose a unique one');
                form.email.classList.add('error');
            }
            else if (result === "failedPass"){
                createMessage('zdmi-close', "Your old password fields does not matches");
                form.old_pass.classList.add('error');
            }
            else // "error"
                createMessage('zmdi-close','An error occurred while editing</br>Please try again')
        }
    })
});