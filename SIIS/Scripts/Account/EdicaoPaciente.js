$(function () {

    $("#TipoPermissao").change(function () {

        if ($("#TipoPermissao").val() == "QualquerProfissional") {
            $("#divListaProfissionaisPermitidos").hide();
            $("#txtExplicativoQualquerProfissionalPodeAcessar").show();
        } else {
            $("#divListaProfissionaisPermitidos").show();
            $("#txtExplicativoQualquerProfissionalPodeAcessar").hide();
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
            data: { numero: numero, sigla: sigla, uf: uf, edicao: true },
            success: function (data) {
                var result = data;
                if (!result.Erro) {
                    $('#divPermissoes').html(data);
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
        data: { numero: numero, sigla: sigla, uf: uf, edicao: true },
        success: function (data) {
            var result = data;
            if (!result.Erro) {
                $('#divPermissoes').html(data);
            }
        }
    });
};