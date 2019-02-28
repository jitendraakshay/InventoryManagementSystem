//NB: CUSTOM HANDLER FOR MULTISELECT DROP DOWN

ko.bindingHandlers.selectPicker = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        if ($(element).is('select')) {
            if (ko.isObservable(valueAccessor())) {
                if ($(element).prop('multiple') && $.isArray(ko.utils.unwrapObservable(valueAccessor()))) {
                    // in the case of a multiple select where the valueAccessor() is an observableArray, call the default Knockout selectedOptions binding
                    ko.bindingHandlers.selectedOptions.init(element, valueAccessor, allBindingsAccessor);
                } else {
                    // regular select and observable so call the default value binding
                    ko.bindingHandlers.value.init(element, valueAccessor, allBindingsAccessor);
                }
            }
            $(element).addClass('selectpicker').selectpicker();
        }
    },
    update: function (element, valueAccessor, allBindingsAccessor) {
        if ($(element).is('select')) {
            var isDisabled = ko.utils.unwrapObservable(allBindingsAccessor().disable);
            if (isDisabled) {
                // the dropdown is disabled and we need to reset it to its first option
                $(element).selectpicker('val', $(element).children('option:last').val());
            }
            // React to options changes
            ko.unwrap(allBindingsAccessor.get('options'));
            // React to value changes
            ko.unwrap(allBindingsAccessor.get('value'));
            // Wait a tick to refresh
            setTimeout(() => { $(element).selectpicker('refresh'); }, 0);
        }
    }
};

//END CUSTOM HANDLER FOR MULTISELECT DROP DOWN

//NB: CREATING USER OBJECT IN KNOCKOUT
function Users(data) {
    var self = this;
    self.UserID = ko.observable(data.UserID);
    self.FirstName = ko.observable(data.FirstName);
    self.LastName = ko.observable(data.LastName);
    self.UserName = ko.observable(data.UserName);
    self.Email = ko.observable(data.Email);
    self.PhoneNo = ko.observable(data.PhoneNo);
    self.MobileNo = ko.observable(data.MobileNo);
    self.Email = ko.observable(data.Email);
    self.Address = ko.observable(data.Address);
    self.Password = ko.observable(data.Password);
    self.RoleID = ko.observable(data.RoleID);
    self.IsActive = ko.observable(data.IsActive);
    self.EntryBy = ko.observable(data.EntryBy);   
}
//END CREATING USER OBJECT IN KNOCKOUT

//NB: CREATING ROLE OBJECT IN KNOCKOUT
function Role(data) {
    var self = this;
    self.RoleName = ko.observable(data.RoleName);
    self.RoleID = ko.observable(data.RoleID);
}
//END CREATING ROLE OBJECT IN KNOCKOUT


//NB: CREATING Status OBJECT IN KNOCKOUT
function Status(data) {
    var self = this;
    self.StatusName = ko.observable(data.StatusName);
    self.StatusValue = ko.observable(data.StatusValue);
}
//END CREATING Status OBJECT IN KNOCKOUT


