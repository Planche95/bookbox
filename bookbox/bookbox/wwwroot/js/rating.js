$(".star").click(function () {
    $(this).parent().find(".active").removeClass('active');
    $(this).addClass('active');

    var RatingJsonModel =
        {
            BookId: parseInt($(this).parent().attr('id')),
            Rating: parseInt($(this).attr('data-value'))
        };

    $.ajax({
        url: '/Rating/SaveRating/',
        type: "POST",
        dataType: "json",
        data: RatingJsonModel,
        success: function () {

        }
    });
});