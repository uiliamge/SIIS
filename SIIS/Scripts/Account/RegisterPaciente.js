$(function () {

    $("#TipoPermissao").change(function () {

        switch ($("#TipoPermissao").val()) {
            case "QualquerProfissional":
                {
                    $("#divListaProfissionaisPermitidos").addClass("hidden");
                    $("#txtExplicativoQualquerProfissionalPodeAcessar").removeClass("hidden");
                    $("#txtExplicativoPergunteMe").hide(); break;
                }
            case "EscolherQuemPodeAcessar":
                {
                    $("#divListaProfissionaisPermitidos").removeClass("hidden");
                    $("#txtExplicativoQualquerProfissionalPodeAcessar").addClass("hidden");
                    break;
                }
            case "Perguntarme":
                {
                    $("#txtExplicativoPergunteMe").show();
                    $("#divListaProfissionaisPermitidos").addClass("hidden");
                    $("#txtExplicativoQualquerProfissionalPodeAcessar").addClass("hidden");
                    break;
                }
        }
    });

    $("#Cep").blur(function () {

        var cep = this.value.replace(/\D/g, "");

        $.ajax({
            url: "http://api.postmon.com.br/v1/cep/" + cep,
            type: 'GET',
            success: function (data) {

                $("#Cidade").val(data.cidade);
                $("#Uf option[value='" + data.estado_info.codigo_ibge + "']").attr("selected", true);
                $("#Uf").val(data.estado);
                $("#Bairro").val(data.bairro);
                $("#Endereco").val(data.logradouro);

                $("#validationCep").hide();
            },
            error: function (data) {

                $("#validationCep").text("Este CEP não foi localizado. Informe um CEP válido.");
                $("#validationCep").show();
            }
        });
    });

    $("#Cidade,#Uf,#Bairro,#Endereco").focus(function () {

        $("#NumeroEndereco").focus();
    });
});

function addProfissionalSelecionado() {

    var numero = $("#PermissaoNumeroConselho").val();
    var sigla = $("#SiglaConselhoRegional").val();
    var uf = $("#UfConselhoRegional").val();

    if (numero == "" || sigla == "" || uf == "") {
        if (numero == "") {
            $('#validationProfissionais').text('Informe o Nº no Conselho Regional.');
            $("#PermissaoNumeroConselho").focus();
        }
        if (sigla == "") {
            $('#validationProfissionais').text('Informe o Conselho Regional.');
            $("#SiglaConselhoRegional").focus();
        }
        if (uf == "") {
            $('validationProfissionais').text('Informe a UF.');
            $("#UfConselhoRegional").focus();
        }
    }
    else {
        $.ajax({
            url: $("#hdnAddTempDataProfissionaisPermitidos").val(),
            type: 'POST',
            data: { numero: numero, sigla: sigla, uf: uf, edicao: false },
            success: function (data) {
                var result = data;
                if (!result.Erro) {
                    $('#divListaProfissionaisPermitidos').html(data);
                }
            },
            complete: function () {

                $("#PermissaoNumeroConselho").val("");
                $("#PermissaoNumeroConselho").focus();
            }
        });

    }
};

function RemoveProfissionalSelecionado(numero, sigla, uf) {

    $.ajax({
        url: $("#hdnRemoveTempDataProfissionaisPermitidos").val(),
        type: 'POST',
        data: { numero: numero, sigla: sigla, uf: uf, edicao: false },
        success: function (data) {
            var result = data;
            if (!result.Erro) {
                $('#divListaProfissionaisPermitidos').html(data);
            }
        }
    });
};