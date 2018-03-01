var mainJson = null;
var mainLoggedInUserID = null;
var dtAdminList = null;
var userID;
$(document).ready(function () {

    mainJson = jQuery.parseJSON($('#jsonData').html());
    mainLoggedInUserID = jQuery.parseJSON($('#jsonLIUID').html());
    debugger;
    InitAdminListTable();

});

function InitAdminListTable() {

    dtAdminList = $('#tblAdminList').dataTable({

        "sSearch": true,
        "dom": '<"top"fl>rt<"bottom"p><"clear">', // "dom": '<"top"fl>rt<"bottom"ip><"clear">',
        "sPaginationType": "full_numbers",//IMPORTANT
        "aoColumnDefs": [{ 'bSortable': false, 'aTargets': [4] }],
        "oLanguage": { "sEmptyTable": "No data to display" },
        "sSearch": true,
        "bJQueryUI": false,
        "bAutoWidth": false,
        "sAjaxSource": base_url + '/api/admin/admin/list',
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
        "aoColumns": [{ "sTitle": "First Name", "mDataProp": "FirstName", "sWidth": "20%" },
        { "sTitle": "Last Name", "mDataProp": "LastName", "sWidth": "20%" },
        { "sTitle": "Email", "mDataProp": "Email", "sWidth": "15%" },
        { "sTitle": "Cell", "mDataProp": "Cell", "sWidth": "15%" },        
        { "sTitle": "Actions", "mDataProp": "userID", "sClass": "text-center w100 minw100" }


        ],
        "fnServerData": function (sSource, aoData, fnCallback, oSettings) {
            doDataTablePostAjaxCalling(this, sSource, aoData, fnCallback, oSettings);
        },

        "iDeferLoading": [mainJson.Count, mainJson.Count],
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {

            //Action Buttons
            var actionHtml = "";
            actionHtml += ' <a href="javascript:void(0);" onclick="OpenEditModal(' + nRow["_DT_RowIndex"] + ')" data-toggle="tooltip" title="Edit" data-placement="left" data-original-title="Edit" class="btn btn-default btn-sm btn-primary fa fa-pencil"></a>';
            actionHtml += ' <a href="javascript:void(0);" onclick="OpenEditPasswordModal(' + nRow["_DT_RowIndex"] + ')" data-toggle="tooltip" title="Change Password" data-placement="left" data-original-title="Change Password" class="btn btn-default btn-sm btn-dark fa fa-key"></a>';
            //if (aData["isDeletable"]) {
            debugger;
            if (mainLoggedInUserID.LoggedInUserID != aData["userID"]) {
                actionHtml += ' <a href="javascript:void(0);" onclick="DeleteAdmin(' + aData["userID"] + ')" data-toggle="tooltip" title="Delete" data-placement="left" data-original-title="Delete" class="btn btn-default btn-sm btn-danger fa fa-times"></a>';
            }
            //else {
            //    actionHtml += ' <a href="javascript:void(0);" data-toggle="tooltip" title="Delete" data-placement="left" data-original-title="Delete" class="btn btn-default btn-sm btn-danger fa fa-times" disabled></a>'
            // }



            $('td:eq(4)', nRow).html(actionHtml);

        },
        "fnDrawCallback": function (settings, json) {
            //Tooltips
            $('[data-toggle="tooltip"]').tooltip()
        }
    })

}

