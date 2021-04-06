function moveSelector() {
    // находим активную ссылку
    let location = window.location.pathname;
    let cur_url = '/' + location.split('/')[1]; // текущий адрес
    $('.navbar-collapse li').each(function () { // проходимся по всем пунктам меню
        let link = $(this).find('a').attr('href'); // выделяем ссылку 
        link = '/' + link.split('/')[1];
        if (cur_url === link) {
            $(this).addClass('active'); // если ссылка является текущим url, то <li> мы добавляем класс .active
        }
    });

    // определяем координаты активной ссылки
    let tabs = $('#navbarSupportedContent');
    let activeItem = tabs.find('.active');
    if (activeItem.length < 1)
        return;
    let activeHeight = activeItem.innerHeight();
    let activeWidth = activeItem.innerWidth();
    let itemPosTop = activeItem.position();
    let itemPosLeft = activeItem.position();

    // изменяем расположение элемента выделения активной ссылки
    $(".hori-selector").css({
        "top": itemPosTop.top + "px",
        "left": itemPosLeft.left + "px",
        "height": activeHeight + "px",
        "width": activeWidth + "px"
    });

    // отображаем selector
    $(".hori-selector").show();
}

$(document).ready(function () {
    setTimeout(function () { moveSelector(); });
});
$(window).on('resize', function () {
    setTimeout(function () { moveSelector(); });
});
$(".navbar-toggler").click(function () {
    setTimeout(function () { moveSelector(); });
});

let navLinks = document.getElementsByClassName("nav-link");
for (let navLink of navLinks) {
    navLink.addEventListener("DOMSubtreeModified", function () {
        setTimeout(function () { moveSelector(); });
    });
}