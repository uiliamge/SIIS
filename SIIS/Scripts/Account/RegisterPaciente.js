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
        //if (numero == "") {
        //    $('span[data-valmsg-for="Permissao_NumeroConselho"]').text('Informe o Nº no Conselho Regional.');
        //}
        //if (sigla == "") {
        //    $('span[data-valmsg-for="SiglaConselhoRegional"]').text('Informe o Conselho Regional.');
        //}
        //if (uf == "") {
        //    $('span[data-valmsg-for="UfConselhoRegional"]').text('Informe a UF.');
        //}
    }
    else {
        if ($("#lbxProfissionaisSelecionados option:eq(0)").val() == "") {
            $("#lbxProfissionaisSelecionados option:eq(0)").remove();
        }

        var selecionado = numero + " " + sigla + "-" + uf;

        $("#lbxProfissionaisSelecionados").append("<option value='" + selecionado + "'>" + selecionado + "</option>");
        $("#Permissao").append("<option value='" + selecionado + "'>" + selecionado + "</option>");
        $("#PermissaoNumeroConselho").val("");
    }
};