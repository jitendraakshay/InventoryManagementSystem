$(function () {
    $('form#loginForm').find('input.login-now').on('click', function () {

    });

});

var LoggedInUserName = 'admin';

var Contains = function (needle) {
    needle = needle.trim();
    
    // Per spec, the way to identify NaN is that it is not equal to itself
    var findNaN = needle !== needle;
    var indexOf;

    if (!findNaN && typeof Array.prototype.indexOf === 'function') {
        indexOf = Array.prototype.indexOf;
    } else {
        indexOf = function (needle) {
            var i = -1, index = -1;

            for (i = 0; i < this.length; i++) {
                var item = this[i];

                if ((findNaN && item !== item) || item === needle) {
                    index = i;
                    break;
                }
            }
            return index;
        };
    }
    return indexOf.call(this, needle) > -1;
};
var DecodeHtml = function (str) { var map = { amp: '&', lt: '<', gt: '>', quot: '"', '#039': "'" }; return str.replace(/&([^;]+);/g, (m, c) => map[c]) };
function ShowMessage(msg) {
    alert(msg);
}

function ShowNowLoading() {
    var html = '<div id="nowLoading" class="showbox">' +
              '<div class="loader">' +
              '<svg class="circular" viewBox="25 25 50 50">' +
              '<circle class="path" cx="50" cy="50" r="20" fill="none" stroke-width="2" stroke-miterlimit="10"/>' +
              '</svg>' +
              '</div>' +
            '</div>';
    //var html = '<div id="nowLoading"><div class="now-showing-loader"></div><div class="now-text-loader font-class">Working ...</div></div>';
    $('body').append(html);
    $('body').addClass('loading-active');
}

function HideNowLoading() {
    $('body').find('div#nowLoading').remove();
    $('body').removeClass('loading-active');
}

function ShowPopup(msg, title) {
    var popup = $('body').find('div#popupDialog');
    title = (title == undefined) ? "Title" : title;
    msg = (title == undefined) ? "Message " : msg;
    popup.find('h4.title').text(title);
    popup.find('div.message').text(msg);
    popup.find('button.ok').text('Close');
    popup.modal();
}
// ConfirmPopup('message','title',function(){alert(1);});
function ConfirmPopup(msg, title, event) {
    var popup = $('body').find('div#popupDialog');
    title = (title == undefined) ? "Title" : title;
    msg = (title == undefined) ? "Message " : msg;
    popup.find('h4.title').text(title);
    popup.find('div.message').html(msg);
    popup.find('button.ok').text('Ok');
    popup.find('button.ok').on('click', function () { event(); });
    popup.modal();

}

function HidePopup() {
    $('body').find('div#popupDialog').modal('hide');
}

function ShowAlertMessage(type, message) {
    var msgType = type.toLowerCase();
    var icon = null;
    var alertType = null;
    switch (msgType) {
        case 'success':
            icon = 'fa fa-check';
            alertType = 'success';
            break;
        case 'alert':
            icon = 'fa fa-exclamation-circle';
            alertType = 'warning';
            break;
        case 'error':
            icon = 'fa fa-exclamation-triangle';
            alertType = 'danger';
            break;
        case 'info':
            icon = 'fa fa-info-circle'
            alertType = 'info';
            break;
    }
    $.notify({
        message: message.trim(),
        icon: icon
    }, {
        type: alertType,
        offset: { x: 5, y: 10 },
        spacing: 10,
        z_index: 5000,
        animate: {
            enter: 'animated fadeInRight',
            exit: 'animated fadeOutRight'
        },
        delay: 2000,
        timer: 2000
    });
}

function ResetFormData() {

    $("input:not([name='__RequestVerificationToken'])").each(function () {
        $(this).val("");
    });
    $("textarea").each(function () {
        $(this).val("");
    });
    $("select").each(function () { this.selectedIndex = 0 });

    $('input:checkbox').removeAttr('checked');
    //remove Model error
    $('.validation-summary-errors li').remove();
    $('.input-validation-error').addClass('input-validation-valid');
    $('.input-validation-error').removeClass('input-validation-error');
    $('.field-validation-error').addClass('field-validation-valid');
    $('.field-validation-error').removeClass('field-validation-error');
    $('.validation-summary-errors').addClass('validation-summary-valid');
    $('.validation-summary-errors').removeClass('validation-summary-errors');

}

