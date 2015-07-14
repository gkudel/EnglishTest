$(function () {
    $("#helpButton").click(function () {
        var polish = $("#PolishWord").val();
        $.getJSON("/Test/Help/" + polish, function (resp) {
            $("#EnglishWord").val(resp.EnglishhWord);
        });
    });
});