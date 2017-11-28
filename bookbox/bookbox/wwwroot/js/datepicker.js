$(document).ready(function () {
    var options = $.extend({},
        $.datepicker.regional[""], {
            dateFormat: "yy-mm-dd"
        }
    );

    $('.datepicker').datepicker(options);
});

//To validate after pick from jQuery datapicker
$(":input.datepicker").change(function () {
    $(this).focusout();
});