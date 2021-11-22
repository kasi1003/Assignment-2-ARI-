$(document).ready(function () {
    $("#roleSelect").on("change", function () {
        var selectedRole = $("#roleSelect").val();
        if (selectedRole > 0) {
            $("#facultySelect").val(0);
            $("#facultySelect").change();
            if (selectedRole == 8) {
                $('#staffJobOptions').show();
            } else {
                $('#staffJobOptions').hide();
                $('#rankSelect').val(0);
                $('#rankSelect').change();
            }
            if (selectedRole == 6 || selectedRole == 7 || selectedRole == 8) {
                $('#staffLocationOptions').show();
            } else {
                $('#staffLocationOptions').hide();
                
            }
        }     
    });
    $("#facultySelect").on("change", function () {
        var facultyId = $("#facultySelect").val();
        if (facultyId > 0) {
            var selectedRole = $("#roleSelect").val();
            if (selectedRole == 7 || selectedRole == 8) {
                var $depList = $('#departmentSelect');
                var link = "/Account/GetDepartmentsJson?facultyId=" + facultyId;
                $.ajax({
                    url: link,
                    type: "GET",
                    traditional: true,
                    success: function (result) {
                        $depList.empty();
                        $depList.append('<option value="' + 0 + '">' + 'Please Select Department</option>');
                        $.each(result, function (i, item) {
                            $depList.append('<option value="' + item["id"] + '"> ' + item["name"] + ' </option>');
                        });
                        $("#vendorSelect").val(0);

                        $depList.prop('disabled', false);
                        $depList.formSelect();
                    },
                    error: function () {
                    }
                });
            } else {
                $('#departmentSelect').val(0);
                $('#departmentSelect').change();
                $('#departmentSelect').prop('disabled', true);
            }
           
        } 
    });

    $("#submitPersonalDetailsBtn").click(function (event) {
        event.preventDefault();
        if ($("#personalDetailsForm").valid()) {
            startUpdatingProgressIndicator("Updating Personal Details");
            $("#personalDetailsForm").submit();
        }
        
    });
    $("#submitEducationBtn").click(function (event) {
        event.preventDefault();
        if ($("#educationForm").valid()) {
            startUpdatingProgressIndicator("Updating Education Details");
            $("#educationForm").submit();
        }
    });
    $("#submitpublicationBtn").click(function (event) {
        event.preventDefault();
        if ($("#publicationForm").valid()) {
            startUpdatingProgressIndicator("Updating Publication Details");
            $("#publicationForm").submit();
        }
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
                case "Publication":
                    headerText = "Delete " + subject;
                    bodyText = "Publication will no longer be available in the system";
                    questionText = "Are you sure you wish to delete this publication?";
                    link = "/Staff/DeletePublication?publicationId=" + id;
                    break;
                case "Education":
                    headerText = "Delete " + subject;
                    bodyText = "Qualification will no longer be available in the system";
                    questionText = "Are you sure you wish to delete this qualification?";
                    link = "/Staff/DeleteQualification?qualificationId=" + id;
                    break;
                default:
                // code block
            }

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
    function startUpdatingProgressIndicator(message) {
        $('#loadText').text(message);
        $(".overlay").show();
    }

    function stopUpdatingProgressIndicator() {
        $(".overlay").hide();
    }

    $('#btnStepTwo').click(function () {
        $('#applyForeword').addClass("hide");
        $('#applyCheckList').removeClass("hide");
    });
    $('#btnStepThree').click(function () {
        $('#applyUpload').removeClass("hide");
        $('#applyCheckList').addClass("hide");
    });
});




