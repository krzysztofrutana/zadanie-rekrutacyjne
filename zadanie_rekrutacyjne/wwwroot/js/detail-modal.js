// SHOW PARTIAL VIEW WITH MODAL DETAIL FORM WHEN CURRENT DETAIL BUTTON CLICKED
$('a.details').on('click', function () {
    $.ajax({
        url: this.href,
        type: 'GET',
        cache: false,
        success: function (result) {
            console.log('show create modal');
            $('#detailModal').html(result).find('#detail-modal').modal({
                show: true
            });
        }
    });

    return false;
});
