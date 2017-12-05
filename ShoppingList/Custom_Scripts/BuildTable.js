$(document).ready(function () {
    console.log("Ajax being called!");
    $.ajax({
        url: '/ListItems/BuildListItemTable',
        success: function (result) {
            $('#tableDiv').html(result);
        }
    });

});