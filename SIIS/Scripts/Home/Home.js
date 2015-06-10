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

function FiltrarExtratosProfissional(idResponsavel) {

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
            cidade: $("#FiltroCidade").val(),
            plano: $("#FiltroPlano").val(),
            page: 1
        },
        datatype: "html",
        success: function (data) {
            $("#divExtratosResponsavel").html(data);
            waitingDialog.hide();
        }
    });
};

function OrdenarExtratosResponsavel(idResponsavel, page, orderBy) {
    waitingDialog.show('Ordernando os Extratos', { dialogSize: 'sm', progressType: 'success' });

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
            cidade: $("#FiltroCidade").val(),
            plano: $("#FiltroPlano").val(),
            page: page,
            orderBy: orderBy
        },
        datatype: "html",
        success: function (data) {
            $("#divExtratosResponsavel").html(data);
            waitingDialog.hide();
        }
    });
}

function LimparFiltroExtratosProfissional(idProfissional) {

    $("#FiltroCodigo").val("");
    $("#FiltroCpf").val("");
    $("#FiltroDataInicio").val("");
    $("#FiltroDataFim").val("");
    $("#FiltroNome").val("");
    $("#FiltroCidade").val("");
    $("#FiltroPlano").val("");

    FiltrarExtratosProfissional(idProfissional);
}