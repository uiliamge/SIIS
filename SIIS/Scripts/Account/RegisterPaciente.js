$(function () {

    $("#TipoPermissao").change(function () {

        if ($("#TipoPermissao").val() == "QualquerProfissional") {
            $("#divListaProfissionaisPermitidos").addClass("hidden");
            $("#txtExplicativoQualquerProfissionalPodeAcessar").removeClass("hidden");
        } else {
            $("#divListaProfissionaisPermitidos").removeClass("hidden");
            $("#txtExplicativoQualquerProfissionalPodeAcessar").addClass("hidden");
        }
    });
});

function addProfissionalSelecionado() {
    
    var numero = $("#PermissaoNumeroConselho").val();
    var sigla = $("#SiglaConselhoRegional").val();
    var uf = $("#UfConselhoRegional").val();

    if (numero == "" || sigla == "" || uf == "") {
        if (numero == "") {
            $('#validationProfissionais').text('Informe o Nº no Conselho Regional.');
        }
        if (sigla == "") {
            $('#validationProfissionais').text('Informe o Conselho Regional.');
        }
        if (uf == "") {
            $('validationProfissionais').text('Informe a UF.');
        }
    }
    else {
        $.ajax({
            url: $("#hdnAddTempDataProfissionaisPermitidos").val(),
            type: 'POST',
            data: { numero: numero, sigla: sigla, uf: uf },
            success: function (data) {
                var result = data;
                if (!result.Erro) {
                    $('#divListaProfissionaisPermitidos').html(data);
                }
            }
        });

        $("#PermissaoNumeroConselho").val("");
    }
};

function RemoveProfissionalSelecionado(numero, sigla, uf) {

    $.ajax({
        url: $("#hdnRemoveTempDataProfissionaisPermitidos").val(),
        type: 'POST',
        data: { numero: numero, sigla: sigla, uf: uf },
        success: function (data) {
            var result = data;
            if (!result.Erro) {
                $('#divListaProfissionaisPermitidos').html(data);
            }
        }
    });
};