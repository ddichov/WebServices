/// <reference path="cryptojs-sha1.js" />
/// <reference path="class.js" />
/// <reference path="vendor/jquery.js" />
/// <reference path="http-requester.js" />

var persisters = (function () {

    var nickname = localStorage.getItem("nickname");
    var sessionKey = localStorage.getItem("sessionKey");
    var imageUrl = localStorage.getItem("userImageUrl");

    function saveLoginData(data) {
        localStorage.setItem("nickname", data.nickname);
        localStorage.setItem("sessionKey", data.sessionKey);
        localStorage.setItem("userImageUrl", data.imageUrl);
        nickname = data.nickname;
        sessionKey = data.sessionKey;
    }

    function clearLoginData() {
        localStorage.removeItem("nickname");
        localStorage.removeItem("sessionKey");
        localStorage.removeItem("userImageUrl");
        nickname = null;
        sessionKey = null;
        imageUrl = null;
    }

    var UserPersister = Class.create({
        init: function (rootUrl) {
            this.rootUrl = rootUrl + "users/";
        },
        register: function (user, success, error) {
            var url = this.rootUrl + "register";
            var userData = {
                nickname: user.nickname,
                password: CryptoJS.SHA1(user.password + user.password).toString(),
                email: user.email,
                imageUrl: user.imageUrl
            }

            httpRequester.postJSON(url, userData, function (response) {
                saveLoginData(response);
                success(response);
            }, error);
        },
        login: function (user, success, error) {
            var url = this.rootUrl + "login";
            var userData = {
                nickname: user.nickname,
                password: CryptoJS.SHA1(user.password + user.password).toString()
            }

            httpRequester.postJSON(url, userData, function (response) {
                saveLoginData(response);
                success(response);
            }, error);
        },
        logout: function (success, error) {
            var url = this.rootUrl + "logout/" + sessionKey;
            httpRequester.getJSON(url, function (response) {
                clearLoginData();
                success(response);
            }, error);
        },
        getContacts: function (success, error) {
            var url = this.rootUrl + "get-contacts/" + sessionKey;
            httpRequester.getJSON(url, success, error);
        },
        addContact: function (nickname, success, error) {
            var url = this.rootUrl + "add-contact/" + sessionKey;
            httpRequester.postJSON(url, nickname, success, error);
        },
        getNickname: function () {
            return nickname;
        },
        getImageUrl: function () {
            return imageUrl;
        }
    });

    function CheckForLoggedUser() {
        return nickname != null && sessionKey != null;
    }

    return {
        get: function (rootUrl) {
            return new UserPersister(rootUrl);
        },
        CheckForLoggedUser: CheckForLoggedUser
    }
}())