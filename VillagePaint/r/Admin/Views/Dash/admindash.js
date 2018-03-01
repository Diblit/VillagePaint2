var mainJson = null;

$(document).ready(function () {

    mainJson = jQuery.parseJSON($('#jsonData').html());

    var Clients = '';
    var Admins = '';

    $.each(mainJson.List, function (index, item) {
        if (index == "countClients") {
            Clients = item;
        }
        if (index == "countAdmins") {
            Admins = item;
        }
    });

    $('#lblDashClientCount').html(Admins);
    $('#lblDashAdminCount').html(Clients);

});