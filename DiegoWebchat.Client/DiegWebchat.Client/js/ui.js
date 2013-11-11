/// <reference path="jquery.js" />
var ui = (function () {

    var defaultImageUrl = "http://res.cloudinary.com/haiubldgg/image/upload/v1376586743/765-default-avatar_u2jyrn.png";

    function loadUserInfo(data) {
        $("#sender_username").text(data.nickname);

        if (!data.imageUrl) {
            $("#sender_image").attr("src", defaultImageUrl);
        } else {
            $("#sender_image").attr("src", data.imageUrl);
        }
    }

    function loadContacts(contacts) {
        var container = $("#contact-names");
        container.html("");
        var count = contacts.length;

        for (var i = 0; i < count; i += 1) {
            var contact = contacts[i];
            var currentImageUrl = contact.imageUrl;
            var imageId = contact.nickname + "_img";

            if (!currentImageUrl) {
                currentImageUrl = defaultImageUrl;
            }

            var contactsHolder = $("<div class='row' id='" + contact.nickname
                          + "'><div class='two phone-one columns'>"
                          + "<a href='#'><img id='" + imageId + "' src='" + currentImageUrl + "'></a> </div>"
                          + " <div class='ten phone-three columns'><h5 class='right'>0</h5>"
                          + "<h4><a href='#'>" + contact.nickname + "</a></h4> "
                          + "<h6 class='right'><a class='chatMate'id='" + "x-" + contact.nickname
                          + "' href='#'><span>Add to chat</span> +</a></h6>"
                          + "</div></div>");
            container.append(contactsHolder);
        }
    }

    function loadChat() {
        $("section").slideDown(1000);
        $("footer").slideUp(2000);
    }

    function hideChat() {
        $("footer").slideDown(1000);
        $("section").slideUp(2000);
    }

    function differentPassMessage() {
        alert("Passwords do not match!");
    }

    return {
        loadChat: loadChat,
        hideChat: hideChat,
        loadContacts: loadContacts,
        loadUserInfo: loadUserInfo,
        differentPassMessage: differentPassMessage
    }
}());