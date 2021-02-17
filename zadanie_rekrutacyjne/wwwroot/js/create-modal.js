// SHOW PARTIAL VIEW WITH MODAL CREATE FORM WHEN CREATE BUTTON CLICKED
$('a.create').on('click', function () {
    $.ajax({
        url: this.href,
        type: 'GET',
        cache: false,
        success: function (result) {
            console.log('show create modal');
            $('#createModal').html(result).find('#create-modal').modal({
                show: true
            });
        }
    });

    return false;
});

// SEND FORM INFORMATION TO CONTROLLER, GET NEW MODAL BODY IF MODEL IS NOT VALID, ELSE CLOSE MODAL
$('#createModal').on('click', '[data-save="modal"]', function (event) {
    event.preventDefault();

    var form = $('#create-form'); 
    $.ajax({
        url: form.attr("action"),
        method: 'POST',
        data: form.serialize(),
        error: function (xhr, textStatus, errorThrown) {
            console.error(xhr.responseText + "\n" + textStatus + "\n" + errorThrown);
        },
        success: function (result) {
            var newBody = $('.modal-body', result);
            $('#createModal').find('.modal-body').replaceWith(newBody);

            var isValid = newBody.find('[name="IsValid"]').val() == 'True';
            if (isValid) {
                $('.btn-create-modal-close').click();
                console.log('close modal');
                window.location.reload(true);
            }

        }
    });
});