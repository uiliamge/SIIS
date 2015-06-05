$(function () {

    $("#btnVerificarCpf").click(function () {
        verificarCpf();
    });

    $("#Paciente_CpfCnpj").change(function () {

        if ($("#TipoPermissao").val() == "QualquerProfissional") {
            $("#divListaProfissionaisPermitidos").addClass("hidden");
            $("#txtExplicativoQualquerProfissionalPodeAcessar").removeClass("hidden");
        } else {
            $("#divListaProfissionaisPermitidos").removeClass("hidden");
            $("#txtExplicativoQualquerProfissionalPodeAcessar").addClass("hidden");
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

function verificarCpf() {
    debugger;
    $.ajax({
        url: $("#urlBuscarPaciente").val(),
        type: 'POST',
        data: { cpf: $("#Paciente_CpfCnpj").val() },
        success: function (data) {
            var result = data;
            if (!result.Erro) {
                $('#divInformacoesPaciente').html(data);
            }
        },
        complete: function (result) {
            debugger;
            //$("#PermissaoNumeroConselho").val("");
            //$("#PermissaoNumeroConselho").focus();
        }
    });
};