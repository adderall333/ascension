function test(){
    
    var location = window.location.pathname;
    var cur_url = '/' + location.split('/')[1]; // текущий адрес

    //var cur_active;
    $('.navbar-collapse li').each(function () { // проходимся по всем пунктам меню
        var link = $(this).find('a').attr('href'); // выделяем ссылку 
        link = '/' + link.split('/')[1];
        if(cur_url === link){
            //cur_active = $(this);
            $(this).addClass('active'); // если ссылка является текущим url, то <li> мы добавляем класс .active
        }
    });
    
    // определяем координаты активной ссылки
    var tabs = $('#navbarSupportedContent');
    var activeItem = tabs.find('.active');
    var activeHeight = activeItem.innerHeight();
    var activeWidth = activeItem.innerWidth();
    var itemPosTop = activeItem.position();
    var itemPosLeft = activeItem.position();
    
    // изменяем расположение элемента выделения активной ссылки
    $(".hori-selector").css({
        "top":itemPosTop.top + "px",
        "left":itemPosLeft.left + "px",
        "height": activeHeight + "px",
        "width": activeWidth + "px"
    });

    $(".hori-selector").show(); // отображаем элемент выделения
    
    /*
    $("#navbarSupportedContent").on("click","li",function(e){
        //$('#navbarSupportedContent ul li').removeClass("active");
        cur_active.removeClass("active");
        
        $(this).addClass('active');
        var activeWidthNewAnimHeight = $(this).innerHeight();
        var activeWidthNewAnimWidth = $(this).innerWidth();
        var itemPosNewAnimTop = $(this).position();
        var itemPosNewAnimLeft = $(this).position();
        $(".hori-selector").css({
            "top":itemPosNewAnimTop.top + "px",
            "left":itemPosNewAnimLeft.left + "px",
            "height": activeWidthNewAnimHeight + "px",
            "width": activeWidthNewAnimWidth + "px"
        });
        
    });
    */
}

$(document).ready(function(){
    setTimeout(function(){ test(); });
});
$(window).on('resize', function(){
    setTimeout(function(){ test(); });
});
$(".navbar-toggler").click(function(){
    setTimeout(function(){ test(); });
});