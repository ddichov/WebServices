/// <reference path="ui.js" />
/// <reference path="vendor/jquery.js" />
var diegoChat = (function () {

    function load() {
        $(document).ready(function () {

            var baseUrl = "http://diegowebchat.apphb.com/api/";
            var usersPersister = persisters.get(baseUrl);
            var hasLoggedUser = persisters.CheckForLoggedUser();

            if (hasLoggedUser) {
                OnLogInSucces();
                var nickname = usersPersister.getNickname();
                var imageUrl = usersPersister.getImageUrl();
                var data = {
                    nickname: nickname,
                    imageUrl: imageUrl
                }

                ui.loadUserInfo(data);
            }

            $("body").on('click', '#login-btn', function () {
                var nickname = $("#login-nickname").val();
                var pass = $("#login-pass").val();
                var user = {
                    nickname: nickname,
                    password: pass
                };

                usersPersister.login(user, function (data) {
                    OnLogInSucces();
                    ui.loadUserInfo(data);

                    setInterval(function () {
                        usersPersister.getContacts(function (data) {
                            ui.loadContacts(data)
                        }, function (error) {
                            alert(error.responseJSON.Message);
                        });
                    }, 60000);
                }, function (error) {
                    alert(error.responseJSON.Message);
                });
            });

            $("body").on('click', '#signin-button', function () {
                $("#login-form").fadeOut(50);
                $("#signin-form").fadeIn(2000).css("display", "inline-block");
            });

            $("body").on('click', '#reg-btn', function () {

                var nickname = $("#reg-nickname").val();
                var pass = $("#reg-pass").val();
                var repass = $("#reg-repass").val();

                if (pass != repass) {
                    ui.differentPassMessage();
                    return;
                }

                var email = $("#reg-email").val();
                var imageUrl = $("#reg-imgUrl").val();

                var user = {
                    nickname: nickname,
                    password: pass,
                    email: email,
                    imageUrl: imageUrl
                };

                usersPersister.register(user, function (data) {
                    OnLogInSucces();
                    ui.loadUserInfo(data);
                    setInterval(function () {
                        usersPersister.getContacts(function (data) {
                            ui.loadContacts(data)
                        }, function (error) {
                            alert(error.responseJSON.Message);
                        });
                    }, 60000);
                }, function (error) {
                    alert(error.responseJSON.Message);
                });

            });

            $("body").on('click', '#logout-btn', function () {
                usersPersister.logout(function () {
                    ui.hideChat();
                });
            });

            $("body").on('click', '#add-contact', function () {
                var html = "<input type='text' id='add-nickname' />" +
                    "<input type='button' id='add-contact-btn' value='Add'/>"

                $("#add-contact").after(html);

            });

            $("body").on('click', '#add-contact-btn', function () {
                var nickname = $("#add-nickname").val();

                usersPersister.addContact(nickname, function () {
                    OnLogInSucces();
                }, function (error) {
                    alert(error.responseJSON.Message);
                });

                $("#add-nickname").remove();
                $("#add-contact-btn").remove();
            });

            function OnLogInSucces() {
                ui.loadChat();
                usersPersister.getContacts(function (data) {
                    ui.loadContacts(data)
                }, function (error) {
                    alert(error.responseJSON.Message);
                });
            };


        });
    }

    return {
        load: load
    }
}());