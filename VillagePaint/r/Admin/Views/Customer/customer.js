var mainJson = null;
var dtClientList = null;
var customerID;
$(document).ready(function () {

    mainJson = jQuery.parseJSON($('#jsonData').html());
    InitClientListTable();
});

function InitClientListTable() {

    dtClientList = $('#tblCustomerList').dataTable({

        "sSearch": true,
        "dom": '<"top"fl>rt<"bottom"p><"clear">', // "dom": '<"top"fl>rt<"bottom"ip><"clear">',
        "sPaginationType": "full_numbers",//IMPORTANT
        "aoColumnDefs": [{ 'bSortable': false, 'aTargets': [6] }],
        "oLanguage": { "sEmptyTable": "No data to display" },
        "sSearch": true,
        "bJQueryUI": false,
        "bAutoWidth": false,
        "sAjaxSource": base_url + '/api/admin/customer/list',
        "aaData": mainJson.List,
        "aaSorting": [],
        "bProcessing": true,
        "bServerSide": true,
        "orderClasses": false,
        "iDisplayLength": 10,
        "aLengthMenu": [
            [5, 10, 25, 50, 100],
            [5, 10, 25, 50, 100]
        ],

        "bSort": true,
        "bRetrieve": true,
        'bSortable': false,
        'aTargets': [-1],
        "aoColumns": [
            { "sTitle": "Card Number", "mDataProp": "CardNumber", "sWidth": "10%" },
            { "sTitle": "Company Name", "mDataProp": "CompanyName", "sWidth": "20%" },
            { "sTitle": "First Name", "mDataProp": "FirstName", "sWidth": "20%" },
            { "sTitle": "Last Name", "mDataProp": "LastName", "sWidth": "20%" },
            { "sTitle": "Email", "mDataProp": "Email", "sWidth": "15%" },
            { "sTitle": "Cell", "mDataProp": "Cell", "sWidth": "15%" },
            { "sTitle": "Actions", "mDataProp": "customerID", "sClass": "text-center w100 minw100" }
        ],
        "fnServerData": function (sSource, aoData, fnCallback, oSettings) {
            doDataTablePostAjaxCalling(this, sSource, aoData, fnCallback, oSettings);
        },

        "iDeferLoading": [mainJson.Count, mainJson.Count],
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {

            //Action Buttons
            var actionHtml = "";
            actionHtml += ' <a href="javascript:void(0);" onclick="OpenEditModal(' + nRow["_DT_RowIndex"] + ')" data-toggle="tooltip" title="Edit" data-placement="left" data-original-title="Edit" class="btn btn-default btn-sm btn-primary fa fa-pencil"></a>';
            //if (aData["isDeletable"]) {
            actionHtml += ' <a href="javascript:void(0);" onclick="DeleteClient(' + aData["customerID"] + ')" data-toggle="tooltip" title="Delete" data-placement="left" data-original-title="Delete" class="btn btn-default btn-sm btn-danger fa fa-times"></a>';
            // }
            //else {
            //    actionHtml += ' <a href="javascript:void(0);" data-toggle="tooltip" title="Delete" data-placement="left" data-original-title="Delete" class="btn btn-default btn-sm btn-danger fa fa-times" disabled></a>'
            // }



            $('td:eq(6)', nRow).html(actionHtml);

        },
        "fnDrawCallback": function (settings, json) {
            //Tooltips
            $('[data-toggle="tooltip"]').tooltip()
        }
    })

}

