"use strict";
var setOnline = function (id) {
    $("#chat_user_" + id).prependTo($(".list"));
    $("#status-icon-" + id).removeClass("offline").addClass("online");
    $("#status-user-" + id).html("online");
    $("#battle-btn-" + id).show();
};

var setOffline = function (id) {
    $("#chat_user_" + id).appendTo(".list");
    $("#status-icon-" + id).removeClass("online").addClass("offline");
    $("#status-user-" + id).html("offline");
    $("#battle-btn-" + id).hide();
};


var redirect = function (url, method, args) {
    var form = '';
    $.each(args, function (key, value) {
        form += '<input type="hidden" name="' + key + '" value="' + value + '">';
    });
    $('<form action="' + url + '" method="' + method + '">' + form + '</form>').appendTo('body').submit();
};

