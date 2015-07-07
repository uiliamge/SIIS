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

//Profissional
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

//Paciente
function FiltrarExtratosPaciente(idPaciente) {

    waitingDialog.show('Consultando os seus extratos e filtrando', { dialogSize: 'sm', progressType: 'success' });

    $.ajax({
        type: "POST",
        url: $("#urlListarExtratosPaciente").val(),
        data: {
            idPaciente: idPaciente,
            codigo: $("#FiltroCodigo").val(),
            responsavel: $("#FiltroResponsavel").val(),
            dataInicio: $("#FiltroDataInicio").val(),
            dataFim: $("#FiltroDataFim").val(),
            cidade: $("#FiltroCidade").val(),
            plano: $("#FiltroPlano").val(),
            page: 1
        },
        datatype: "html",
        success: function (data) {
            $("#divExtratosPaciente").html(data);
            waitingDialog.hide();
        }
    });
};

function OrdenarExtratosPaciente(idPaciente, page, orderBy) {
    waitingDialog.show('Ordernando os Extratos', { dialogSize: 'sm', progressType: 'success' });

    $.ajax({
        type: "POST",
        url: $("#urlListarExtratosPaciente").val(),
        data: {
            idPaciente: idPaciente,
            codigo: $("#FiltroCodigo").val(),
            responsavel: $("#FiltroResponsavel").val(),
            dataInicio: $("#FiltroDataInicio").val(),
            dataFim: $("#FiltroDataFim").val(),
            cidade: $("#FiltroCidade").val(),
            plano: $("#FiltroPlano").val(),
            page: page,
            orderBy: orderBy
        },
        datatype: "html",
        success: function (data) {
            $("#divExtratosPaciente").html(data);
            waitingDialog.hide();
        }
    });
}

function LimparFiltroExtratosPaciente(idPaciente) {

    $("#FiltroCodigo").val("");
    $("#FiltroResponsavel").val("");
    $("#FiltroDataInicio").val("");
    $("#FiltroDataFim").val("");
    $("#FiltroCidade").val("");
    $("#FiltroPlano").val("");

    FiltrarExtratosPaciente(idPaciente);
}

function ListarExtratosPorAprovacao(idPaciente) {

    waitingDialog.show('Verificando extratos por aprovação', { dialogSize: 'sm', progressType: 'success' });

    $.ajax({
        type: "POST",
        url: $("#urlListarExtratosPorAprovacao").val(),
        data: {
            idPaciente: idPaciente,            
            page: 1
        },
        datatype: "html",
        success: function (data) {
            $("#divExtratosPorAprovacao").html(data);
            waitingDialog.hide();
        }
    });
};

function OrdenarExtratosPorAprovacao(idPaciente, page, orderBy) {
    waitingDialog.show('Ordernando os Extratos por Aprovação', { dialogSize: 'sm', progressType: 'success' });

    $.ajax({
        type: "POST",
        url: $("#urlListarExtratosPorAprovacao").val(),
        data: {
            idPaciente: idPaciente,
            page: page,
            orderBy: orderBy
        },
        datatype: "html",
        success: function (data) {
            $("#divExtratosPorAprovacao").html(data);
            waitingDialog.hide();
        }
    });
}

function AprovarExtrato(id) {

    waitingDialog.show('Aprovando extrato', { dialogSize: 'sm', progressType: 'success' });

    $.ajax({
        type: "POST",
        url: $("#urlAprovarExtrato").val(),
        data: { id: id },
        datatype: "html",
        success: function (data) {
            ListarExtratosPorAprovacao($("#IdDoPaciente").val());
        }
    });
};

