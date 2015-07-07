function FiltrarLogAcessos(idPaciente) {

    waitingDialog.show('Consultando o log e filtrando', { dialogSize: 'sm', progressType: 'success' });
    $.ajax({
        type: "POST",
        url: $("#urlListarLog").val(),
        data: {
            idPaciente: idPaciente,
            numeroConselho: $("#NumeroConselho").val(),
            responsavel: $("#FiltroResponsavel").val(),
            dataInicio: $("#FiltroDataInicio").val(),
            dataFim: $("#FiltroDataFim").val(),
            page: 1
        },
        datatype: "html",
        success: function (data) {
            $("#divLogAcessos").html(data);
            waitingDialog.hide();
        }
    });
};

function OrdenarLog(idPaciente, page, orderBy) {
    waitingDialog.show('Ordernando o Log', { dialogSize: 'sm', progressType: 'success' });

    $.ajax({
        type: "POST",
        url: $("#urlListarLog").val(),
        data: {
            idPaciente: idPaciente,
            numeroConselho: $("#NumeroConselho").val(),
            responsavel: $("#FiltroResponsavel").val(),
            dataInicio: $("#FiltroDataInicio").val(),
            dataFim: $("#FiltroDataFim").val(),
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