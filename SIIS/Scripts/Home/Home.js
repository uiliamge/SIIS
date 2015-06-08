$(function () {

});

function AbrirExtrato(idExtrato) {

    waitingDialog.show('Abrindo Extrato ' + idExtrato, { dialogSize: 'sm', progressType: 'success' });

    var extrato = $("#formPreencherExtrato").serialize();

    $.ajax({
        type: "GET",
        url: $("#urlAbrirExtrato").val(),
        data: { id: idExtrato },
        datatype: "html",
        success: function (data) {
            $("#bodyLayout").html(data);
            waitingDialog.hide();
        }
    });
};