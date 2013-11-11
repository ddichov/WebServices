/// <reference path="jquery-2.0.2.js" />

var httpRequester = (function () {

    function getJSON(url, success, error) {
        $.ajax({
            url: url,
            type: "GET",
            contentType: "application/json",
            success: success,
            error: error
        });
    }

    function postJSON(url, data, success, error) {
        $.ajax({
            url: url,
            type: "POST",
            data: JSON.stringify(data),
            contentType: "application/json",
            success: success,
            error: error
        });
    }

    return {
        getJSON: getJSON,
        postJSON: postJSON
    }
}())