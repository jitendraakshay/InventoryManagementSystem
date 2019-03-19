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
function Products(data) {
    var self = this;
    self.ProductID = ko.observable(data.ProductID);
    self.ProductName = ko.observable(data.ProductName);
    self.ProductDescription = ko.observable(data.ProductDescription);
    self.IsProductActive = ko.observable(data.IsProductActive);
    self.EntryBy = ko.observable(data.EntryBy);
    self.EntryDate = ko.observable(data.EntryDate);
}

function Attribute(data) {
    var self = this;
    self.OptionID = ko.observable(data.OptionID);
    self.ProductID = ko.observable(data.ProductID);
    self.OptionName = ko.observable(data.OptionName);
    self.OptionDescription = ko.observable(data.OptionDescription);
    self.IsOptionActive = ko.observable(data.IsOptionActive);
    self.EntryBy = ko.observable(data.EntryBy);
    self.EntryDate = ko.observable(data.EntryDate);
}

function AttributeValue(data) {
    
    var self = this;
    self.OptionID = ko.observable(data.OptionID);
    self.OptionName = ko.observable(data.OptionName);
    self.ProductID = ko.observable(data.ProductID);
    self.ProductName = ko.observable(data.ProductName);
    self.ValueName = ko.observable(data.ValueName);
    self.ValueID = ko.observable(data.ValueID);
    self.IsValueActive = ko.observable(data.IsValueActive);   
    self.EntryBy = ko.observable(data.EntryBy);
    self.EntryDate = ko.observable(data.EntryDate);
}

function SKUs(data) {
    
    var self =this;    
    self.OptionID = ko.observable(data.OptionID);
    self.OptionName = ko.observable(data.OptionName);
    self.ProductID = ko.observable(data.ProductID);
    self.ProductName = ko.observable(data.ProductName);
    self.ValueID = ko.observable(data.ValueID);
    self.ValueName = ko.observable(data.ValueName);
    self.SKUID = ko.observable(data.SKUID);
    self.SKU = ko.observable(data.SKU);
  
}
//END CREATING USER OBJECT IN KNOCKOUT

