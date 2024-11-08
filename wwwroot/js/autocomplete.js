$(document).ready(function() {
    $("#searchInput").autocomplete({
        source: function(request, response) {
            if (request.term.length < 3) {
                response([]);
                return;
            }
            
            $.ajax({
                url: "/Home/Search",
                dataType: "json",
                data: {
                    searchTerm: request.term
                },
                success: function(data) {
                    if (data.length === 0) {
                        // Показуємо повідомлення, якщо нічого не знайдено
                        response([{
                            label: "Нічого не знайдено",
                            value: "",
                            id: -1
                        }]);
                    } else {
                        response($.map(data, function(item) {
                            return {
                                label: highlightMatch(item.name, request.term),
                                value: item.name,
                                id: item.id
                            };
                        }));
                    }
                },
                error: function() {
                    response([{
                        label: "Сталася помилка при пошуку",
                        value: "",
                        id: -1
                    }]);
                }
            });
        },
        minLength: 3,
        delay: 300,
        select: function(event, ui) {
            if (ui.item.id === -1) {
                event.preventDefault();
                return;
            }
            $("#selectedId").val(ui.item.id);
            $("#selectedItemId").text(ui.item.id);
            // Додаємо анімацію для виділення вибраного елемента
            $("#selectedItemId").fadeOut(100).fadeIn(100);
        },
        focus: function(event, ui) {
            // Запобігаємо автозаповненню при навігації стрілками
            event.preventDefault();
        }
    }).autocomplete("instance")._renderItem = function(ul, item) {
        return $("<li>")
            .append("<div class='ui-menu-item-wrapper'>" + item.label + "</div>")
            .appendTo(ul);
    };

    function highlightMatch(text, term) {
        if (!term) return text;
        var regex = new RegExp("(" + $.ui.autocomplete.escapeRegex(term) + ")", "gi");
        return text.replace(regex, "<strong class='highlight'>$1</strong>");
    }

    // Додаємо обробник для очищення вибраного ID
    $("#searchInput").on("input", function() {
        if ($(this).val().length < 3) {
            $("#selectedId").val("");
            $("#selectedItemId").text("");
        }
    });
});