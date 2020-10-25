$(document).ready(function () {
    $('main').load('/Home/GetNotebooks');
});

$(".nav-link").on('click', function (event) {
    event.stopPropagation();
    event.stopImmediatePropagation();

    $(".active").removeClass("active");
    $(this).addClass('active');

    $('main').load(`/Home/${$(this).attr("action_name")}`);
});
