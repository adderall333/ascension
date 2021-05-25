var promoCode;
var promoPrice;
var fadeTime = 300;

$('.quantity input').change(function () {
    updateQuantity(this);
});

$('.remove button').click(function () {
    removeItem(this);
});

$(document).ready(function () {
    updateSumItems();
});


function recalculateCart(onlyTotal) {
    var subtotal = 0;

    $('.basket-product').each(function () {
        subtotal += parseFloat($(this).children('.subtotal').text());
    });

    var total = subtotal;
    

    if (onlyTotal) {
        $('.total-value').fadeOut(fadeTime, function () {
            $('#basket-total').html(total.toFixed(2));
            $('.total-value').fadeIn(fadeTime);
        });
    } else {
        $('.final-value').fadeOut(fadeTime, function () {
            $('#basket-subtotal').html(subtotal.toFixed(2));
            $('#basket-total').html(total.toFixed(2));
            if (total == 0) {
                $('.checkout-cta').fadeOut(fadeTime);
            } else {
                $('.checkout-cta').fadeIn(fadeTime);
            }
            $('.final-value').fadeIn(fadeTime);
        });
    }
}

function updateQuantity(quantityInput) {
    var productRow = $(quantityInput).parent().parent();
    var price = parseFloat(productRow.children('.price').text());
    var quantity1 = $(quantityInput).val();
    
    if (quantity1 < 1) {
        quantity1 = 1;
        alert("You can't choose quantity lower that 1!", location.reload())
    }
    var quantity = quantity1;
    var linePrice = price * quantity;
    productRow.children('.subtotal').each(function () {
        $(this).fadeOut(fadeTime, function () {
            $(this).text(linePrice.toFixed(2));
            recalculateCart();
            $(this).fadeIn(fadeTime);
        });
    });

    productRow.find('.item-quantity').text(quantity);
    updateSumItems();
}

function updateSumItems() {
    var sumItems = 0;
    $('.quantity input').each(function () {
        sumItems += parseInt($(this).val());
    });
    $('.total-items').text(sumItems);
}

function removeItem(removeButton) {
    var productRow = $(removeButton).parent().parent();
    productRow.slideUp(fadeTime, function () {
        productRow.remove();
        recalculateCart();
        updateSumItems();
        location.reload()
    });
}

