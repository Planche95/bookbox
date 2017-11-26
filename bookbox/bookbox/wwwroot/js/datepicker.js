$(document).ready(function () {
    var options = $.extend({},
        $.datepicker.regional[""], {
            dateFormat: "dd.mm.yy"
        }
    );

    $('.datepicker').datepicker(options);
});

//Double focus to validate after date change
$(":input.datepicker").change(function () {
    $(this).focusin();
    $(this).focusout();
});