var intMessageFadeTimeout = 10000;
var sFileUploadURL = 'Files/Upload';


//Prevent forms from submitting so that I can use the jquery button click method with an ajax call instead...
$(".preventsubmit").submit(function (event) {
    event.preventDefault();
});

$("#btnUpload").button().click(function () {

    //var data = new FormData();
    //jQuery.each(jQuery('#file')[0].files, function (i, file) {
    //    data.append('file-' + i, file);
    //});
    var formData = new FormData($("#frmFileUpload")[0]);

    $.ajax({
        url: sFileUploadURL,
        type: 'POST',
        data: formData,
        async: false,
        cache: false,
        contentType: false,
        processData: false,
        success: function (returndata) {
            alert(returndata);
        }
    });
});