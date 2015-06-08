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
function RemoveComposicao(id) {

    waitingDialog.show('Gravando e removendo composição', { dialogSize: 'sm', progressType: 'success' });

    $.ajax({
        type: "POST",
        url: formPreencherExtrato.action,
        data: $("#formPreencherExtrato").serialize() + "&composicaoParaDeletar=" + id,
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
function AddSecao(idComposicao) {

    waitingDialog.show('Gravando e adicionando uma nova seção', { dialogSize: 'sm', progressType: 'success' });

    $.ajax({
        type: "POST",
        url: formPreencherExtrato.action,
        data: $("#formPreencherExtrato").serialize() + "&addSecao=true&composicaoDaSecao=" + idComposicao,
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
function RemoveSecao(id) {

    waitingDialog.show('Gravando e removendo seção', { dialogSize: 'sm', progressType: 'success' });

    $.ajax({
        type: "POST",
        url: formPreencherExtrato.action,
        data: $("#formPreencherExtrato").serialize() + "&secaoParaDeletar=" + id,
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