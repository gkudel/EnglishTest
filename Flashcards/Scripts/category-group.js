$(function () {
    $("#CategoryGroupGrid tr:not(:has(th))").on({
        "click": function () {
            var element = $(this);
            var selected = element.hasClass("success");
            $("#CategoryGroupGrid tr:not(:has(th))").removeClass("success");
            if (!selected) element.addClass("success");
        },
        "mouseenter mouseleave": function () {
            $(this).toggleClass("hover");
        }
    });
});