function SaveAdminAdd() {
    var isValid = true;
    var errorText = "";
    $(".haserror").removeClass("haserror");

    var AdminFirstNameAddO = $('#tbAdminNameAdd');
    var AdminFirstNameAdd = AdminFirstNameAddO.val();
    var AdminLastNameAddO = $('#tbAdminSurnameAdd');
    var AdminLastNameAdd = AdminLastNameAddO.val();
    var AdminEmailAddO = $('#tbAdminEmailAdd');
    var AdminEmailAdd = AdminEmailAddO.val();
    var AdminCellAddO = $('#tbAdminCellAdd');
    var AdminCellAdd = AdminCellAddO.val();
    var AdminPasswordAddO = $('#tbAdminPasswordAdd');
    var AdminPasswordAdd = AdminPasswordAddO.val();
    var AdminConfirmPasswordAddO = $('#tbAdminConfirmPasswordAdd');
    var AdminConfirmPasswordAdd = AdminConfirmPasswordAddO.val();

    debugger;
    ///Validation
    if (!tcg.valid8r.req(AdminFirstNameAdd, AdminFirstNameAddO)) {
        isValid = false;
        errorText += "Please fill in Name<br />";
    }
    if (!tcg.valid8r.req(AdminEmailAdd, AdminEmailAddO)) {
        isValid = false;
        errorText += "Please fill in Email<br />";
    }
    if (!tcg.valid8r.req(AdminCellAdd, AdminCellAddO)) {
        isValid = false;
        errorText += "Please fill in Cell<br />";
    }
    if (!tcg.valid8r.req(AdminPasswordAdd, AdminPasswordAddO)) {
        isValid = false;
        errorText += "Please fill in Password<br />";
    }
    //if (!tcg.valid8r.req(AdminConfirmPasswordAdd, AdminConfirmPasswordAddO)) {
    //    isValid = false;
    //    errorText += "Please fill in Confirm Password<br />";
    //}
    if(!tcg.valid8r.passwordconfirmed(AdminPasswordAdd, AdminConfirmPasswordAdd, AdminConfirmPasswordAddO)){
        isValid = false;
        errorText += "Passwords do not match<br />";
    }
    //if (AdminPasswordAdd != "" && AdminConfirmPasswordAdd != "") {
    //    if (AdminPasswordAdd != AdminConfirmPasswordAdd){
    //        isValid = false;
    //        errorText += "Passwords do not match<br />";
    //    }
        
    //}

    if (isValid) {
        JsonO = {
            FirstName: AdminFirstNameAdd,
            LastName: AdminLastNameAdd,
            Email: AdminEmailAdd,
            Cell: AdminCellAdd,
            PasswordHash: AdminPasswordAdd,
        };

        url = base_url + "/api/admin/admin/add";

        tcg.ajax.post(url, JsonO, function (data) {
            if (data.isSuccess == true) {

                dtAdminList.fnDraw();
                cancelEdit();
                $("#modalAddAdmin").modal("hide");
                tcg.widgets.Growl("Successfully added a Admin", "Success", "success", "3500");
            }
            else {
                //tcg.widgets.Growl(data.errorText, "Error", "danger", 4000);
            }
        },
            function () {
                tcg.widgets.Growl("An error has occured while trying to add the Admin", "Error", "danger", 4000);
            });
    }
    else {
        $('#lblErrorAdd').html(errorText);
    }
}
function cancelEdit() {
    $(".haserror").removeClass("haserror");

    $('#tbAdminNameAdd').val('');
    $('#tbAdminSurnameAdd').val('');
    $('#tbAdminEmailAdd').val('');
    $('#tbAdminCellAdd').val('');
    $('#tbAdminPasswordAdd').val('');
    $("#lblErrorAdd").html('');

    $("#tbAdminNameEdit").val('');
    $("#tbAdminSurnameEdit").val('');
    $("#tbAdminEmailEdit").val('');
    $("#tbAdminCellEdit").val('');
    $("#lblErrorEdit").html('');

    $("#tbAdminPasswordEdit").val('');
    $("#tbAdminConfirmPasswordEdit").val('');
    $("#lblErrorChange").html('');
}

function OpenEditModal(row) {

    cancelEdit();
    var Admin = dtAdminList.fnGetData(row);

    var UserID = Admin["userID"];
    var FirstName = Admin["FirstName"];
    var LastName = Admin["LastName"];
    var Email = Admin["Email"];
    var Cell = Admin["Cell"];
    var Pass = Admin["PasswordHash"]

    $("#tbAdminNameEdit").val(FirstName);
    $("#tbAdminSurnameEdit").val(LastName);
    $("#tbAdminEmailEdit").val(Email);
    $("#tbAdminCellEdit").val(Cell);

    userID = UserID;

    $("#modalEditAdmin").modal("show");
}
function OpenEditPasswordModal(row) {
    cancelEdit();
    var Admin = dtAdminList.fnGetData(row);
    var UserID = Admin["userID"];
    userID = UserID;
    $("#modalEditPasswordAdmin").modal("show");
}


