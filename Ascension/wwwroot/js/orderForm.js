let edit_button = document.getElementById('checkout-button');
///
// function AddressType()
// {
//     let old=document.getElementById("submitId");
//     if(old==null)
//     {
//         let address=document.getElementById("insertedField");
//         let p=document.createElement("p");
//         var submit=address.appendChild(p);
//         submit.id="submitId";
//         submit.innerHTML=`<button asp-controller="Order" asp-action="PaymentInfo" class="button" value="1" name="calc_shipping" type="submit">Submit</button>`
//     }
// }
// let scenario=document.getElementById("pickup-delivery").value;
// scenario.addEventListener("change", function() {
//     if(scenario ==="Delivery")
//     {
//         document.innerHTML=`
//     <div class="form-group">
//         <input class="auth-input " type="text" name="address" id="address" placeholder="City, Street, House, Flat"/>
//     </div>`
// //     }
// //     else if(scenario === "PickUp")
// //     {
//         document.innerHTML=`
//     <div class="form-group">
//         <input class="auth-input " readonly type="text" name="address" id="address" value="Pickup from Russian Federation, Kazan, Kremlevskaya, 35 "/>
//     </div>`
//     }
// });
// function DeliveryType()
// {
//     // let old=document.getElementById("insertedField");
//     // if(old!=null)
//     // {
//     //     old.remove()
//     // }
//     let scenario=document.getElementById("pickup-delivery").value;
//    //ShowDeliveryType(scenario);
//    // let form=document.getElementById("calcalute-shipping-wrap");
//    //  let p=document.createElement("p");
//    //  var address=form.appendChild(p);
//    //  address.id="insertedField"
//     if(scenario ==="Delivery")
//     {
//         document.innerHTML=`<div class="form-group"><input class="auth-input " type="text" name="address" id="address" placeholder="City, Street, House, Flat"/></div>`}
//     else if(scenario === "PickUp")
//     {
//         document.innerHTML=`<div class="form-group"><input class="auth-input " readonly type="text" name="address" id="address" value="Pickup from Russian Federation, Kazan, Kremlevskaya, 35 "/></div>`
//     }
// }
// function ShowDeliveryType(scenario)
// {
//     document.getElementById("DeliveryTypeTotal").innerText=scenario
// }
///
// function addActivityPickup() {
//     // ... Code to add item here
// }
// function addActivityDelivery(){
//    
// }

edit_button.addEventListener("click", () => {
    let form = document.getElementById('checkout-form');
    let name = form.name.value;
    let surname = form.surname.value;
    let email = form.email.value;
    let address = form.address.value

    for(let i=0; i<form.elements.length; i++) {
        let input = form.elements[i];
        input.classList.remove('error');
    }

    let checkName = /^[A-Z][a-zA-Z]+$/.test(name);
    let checkSurname = /^[A-Z][a-zA-Z]+$/.test(surname);
    let checkEmail = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/.test(email)

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

        for(let i=0; i<form.elements.length; i++) {
            let input = form.elements[i];
            if (input.type !== 'submit' && input.value.length < 1) {
                check = false;
                input.classList.add('error');
            }

        }

        if (!check) {
            createMessage('zmdi-close','Fill in all the fields');
            return false;
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
    fD.append('address', address);

    $('#loading').addClass('processing');
    $.ajax({
        type: 'POST',
        url: '/Checkout/TrySendForm',
        data: fD,
        processData: false,
        contentType: false,
        success: function(res, status, xhr) {
            $('#loading').removeClass('processing');
            let result = xhr.getResponseHeader("order_result")
            if (result === "ok") {
                createMessage('zmdi-check','You have successfully send form');
                Timer();
            }
            else // "error"
                createMessage('zmdi-close','An error occurred while entering</br>Please try again')
        }
    })
    let timer = 4;
    function Timer() {
        timer--;
        document.getElementById("timer").innerHTML = 'Redirecting to Card Form: ' + timer;
        if (timer === 0) {
            location.replace('/Checkout/Card')
        }
        setTimeout(() => Timer(), 1000);
    }
});