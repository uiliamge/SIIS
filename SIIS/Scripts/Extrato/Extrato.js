$(function () {

    $("#btnVerificarCpf").click(function () {
        verificarCpf();
    });

    $("#Paciente_CpfCnpj").change(function () {

        $("#divPreencherComposicoes").hide();
    });

    $("#Paciente_CpfCnpj").focus();
});

function Submit() {

    waitingDialog.show('Gravando o Extrato', { dialogSize: 'sm', progressType: 'success' });

    var extrato = $("#formPreencherExtrato").serialize();

    $.ajax({
        type: "POST",
        url: formPreencherExtrato.action,
        data: $("#formPreencherExtrato").serialize(),
        datatype: "html",
        success: function (data) {
            $("#bodyLayout").html(data);
            waitingDialog.hide();

            setTimeout(function () {
                $("#Composicoes_0__Descricao").focus();
            }, 200);
        }
    });
};

function verificarCpf() {

    waitingDialog.show('Verificando CPF e gravando', { dialogSize: 'sm', progressType: 'success' });

    var extrato = $("#formPreencherExtrato").serialize();

    $.ajax({
        type: "POST",
        url: $("#urlVerificarSalvarExtrato").val(),
        data: $("#formPreencherExtrato").serialize(),
        datatype: "html",
        success: function (data) {
            $("#bodyLayout").html(data);
            waitingDialog.hide();

            setTimeout(function () {
                $("#Composicoes_0__Descricao").focus();
            }, 200);
        }
    });
};

function AddComposicao() {

    waitingDialog.show('Gravando e adicionando uma nova composição', { dialogSize: 'sm', progressType: 'success' });

    $.ajax({
        type: "POST",
        url: formPreencherExtrato.action,
        data: $("#formPreencherExtrato").serialize() + "&addComposicao=true",
        dataType: "html",
        success: function (data) {
            $("#bodyLayout").html(data);
            waitingDialog.hide();

            setTimeout(function () {
                $("#Composicoes_0__Descricao").focus();
            }, 200);
        }
    });
};
function RemoveComposicao(indice) {

    $.ajax({
        url: $("#urlRemoveTempDataComposicoes").val(),
        type: "POST",
        data: { indice: indice },
        success: function (data) {
            var result = data;
            if (!result.Erro) {
                $("#divPreencherComposicoes").html(data);
            }
        }
    });
};
function AddSecao(indiceComposicao) {

    $.ajax({
        url: $("#urlAddTempDataSecoes").val(),
        type: "POST",
        data: { indiceComposicao: indiceComposicao },
        success: function (data) {
            var result = data;
            if (!result.Erro) {
                $("#divPreencherSecoes_" + indiceComposicao).html(data);
            }
        }
    });
};
function RemoveSecao(indiceSecao, indiceComposicao) {

    $.ajax({
        url: $("#urlRemoveTempDataSecoes").val(),
        type: "POST",
        data: { indice: indiceSecao, indiceComposicao: indiceComposicao },
        success: function (data) {
            var result = data;
            if (!result.Erro) {
                $("#divPreencherSecoes_" + indiceComposicao).html(data);
            }
        }
    });
};