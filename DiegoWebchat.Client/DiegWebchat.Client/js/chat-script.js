/// <reference path="vendor/jquery.js" />

$(document).ready(function () {
    var defaultImageUrl = "http://res.cloudinary.com/haiubldgg/image/upload/v1376586743/765-default-avatar_u2jyrn.png";

    // Initialize the PubNub API connection.
    var pubnub = PUBNUB.init({
        publish_key: 'pub-c-a54c6b0d-ee2a-4161-a103-f781d15e590f',
        subscribe_key: 'sub-c-624a1f98-e270-11e2-8036-02ee2ddab7fe'
    });

    // Initialize second PubNub API connection.
    var pubnubMulty = PUBNUB.init({
        publish_key: 'pub-c-a54c6b0d-ee2a-4161-a103-f781d15e590f',
        subscribe_key: 'sub-c-624a1f98-e270-11e2-8036-02ee2ddab7fe'
    });
    var messageQueue = [];

    // Grab references for all of our elements.
    var messageContent = $('#messageContent'),
    sendMessageButton = $('#sendMessageButton'),
    messageList = $('#conversation');

    var currentUser = $("#sender_username").text();
    var receiverUser = $("#receiver_username").text();
    var channelsArr = [currentUser, receiverUser];
    var showHistoryBtn = $("#showHistory");

    var removeChannel = $(".removeChannel"); // to do: add element with id="removeChannel"
    var closeAll = $("#logout-btn");
    var addChannel = $(".chatMate");
    var changeChatMate = $("#contact-names .row");
    var isHistoryEnabled = false;
    var includeHistory = $("#include_history");

    //====== Start Multy =============

    // Grab references for Multy
    var messageContentMulty = $('#messageContentMulty');
    var sendMessageButtonMulty = $('#sendMessageButtonMulty');
    var messageListMulty = $('#conversationMulty');
    var multyChannel = 'multychannel';

    // Compose and send a message - Multy channel.
    sendMessageButtonMulty.click(function (event) {
        var message = messageContentMulty.val();
        if (message != '') {
            pubnub.publish({
                channel: multyChannel,
                message: {
                    username: currentUser,
                    text: message
                }
            });
            messageContentMulty.val("");
        }
    });

    // Send a message when the user hits the enter button in the text area - Multy-channel
    messageContentMulty.bind('keydown', function (event) {
        if ((event.keyCode || event.charCode) !== 13) {
            return true;
        }
        sendMessageButtonMulty.click();
        return false;
    });

    // Handles all Multy-channel messages
    function handleMultyChannelMessage(message) {
        var messageEl = '';
        if (message.username === currentUser) {
            messageEl = $("<li class='text sent'>"
                          + "<div class='reflect'></div><p><strong>"
                          + "<a class='username' href='#'>" + message.username + "</a> said: "
                          + "</strong>"
                          + message.text
                          + "</p></li>");
        }
        else {
            messageEl = $("<li class='text receive'>"
                          + "<div class='reflect'></div><p><strong>"
                          + "<a class='username' href='#'>" + message.username + "</a> said: "
                          + "</strong>"
                          + message.text
                          + "</p></li>");
        }
        messageListMulty.append(messageEl);

        $("html, body").animate({ scrollTop: $(document).height() - $(window).height() }, 'slow');
        var scroller = document.getElementById("page-multy")
        scroller.scrollTop = scroller.scrollHeight;
    };

    // Subscribe to messages coming in from the channel.
    pubnubMulty.subscribe({
        backfill: isHistoryEnabled,
        channel: multyChannel,
        message: handleMultyChannelMessage,
    });

    ////============= END Multy =============
    function renderChatMessage(message)
    {
        var messageEl = '';
        if (message.username === currentUser && message.receiver === receiverUser) {
            messageEl = $("<li class='text sent'>"
                          + "<div class='reflect'></div><p><strong>"
                          + "<a class='username' href='#'>" + message.username + "</a> said: "
                          + "</strong>"
                          + message.text
                          + "</p></li>");
            messageList.append(messageEl);
        }
        else if (message.username === receiverUser) {
            messageEl = $("<li class='text receive'>"
                          + "<div class='reflect'></div><p><strong>"
                          + "<a class='username' href='#'>" + message.username + "</a> said: "
                          + "</strong>"
                          + message.text
                          + "</p></li>");
            messageList.append(messageEl);
        }
    }
    //includeHistory with current user
    includeHistory.click(function (event) {
        messageList.html('');

        for (var i = 0; i < messageQueue.length; i++) {
            var message = messageQueue[i];
            renderChatMessage(message);
        }

        $("html, body").animate({ scrollTop: $(document).height() - $(window).height() }, 'slow');
        var scroller = document.getElementById("iPhoneBro")
        scroller.scrollTop = scroller.scrollHeight;
    });

    // Sow History 
    showHistoryBtn.click(function (event) {
        if (event.currentTarget.checked) {
            isHistoryEnabled = true;
        }
        else {
            isHistoryEnabled = false;
        }

        pubnub.unsubscribe({
            channel: channelsArr
        });
        messageList.html('');
        pubnub.subscribe({
            backfill: isHistoryEnabled,
            channel: channelsArr,
            message: handleMessage,
        });

        messageListMulty.html("");
        pubnubMulty.subscribe({
            backfill: isHistoryEnabled,
            channel: multyChannel,
            message: handleMultyChannelMessage,
        });
        return this;
    });

    // logout
    closeAll.click(function () {
        pubnub.unsubscribe({
            channel: channelsArr
        });

        pubnubMulty.unsubscribe({
            channel: multyChannel
        });

        channelsArr = [];
    });

    // Change current massage receiver
    $("#contact-names").on('click', '.row', function (event) {
        currentUser = $('#sender_username').text();

        pubnub.unsubscribe({
            channel: channelsArr
        });

        var newUser = event.currentTarget.id;
        var imageId = "#" + newUser + "_img";

        var imageSrc = $(imageId).attr('src');
        $("#receiver_username").text(newUser);
        $("#receiver_image").attr('src', imageSrc);

        receiverUser = newUser;
        channelsArr = [currentUser, receiverUser];
        messageList.html('');

        pubnub.subscribe({
            backfill: isHistoryEnabled,
            channel: channelsArr,
            message: handleMessage,
        });
    });

    // Handles messages coming in from pubnub.subscribe.
    function handleMessage(message) {
        var messageEl = '';
        if (message.username === currentUser) {
            messageEl = $("<li class='text sent'>"
                          + "<div class='reflect'></div><p><strong>"
                          + "<a class='username' href='#'>" + message.username + "</a> said: "
                          + "</strong>"
                          + message.text
                          + "</p></li>");
            messageList.append(messageEl);
        }
        else if (message.username === receiverUser) {
            messageEl = $("<li class='text receive'>"
                          + "<div class='reflect'></div><p><strong>"
                          + "<a class='username' href='#'>" + message.username + "</a> said: "
                          + "</strong>"
                          + message.text
                          + "</p></li>");
            messageList.append(messageEl);
        }
        var newMesssage = {
            username: message.username,
            text: message.text,
            receiver: receiverUser
        }
        messageQueue.push(newMesssage);

        // Scroll to bottom of page
        $("html, body").animate({ scrollTop: $(document).height() - $(window).height() }, 'slow');
        var scroller = document.getElementById("iPhoneBro")
        scroller.scrollTop = scroller.scrollHeight;
    };

    // Compose and send a message when the user clicks send message button.
    sendMessageButton.click(function (event) {
        var message = messageContent.val();
        if (message != '') {
            pubnub.publish({
                channel: receiverUser,
                message: {
                    username: currentUser,
                    text: message
                }
            });
            messageContent.val("");
        }
    });

    // Send a message when the user hits the enter button in the text area.
    messageContent.bind('keydown', function (event) {
        if ((event.keyCode || event.charCode) !== 13) {
            return true;
        }
        sendMessageButton.click();
        return false;
    });

    // Subscribe to messages coming in from the pubnub.
    pubnub.subscribe({
        backfill: isHistoryEnabled,
        channel: channelsArr,
        message: handleMessage,
    });
});