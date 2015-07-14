$(function () {
    $("button[data-setup-browser]").on({
        "click": function (event) {
            var e = $(this);
            $.getJSON("/Admin/Browser/ReadBrowser/" + $(this).attr("data-setup-browser"), function (data) {                
                var grid = $(e.attr("data-setup-browser-grid"));
                if (grid) {
                    var table = null;
                    var body = null;
                    for (var i = 0; i < data.length; i++) {
                        var element = data[i];
                        if (table == null) {
                            table = $("<table/>", {
                                "class": "table"
                            });
                            table.appendTo(grid);
                            var header = $("<thead/>");
                            header.appendTo(table);
                            var row = $("<tr/>");
                            row.appendTo(header);
                            for (var property in element) {
                                $("<th/>", {
                                    html: property
                                }).appendTo(row);
                            }
                            body = $("<tbody/>");
                            body.appendTo(table);
                        }
                        var row = $("<tr/>");
                        row.appendTo(body);
                        for (var property in element) {
                            $("<td/>", {
                                html: element[property]
                            }).appendTo(row);
                        }
                    }
                }
            });
        }
    });
});