function SaveAdminEdit() {
    var isValid = true;
    var errorText = "";
    $(".haserror").removeClass("haserror");

    var AdminFirstNameAddO = $('#tbAdminNameEdit');
    var AdminFirstNameAdd = AdminFirstNameAddO.val();
    var AdminLastNameAddO = $('#tbAdminSurnameEdit');
    var AdminLastNameAdd = AdminLastNameAddO.val();
    var AdminEmailAddO = $('#tbAdminEmailEdit');
    var AdminEmailAdd = AdminEmailAddO.val();
    var AdminCellAddO = $('#tbAdminCellEdit');
    var AdminCellAdd = AdminCellAddO.val();


    ///Validation
    if (!tcg.valid8r.req(AdminFirstNameAdd, AdminFirstNameAddO)) {
        isValid = false;
        errorText += "Please fill in Name<br />";
    }
    if (!tcg.valid8r.req(AdminEmailAdd, AdminEmailAddO)) {
        isValid = false;
        errorText += "Please fill in Email<br />";
    }
    if (!tcg.valid8r.req(AdminCellAdd, AdminCellAddO)) {
        isValid = false;
        errorText += "Please fill in Cell<br />";
    }

    if (isValid) {
        JsonO = {
            userID: userID,
            FirstName: AdminFirstNameAdd,
            LastName: AdminLastNameAdd,
            Email: AdminEmailAdd,
            Cell: AdminCellAdd,
        };

        url = base_url + "/api/admin/admin/edit";

        tcg.ajax.post(url, JsonO, function (data) {
            if (data.isSuccess == true) {

                dtAdminList.fnDraw();
                cancelEdit();
                $("#modalEditAdmin").modal("hide");
                tcg.widgets.Growl("Successfully edited a Admin", "Success", "success", "3500");
            }
            else {
                //tcg.widgets.Growl(data.errorText, "Error", "danger", 4000);
            }
        },
            function () {
                tcg.widgets.Growl("An error has occured while trying to add the Admin", "Error", "danger", 4000);
            });
    }
    else {
        $('#lblErrorEdit').html(errorText);
    }
}

function DeleteAdmin(userID) {

    tcg.widgets.confirm('Are you sure you want to delete this Admin?', 'Delete Admin', function () {

        var url = base_url + "/api/admin/admin/delete?userID=" + userID;

        var successF = function (data) {
            if (data.isSuccess) {

                dtAdminList.fnDraw();
                tcg.widgets.Growl("Admin has been successfully removed", "Success", "success", 3500);
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


function SaveAdminPasswordChange() {
    var isValid = true;
    var errorText = "";
    $(".haserror").removeClass("haserror");
    var AdminChangePasswordO = $('#tbAdminPasswordEdit');
    var AdminChangePassword = AdminChangePasswordO.val();
    var AdminChangePasswordConFirmO = $('#tbAdminConfirmPasswordEdit');
    var AdminChangePasswordConFirm = AdminChangePasswordConFirmO.val();

    if (!tcg.valid8r.req(AdminChangePassword, AdminChangePasswordO)) {
        isValid = false;
        errorText += "Please fill in Password<br />";
    }
    if (!tcg.valid8r.passwordconfirmed(AdminChangePassword, AdminChangePasswordConFirm, AdminChangePasswordConFirmO)) {
        isValid = false;
        errorText += "Passwords do not match<br />";
    }

    if (isValid) {
        JsonO = {
            userID: userID,
            PasswordHash: AdminChangePassword,
        };

        url = base_url + "/api/admin/admin/passwordchange";

        tcg.ajax.post(url, JsonO, function (data) {
            if (data.isSuccess == true) {

                dtAdminList.fnDraw();
                cancelEdit();
                $("#modalEditPasswordAdmin").modal("hide");
                tcg.widgets.Growl("Successfully change a Admin Password", "Success", "success", "3500");
            }
            else {
                //tcg.widgets.Growl(data.errorText, "Error", "danger", 4000);
            }
        },
            function () {
                tcg.widgets.Growl("An error has occured while trying to change the Admin password", "Error", "danger", 4000);
            });
    }
    else {
        $('#lblErrorChange').html(errorText);
    }
}