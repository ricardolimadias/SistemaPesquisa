function Encadeado (campo)
{
    var valorencadeado = $(campo).attr("data-value");
    if (valorencadeado == "Parar Resposta Formulario") {
        window.btn = "fechar";
        $("form").submit();
    }
}

//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
(function (i, s, o, g, r, a, m) {
    i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
        (i[r].q = i[r].q || []).push(arguments)
    }, i[r].l = 1 * new Date(); a = s.createElement(o),
    m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
})(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');
ga('create', 'UA-46156385-1', 'cssscript.com');
ga('send', 'pageview');
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
$(document).ready(function () {
    GetDadosRespostas();
});
function GetDadosRespostas() {
    $.get('/TB_Formulario/index', {}, function (data) {
        var tblRespostas = $("#tblRespostas");
        $.each(data, function (index, item) {
            var tr = $("<tr></tr>");
            tr.html(("<td>" + item.Questao + "</td>")
                + " " + ("<td>" + item.QuestaoId - 0 + "</td>")
                + " " + ("<td>" + SubItem.AlternativaId + "</td>")
                + " " + ("<td>" + item.QuestaoId-1 + "</td>")
                + " " + ("<td>" + item.QuestaoId-2 + "</td>")
                + " " + ("<td>" + item.QuestaoId-3 + "</td>")
                + " " + ("<td>" + item.QuestaoId-4 + "</td>")
                + " " + ("<td>" + item.QuestaoId-5 + "</td>")
                + " " + ("<td>" + item.ParticipanteId + "</td>"));
            tblRespostas.append(tr);
        });
    });
}

//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
window.btn = "";
$(document).ready(function () {
    
    $("form").on("submit", function (event) {
        event.preventDefault();
        var o = {}; var a = $(this).serializeArray();
        $.each(a, function () {
            if (o[this.name]) {
                if (!o[this.name].push) {
                    o[this.name] = [o[this.name]];
                } o[this.name].push(this.value || '');
            } else { o[this.name] = this.value || ''; }
        });
        //console.log(o);
        var url = window.location.origin + '/TB_Formulario/save';
        //console.log(window.btn);
        $.ajax({
            url: url, data: { valor: o, participante: $("#participante").html(), acao: window.btn, pesquisa: $("#pesquisa").html() }, type: "Get", dataType: "text", error: function (jqXHR, textStatus, errorThrown) {
                if (textStatus) { console.log('Erro ao carregar os dados.', textStatus) };
            }, success: function (data) {
                console.log(data);
                var result = $.parseJSON(data);
                alert(result.msg);
                if (result.status == "Erro") return;
                $("Form").remove();
                window.close();
            }
        });
    });


 //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        var counter = 0;
        $("#addButton").click(function () {

            if (TipoResposta = 1) {
                alert(TipoResposta);
                var newTextBoxDiv = $(document.createElement('div'))
             .attr("id", 'TextBoxDiv' + counter);

                newTextBoxDiv.after().html('<label>Textbox #' + counter + ' : </label>' + '<input type="textbox" name="textbox' + counter +
                      '" id="idCampo' + counter + '" value="" >');

                newTextBoxDiv.appendTo("#TextBoxesGroup");

                counter++;
            }

            if (TipoResposta = 2) {
                alert(TipoResposta);
                var newCheckBoxDiv = $(document.createElement('div'))
             .attr("id", 'CheckboxBoxDiv' + counter);

                newCheckBoxDiv.after().html('<label>Checkbox #' + counter + ' : </label>' + '<input type="checkbox" name="checkbox' + counter +
                      '" id="idCampo' + counter + '" value="" >');

                newCheckBoxDiv.appendTo("#CheckboxesGroup");

                counter++;
            }

            if (TipoResposta = 3) {
                alert(TipoResposta);
                var newRadioBoxDiv = $(document.createElement('div'))
             .attr("id", 'RadioBoxDiv' + counter);

                newRadioBoxDiv.after().html('<label>Radiobox #' + counter + ' : </label>' + '<input type="radio" name="radio' + counter +
                      '" id="idCampo' + counter + '" value="" >');

                newRadioBoxDiv.appendTo("#RadiosGroup");

                counter++;
            }

            if (TipoResposta = 4) {
                alert(TipoResposta);
                var newRangeDiv = $(document.createElement('div'))
             .attr("id", 'RangeDiv' + counter);

                newRangeDiv.after().html('<label>Range #' + counter + ' : </label>' + '<input type="range" name="range' + counter +
                      '" id="idCampo' + counter + '" value="" >');

                newRangeDiv.appendTo("#RangeGroup");

                counter++;
            }

            if (TipoResposta = 5) {
                alert(TipoResposta);
                var newRangeDiv = $(document.createElement('div'))
             .attr("id", 'RangeDiv' + counter);

                newRangeDiv.after().html('<label>Range #' + counter + ' : </label>' + '<input type="range" name="range' + counter +
                      '" id="idCampo' + counter + '" value="" >');

                newRangeDiv.appendTo("#RangeGroup");

                counter++;
            }

        });
    });