//NB: CREATING USERS VIEW MODEL IN KNOCKOUT
var UserViewModel = function () {
    var self = this;
   
    //Start : User Observables declare here
    self.UserID = ko.observable();
    self.FirstName = ko.observable();
    self.LastName = ko.observable();
    self.UserName = ko.observable();
    self.Email = ko.observable();
    self.PhoneNo = ko.observable();
    self.MobileNo = ko.observable();    
    self.Address = ko.observable();
    self.Password = ko.observable();
    self.CPassword = ko.observable();
    self.UserArray = ko.observableArray([]);
    self.IsActive = ko.observable(true);
    self.StatusName = ko.observable();
    self.StatusValue = ko.observable();
    self.EntryBy = ko.observable();
    self.RoleName = ko.observable();
    self.RoleID = ko.observable();
    self.SelectedRole = ko.observable();
    self.RoleArray = ko.observableArray([]);
    self.Role = ko.observable();
    self.AddedRoleArray = ko.observableArray([]);
    //End : User Observables end here
    var roleIDs;
    //NB : GETTING USER ROLES
    self.getRoles = function () {
        $.ajax({
            url: "/Admin/Role/getRoles",
            type: "GET",
            async: false,
            success: function (response) {

                var obj = JSON.parse(response);

                var mappedTasks = $.map(ko.toJS(obj.ResponseData), function (item) {
                    return new Role(item);
                });
                
                self.RoleArray(ko.toJS(mappedTasks));
                
            },
            error: function () {
                ShowAlertMessage('error', "Some Error Occured");
            }

        });
    }
    //END GETTING USER ROLES
    
    //NB : GETTING ALL USER 
    self.getAllUsers = function () {
        $.ajax({
            url: "/Admin/User/getAllUsers",
            type: "GET",
            success: function (response) {

                var obj = JSON.parse(response);

                var mappedTasks = $.map(ko.toJS(obj.ResponseData), function (item) {
                    return new Users(item);
                });
                
                self.UserArray(ko.toJS(mappedTasks));
            },
            error: function () {
                ShowAlertMessage('error', "Some Error Occured");
            }

        });
    }
    
    //END GETTING ALL USER
    $('#ddlRole').on('hidden.bs.select', function (e) {        // console.log(e.target.value);
        self.AddedRoleArray([]);
        $.each(e.target.selectedOptions, function (index, obj) {           
            self.AddedRoleArray.push(obj.value);
        });
    });
     //NB : SAVE USER 
    self.saveUser = function () {        
        if (self.Validate()) {           
        $.ajax({
            type: "POST",
            url: "/Admin/User/saveUser",
            data: {
                UserID: ko.toJS(self.UserID),
                FirstName: ko.toJS(self.FirstName),
                LastName: ko.toJS(self.LastName),
                UserName: ko.toJS(self.UserName),
                Email: ko.toJS(self.Email),
                PhoneNo: ko.toJS(self.PhoneNo),
                MobileNo: ko.toJS(self.MobileNo),
                Password: ko.toJS(self.Password),
                RoleID: JSON.stringify(ko.toJS(self.AddedRoleArray())),
                Address: ko.toJS(self.Address),
                IsActive: ko.toJS(self.IsActive)
            },
            success: function (data) {
                jAlert(data.message, "Success");
                //ShowAlertMessage('success', data.message);
                self.clearForm();
                self.getAllUsers();
            },
            error: function () {
                jAlert("Some Error Occured", "Error");
                //ShowAlertMessage('error', "Some Error Occured");
            }
        });       

        }

    }
    //END SAVE USER
   

    //NB: DELETE USER
    self.deleteUser = function (data) {

        var msg = "";      
        var active = "";
        if (data.IsActive == true) {
            msg = 'Disable account?';
            active = 'false';
        }
        else {
            msg = 'Enable account?';
            active = 'true';
        }

        jConfirm('Are you sure?', msg, function (r) {
            if (r) {
                $.ajax({
                    url: "/Admin/User/deleteUser",
                    type: "GET",
                    data: {
                        UserID: data.UserID,
                        IsActive: active
                    },
                    success: function (response) {
                        jAlert(response.message, "Success");
                        //ShowAlertMessage('success', response.message);   
                        //self.UserArray.remove(data);    
                        self.getAllUsers();
                    },
                    error: function () {
                        jAlert("Some Error Occured", "Error");
                        //ShowAlertMessage('error', "Some Error Occured");
                    }
                });
            }
        });
        
    }
    //END DELETE USER


    //NB: RESET ACCOUNT
    self.resetPassword = function (data) {
        jConfirm('Are you sure?', "Reset Password?", function (r) {
            if (r) {
                $.ajax({
                    url: "/Admin/User/resetPassword",
                    type: "GET",
                    data: {
                        UserID: data.UserID,
                        UserName: data.UserName
                    },
                    success: function (response) {
                        jAlert(response.message, "Success");
                        //ShowAlertMessage('success', response.message);
                        //self.UserArray.remove(data);    
                        self.getAllUsers();
                    },
                    error: function () {
                        jAlert("Some Error Occured", "Error");
                        //ShowAlertMessage('error', "Some Error Occured");
                    }
                });
            }
        });
    }
    //END RESET ACCOUNT


    //NB: UPDATE USER
    self.updateUser = function (data) {
        self.UserID(ko.toJS(data.UserID));
        self.FirstName(ko.toJS(data.FirstName));
        self.LastName(ko.toJS(data.LastName));
        self.UserName(ko.toJS(data.UserName));
        self.Email(ko.toJS(data.Email));
        self.PhoneNo(ko.toJS(data.PhoneNo));
        self.MobileNo(ko.toJS(data.MobileNo));
        if (data.RoleID != undefined || data.RoleID != undefined) {
            roleIDs = ko.toJS(data.RoleID).split(",");
            $("#ddlRole").selectpicker("val", roleIDs);
            $('#ddlRole').selectpicker('refresh');
            self.AddedRoleArray([]);
            self.AddedRoleArray.push(ko.toJS(data.RoleID));
        }
        //self.RoleID(ko.toJS(data.RoleID));
        self.Address(ko.toJS(data.Address));
        self.IsActive(ko.toJS(data.IsActive));
        $('.nav-tabs a[href="#CreateUser"]').tab('show');
        $('#spnCreateUser').text("Update User");
        $('#btnSaveUser').text("Update User");
        
    }
    //END UPDATE USER
    $(document).on('change', '.selectpicker', function () {
        $('.selectpicker').selectpicker('refresh');
    });
    //NB: CLEAR FORM
    self.clearForm = function () {
        self.UserID(null);
        self.FirstName(null);
        self.LastName(null);
        self.UserName(null);
        self.Email(null);
        self.PhoneNo(null);
        self.MobileNo(null);
        self.Password(null);
        self.RoleID(null);
        self.Address(null);
        self.RoleArray(null);
        self.getRoles();
        self.IsActive(true);
        self.CPassword(null);
        $('#spnCreateUser').text("Create User");
        $('#btnSaveUser').text("Save User");
    }
    //END CLEAR FORM


    //NB: Validate USER BEFORE SAVE

    self.Validate = function () {
        var errMsg = '';

        if (Validate.empty(ko.toJS(self.FirstName()))) {
            errMsg += 'Invalid First Name !!!<br>';
        }

        if (Validate.empty(ko.toJS(self.LastName()))) {
            errMsg += 'Invalid Last Name !!!<br>';
        }
        if (Validate.empty(ko.toJS(self.UserName()))) {
            errMsg += 'Invalid User Name !!!<br>';
        }
        
        if (Validate.email(self.Email())) {
            errMsg += "Invalid Email !!!<br>";
        }       
        if (Validate.phone(self.MobileNo())) {
            errMsg += 'Invalid Mobile Number !!!<br>';
        }
        if (self.UserID() == undefined)
        {
            if (Validate.empty(self.Password())) {
                errMsg += 'Invalid Password !!!<br>';
            }
            if (ko.toJS(self.Password()) != ko.toJS(self.CPassword())) {
                errMsg += 'Does Not Match Password !!!<br>';
            }
        }
        if (errMsg !== '') {
            
            jAlert(errMsg, "Warning.");
            return false;
        }
        else {
            return true;
        }
    }

    //END VALIDATE USER BEFORE SAVE

    //TAB CHANGE EVENT TO CLEAR FORM THAT IS SET FROM EDIT USER
    $("a[href='#AllUsers']").on('shown.bs.tab', function (e) {
        self.clearForm();
    });
    //END TAB CHANGE EVENT

    //NB: CALLING JAVASCRIPT FUNCTION ON LOAD
    self.getAllUsers();
    self.getRoles();
    //END CALLING JAVASCRIPT FUNCTION ON LOAD



    $('#empid').on('click', function () {
        alert($(this).val());
        console.log($(this).val());
    });
}
//END CREATING USERS VIEW MODEL IN KNOCKOUT

//NB: DOCUMENT READY FUNCTION
$(document).ready(function () {    
    ko.applyBindings(new UserViewModel()); 

});
//END DOCUMENT READY FUNCTION