"use strict";

function UpdateQuestion(val) {
    location.href = '?PesquisaId='+ val;
}
function UpdateAlternative(val) {
    var pesquisa = $('#PesquisaId').val()
    location.href = '?PesquisaId=' + pesquisa + '&QuestaoId=' + val;
}