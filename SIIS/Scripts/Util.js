$(function () {

    //captura click no link para executar async.
    var getPage = function () {

        waitingDialog.show('Paginando', { dialogSize: 'sm', progressType: 'success' });

        var $a = $(this);
        var options = {
            url: $a.attr("href"),
            data: $("form").serialize(),
            type: "get"
        };

        $.ajax(options).done(function (data) {            
            var target = $a.parents("div.pagedList").attr("data-AL-target");
            $(target).replaceWith(data);

            waitingDialog.hide();
        });
        return false;
    };

    $(".body-content").on("click", ".pagedList a", getPage);

    atualizarMoedas();
    atualizarCpf();
    atualizarCnpj();
    atualizarCpfCnpj();
    atualizarNumericos();
    atualizarCep();
    atualizarTelefone();
    atualizarDate();
    
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

function atualizarCpfCnpj() {
    $(".cpfCnpj").attr("maxlength", "18");

    $(".cpfCnpj").keyup(function () {

        if ($(this).val().length < 14) {
            cpf(this);
        } else {
            cnpj(this);
        }
    });

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

function atualizarTelefone() {

    $(".telefone").attr("maxlength", "15");
    $(".telefone").keyup(function () {
        telefone(this);
    });
};

function atualizarDate() {

    $(".data").mask("99/99/9999");

    $(".data").blur(function () {

        var dia = $(this).val().split("/")[0];
        var mes = $(this).val().split("/")[1];
        var ano = $(this).val().split("/")[2];

        if ((dia == "00" || dia > 31) || (mes == "00" || mes > 12) || (ano == "0000" || ano < 1900)) {
            $(this).focus();
        }
    });
};


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

function telefone(obj) {

    var v = $(obj).val();
    v = v.replace(/\D/g, "");             //Remove tudo o que não é dígito
    v = v.replace(/^(\d{2})(\d)/g, "($1) $2"); //Coloca parênteses em volta dos dois primeiros dígitos
    v = v.replace(/(\d)(\d{4})$/, "$1-$2");    //Coloca hífen entre o quarto e o quinto dígitos
    $(obj).val(v);
}

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

function apenasNumero(obj) {
    //Remove tudo o que não é número
    var v = $(obj).val().replace(/\D/g, "");
    $(obj).val(v);
}

function atualizarMoedas() {
    $(".moeda").each(function () {
        var decimal = $(this).attr("decimal");
        if (decimal != "undefined")
            $(this).autoNumeric('init', { aSep: '.', aDec: ',', mDec: decimal, vMax: '999999999.99' });
        else
            $(this).autoNumeric('init', { aSep: '.', aDec: ',', vMax: '999999999.99' });

    });

    $(".moedaNegativa").each(function () {
        var decimal = $(this).attr("decimal");
        if (decimal != "undefined")
            $(this).autoNumeric('init', { aSep: '.', aDec: ',', mDec: decimal, vMin: '-999999999.99', vMax: '999999999.99' });
        else
            $(this).autoNumeric('init', { aSep: '.', aDec: ',', vMin: '-999999999.99', vMax: '999999999.99' });
    });

    $(".moedaQuatroCasas").each(function () {
        var decimal = $(this).attr("decimal");
        if (decimal != "undefined")
            $(this).autoNumeric('init', { aSep: '.', aDec: ',', mDec: decimal, vMax: '999999999.9999' });
        else
            $(this).autoNumeric('init', { aSep: '.', aDec: ',', vMax: '999999999.9999' });
    });

    $(".moeda14").each(function () {
        var decimal = $(this).attr("decimal");
        if (decimal != "undefined")
            $(this).autoNumeric('init', { aSep: '.', aDec: ',', mDec: decimal, vMax: '99999999999999.99' });
        else
            $(this).autoNumeric('init', { aSep: '.', aDec: ',', vMax: '99999999999999.99' });

    });
}

var waitingDialog = (function ($) {

    // Creating modal dialog's DOM
    var $dialog = $(
		'<div class="modal fade" data-backdrop="static" data-keyboard="false" tabindex="-1" role="dialog" aria-hidden="true" style="padding-top:15%; overflow-y:visible;">' +
		'<div class="modal-dialog modal-m">' +
		'<div class="modal-content">' +
			'<div class="modal-header"><h5 style="margin:0; text-align: center;"></h5></div>' +
			'<div class="modal-body">' +
				'<div class="progress progress-striped active" style="margin-bottom:0;"><div class="progress-bar" style="width: 100%"></div></div>' +
			'</div>' +
		'</div></div></div>');

    return {
        /**
		 * Opens our dialog
		 * @param message Custom message
		 * @param options Custom options:
		 * 				  options.dialogSize - bootstrap postfix for dialog size, e.g. "sm", "m";
		 * 				  options.progressType - bootstrap postfix for progress bar type, e.g. "success", "warning".
		 */
        show: function (message, options) {
            // Assigning defaults
            var settings = $.extend({
                dialogSize: 'm',
                progressType: ''
            }, options);
            if (typeof message === 'undefined') {
                message = 'Loading';
            }
            if (typeof options === 'undefined') {
                options = {};
            }
            // Configuring dialog
            $dialog.find('.modal-dialog').attr('class', 'modal-dialog').addClass('modal-' + settings.dialogSize);
            $dialog.find('.progress-bar').attr('class', 'progress-bar');
            if (settings.progressType) {
                $dialog.find('.progress-bar').addClass('progress-bar-' + settings.progressType);
            }
            $dialog.find('h5').text(message);
            // Opening dialog
            $dialog.modal();
        },
        /**
		 * Closes dialog
		 */
        hide: function () {
            $dialog.modal('hide');
        }
    }

})(jQuery);

var saibaMaisDialog = (function ($) {

    // Creating modal dialog's DOM
    var $dialog = $(
		'<div class="modal fade" data-backdrop="static" data-keyboard="false" tabindex="-1" role="dialog" aria-hidden="true" style="padding:0; overflow-y:visible;">' +
		'<div class="modal-dialog modal-lg" style=" height: 95% !important;">' +
		'<div class="modal-content" style=" height: 95% !important;">' +
			'<div class="modal-header"><button type="button" class="close" data-dismiss="modal">&times;</button><h4 class="modal-title">Mais sobre o Projeto SIIS</h4></div>' +
			'<div class="modal-body" style=" height: 85% !important; padding: 5px;">' +
				'<iframe style="width: 100%; height: 100%;" src="#"></iframe>' +
                '<p class="text-center"><a target="_blank" href="#">Abrir este documento em uma nova guia.</a></p>' +
			'</div>' +
		'</div></div></div>');

    return {
        /**
		 * Opens our dialog
		 * @param message Custom message
		 * @param options Custom options:
		 * 				  options.dialogSize - bootstrap postfix for dialog size, e.g. "sm", "m";
		 * 				  options.progressType - bootstrap postfix for progress bar type, e.g. "success", "warning".
		 */
        show: function (caminhoArquivo) {

            $dialog.find('iframe').attr("src", caminhoArquivo + "?#zoom=25");
            $dialog.find('a').attr("href", caminhoArquivo);
            $dialog.modal();
        },
        hide: function () {
            $dialog.modal('hide');
        }
    }

})(jQuery);

function AbrirModalExtrato(idExtrato) {

    waitingDialog.show('Abrindo Extrato ' + idExtrato, { dialogSize: 'sm', progressType: 'success' });

    $("#modalExtrato").modal("show");

    $.ajax({
        type: "GET",
        url: $("#urlAbrirModalExtrato").val(),
        data: { id: idExtrato },
        datatype: "html",
        success: function (data) {
            $("#tituloModalExtrato").text("Visualizando Extrato " + idExtrato);
            $("#divModalExtrato").html(data);
            waitingDialog.hide();
        }
    });
};