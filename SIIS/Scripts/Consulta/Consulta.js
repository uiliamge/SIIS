$(function() {
    

});

function FiltrarConsultaExtratos() {
    debugger;
    if (validarFiltrarCpf()) {

        waitingDialog.show('Consultando e filtrando', { dialogSize: 'sm', progressType: 'success' });

        $.ajax({
            type: "POST",
            url: $("#urlListarExtratos").val(),
            data: {
                codigo: $("#FiltroCodigo").val(),
                cpf: $("#FiltroCpf").val(),
                dataInicio: $("#FiltroDataInicio").val(),
                dataFim: $("#FiltroDataFim").val(),
                cidade: $("#FiltroCidade").val(),
                responsavel: $("#FiltroResponsavel").val(),
                page: 1
            },
            datatype: "html",
            success: function(data) {
                $("#divExtratosConsulta").html(data);
                waitingDialog.hide();
            }
        });
    }
};

function OrdenarConsultaExtratos(page, orderBy) {

    if (validarFiltrarCpf()) {

        waitingDialog.show('Ordernando os Extratos', { dialogSize: 'sm', progressType: 'success' });

        $.ajax({
            type: "POST",
            url: $("#urlListarExtratos").val(),
            data: {
                codigo: $("#FiltroCodigo").val(),
                cpf: $("#FiltroCpf").val(),
                dataInicio: $("#FiltroDataInicio").val(),
                dataFim: $("#FiltroDataFim").val(),
                cidade: $("#FiltroCidade").val(),
                responsavel: $("#FiltroResponsavel").val(),
                page: page,
                orderBy: orderBy
            },
            datatype: "html",
            success: function(data) {
                $("#divExtratosConsulta").html(data);
                waitingDialog.hide();
            }
        });
    }
}

function LimparFiltroExtratosConsulta() {

    $("#FiltroCodigo").val("");
    $("#FiltroCpf").val("");
    $("#FiltroDataInicio").val("");
    $("#FiltroDataFim").val("");
    $("#FiltroNome").val("");
    $("#FiltroCidade").val("");
    $("#FiltroPlano").val("");
}

function validarFiltrarCpf() {
    if ($("#FiltroCpf").val() == "") {
        $("#validationFiltrarCpf").show();
        $("#FiltroCpf").focus();

        return false;
    } else {
        $("#validationFiltrarCpf").hide();
        return true;
    }
}