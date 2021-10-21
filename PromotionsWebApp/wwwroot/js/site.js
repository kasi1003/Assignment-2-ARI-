//Page Loader Code
document.addEventListener("DOMContentLoaded", function () {
    $('#pageLoader.preloader-background').delay(1700).fadeOut('slow');
    $('#pageLoader.preloader-wrapper')
        .delay(1700)
        .fadeOut();

});


$(document).ready(function () {

});

//confirmation events
function initConfirmModal(confirmType, subject, id, ajaxReq = false) {
    var headerText = "";
    var bodyText = "";
    var questionText = "";
    var link = "";
    if (confirmType === "Delete") {
        switch (subject) {
            case "User":
                headerText = "Delete " + subject;
                bodyText = "User will no longer be have access to system.";
                questionText = "Are you sure you wish to delete this user?";
                link = "/Account/Delete?userId=" + id;
                break;
            default:
            // code block
        }

    } else if (confirmType === "Close") {
        
    }


    $('#confirmModalHeader').text(headerText);
    $('#confirmModalText').text(bodyText);
    $('#confirmModalQuestion').text(questionText);
    if (!ajaxReq)
        $('#confirmSubmitBtn').attr('href', link);
    else {
        $('#confirmSubmitBtn').click(function () {

            //$.ajax({
            //    type: "GET",
            //    url: link,
            //    traditional: true,
            //    success: function (result) {
            //        if (result) {
            //            var docId = "#docId" + id;
            //            $(docId).remove();
            //        }
            //    }
            //});

        });
    }
    $('#confirmationModal').modal('open');
};


