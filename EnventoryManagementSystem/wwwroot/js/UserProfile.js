var fileUpload;
$(function () {
    Init();
    var ele = $('div#userProfileContainer');
    ele.find('#OldPassword').text('');


    //$('#imageimg').attr('src', '/Content/dist/uploadedImages/' + res.data.ProfileImage);
    

    $('#btnSaveUserProfile').on('click', function (e) {
        e.isDefaultPrevented();
        SaveUserProfile();
    });

    $('#btnChangePassword').on('click', function (e) {
        e.isDefaultPrevented();
        ChangePassword();
    });
});



$("#StateID").on("change", function () {
    var StateID = ($('#StateID').val() == null || $('#StateID').val() == "") ? 0 : $('#StateID').val();
    var model = {        
        stateID: StateID    };
        $.ajax({
            url: "/Admin/UserProfile/GetCityByStates",
            type: "GET",
            data: model,
            dataType: 'json',
            success: function (response) {

                var optionsHtml = '<option value="0" selected disabled>-- Select --</option>';
                $.each(response.data, function (index, value) {
                    optionsHtml += '<option value="' + value.CityID + '">' + value.CityName + '</option>';
                });
                $("#CityID").html(optionsHtml);

                //$('#CityID').SumoSelect({
                //    search: true,
                //    searchText: 'Search...'
                //});
                //$('#CityID')[0].sumo.reload();
            },
            error: function () {
                ShowAlertMessage('error', "Some Error Occured");
            }

        });
    

});

function SaveUserProfile() {

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
                ShowAlertMessage('success', res);
               
            },
            error: function () {
                ShowAlertMessage('error', "Some Error Occured in Uploading Image");
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