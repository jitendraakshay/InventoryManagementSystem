var fileUpload;
$(function () {
    Init();
    var ele = $('div#userProfileContainer');
    ele.find('#OldPassword').text('');
    GetProfileImage();
    GetUserProfile();
    //$('#imageimg').attr('src', '/Content/dist/uploadedImages/' + res.data.ProfileImage);
    

    $('#btnSaveUserProfileImage').on('click', function (e) {
        e.isDefaultPrevented();
        SaveUserProfileImage();
    });
    $('#btnSaveUserProfile').on('click', function (e) {
        e.isDefaultPrevented();
        SaveUserProfile();
    });
    $('#btnSavePassword').on('click', function (e) {
        e.isDefaultPrevented();
        SavePassword();
    });
    

    $('#btnChangePassword').on('click', function (e) {
        e.isDefaultPrevented();
        ChangePassword();
    });
});
var UserID;


function SavePassword() {
    if (ValidatePassword()) {
        $.ajax({
            url: "/Admin/UserProfile/SavePassword",
            type: "POST",
            data: {
                'OldPassword': $("#txtOldPassword").val(),
                'NewPassword': $("#txtNewPassword").val()
            },
            success: function (res) {

                if (res.result == false) {
                    jAlert(res.message, "Error");
                }
                else {
                    jAlert(res.message, "Success");
                }

            },
            error: function () {
                jAlert("Some Error Occured", "Error");

            }
        });
    }
}

function SaveUserProfile() {
    if (ValidateProfile()) {
        $.ajax({
            url: "/Admin/UserProfile/SaveUserProfile",
            type: "POST",
            data: {
                UserID: UserID,
                FirstName: $("#txtFirstName").val(),                
                LastName: $("#txtLastName").val(),
                Email: $("#txtEmail").val(),
                PhoneNo: $("#txtPhoneNo").val(),
                MobileNo: $("#txtMobileNo").val(),
                Address: $("#txtAddress").val()
            },
            success: function (res) {
                jAlert(res.message, "Success");
                SetProfileImage();
            },
            error: function () {
                jAlert("Some Error Occured in Uploading Image", "Error");

            }
        });
    }
    console.log($("#txtPhoneNo").val());
}

