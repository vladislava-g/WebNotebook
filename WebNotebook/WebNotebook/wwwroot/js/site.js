// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$('#logOut').on('click', function () {

    $.ajax({
        type: "POST",
        url: `/Account/Logout`,
        data: {},
        success: function (response) {

        },
        error: function (response) {
            console.log(response.responseText);

        }
    });
    window.location.href = '/Account';
})