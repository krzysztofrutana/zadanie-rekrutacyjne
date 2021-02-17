// SHOW PARTIAL VIEW WITH MODAL DETAIL FORM WHEN CURRENT EDIT BUTTON CLICKED
$('a.edit').on('click', function () {
    $.ajax({
        url: this.href,
        type: 'GET',
        cache: false,
        success: function (result) {
            console.log('show modal');
            $('#editModal').html(result).find('#edit-modal').modal({
                show: true
            });
        }
    });

    return false;
});


// SEND FORM INFORMATION TO CONTROLLER, GET NEW MODAL BODY IF MODEL IS NOT VALID, ELSE CLOSE MODAL
$('#editModal').on('click', '[data-save="modal"]', function (event) {
    event.preventDefault();
    var form = $('#edit-form');
    console.log(form);
    $.ajax({
        url: form.attr("action"),
        method: 'POST',
        data: form.serialize(),
        error: function (xhr, textStatus, errorThrown) {
            console.error(xhr.responseText + "\n" + textStatus + "\n" + errorThrown);
        },
        success: function (result) {
            var newBody = $('.modal-body', result);
            $('#editModal').find('.modal-body').replaceWith(newBody);

            var isValid = newBody.find('[name="IsValid"]').val() == 'True';
            if (isValid) {
                $('.btn-edit-modal-close').click();
                console.log('close modal');
                window.location.reload(true);
            }

        }
    });
});