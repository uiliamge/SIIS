$(function () {
    atualizarCpf();
    atualizarCnpj();
    atualizarNumericos();
    atualizarCep();
});

function atualizarCpf() {
    $(".cpf").attr("maxlength", "14");

    $(".cpf").keyup(function () {
        cpf(this);
    });
}

function atualizarCnpj() {
    $(".cnpj").attr("maxlength", "18");

    $(".cnpj").keyup(function () {
        cnpj(this);
    });
}

function cpf(obj) {
    //Remove tudo o que não é dígito
    var v = $(obj).val().replace(/\D/g, "");

    //Coloca um ponto entre o terceiro e o quarto dígitos
    v = v.replace(/(\d{3})(\d)/, "$1.$2");

    //Coloca um ponto entre o terceiro e o quarto dígitos
    //de novo (para o segundo bloco de números)
    v = v.replace(/(\d{3})(\d)/, "$1.$2");

    //Coloca um hífen entre o terceiro e o quarto dígitos
    v = v.replace(/(\d{3})(\d{1,2})$/, "$1-$2");

    $(obj).val(v);
}

function cnpj(obj) {
    //Remove tudo o que não é dígito
    var v = $(obj).val().replace(/\D/g, "");

    //Coloca ponto entre o segundo e o terceiro dígitos
    v = v.replace(/^(\d{2})(\d)/, "$1.$2");

    //Coloca ponto entre o quinto e o sexto dígitos
    v = v.replace(/^(\d{2})\.(\d{3})(\d)/, "$1.$2.$3");

    //Coloca uma barra entre o oitavo e o nono dígitos
    v = v.replace(/\.(\d{3})(\d)/, ".$1/$2");

    //Coloca um hífen depois do bloco de quatro dígitos
    v = v.replace(/(\d{4})(\d)/, "$1-$2");

    $(obj).val(v);
}

function atualizarNumericos() {
    $(".numero").keydown(function (e) {
        var key = e.charCode || e.keyCode || 0;

        // allow backspace, tab, delete, arrows, numbers and keypad numbers ONLY
        if (e.shiftKey == true && (key == 7 || key == 9))
            return true;
        if (e.shiftKey == false && (key == 8 || key == 9 || key == 46 || key == 194 || key == 110 || key == 188 || key == 190 || (key >= 37 && key <= 40) || (key >= 48 && key <= 57) || (key >= 96 && key <= 105)))
            return true;
        else
            return false;
    });

    $(".numero").keypress(function (e) {
        var key = e.charCode || e.keyCode || 0;

        // allow backspace, tab, delete, arrows, numbers and keypad numbers ONLY
        if (e.shiftKey == true && (key == 7 || key == 9))
            return true;
        if (e.shiftKey == false && (key == 8 || key == 9 || key == 46 || key == 194 || key == 110 || key == 188 || key == 190 || (key >= 37 && key <= 40) || (key >= 48 && key <= 57) || (key >= 96 && key <= 105)))
            return true;
        else
            return false;
    });
}

function atualizarCep() {
    $(".cep").attr("maxlength", "10");

    $(".cep").keyup(function () {
        cep(this);
    });
}

//MASCARA PARA CEP '00.000-000'
function cep(campo) {
    var v = $(campo).val().replace(/\D/g, "");
    var tam = v.length;

    if ((tam > 2) && (tam <= 5)) {
        v = v.substring(0, 2) + '.' + v.substring(2, tam);
    }

    if ((tam >= 6) && (tam <= 8)) {
        v = v.substring(0, 2) + "." + v.substring(2, 5) + "-" + v.substring(5, 8);
    }

    $(campo).val(v);
}