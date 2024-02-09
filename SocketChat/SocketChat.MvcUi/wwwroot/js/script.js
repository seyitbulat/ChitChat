
$(document).bind('keydown', function (e) {
    if (e.which === 13) { // return
        $('#searchBtn').trigger('click');
    }
});

document.addEventListener("DOMContentLoaded", function () {
    document.querySelector(".login-container").style.display = "flex";

    $.each([1, 2, 3, 4, 5, 6], function (index, value) {
        var button = `<button class="hub-btn" data-button-id="` + value + `" onclick="selectHub(` + value + `)">Hub ` + value + `</button>`;
        $(".hub-container").append(button);
    });

   
});

let selectedHub = null;

function selectHub(hubId) {

    if (hubId == selectedHub) {
        $('.hub-btn[data-button-id="' + selectedHub + '"]').removeClass("selected-hub")
        selectedHub = null;
        return;
    }
    if (hubId != selectedHub) {
        if (selectedHub !== null) {

            $('.hub-btn[data-button-id="' + selectedHub + '"]').removeClass("selected-hub")
        }

        selectedHub = hubId;
        var button = $('.hub-btn[data-button-id="' + hubId + '"]');
        button.addClass("selected-hub");
    }
}

function login() {
    const username = document.getElementById("username").value;
    console.log(username);
    if (username) {
        if (selectedHub !== null) {

            var postObject = { "UserName": username ,"HubId":selectedHub};
            $.ajax({
                url: '/Login/Login',
                type: 'POST',
                data: postObject,
                datatype: 'json',
                cache: false,
                success: function (response) {
                    if (response.isSuccess) {
                        //$.ajax({
                        //    url: '/Home/GetUser',
                        //    type: 'POST',
                        //    cache: false,
                        //    dataType: 'json',
                        //    data: postObject,
                        //});
                        window.location.href = "/Home/Index";
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: "Hata",
                            html: response.message
                        })

                    }
                }
            });
        } else {
            Swal.fire({
                icon: 'error',
                title: "Hata",
                html: "Lutfen bir chathub seciniz!"
            })
        }
    } else {
        Swal.fire({
            icon: 'error',
            title: "Hata",
            html: "Lütfen kullanıcı adı girin."
        })
    }
}

function sendMessage() {
    const message = document.getElementById("message").value;
    // Mesaj gönderme işlemleri
}