function ValidateProfile() {
    var errMsg = '';

    if (Validate.empty($("#txtFirstName").val())) {
        errMsg += 'Invalid First Name !!!<br>';
    }
    
    if (Validate.empty($("#txtLastName").val())) {
        errMsg += 'Invalid Last Name !!!<br>';
    }
    if (Validate.email($("#txtEmail").val())) {
        errMsg += "Invalid Email !!!<br>";
    }
    
    if ($("#txtPhoneNo").val()!="") {
        if (Validate.phone($("#txtPhoneNo").val()) || $("#txtPhoneNo").val().length > 10 || $("#txtPhoneNo").val().length < 10) {
            errMsg += 'Invalid Phone Number !!!<br>';
        }
    }
    if (Validate.phone($("#txtMobileNo").val()) || $("#txtMobileNo").val().length > 10 || $("#txtMobileNo").val().length < 10) {
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

function ValidatePassword() {
    var errMsg = '';

    if (Validate.empty($("#txtOldPassword").val())) {
        errMsg += 'Invalid Old Password !!!<br>';
    }

    if (Validate.empty($("#txtNewPassword").val())) {
        errMsg += 'Invalid New Password !!!<br>';
    }
    if ($("#txtNewPassword").val() != $("#txtConfirmpassword").val()) {
        errMsg += 'Confirm Password Does Not Match !!!<br>';
    }

    if (errMsg !== '') {

        jAlert(errMsg, "Warning.");
        return false;
    }
    else {
        return true;
    }
}


function SaveUserProfileImage() {

    var ele = $('div#userProfileContainer');    
   
        var values = {
            ProfileImage: imageName
        }

        var file = fileUpload[0].files[0];
        var dataImg = new FormData();
        dataImg.append('file', file);
        $.ajax({
            url: "/Admin/UserProfile/FileUpload",
            type: "POST",
            cache: false,
            contentType: false,
            processData: false,
            data: dataImg,
            success: function (res) {
                jAlert(res, "Success");
                SetProfileImage();
            },
            error: function () {
                jAlert("Some Error Occured in Uploading Image", "Error");
                
            }
    });

        
}

function GetProfileImage() {
    $.ajax({
        url: "/Admin/UserProfile/GetProfileImage",
        type: "GET",
        async: false,
        success: function (response) {
            var extentison = response.imageType.substring(1)
            var image = "data:Image/" + extentison + ";base64," + response.profileImage;
            $("#imageimg").attr('src', image);
        },
        error: function () {
            jAlert("Some Error Occured", "Error");
            
        }

    });
}
function SetProfileImage() {
    $.ajax({
        url: "/Admin/UserProfile/GetProfileImage",
        type: "GET",
        async: false,
        success: function (response) {
            var extentison = response.imageType.substring(1)
            var image = "data:Image/" + extentison + ";base64," + response.profileImage;
            $("#profileImage").attr('src', "");
            $("#profileImage").attr('src', image);
        },
        error: function () {
            jAlert("Some Error Occured", "Error");
        }

    });
}
function GetUserProfile() {
    $.ajax({
        url: "/Admin/UserProfile/GetUserProfile",
        type: "POST",
        async: false,
        success: function (response) {
            var obj = JSON.parse(response);
            console.log(obj.ResponseData[0].FirstName);
            $("#txtFirstName").val(obj.ResponseData[0].FirstName);
            $("#txtMiddleName").val(obj.ResponseData[0].MiddleName);
            $("#txtLastName").val(obj.ResponseData[0].LastName);
            $("#txtEmail").val(obj.ResponseData[0].Email);
            $("#txtPhoneNo").val(obj.ResponseData[0].PhoneNo);
            $("#txtMobileNo").val(obj.ResponseData[0].MobileNo);
            $("#txtAddress").val(obj.ResponseData[0].Address);
            UserID = obj.ResponseData[0].UserID;
          
        },
        error: function () {
            jAlert("Some Error Occured", "Error");
        }

    });
}


function ChangePassword() {

    var ele = $('div#userProfileContainer');

    var userPassword = $('div#userProfileContainer').find('user-password');

    if (userPassword.length <= 0) {
        $('#formUserProfile').find('#PassCode').rules('remove', 'required');
        $('#formUserProfile').find('#ConfirmPassCode').rules('remove', 'required');
    }
    
    //var isValid = $('#formChangePassword').data('unobtrusiveValidation').validate();
    var isValid = true;
    if (isValid) {
        var values = {
            OldPassword:ele.find('#OldPassword').val(),
            PassCode: ele.find('#PassCode').val(),
            ConfirmPassCode: ele.find('#ConfirmPassCode').val()
        };
        $.ajax({
            type: "POST",
            dataType: "json",
            url: '/Admin/UserProfile/UserProfileChangePassword',
            data: AddAntiForgeryToken(values),
            success: function (data) {
                if(data.Message)
                if (data.ErrorOccured) {
                    ShowAlertMessage('alert', data.Message);
                }
                else {
                    ShowAlertMessage('success', 'Password changed succesfully!');
                    
                    $('#OldPassword').val('');
                    $('#PassCode').val('');
                    $('#ConfirmPassCode').val('');
                }
            },
            error: function (jqXHR, textStatus) {
                if (jqXHR.redirect) {
                    alert(jqXHR.redirect);
                    // data.redirect contains the string URL to redirect to
                    window.location.href = jqXHR.redirect;
                }
            }
        });
    } else {
        return false;
    }
}



function Init() {
   
    $('#PassCode').val('');
    $('#ConfirmPassCode').val('');
}

function AddAntiForgeryToken(data) {
    data.__RequestVerificationToken = $('form input[name=__RequestVerificationToken]').val();
    return data;
}


function ResetErrorMessage() {
    $('.input-validation-error').addClass('input-validation-valid');
    $('.input-validation-error').removeClass('input-validation-error');
    //Removes validation message after input-fields
    $('.field-validation-error').addClass('field-validation-valid');
    $('.field-validation-error').removeClass('field-validation-error');
    //Removes validation summary
    $('.validation-summary-errors').addClass('validation-summary-valid');
    $('.validation-summary-errors').removeClass('validation-summary-errors');
}

var imageName = '';
function setUploadButtonState(str, event) {
    $('#' + str + 'msg').text('')
    var maxFileSize = 2024000 // 1MB -> 1000 * 1024
    fileUpload = $('#' + str);

    if (fileUpload.val() == '') {
        return false;
    }
    else {
        if (fileUpload[0].files[0].size < maxFileSize) {

            var file = fileUpload[0].files[0];
            var fileType = file["type"];
            var validImageTypes = ["image/bmp", "image/jpeg", "image/png", "image/jpg", "image/gif"];
            if ($.inArray(fileType, validImageTypes) < 0) {
                document.getElementById("image").value = "";
                document.getElementById(str + 'img').src = "";
                // document.getElementById(str + 'img').removeAttribute('src');
                $('#' + str + 'msg').text('Invalid File!');
                ImageError = true;
                return false;
            } else {
                loadFile(str, event);
                ImageError = false;
                return true;
            }
        } else {
            document.getElementById(str + 'img').src = "";
            // document.getElementById(str + 'img').removeAttribute('src');
            $('#' + str + 'msg').text('File too big !');
            ImageError = true;
            return false;
        }
    }
}

var loadFile = function (str, event) {
    var imageName = str;

    var reader = new FileReader();
    reader.onload = function () {
        var output = document.getElementById(str + 'img');

        output.src = reader.result;

    };
    reader.readAsDataURL(event.target.files[0]);
};