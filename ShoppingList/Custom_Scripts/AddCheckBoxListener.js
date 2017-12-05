$(document).ready(function () {
    console.log("Listeners being called!");

    $('.ActiveCheck').change(function () {
        var self = $(this);
        var id = self.attr('id');
        var value = self.prop('checked');

        $.ajax({
            url: '/ListItems/AJAXEdit',
            data: {
                id: id,
                value: value
            },
            type: 'POST',
            success: function (result) {
                $('#tableDiv').html(result);
            }
        });
    });

})