//NB: CREATING USERS VIEW MODEL IN KNOCKOUT
var ProductViewModel = function () {
    var self = this;
   
    //Start : User Observables declare here
    self.ProductID = ko.observable();
    self.ProductName = ko.observable();
    self.ProductDescription = ko.observable();
    self.IsProductActive = ko.observable(true);
    self.EntryBy = ko.observable();
    self.EntryDate = ko.observable();
    self.OptionID = ko.observable();    
    self.OptionName = ko.observable();
    self.OptionDescription = ko.observable();
    self.IsOptionActive = ko.observable(true);
    self.SelectedAttribute = ko.observable();
    self.ProductArray = ko.observableArray();
    self.AttributeArray = ko.observableArray([]);
    self.ValueName = ko.observable();
    self.ValueID = ko.observable();
    self.IsValueActive = ko.observable(true);
    self.AttributeValueArray = ko.observableArray([]);
    self.TempProductID = ko.observable();
    self.SelectedAttributeValue = ko.observable();
    self.IsSKUActive = ko.observable();
    self.SKUs = ko.observable();
    self.SKU = ko.observable();
    self.SKUID = ko.observable();
    self.SKUArray = ko.observableArray([]);
    self.SKU
    //End : User Observables end here
    self.getAllProducts = function () {
        $.ajax({
            url: "/Admin/Products/getAllProducts",
            type: "GET",
            async: false,
            success: function (response) {

                var obj = JSON.parse(response);
                self.ProductArray([]);
                var mappedTasks = $.map(ko.toJS(obj.ResponseData), function (item) {
                    return new Products(item);
                });

                self.ProductArray(ko.toJS(mappedTasks));

            },
            error: function () {
                jAlert("Some Error Occured", "Error");

            }

        });
    }
    self.getALLSkus = function () {
        $.ajax({
            url: "/Admin/Products/getALLSkus",
            type: "GET",
            async: false,
            success: function (response) {

                //var obj = JSON.parse(response);
                //self.SKUArray([]);
                //var mappedTasks = $.map(ko.toJS(obj.ResponseData), function (item) {
                //    return new SKUs(item);
                //});

                //self.SKUArray(ko.toJS(mappedTasks));

            },
            error: function () {
                jAlert("Some Error Occured", "Error");

            }

        });
    }
    self.SKUAttributeArray = ko.observableArray([]);
    self.getAllAttribute = function () {
        $.ajax({
            url: "/Admin/Products/getAllAttributes",
            type: "GET",
            async: false,
            data: { productID: ko.toJS(self.TempProductID) },
            success: function (response) {

                var obj = JSON.parse(response);
                self.AttributeArray([]);
                var mappedTasks = $.map(ko.toJS(obj.ResponseData), function (item) {
                    return new Attribute(item);
                });

                self.AttributeArray(ko.toJS(mappedTasks));
               
            },
            error: function () {
                jAlert("Some Error Occured", "Error");

            }

        });
    }

    $("#divHidden").hide();
    self.getAllAttributeValues = function () {
        $.ajax({
            url: "/Admin/Products/getAllAttributeValues",
            type: "GET",
            async: false,
            data: { productID: ko.toJS(self.TempProductID), optionID: ko.toJS(self.OptionID()) },
            success: function (response) {

                var obj = JSON.parse(response);
                self.AttributeValueArray([]);
                var mappedTasks = $.map(ko.toJS(obj.ResponseData), function (item) {
                    return new AttributeValue(item);
                });

                self.AttributeValueArray(ko.toJS(mappedTasks));               
            },
            error: function () {
                jAlert("Some Error Occured", "Error");

            }

        });
    }

    //NB : SAVE Product 
    self.saveProduct = function () {
        if (self.ValidateProduct()) {
            $.ajax({
                type: "POST",
                url: "/Admin/Products/saveProduct",
                data: {
                    ProductID: ko.toJS(self.ProductID),
                    ProductName: ko.toJS(self.ProductName),
                    ProductDescription: ko.toJS(self.ProductDescription),
                    IsProductActive: ko.toJS(self.IsProductActive),
                    EntryBy: ko.toJS(self.EntryBy),
                    EntryDate: ko.toJS(self.EntryDate)
                },
                success: function (data) {
                    //jAlert(data.message, "Success");                    
                    $('.nav-tabs a[href="#Attribute"]').tab('show');                    
                    $('#Attribute').show();                    
                    self.TempProductID = data;
                    alert(data);
                    self.clearProductForm();
                    self.getAllProducts();
                    self.getAllAttribute();
                },
                error: function () {
                    jAlert("Some Error Occured", "Error");
                    //ShowAlertMessage('error', "Some Error Occured");
                }
            });

        }

    }

    self.saveAttribute = function () {
        if (self.ValidateAttribute()) {
            $.ajax({
                type: "POST",
                url: "/Admin/Products/saveAttribute",
                data: {
                    ProductID: ko.toJS(self.TempProductID),
                    OptionID: ko.toJS(self.OptionID),
                    OptionName: ko.toJS(self.OptionName),
                    OptionDescription: ko.toJS(self.OptionDescription),
                    IsOptionActive: ko.toJS(self.IsOptionActive),
                    EntryBy: ko.toJS(self.EntryBy),
                    EntryDate: ko.toJS(self.EntryDate)
                },
                success: function (data) {
                    //jAlert(data.message, "Success");
                    self.clearAttributeForm();
                    self.getAllAttribute();                    
                },
                error: function () {
                    jAlert("Some Error Occured", "Error");
                    //ShowAlertMessage('error', "Some Error Occured");
                }
            });

        }

    }

    self.saveAttributeValue = function () {
        if (self.ValidateAttributeValue()) {
           
            $.ajax({
                type: "POST",
                url: "/Admin/Products/saveAttributeValue",
                data: {
                    ProductID: ko.toJS(self.TempProductID),
                    OptionID: ko.toJS(self.SelectedAttribute),
                    ValueID: ko.toJS(self.ValueID),
                    ValueName: ko.toJS(self.ValueName),
                    IsValueActive: ko.toJS(self.IsValueActive),
                    EntryBy: ko.toJS(self.EntryBy),
                    EntryDate: ko.toJS(self.EntryDate)
                },
                success: function (data) {
                    //jAlert(data.message, "Success");                    
                    self.getAllAttributeValues();
                    self.clearAttributeValue();
                },
                error: function () {
                    jAlert("Some Error Occured", "Error");
                    //ShowAlertMessage('error', "Some Error Occured");
                }
            });

        }

    }
        
    //END SAVE Product
   
    self.NextAttribute = function (data) {        
        if (self.ProductArray().length > 0) {
            self.TempProductID = data.ProductID;
            self.getAllAttribute();
            $("#spnProductID").text(ko.toJS(self.TempProductID));
            $('.nav-tabs a[href="#Attribute"]').tab('show');
            
        }
        else {
            jAlert("Please Add Product", "Warning.");
        }
    }

    self.NextOptionValue = function (data) {
        
        if (self.AttributeArray().length > 0) {
            $('.nav-tabs a[href="#AttributeValue"]').tab('show');
            self.OptionID(data.OptionID);
            self.getAllAttributeValues();
           
        }
        else {
            jAlert("Please Add Attributes", "Warning.");
        }
    }

    self.NextSKU = function (data) {
        //self.SKUArray([]);
        $('.nav-tabs a[href="#SKU"]').tab('show');
        //self.getALLSkus();
        //for (i = 0; i < ko.toJS(self.AttributeValueArray()).length; i++) {
           
        //    var data =
        //    {
        //        OptionID: ko.toJS(self.AttributeValueArray()[i].OptionID),
        //        OptionName: ko.toJS(self.AttributeValueArray()[i].OptionName),
        //        ProductID: ko.toJS(self.AttributeValueArray()[i].ProductID),
        //        ProductName: ko.toJS(self.AttributeValueArray()[i].ProductName),
        //        ValueID: ko.toJS(self.AttributeValueArray()[i].ValueID),
        //        ValueName: ko.toJS(self.AttributeValueArray()[i].ValueName), 
        //        SKU: ko.toJS(self.AttributeValueArray()[i].ProductName).substring(0, 3) + "-" + ko.toJS(self.AttributeValueArray()[i].OptionName).substring(0, 3) + "-" + ko.toJS(self.AttributeValueArray()[i].ValueName).substring(0,3)
        //    }
           
        //    self.SKUArray.push(new SKUs(data));

        //}
        self.GenerateSKUFields();
        console.log($("#spnProductID").text());
        $.ajax({
            url: "/Admin/Products/getALLSkus",
            type: "GET",
            async: false,
            data: {
                productID: $("#spnProductID").text()},
            success: function (response) {
                console.log(response);
                var obj = JSON.parse(response);
                $("#spnSKUID").text(obj);

                
            },
            error: function () {
                jAlert("Some Error Occured", "Error");

            }

        });

    }

    //NB: UPDATE Product
    self.UpdateProduct = function (data) {
        self.ProductID(ko.toJS(data.ProductID));
        self.ProductName(ko.toJS(data.ProductName));
        self.ProductDescription(ko.toJS(data.ProductDescription));
        self.IsProductActive(ko.toJS(data.IsProductActive));        
    }
    //END UPDATE USER

    $(document).on('click', '#btnSaveSKU', function () {
        var productID = "";
        var optionID = "";
        var valueID = "";
        var result = true;
        if (ko.toJS(self.AttributeArray().length) > 0) {
            for (i = 0; i < ko.toJS(self.AttributeArray().length); i++) {
                if ($("#ddl" + ko.toJS(self.AttributeArray()[i].OptionName)).val() == "" || $("#ddl" + ko.toJS(self.AttributeArray()[i].OptionName)).val() == undefined) {
                    jAlert("Please Select All Attribute Value", "Warning.");
                    result = false;
                }
            }

        }
        else {
            jAlert("Attribute Not Found", "Success.");
        }
        if (result == true) {

            for (i = 0; i < ko.toJS(self.AttributeArray().length); i++) {
                var data =
                {
                    ProductID: ko.toJS(self.TempProductID),
                    OptionID: $('#spn' + ko.toJS(self.AttributeArray()[i].OptionName)).text(),
                    ValueID: $('#ddl' + ko.toJS(self.AttributeArray()[i].OptionName)).val(),
                    ValueName: $('#ddl' + ko.toJS(self.AttributeArray()[i].OptionName)).find('option:selected').text(),
                    SKU: $('#ddl' + ko.toJS(self.AttributeArray()[i].OptionName)).find('option:selected').text().substring(0, 3)
                }
                self.SKUAttributeArray.push(new SKUs(data));
            }

            var url = '/Admin/Products/saveSKUs';
            var data = {
                'sku': ko.toJS(self.SKUAttributeArray())
            };
            $.post(url, data, function (result) {
                if (result == 1) {
                    jAlert("SKU Saved Successfully...", "Success.");
                    self.SKUAttributeArray([]);
                }
                else {
                    jAlert("Something Went Wrong...", "Success.");
                    self.SKUAttributeArray([]);
                }
                
            });
            self.SKUAttributeArray([]);
        }
    });

    self.UpdateOption = function (data) {
        console.log(data);
        self.TempProductID=ko.toJS(data.ProductID);
        self.OptionID(ko.toJS(data.OptionID));
        self.OptionName(ko.toJS(data.OptionName));
        self.OptionDescription(ko.toJS(data.OptionDescription));
        self.IsOptionActive(ko.toJS(data.IsOptionActive));            
    }

    self.UpdateOptionValue = function (data) {
       
        self.ValueName(ko.toJS(data.ValueName));
        self.ValueID(ko.toJS(data.ValueID));

        var Index = 0;
       
        for (var k = 0; k < self.AttributeArray().length; k++) {            
            if (ko.toJS(self.AttributeArray()[k].OptionID) == ko.toJS(data.OptionID)) {
                Index = k;
            }
        }       
        //self.SelectedAttribute(self.AttributeArray()[Index]);
        $('.selectpicker').selectpicker('val', Index + 1);
        $("#ddlAttribute").attr("disabled", "disabled");
    }
    //NB: CLEAR PRODUCT FORM
    self.clearProductForm = function () {
        self.ProductID(null);
        self.ProductName(null);
        self.ProductDescription(null);
        self.IsProductActive(true);        
    }
    //END CLEAR PRODUCT FORM

    //NB: CLEAR ATTRIBUTE FORM
    self.clearAttributeForm = function () {
        //self.SelectedProduct(null);
        self.OptionName(null);
        self.OptionID(null);
        self.IsOptionActive(true);
        self.OptionDescription(null);        
    }
    //END CLEAR ATTRIBUTE FORM

    self.GenerateSKUFields = function () {
        var html = '<div class="row">';
        for (i = 0; i < ko.toJS(self.AttributeArray().length); i++) {
            html += '<div class="col-md-3 col-xs-3 col-lg-3 col-sm-3">';
            html += '<div class="form-group">';
            html += '<label>' + ko.toJS(self.AttributeArray()[i].OptionName) +'<span class="mandatory">*</span></label>';
            html += '<select class="selectpicker form-control" data-live-search="true" style="display:block !important;" id=ddl' + ko.toJS(self.AttributeArray()[i].OptionName) + '></select>';
            html += '</div>';
            html += '</div>';
        }
        html += '</div>';
        html += '<div class="row">';        
        html += '<div class="col-md-12 col-xs-12 col-lg-12 col-sm-12"><a class="btn btn-primary pull-right margin-top-minus-5" id="btnSaveSKU">Save</a></div >';
        html += '</div>';

        $("#divSKU").html(html);

        
        var productID = "";
        var optionID = "";

        for (i = 0; i < ko.toJS(self.AttributeArray().length); i++) {
            var hidden = "";
            $.ajax({
                url: "/Admin/Products/getAllAttributeValues",
                type: "GET",
                async: false,
                data: { productID: ko.toJS(self.TempProductID), optionID: ko.toJS(self.AttributeArray()[i].OptionID) },
                success: function (response) {
                    var obj = JSON.parse(response);
                    

                    console.log(obj.ResponseData);
                    var optionsHtml = '<option value="" disabled selected>-- Select ' + ko.toJS(self.AttributeArray()[i].OptionName) + ' --</option>';
                    $.each(obj.ResponseData, function (index, value) {
                        optionsHtml += '<option value="' + value.ValueID + '">' + value.ValueName + '</option>';
                        productID = value.ProductID;
                        optionID = value.OptionID;
                    });
                    var id = 'ddl' + ko.toJS(self.AttributeArray()[i].OptionName);
                    $('#' + id).html(optionsHtml);
                    hidden = '<span id=spn' + ko.toJS(self.AttributeArray()[i].ProductName) + '>' + productID + '</span>';
                    $("#divHidden").append(hidden);
                    hidden = '<span id=spn' + ko.toJS(self.AttributeArray()[i].OptionName) + '>' + optionID + '</span>';
                    $("#divHidden").append(hidden);

                },
                error: function () {
                    jAlert("Some Error Occured", "Error");

                }

            });
        }
        
        $('.selectpicker').selectpicker("render");
    }

    self.clearAttributeValue = function () {
        //$('.selectpicker').selectpicker('val', 0);
        self.ValueID(null);        
        self.ValueName(null);
        $("#ddlAttribute").removeAttr("disabled"); 
        self.IsValueActive(true);
    }
    

    self.ValidateProduct = function () {
        var errMsg = '';

        if (Validate.empty(ko.toJS(self.ProductName()))) {
            errMsg += 'Invalid Product Name !!!<br>';
        }

        if (errMsg !== '') {

            jAlert(errMsg, "Warning.");
            return false;
        }
        else {
            return true;
        }
    }

    self.ValidateAttribute = function () {
        var errMsg = '';
        console.log(ko.toJS(self.TempProductID));
        if (self.TempProductID == undefined || self.TempProductID == "" || self.TempProductID == null) {
            errMsg += 'Please Select Product !!!<br>';
        }
        if (Validate.empty(ko.toJS(self.OptionName()))) {
            errMsg += 'Invalid Option Name !!!<br>';
        }

        if (errMsg !== '') {

            jAlert(errMsg, "Warning.");
            return false;
        }
        else {
            return true;
        }
    }

    self.ValidateAttributeValue = function () {
        var errMsg = '';
       
        if (self.SelectedAttribute() == undefined) {
            errMsg += 'Please Select Attribute !!!<br>';
        }
        if (Validate.empty(ko.toJS(self.ValueName()))) {
            errMsg += 'Invalid Value Name !!!<br>';
        }
        
        if (errMsg !== '') {

            jAlert(errMsg, "Warning.");
            return false;
        }
        else {
            return true;
        }
    }
    
    self.getAllProducts();
    //self.getAllAttribute();
    //self.getAllAttributeValues();
}
//END CREATING USERS VIEW MODEL IN KNOCKOUT

//NB: DOCUMENT READY FUNCTION
$(document).ready(function () {
    ko.applyBindings(new ProductViewModel());


});
//END DOCUMENT READY FUNCTION