function AjaxFaliureMessage(textStatus, error) {
    var html = '<div class="panel-body alert-body"><div class="alert ';

    html += 'alert-danger';

    html += ' alert-dismissable"><button type="button" class="close" data-dismiss="alert" aria-hidden="true">×</button>' +
                     error + '</div></div>';

    $('div#page-wrapper').prepend(html);
    $('div.alert-body').delay(2000).fadeOut();

}

function ConvertTextToAbbr(text) {
    var splitText = text.split(" ");

    var abbr = '';
    if (splitText.length >= 2) {

        abbr = splitText[0].charAt(0).toUpperCase() + splitText[1].charAt(0).toUpperCase();

    }
    else {
        abbr = splitText[0].charAt(0).toUpperCase() + splitText[0].charAt(1).toLowerCase();
    }
    return abbr;
}

function ShowConfirmModal(title, msg, event) {
    var confirmModal = $('body').find('#confirmModal');
    confirmModal.find('#confirmModalLabel').html(title);
    confirmModal.find('.modal-body #confirmMsg').html(msg);
    confirmModal.modal('show');
    
    confirmModal.find('.modal-footer #yes').off('click').on('click', function () {
        event();
        HideConfirmModal();
    });
}

function HideConfirmModal() {
    $('body').find('#confirmModal').modal('hide');
}

function ClickCancelModal() {
    $('body').find('#confirmModal').find('.modal-footer #cancel').trigger('click');
    
}

function ShowLoader() {
    $('div.now-loading').show();
    $('div.now-loading').append('<img src="/Content/dist/images/1.gif">');
}
function HideLoader() {
    $('div.now-loading').hide();
}

$('.input-group-addon').on('click', function () {
    var id = $(this).parent().find('input[class = "form-control"]').prop('id');
    $('#' + id).datepicker('show');
})

//format yyyy-mm-dd
function ConvertDateForCalender(dateObject) {
    var dateString = dateObject.substr(6);
    var currentTime = new Date(parseInt(dateString));
    var month = currentTime.getMonth() + 1;
    month = parseInt(month) < 10 ? "0" + month : month;
    var day = currentTime.getDate();
    day = parseInt(day) < 10 ? "0" + day : day;
    var year = currentTime.getFullYear();
    var date = year + "-" + month + "-" + day;

    return date;
}

//format mm/dd/yyyy
function ConvertDate(dateObject) {
    var dateString = dateObject.substr(6);
    var currentTime = new Date(parseInt(dateString));
    var month = currentTime.getMonth() + 1;
    month = parseInt(month) < 10 ? "0" + month : month;
    var day = currentTime.getDate();
    day = parseInt(day) < 10 ? "0" + day : day;
    var year = currentTime.getFullYear();
    var date = month + "/" + day + "/" + year;

    return date;
}

function LoadSingleSumoSelect(element) {
    $('#' + element).SumoSelect({
        search: true,
        searchText: 'Search...'
    });
    $('#' + element).prop("selectedIndex", -1);
}

function UnloadSingleSumoSelect(element) {
    $('#' + element)[0].sumo.unSelectAll();
    $('#' + element).prop("selectedIndex", -1);
}

function LoadMultiSumoSelect(element) {
    $('#' + element).SumoSelect({
        search: true,
        searchText: 'Search...'
    });
}

function UnloadMultiSumoSelect(element) {
    $('#' + element)[0].sumo.unSelectAll();
}

function SortPageLength(newLength) {
    if (newLength <= 0 || newLength == null) {
        return [5, 10, 25, 50, 100, 200];
    }
    else {
        var pages = [newLength, 5, 10, 25, 50, 100, 200];

        //removes dublicate entries
        var seen = {};
        pages.forEach(function (val, i) {
            var txt = val;
            if (seen[txt]) {
                pages.splice(i, 1);
            }
            else {
                seen[txt] = true;
            }
        });

        //sorts the array
        return pages.sort(function (a, b) {
            return a - b;
        });
    }
}


