
function Role(data) {
    var self = this;    
    self.RoleName = ko.observable(data.RoleName);
    self.RoleID = ko.observable(data.RoleID);
}

var RoleViewModel = function () {
    var self = this;

    //Start : Role Observables declare here
    self.RoleName = ko.observable();
    self.RoleID = ko.observable();
    self.SelectedRole = ko.observable();
    self.RoleArray = ko.observableArray([]);
    self.Role = ko.observable();   
    //End : Role Observables end here
    self.AllOptions = ko.observable(false);
   
    var AllowedMenu;



    $("div#roleContainer").find('input.user-group-item').on('click', function () {
        var multiItem = $(this).parent().closest('td').attr('data-val');
        var data = $(this).attr('data');
        var arr = Array();
        if (multiItem.length > 0) {
            arr = multiItem.split(',');
        }
        if ($(this).prop('checked')) {
            arr.push(data);
        }
        else {
            arr.splice(arr.indexOf(data), 1);
        }
        $(this).parent().closest('td').attr('data-val', arr.toString());
    });


    
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

    

    self.ChangeRole = function () {
        self.AllOptions(false);
        var roleID = self.SelectedRole().RoleID;
        var model = {
            roleID: roleID
        };       

        $("div#roleContainer").find('input.user-group-item:checked').prop('checked', false);
        $("div#roleContainer").find('td.menu-options').attr('data-val', '');
        $.ajax({
            url: "/Admin/Role/GetMenuByRole",
            type: "GET",
            async: false,
            data: model,
            success: function (response) {
                AllowedMenu = JSON.stringify(response.data);                
                self.SetMenuRole();
            },
            error: function () {
                jAlert("Some Error Occured", "Error");
                //ShowAlertMessage('error', "Some Error Occured");
            }

        });
    }

    
    self.SetMenuRole = function () {

        var menus = AllowedMenu;
        if (menus != undefined && menus !== null && menus.length > 0) {
            menus = unescape(menus);
            menus = JSON.parse(menus);
            
            $('div#roleContainer').find('table td.menu-options').each(function () {
                var menuID = parseInt($(this).attr('data-id'));

                var ele = $(this);
                for (var j = 0; j < menus.length; j++) {
                    if (menus[j].menuID === menuID) {
                        ele.attr('data-val', menus[j].options);
                        var optionsList = menus[j].options.split(',');                        
                        ele.find('input[type=checkbox]').each(function () {
                            if (Contains.call(optionsList, $(this).attr('data'))) { $(this).prop('checked', true); }
                        });
                    }
                }
            });
        }
        else {
            $("div#roleContainer").find('input.edit-to-dropdown').hide();
        }
    }


    self.SaveMenuRole = function () {
        if (self.SelectedRole() != undefined) {
            var menu = Array();
            $('div#roleContainer').find('table td.menu-options input[type=checkbox]:checked').parents('td.menu-options').each(function (i, e) {
                console.log("e,e>>", i + " " + e);
                var obj = new Object();
                obj.MenuID = parseInt($(this).attr('data-id'));
                obj.Options = $(this).attr('data-val');
                menu.push(obj);
            });
            
            var ele = $('div#roleContainer');
            $.ajax({
                type: "POST",
                url: "/Admin/Role/SaveRole",
                data: AddAntiForgeryToken({
                    Name: self.SelectedRole().RoleName,
                    RoleID: self.SelectedRole().RoleID,
                    Menus: JSON.stringify(menu)
                }),
                success: function (data) {
                    if (data !== null && typeof data.result !== 'undefined' && data.result) {
                        $('div#roleContainer').find('select.all-roles-list option:selected').attr('data-val', escape(JSON.stringify(menu)));
                        ShowAlertMessage('success', "Role Added Successfully.");
                        self.SetMenuRole();
                    }

                },
                error: function () {
                    jAlert("Some Error Occured", "Error");
                    //ShowAlertMessage('error', "Some Error Occured");
                }
            });
        }
        else {
            jAlert("Please Select Role", "Error");
            //ShowAlertMessage('error', "Please Select Role");
        }
    }


    self.OpenRoleAddPopup = function () {
        $('#customPopupDialog').modal('show');
        $('form#formRole').find('#Name').val("");
    }
    self.OpenDeleteRolePopup = function () {
        $('#customDeletePopupDialog').modal('show');        
    }


    self.SelectAllOptions = function () {
        var userMenu;
        var menu;
        if (ko.toJS(self.AllOptions) == true) {

            $.ajax({
                url: "/Admin/Role/GetMenus",
                type: "GET",
                async: false,
                success: function (response) {
                    userMenu = JSON.stringify(response.data);        
                    console.log(userMenu);

                },
                error: function () {
                    ShowAlertMessage('error', "Some Error Occured");
                }

            });           
            var menu = userMenu;
            
            menu = unescape(menu);
            menu = JSON.parse(menu);
            
            $('div#roleContainer').find('table td.menu-options').each(function () {
               

                var ele = $(this);
                for (var j = 0; j < menu.length; j++) {
                    var opt = menu[j].options.trim();
                    console.log(opt+"\"");
                        ele.attr('data-val',opt);
                        var optionsList = opt.split(',');
                        ele.find('input[type=checkbox]').each(function () {
                            if (Contains.call(optionsList, $(this).attr('data'))) { $(this).prop('checked', true); }
                        });
                   
                }
            });
        }
        else {

            $("div#roleContainer").find('input.user-group-item:checked').prop('checked', false);
            $("div#roleContainer").find('td.menu-options').attr('data-val', '');
        }
    }

    self.DeleteRole = function (data) {       
        jConfirm('Are you sure?', 'Delete Role', function (r) {
            if (r) {
                $.ajax({
                    url: "/Admin/Role/DeleteRole",
                    type: "POST",
                    data: AddAntiForgeryToken({
                        roleID: data.RoleID
                    }),
                    success: function (response) {
                        jAlert(response.message, "Success");
                        //ShowAlertMessage('success', response.message);
                        $('div#customPopupDialog').modal('hide');
                        self.RoleArray.remove(data);
                        self.getRoles();
                        $('div#customDeletePopupDialog').modal('hide');
                    },
                    error: function () {
                        jAlert("Some Error Occured", "Error");
                        //ShowAlertMessage('error', "Some Error Occured");
                    }
                });
            }
        });
}
   

    $('#customPopupDialog').find('button.ok').on('click', function () {
        if ($('form#formRole').find('#Name').val() == undefined || $('form#formRole').find('#Name').val() == "") {
            ShowAlertMessage('error', "Enter Role Name");
        }
        else if ($('form#formRole').find('#Name').val().length <= 3 || $('form#formRole').find('#Name').val().length > 50) {
            ShowAlertMessage('error', "Role name between 3 to 50 characters");
        }
        else {
            var roleName = $('form#formRole').find('#Name').val();
            var flag = false;

            for (var i = 0; i < self.RoleArray().length; i++) {
                if (ko.toJS(self.RoleArray()[i].RoleName).trim().toLowerCase() == roleName.trim().toLowerCase()) {
                    flag = true;
                }
            }
            if (flag == true) {
                ShowAlertMessage('alert', "Role already exists");
            }
            else {
                $.ajax({
                    url: "/Admin/Role/SaveNewRole",
                    type: "POST",
                    data: AddAntiForgeryToken({
                        roleName: roleName
                    }),
                    success: function (response) {
                        ShowAlertMessage('success', response.message);
                        $('div#customPopupDialog').modal('hide');
                        self.getRoles();
                    },
                    error: function () {
                        ShowAlertMessage('error', "Some Error Occured");
                    }
                });
            }
        }
    });

  

    function AddAntiForgeryToken(data) {
        data.__RequestVerificationToken = $('form input[name=__RequestVerificationToken]').val();
        return data;
    };    
    self.getRoles();
    
}

$(function () {
    ko.applyBindings(new RoleViewModel());   
});