$(function () {

});

function AbrirExtrato(idExtrato) {

    waitingDialog.show('Abrindo Extrato ' + idExtrato, { dialogSize: 'sm', progressType: 'success' });

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

function FiltrarExtratosResponsavel(idResponsavel) {

    waitingDialog.show('Consultando os seus extratos e filtrando', { dialogSize: 'sm', progressType: 'success' });

    $.ajax({
        type: "POST",
        url: $("#urlFiltrarExtratosResponsavel").val(),
        data: {
            idResponsavel: idResponsavel,
            codigo: $("#FiltroCodigo").val(),
            cpf: $("#FiltroCpf").val(),
            dataInicio: $("#FiltroDataInicio").val(),
            dataFim: $("#FiltroDataFim").val(),
            nome: $("#FiltroNome").val(),
            cidade: $("#FiltroCidade").val()
        },
        datatype: "html",
        success: function (data) {
            $("#divExtratosResponsavel").html(data);
            waitingDialog.hide();
        }
    });
};