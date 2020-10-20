init();

async function init() {
    let login = document.querySelector("form#login");
    let register = document.querySelector("form#register");

    {
        let switchButtons = document.querySelectorAll("#switch-to-login, #switch-to-register");

        for (let button of switchButtons) {
            button.onclick = function () {
                newUser = this.id == "switch-to-register";
                newUser ? login.closest(".content").classList.add("hidden") : login.closest(".content").classList.remove("hidden");

                newUser ? register.closest(".content").classList.remove("hidden") : register.closest(".content").classList.add("hidden");
            }
        }
    }

    //Login Logic
    login.onsubmit = async function (e) {
        e.preventDefault();
        loading();

        let result = await verifyCredentials(this);

        if (result.success) {
            top.location.replace(`../Home/Index`);

        } else {
            simpleNotify.notify(result.message, result.success ? "success" : "red");
            loading_stop();
        }


        function verifyCredentials({ elements: { email, password } }) {

            let user = {
                Email: email.value,
                Password: password.value,
            };

            return new Promise((resolve, reject) => {
                $.ajax({
                    type: "POST",
                    traditional: true,
                    contentType: "application/json; charset=utf-8",
                    url: '/Account/Login',
                    dataType: "json",
                    data: JSON.stringify(user),
                    success: (result, textStatus, xhr) => {
                        result.status = xhr.status;
                        resolve(result);
                    },
                    error: (result, textStatus, xhr) => {
                        result.status = xhr.status;
                        resolve(result);
                    },
                });
            });
        }
    }


    //SignUp Logic

    register.onsubmit = async function (e) {
        e.preventDefault();

        loading();
        let result = await verifyCredentials(this);
        if (result.success) {
            top.location.replace(`../Home/Index`);

        } else {
            simpleNotify.notify(result.message, result.success ? "success" : "red");
            loading_stop();
        }

        function verifyCredentials({ elements: { email, password, name } }) {

            let user = {
                Email: email.value,
                Password: password.value,
                Name: name.value,
            };
            console.log(user);

            return new Promise((resolve, reject) => {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(user),
                    url: `/Account/Registration`,
                    dataType: "json",
                    success: (result, textStatus, xhr) => {
                        result.status = xhr.status;
                        resolve(result);
                    },
                    error: (result, textStatus, xhr) => {
                        result.status = xhr.status;
                        resolve(result);
                    },
                });
            });
        }
    }

    function loading(){
        $("#preloader").removeClass("d-none");
        $('.btn').prop('disabled', true);
    }

    function loading_stop(){
        $("#preloader").addClass("d-none");
        $('.btn').prop('disabled', false);
    }

}