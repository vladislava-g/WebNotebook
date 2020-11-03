$(document).ready(function () {
    $("#loader").removeClass("d-none");
    $("main").addClass("d-none");
    $('main').load(`/Notebook/GetNotebooks?id=${$('#userId').text()}`);

});

$(".nav-link").on('click', function (event) {
    event.stopPropagation();
    event.stopImmediatePropagation();

    $(".active").removeClass("active");
    $(this).addClass('active');

    $("#loader").removeClass("d-none");
    $("main").addClass("d-none");
    $('main').load(`/${$(this).attr("action_name")}?id=${$('#userId').text()}`);

});

//var newDiv = document.createElement("div");
//newDiv.innerHTML = xmlhttp.responseText;
//document.getElementById("results").childNodes.addAt(0, newDiv);