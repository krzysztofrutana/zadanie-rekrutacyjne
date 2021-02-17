// SHOW PARTIAL VIEW WITH MODAL DETAIL FORM WHEN CURRENT DELETE BUTTON CLICKED
$('a.delete').on('click', function () {
    $.ajax({
        url: this.href,
        type: 'GET',
        cache: false,
        success: function (result) {
            console.log('show delete modal');
            $('#deleteModal').html(result).find('#delete-modal').modal({
                show: true
            });
        }
    });

    return false;
});


// SEND FORM INFORMATION TO CONTROLLER ABOUT CATEGORY TO DELETE
$('#deleteModal').on('click', '[data-save="modal"]', function (event) {
    event.preventDefault();
    var form = $('#delete-form');
    $.ajax({
        url: form.attr("action"),
        method: 'POST',
        data: form.serialize(),
        error: function (response) {
            console.error(response);
        },
        success: function (result) {
            $('.btn-delete-modal-close').click();
            console.log('close modal');
            window.location.reload(true);
        }
    });

});