
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
    self.saveUnit = function () {
        if (self.Validate()) {
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
                },
                error: function () {
                    jAlert("Some Error Occured", "Error");
                    //ShowAlertMessage('error', "Some Error Occured");
                }
            });

        }

    }
    //END SAVE Unit
    

    ////TAB CHANGE EVENT TO CLEAR FORM THAT IS SET FROM EDIT USER
    //$("a[href='#AllUnits']").on('shown.bs.tab', function (e) {
    //    self.clearForm();
    //});
    ////END TAB CHANGE EVENT

    
    //NB: UPDATE USER
    self.UpdateUnit = function (data) {
        self.UnitID(ko.toJS(data.UnitID));
        self.UnitName(ko.toJS(data.UnitName));
        self.Description(ko.toJS(data.Description));
        self.IsActive(ko.toJS(data.IsActive));        
        //$('.nav-tabs a[href="#CreateUnit"]').tab('show');
        //$('#spnCreateUnit').text("Update Unit");
        //$('#btnSaveUnit').text("Update Unit");

    }
    //END UPDATE USER
    
    //NB: CLEAR FORM
    self.clearForm = function () {
        self.UnitID(null);
        self.UnitName(null);
        self.Description(null);        
        self.IsActive(true);        
        //$('#spnCreateUnit').text("Create Unit");
        //$('#btnSaveUnit').text("Save Unit");
    }
    //END CLEAR FORM


    

    self.Validate = function () {
        var errMsg = '';

        if (Validate.empty(ko.toJS(self.UnitName()))) {
            errMsg += 'Invalid Unit Name !!!<br>';
        }

        if (errMsg !== '') {

            jAlert(errMsg, "Warning.");
            return false;
        }
        else {
            return true;
        }
    }

    self.getUnits();
}
//END CREATING USERS VIEW MODEL IN KNOCKOUT

//NB: DOCUMENT READY FUNCTION
$(document).ready(function () {
    ko.applyBindings(new UnitViewModel());
    

});
//END DOCUMENT READY FUNCTION