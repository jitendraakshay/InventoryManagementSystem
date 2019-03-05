
//NB: CREATING USER OBJECT IN KNOCKOUT
function Units(data) {
    var self = this;
    self.UnitID = ko.observable(data.UnitID);
    self.UnitName = ko.observable(data.UnitName);
    self.Description = ko.observable(data.Description);
    self.IsActive = ko.observable(data.IsActive);
    self.EntryBy = ko.observable(data.EntryBy);
    self.EntryDate = ko.observable(data.EntryDate);
}
//END CREATING USER OBJECT IN KNOCKOUT

//NB: CREATING USERS VIEW MODEL IN KNOCKOUT
var UnitViewModel = function () {
    var self = this;

    //Start : User Observables declare here
    self.UnitID = ko.observable();
    self.UnitName = ko.observable();
    self.Description = ko.observable();
    self.IsActive = ko.observable(true);
    self.EntryBy = ko.observable();
    self.EntryDate = ko.observable();
    self.UnitArray = ko.observableArray([]);

    //End : User Observables end here
  
    self.getUnits = function () {
        $.ajax({
            url: "/Admin/Units/getAllUnits",
            type: "GET",
            async: false,
            success: function (response) {

                var obj = JSON.parse(response);

                var mappedTasks = $.map(ko.toJS(obj.ResponseData), function (item) {
                    return new Units(item);
                });

                self.UnitArray(ko.toJS(mappedTasks));

            },
            error: function () {
                jAlert("Some Error Occured", "Error");

            }

        });
    }

    //NB : SAVE Unit 
    self.saveUser = function () {
        
            $.ajax({
                type: "POST",
                url: "/Admin/Units/saveUnit",
                data: {
                    UnitID: ko.toJS(self.UnitID),
                    UnitName: ko.toJS(self.UnitName),
                    Description: ko.toJS(self.Description),
                    IsActive: ko.toJS(self.IsActive),
                    EntryBy: ko.toJS(self.EntryBy),
                    EntryDate: ko.toJS(self.EntryDate)                    
                },
                success: function (data) {
                    jAlert(data.message, "Success");      
                    self.getUnits();
                    self.clearForm();
                    $('.nav-tabs a[href="#AllUnits"]').tab('show');
                },
                error: function () {
                    jAlert("Some Error Occured", "Error");
                    //ShowAlertMessage('error', "Some Error Occured");
                }
            });

       

    }
    //END SAVE USER


    //TAB CHANGE EVENT TO CLEAR FORM THAT IS SET FROM EDIT USER
    $("a[href='#AllUnits']").on('shown.bs.tab', function (e) {
        self.clearForm();
    });
    //END TAB CHANGE EVENT

    //NB: DELETE USER
    self.activateUnit = function (data) {

        //var msg = "";
        //var active = "";
        //if (data.IsActive == true) {
        //    msg = 'Disable account?';
        //    active = 'false';
        //}
        //else {
        //    msg = 'Enable account?';
        //    active = 'true';
        //}

        //jConfirm('Are you sure?', msg, function (r) {
        //    if (r) {
        //        $.ajax({
        //            url: "/Admin/User/deleteUser",
        //            type: "GET",
        //            data: {
        //                UserID: data.UserID,
        //                IsActive: active
        //            },
        //            success: function (response) {
        //                jAlert(response.message, "Success");
        //                //ShowAlertMessage('success', response.message);   
        //                //self.UserArray.remove(data);    
        //                self.getAllUsers();
        //            },
        //            error: function () {
        //                jAlert("Some Error Occured", "Error");
        //                //ShowAlertMessage('error', "Some Error Occured");
        //            }
        //        });
        //    }
        //});

    }
    //END DELETE USER


    //NB: RESET ACCOUNT
    self.resetPassword = function (data) {
        //jConfirm('Are you sure?', "Reset Password?", function (r) {
        //    if (r) {
        //        $.ajax({
        //            url: "/Admin/User/resetPassword",
        //            type: "GET",
        //            data: {
        //                UserID: data.UserID,
        //                UserName: data.UserName
        //            },
        //            success: function (response) {
        //                jAlert(response.message, "Success");
        //                //ShowAlertMessage('success', response.message);
        //                //self.UserArray.remove(data);    
        //                self.getAllUsers();
        //            },
        //            error: function () {
        //                jAlert("Some Error Occured", "Error");
        //                //ShowAlertMessage('error', "Some Error Occured");
        //            }
        //        });
        //    }
        //});
    }
    //END RESET ACCOUNT


    //NB: UPDATE USER
    self.UpdateUnit = function (data) {
        self.UnitID(ko.toJS(data.UnitID));
        self.UnitName(ko.toJS(data.UnitName));
        self.Description(ko.toJS(data.Description));
        self.IsActive(ko.toJS(data.IsActive));        
        $('.nav-tabs a[href="#CreateUnit"]').tab('show');
        $('#spnCreateUnit').text("Update Unit");
        $('#btnSaveUnit').text("Update Unit");

    }
    //END UPDATE USER
    
    //NB: CLEAR FORM
    self.clearForm = function () {
        self.UnitID(null);
        self.UnitName(null);
        self.Description(null);        
        self.IsActive(true);        
        $('#spnCreateUnit').text("Create Unit");
        $('#btnSaveUnit').text("Save Unit");
    }
    //END CLEAR FORM


    //NB: Validate USER BEFORE SAVE

    //self.Validate = function () {
    //    var errMsg = '';

    //    if (Validate.empty(ko.toJS(self.FirstName()))) {
    //        errMsg += 'Invalid First Name !!!<br>';
    //    }

    //    if (Validate.empty(ko.toJS(self.LastName()))) {
    //        errMsg += 'Invalid Last Name !!!<br>';
    //    }
    //    if (Validate.empty(ko.toJS(self.UserName()))) {
    //        errMsg += 'Invalid User Name !!!<br>';
    //    }

    //    if (self.UserID() == undefined) {
    //        if (Validate.empty(self.Password())) {
    //            errMsg += 'Invalid Password !!!<br>';
    //        }
    //        if (ko.toJS(self.Password()) != ko.toJS(self.CPassword())) {
    //            errMsg += 'Does Not Match Password !!!<br>';
    //        }
    //    }

    //    if (Validate.email(self.Email())) {
    //        errMsg += "Invalid Email !!!<br>";
    //    }
    //    if (self.PhoneNo() != undefined) {
    //        if (Validate.phone(self.PhoneNo()) || self.PhoneNo().length > 10 || self.PhoneNo().length < 10) {
    //            errMsg += 'Invalid Phone Number !!!<br>';
    //        }
    //    }
    //    if (Validate.phone(self.MobileNo()) || self.MobileNo().length > 10 || self.MobileNo().length < 10) {
    //        errMsg += 'Invalid Mobile Number !!!<br>';
    //    }

    //    if (errMsg !== '') {

    //        jAlert(errMsg, "Warning.");
    //        return false;
    //    }
    //    else {
    //        return true;
    //    }
    //}

    self.getUnits();
}
//END CREATING USERS VIEW MODEL IN KNOCKOUT

//NB: DOCUMENT READY FUNCTION
$(document).ready(function () {
    ko.applyBindings(new UnitViewModel());

});
//END DOCUMENT READY FUNCTION