function SaveClientAdd() {
    var isValid = true;
    var errorText = "";
    $(".haserror").removeClass("haserror");

    var ClientCardNrAddO = $('#tbClientCardNrAdd');
    var ClientCardNrAdd = ClientCardNrAddO.val();
    var ClientCompanyNameAddO = $('#tbClientCompanyNameAdd');
    var ClientCompanyNameAdd = ClientCompanyNameAddO.val();
      
    var ClientFirstNameAddO = $('#tbClientNameAdd');
    var ClientFirstNameAdd = ClientFirstNameAddO.val();
    var ClientLastNameAddO = $('#tbClientSurnameAdd');
    var ClientLastNameAdd = ClientLastNameAddO.val();
    var ClientEmailAddO = $('#tbClientEmailAdd');
    var ClientEmailAdd = ClientEmailAddO.val();
    var ClientCellAddO = $('#tbClientCellAdd');
    var ClientCellAdd = ClientCellAddO.val();
       
    var ClientAddressStreetAddO = $('#tbClientAddressStreetAdd');
    var ClientAddressStreetAdd = ClientAddressStreetAddO.val();
    var ClientAddressSuburbAddO = $('#tbClientAddressSuburbAdd');
    var ClientAddressSuburbAdd = ClientAddressSuburbAddO.val();
    var ClientAddressCityAddO = $('#tbClientAddressCityAdd');
    var ClientAddressCityAdd = ClientAddressCityAddO.val();
    var ClientZipCodeAddO = $('#tbClientZipCodeAdd');
    var ClientZipCodeAdd = ClientZipCodeAddO.val();

    ///Validation
    if (!tcg.valid8r.req(ClientCardNrAdd, ClientCardNrAddO)) {
        isValid = false;
        errorText += "Please fill in Card Number<br />";
    }
    if (!tcg.valid8r.req(ClientFirstNameAdd, ClientFirstNameAddO)) {
        isValid = false;
        errorText += "Please fill in First Name<br />";
    }    
    if (!tcg.valid8r.req(ClientCellAdd, ClientCellAddO)) {
        isValid = false;
        errorText += "Please fill in Cell<br />";
    }

    if (isValid) {
        JsonO = {
            CardNumber: ClientCardNrAdd,
            CompanyName: ClientCompanyNameAdd,
            FirstName: ClientFirstNameAdd,
            LastName: ClientLastNameAdd,
            Email: ClientEmailAdd,
            Cell: ClientCellAdd,
            AddressStreet: ClientAddressStreetAdd,
            AddressSuburb: ClientAddressSuburbAdd,
            AddressCity: ClientAddressCityAdd,
            ZipCode: ClientZipCodeAdd,
            
        };

        url = base_url + "/api/admin/customer/add";

        tcg.ajax.post(url, JsonO, function (data) {
            if (data.isSuccess == true) {

                dtClientList.fnDraw();
                cancelEdit();
                $("#modalAddCustomer").modal("hide");
                tcg.widgets.Growl("Successfully added a Client", "Success", "success", "3500");
            }
            else {
                //tcg.widgets.Growl(data.errorText, "Error", "danger", 4000);
            }
        },
            function () {
                tcg.widgets.Growl("An error has occured while trying to add the Client", "Error", "danger", 4000);
            });
    }
    else {
        $('#lblErrorAdd').html(errorText);
    }
}
function cancelEdit() {
    $(".haserror").removeClass("haserror");

    $('#tbClientCardNrAdd').val('');
    $('#tbClientCompanyNameAdd').val('');
    $('#tbClientNameAdd').val('');
    $('#tbClientSurnameAdd').val('');
    $('#tbClientEmailAdd').val('');
    $('#tbClientCellAdd').val('');
    $('#tbClientAddressStreetAdd').val('');
    $('#tbClientAddressSuburbAdd').val('');
    $('#tbClientAddressCityAdd').val('');
    $('#tbClientZipCodeAdd').val(''); 
    $('#lblErrorAdd').html('');

    $('#tbClientCardNrEdit').val('');
    $('#tbClientCompanyNameEdit').val('');
    $('#tbClientNameEdit').val('');
    $('#tbClientSurnameEdit').val('');
    $('#tbClientEmailEdit').val('');
    $('#tbClientCellEdit').val('');
    $('#tbClientAddressStreetEdit').val('');
    $('#tbClientAddressSuburbEdit').val('');
    $('#tbClientAddressCityEdit').val('');
    $('#tbClientZipCodeEdit').val('');
    $('#lblErrorEdit').html('');
}

function OpenEditModal(row) {

    cancelEdit();
    var Client = dtClientList.fnGetData(row);

    var CustomerID = Client["customerID"];
    var CardNumber = Client["CardNumber"];
    var CompanyName = Client["CompanyName"];
    var FirstName = Client["FirstName"];
    var LastName = Client["LastName"];
    var Email = Client["Email"];
    var Cell = Client["Cell"];
    var AddressStreet = Client["AddressStreet"];
    var AddressSuburb = Client["AddressSuburb"];
    var AddressCity = Client["AddressCity"];
    var ZipCode = Client["ZipCode"];

    $('#tbClientCardNrEdit').val(CardNumber);
    $('#tbClientCompanyNameEdit').val(CompanyName);
    $('#tbClientNameEdit').val(FirstName);
    $('#tbClientSurnameEdit').val(LastName);
    $('#tbClientEmailEdit').val(Email);
    $('#tbClientCellEdit').val(Cell);
    $('#tbClientAddressStreetEdit').val(AddressStreet);
    $('#tbClientAddressSuburbEdit').val(AddressSuburb);
    $('#tbClientAddressCityEdit').val(AddressCity);
    $('#tbClientZipCodeEdit').val(ZipCode);

    customerID = CustomerID;

    $("#modalEditClient").modal("show");
}

