/*Kopierter Code*/
/*Quelle https://www.youtube.com/watch?v=zvysBd_mkfQ&ab_channel=ASP.NETMVC:  2:38 */
/*Ermöglicht das ein Modal angezeigt werden kann*/
$(function () {
    var modaldiv = $('#modaldiv');
    $('button[data-toggle="ajax-modal"]').click(function (event) {
        var url = $(this).data('url');
        var decodedUrl = decodeURIComponent(url);

        $.get(decodedUrl).done(function (data) {
            //In dem DIV wird das HTML geladen
            modaldiv.html(data);
            modaldiv.find('.modal ').modal('show');
        })
    })
    modaldiv.on('click', '[data-save="modal"]', function (event) {
    var form = $(this).parents('.modal').find('form');
    var actionUrl = form.attr('action');
    var sendData = form.serialize();
        //Werden Daten gesendet. schliesst sich das Modal
    $.post(actionUrl, sendData).done(function (data) {
        modaldiv.find('.modal').modal('hide');
    })
})
})