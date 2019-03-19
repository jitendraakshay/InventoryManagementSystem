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
    self.MiddleName = ko.observable(data.MiddleName);
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
    self.MiddleName = ko.observable();
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
    $("#tblUserList").DataTable().state.clear();
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
                jAlert("Some Error Occured", "Error");
               
            }

        });
    }
    //END GETTING USER ROLES
    
    //NB : GETTING ALL USER 

    
    function LoadUserList() {
        var table = $("#tblUserList").DataTable({
            "bStateSave": true,
            "fixedHeader": true,
            "destroy": true,
            "searching": true,
            "ordering": true,            
            "processing": true,
            "scrollX": true,
            "serverSide": true,            
            "ajax": {
                "url": "/Admin/User/getAllUsers",
                "type": "POST",
                "datatype": "json",
                "error": function (jqXHR, textStatus, errorThrown) {
                    if (jqXHR.status == 302) {
                        $.fn.dataTable.ext.errMode = 'none';
                    }
                }
            },

            "columns": [
                { "data": "serialNo", "name": "serialNo" },  
                { "data": "userID", "name": "userID", "autowidth": true },
                { "data": "firstName", "name": "firstName", "autowidth": true },
                { "data": "lastName", "name": "lastName", "autowidth": true },
                { "data": "userName", "name": "userName", "autowidth": true },
                { "data": "phoneNo", "name": "phoneNo", "autowidth": true },
                { "data": "mobileNo", "name": "mobileNo", "autowidth": true },
                {
                    "data": "isActive", "name": "isActive", "autowidth": true, "render": function (data) {
                        if (data == true) {
                            return "<span class = 'text-blue'>Active</span>";
                            
                        }
                        else {
                            return "<span class = 'text-red'>Inactive</span>";
                        }
                    }
                },
                {
                    "data": null, "name": "Actions", "render": function (data, type, row, meta)
                    {
                        
                        var action = '';            
                        action += "<span class='actions' data-id='" + data + "'>";
                        action += "<a role='button' class='btn btn-primary editUser' href='javascript:void(0)' > <span class='fa  fa-pencil-square-o'></span></a > ";
                        action += "<a role='button' class='btn btn-primary resetPassword' href='javascript:void(0)'><span class='fa fa-recycle'></span></a>";      
                        if (data['isActive'] == true) {
                            action += "<a role='button' class='btn btn-primary deleteUser' href='javascript:void(0)'><span class='fa fa-ban'></span></a>";
                        }
                        else {                            
                            action += "<a role='button' class='btn btn-primary deleteUser' href='javascript:void(0)'><span class='fa fa-check'></span></a>";                         
                        }
                                         
                        action += "</span>";
                        return action;
                    }

                }
                
            ],
           
            "lengthMenu": [SortPageLength(pageLength), SortPageLength(pageLength)],
            "pageLength": pageLength,
            "columnDefs": [
                {
                    "targets": [0, 5],
                    "orderable": false
                }
            ],
            "order": [[1, "asc"]]
        });
        table.columns(1).visible(false);
    }

    
    //$("#tblUserList").on("click", ".editUser", function () {
    //    EditUser($(this));
    //});

    $("#tblUserList").on("click", ".editUser", function () {
        var $row = $(this).closest('tr');
        var data = $('#tblUserList').DataTable().row($row).data();
        self.updateUser(data);
    });

    $("#tblUserList").on("click", ".deleteUser", function () {
        var $row = $(this).closest('tr');
        var data = $('#tblUserList').DataTable().row($row).data();
        self.deleteUser(data);
    });

    $("#tblUserList").on("click", ".resetPassword", function () {
        var $row = $(this).closest('tr');
        var data = $('#tblUserList').DataTable().row($row).data();
        self.resetPassword(data);
    });
    
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
                MiddleName: ko.toJS(self.MiddleName),
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
                LoadUserList();
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
        if (data['isActive'] == true) {
            msg = 'Inactivate account?';
            active = 'false';
        }
        else {
            msg = 'Activate account?';
            active = 'true';
        }

        jConfirm('Are you sure?', msg, function (r) {
            if (r) {
                $.ajax({
                    url: "/Admin/User/deleteUser",
                    type: "GET",
                    data: {
                        UserID: data['userID'],
                        IsActive: active
                    },
                    success: function (response) {
                        jAlert(response.message, "Success");
                        //ShowAlertMessage('success', response.message);   
                        //self.UserArray.remove(data);    
                        LoadUserList();
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
                        UserID: data['userID'],
                        UserName: data['userName']
                    },
                    success: function (response) {
                        jAlert(response.message, "Success");
                        //ShowAlertMessage('success', response.message);
                        //self.UserArray.remove(data);    
                        LoadUserList();
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
       
        self.UserID(data['userID']);
        self.FirstName(data['firstName']);
        self.MiddleName(data['middleName']);
        self.LastName(data['lastName']);
        self.UserName(data['userName']);
        self.Email(data['email']);
        self.PhoneNo(data['phoneNo']);
        self.MobileNo(data['mobileNo']);
        if (data.RoleID != undefined || data.RoleID != '') {
            roleIDs = data['roleID'].split(",");
            $("#ddlRole").selectpicker("val", roleIDs);
            $('#ddlRole').selectpicker('refresh');
            self.AddedRoleArray([]);
            self.AddedRoleArray.push(data['roleID']);
        }
        //self.RoleID(ko.toJS(data.RoleID));
        self.Address(data['address']);
        self.IsActive(data['isActive']);
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

        if (self.UserID() == undefined) {
            if (Validate.empty(self.Password())) {
                errMsg += 'Invalid Password !!!<br>';
            }
            if (ko.toJS(self.Password()) != ko.toJS(self.CPassword())) {
                errMsg += 'Does Not Match Password !!!<br>';
            }
        }

        if (Validate.email(self.Email())) {
            errMsg += "Invalid Email !!!<br>";
        }
        if (self.PhoneNo() != undefined) {
            if (Validate.phone(self.PhoneNo()) || self.PhoneNo().length > 10 || self.PhoneNo().length < 10) {
                errMsg += 'Invalid Phone Number !!!<br>';
            }
        }
        if (Validate.phone(self.MobileNo()) || self.MobileNo().length > 10 || self.MobileNo().length < 10) {
            errMsg += 'Invalid Mobile Number !!!<br>';
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
    //self.getAllUsers();
    self.getRoles();
    LoadUserList();
    //END CALLING JAVASCRIPT FUNCTION ON LOAD
}
//END CREATING USERS VIEW MODEL IN KNOCKOUT

//NB: DOCUMENT READY FUNCTION
$(document).ready(function () {    
    ko.applyBindings(new UserViewModel()); 
    
});
//END DOCUMENT READY FUNCTION