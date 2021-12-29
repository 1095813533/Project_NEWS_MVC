!(function ($) {
    var max_width = 1420;
    var min_top = 300;
    var layer_hidden = true;
    var update_layer = function () {
        if (($(window).width() >= max_width) && ($(window).scrollTop() > min_top)) {
            //                        layer_hidden && $('#backTop').fadeIn(1000);
            layer_hidden && $('#backTop').show();
            layer_hidden = false;
        } else {
            //                        layer_hidden || $('#backTop').fadeOut(1000);
            layer_hidden || $('#backTop').hide();
            layer_hidden = true;
        }
    };

    $(window).resize(function () {
        update_layer();
    });

    $(window).scroll(function () {
        update_layer();
    });

    $(document).ready(function () {
        update_layer();

        $("#backTop").click(function () {
            $("body,html").animate({
                scrollTop: 0
            }, 1000);
            return false;
        });
    });
})(jQuery)