function SaveClientEdit() {
    var isValid = true;
    var errorText = "";
    $(".haserror").removeClass("haserror");

    var ClientCardNrEditO = $('#tbClientCardNrEdit');
    var ClientCardNrEdit = ClientCardNrEditO.val();
    var ClientCompanyNameEditO = $('#tbClientCompanyNameEdit');
    var ClientCompanyNameEdit = ClientCompanyNameEditO.val();

    var ClientFirstNameEditO = $('#tbClientNameEdit');
    var ClientFirstNameEdit = ClientFirstNameEditO.val();
    var ClientLastNameEditO = $('#tbClientSurnameEdit');
    var ClientLastNameEdit = ClientLastNameEditO.val();
    var ClientEmailEditO = $('#tbClientEmailEdit');
    var ClientEmailEdit = ClientEmailEditO.val();
    var ClientCellEditO = $('#tbClientCellEdit');
    var ClientCellEdit = ClientCellEditO.val();

    var ClientAddressStreetEditO = $('#tbClientAddressStreetEdit');
    var ClientAddressStreetEdit = ClientAddressStreetEditO.val();
    var ClientAddressSuburbEditO = $('#tbClientAddressSuburbEdit');
    var ClientAddressSuburbEdit = ClientAddressSuburbEditO.val();
    var ClientAddressCityEditO = $('#tbClientAddressCityEdit');
    var ClientAddressCityEdit = ClientAddressCityEditO.val();
    var ClientZipCodeEditO = $('#tbClientZipCodeEdit');
    var ClientZipCodeEdit = ClientZipCodeEditO.val();

    ///Validation
    if (!tcg.valid8r.req(ClientCardNrEdit, ClientCardNrEditO)) {
        isValid = false;
        errorText += "Please fill in Card Number<br />";
    }
    if (!tcg.valid8r.req(ClientFirstNameEdit, ClientFirstNameEditO)) {
        isValid = false;
        errorText += "Please fill in First Name<br />";
    }
    if (!tcg.valid8r.req(ClientCellEdit, ClientCellEditO)) {
        isValid = false;
        errorText += "Please fill in Cell<br />";
    }

    if (isValid) {
        JsonO = {
            customerID: customerID,
            CardNumber: ClientCardNrEdit,
            CompanyName: ClientCompanyNameEdit,
            FirstName: ClientFirstNameEdit,
            LastName: ClientLastNameEdit,
            Email: ClientEmailEdit,
            Cell: ClientCellEdit,
            AddressStreet: ClientAddressStreetEdit,
            AddressSuburb: ClientAddressSuburbEdit,
            AddressCity: ClientAddressCityEdit,
            ZipCode: ClientZipCodeEdit,
        };

        url = base_url + "/api/admin/customer/edit";

        tcg.ajax.post(url, JsonO, function (data) {
            if (data.isSuccess == true) {

                dtClientList.fnDraw();
                cancelEdit();
                $("#modalEditClient").modal("hide");
                tcg.widgets.Growl("Successfully edited a Client", "Success", "success", "3500");
            }
            else {
                //tcg.widgets.Growl(data.errorText, "Error", "danger", 4000);
            }
        },
            function () {
                tcg.widgets.Growl("An error has occured while trying to add the Client", "Error", "danger", 4000);
            });
    }
    else {
        $('#lblErrorEdit').html(errorText);
    }
}

function DeleteClient(customerID) {

    tcg.widgets.confirm('Are you sure you want to delete this Client?', 'Delete Client', function () {

        var url = base_url + "/api/admin/customer/delete?customerID=" + customerID;

        var successF = function (data) {
            if (data.isSuccess) {

                dtClientList.fnDraw();
                tcg.widgets.Growl("Client has been successfully removed", "Success", "success", 3500);
            }
            else {
                tcg.widgets.Growl(data.errorText, "Error", "danger", 3500);
            }
        };

        var errorF = function (data) {
            tcg.widgets.Growl("An error has occurred. Please try again or contact support if problem persists.", "Error", "danger", 3500);
        }

        tcg.ajax.post(url, {}, successF, errorF);
    });
}

