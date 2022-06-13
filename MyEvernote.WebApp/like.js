
$(function () {

    var noteids = [];

    $("div[data-note-id]").each(function (i, e) {
        noteids.push($(e).data("note-id"));
    });

    $.ajax({
        method: "POST",
        url: "/Note/GetLiked",
        data: { ids: noteids }
    }).done(function (data) {


        if (data.result != null && data.result.length > 0) {
            for (var i = 0; i < data.result.length; i++) {
                var id = data.result[i];
                var likedNote = $("div[data-note-id=" + id + "]");
                var btn = likedNote.find("button[data-liked]");
                var span = btn.find(".like-star");
                btn.data("liked", true);
                span.removeClass("bi-star");
                span.addClass("bi bi-star-fill");
            }

        }

    }).fail(function () {

    });

    $("button[data-liked]").click(function () {
        var btn = $(this);
        var liked = btn.data("liked");
        var noteid = btn.data("note-id");
        var spanStar = btn.find(".like-star");
        var spanCount = btn.find(".like-count");

        console.log(liked);
        console.log("like count: " + spanCount.text());

        $.ajax({
            method: "POST",
            url: "/Note/SetLikeState",
            data: { "noteid": noteid, "liked": !liked }
        }).done(function (data) {

            console.log(data);

            if (data.hasError) {
                alert(data.errorMessage);
            }
            else {
                liked = !liked;
                btn.data("liked", liked); 
                spanCount.text(data.result);

                console.log("like count(after): " + spanCount.text());

                spanStar.removeClass("bi-star");
                spanStar.removeClass("bi-star-fill");

                if (liked) {
                    spanStar.addClass("bi-star-fill");
                }
                else {
                    spanStar.addClass("bi-star");
                }
            }
        }).fail(function () {
            alert("Sunucu ile bağlantı kurulamadı....");
        })
    });

});
