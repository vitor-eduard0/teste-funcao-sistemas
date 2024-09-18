
$(document).ready(function () {
    if (obj) {
        $('#formCadastro #Nome').val(obj.Nome);
        $('#formCadastro #CEP').val(obj.CEP);
        $('#formCadastro #CPF').val(obj.CPF);
        $('#formCadastro #Email').val(obj.Email);
        $('#formCadastro #Sobrenome').val(obj.Sobrenome);
        $('#formCadastro #Nacionalidade').val(obj.Nacionalidade);
        $('#formCadastro #Estado').val(obj.Estado);
        $('#formCadastro #Cidade').val(obj.Cidade);
        $('#formCadastro #Logradouro').val(obj.Logradouro);
        $('#formCadastro #Telefone').val(obj.Telefone);
    }

    $('#formCadastro').submit(function (e) {
        e.preventDefault();
        
        $.ajax({
            url: urlPost,
            method: "POST",
            data: {
                "NOME": $(this).find("#Nome").val(),
                "CEP": $(this).find("#CEP").val(),
                "CPF": $(this).find("#CPF").val(),
                "Email": $(this).find("#Email").val(),
                "Sobrenome": $(this).find("#Sobrenome").val(),
                "Nacionalidade": $(this).find("#Nacionalidade").val(),
                "Estado": $(this).find("#Estado").val(),
                "Cidade": $(this).find("#Cidade").val(),
                "Logradouro": $(this).find("#Logradouro").val(),
                "Telefone": $(this).find("#Telefone").val()
            },
            error:
            function (r) {
                if (r.status == 400)
                    ModalDialog("Ocorreu um erro", r.responseJSON);
                else if (r.status == 500)
                    ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
            },
            success:
            function (r) {
                ModalDialog("Sucesso!", r)
                if (r.status != 202) {
                    $("#formCadastro")[0].reset();                                
                    window.location.href = urlRetorno;
                }
            }
        });
    })

    $(document).ready(function () {
        $('#formBenef').submit(function (e) {
            e.preventDefault();
            $.ajax({
                url: '/Beneficiario/Incluir',
                method: "POST",
                data: {
                    "CPF": $(this).find("#CPF").val(),
                    "NOME": $(this).find("#Nome").val()
                },
                error:
                    function (r) {
                        if (r.status == 400)
                            ModalDialog("Ocorreu um erro", r.responseJSON);
                        else if (r.status == 500)
                            ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
                    },
                success:
                    function (r) {
                        ModalDialog("Sucesso!", r)
                        $("#formBenef")[0].reset();
                        popularGridBenef();
                    }
            });
        });
    })

    popularGridBenef();

    $(document).ready(function () {
        $(".CPF").inputmask("999.999.999-99");
        $('.telefone').inputmask({
            mask: ['(99) 9999-9999', '(99) 99999-9999'],
            keepStatic: true
        });
    });
})

function popularGridBenef() {
    if (document.getElementById("gridBenef"))
        var baseUrl = window.location.protocol + '//' + window.location.hostname + (window.location.port ? ':' + window.location.port : '');
    $('#gridBenef').jtable({
        actions: {
            listAction: '/Beneficiario/BeneficiarioList',
        },
        fields: {
            Id: {
                key: true,
                list: false // Não mostrar o ID na listagem
            },
            CPF: {
                title: 'CPF',
                width: '30%'
            },
            Nome: {
                title: 'Nome',
                width: '40%'
            },
            Acoes: {
                title: 'Ações',
                width: '40%',
                display: function (data) {
                    return '<button onclick="alterarBenef(' + data.record.Id + ')" class="btn btn-primary btn-sm">Alterar</button>' +
                        //'<button onclick="window.location.href=\'' + baseUrl + '/Beneficiario/Excluir/' + data.record.Id + '\'" class="btn btn-primary btn-sm" style="margin-left: 5px;">Excluir</button>';
                        '<button onclick="excluirBenef(' + data.record.Id + ')" class="btn btn-primary btn-sm" style="margin-left: 5px;">Excluir</button>';
                }
            }
        }
    });
    
    if (document.getElementById("gridBenef"))
        $('#gridBenef').jtable('load');
}

function ModalDialog(titulo, texto) {
    var random = Math.random().toString().replace('.', '');
    var texto = '<div id="' + random + '" class="modal fade">                                                               ' +
        '        <div class="modal-dialog">                                                                                 ' +
        '            <div class="modal-content">                                                                            ' +
        '                <div class="modal-header">                                                                         ' +
        '                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>         ' +
        '                    <h4 class="modal-title">' + titulo + '</h4>                                                    ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-body">                                                                           ' +
        '                    <p>' + texto + '</p>                                                                           ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-footer">                                                                         ' +
        '                    <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>             ' +
        '                                                                                                                   ' +
        '                </div>                                                                                             ' +
        '            </div><!-- /.modal-content -->                                                                         ' +
        '  </div><!-- /.modal-dialog -->                                                                                    ' +
        '</div> <!-- /.modal -->                                                                                        ';

    $('body').append(texto);
    $('#' + random).modal('show');
}

function alterarBenef(idBenef) {
    var baseUrl = window.location.protocol + '//' + window.location.hostname + (window.location.port ? ':' + window.location.port : '');
    $.ajax({
        url: baseUrl + "/Beneficiario/Alterar/" + idBenef,
        method: "GET",
        error:
            function (r) {
                if (r.status == 400)
                    ModalDialog("Ocorreu um erro", r.responseJSON);
                else if (r.status == 500)
                    ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
            },
        success:
            function (r) {
                $('#formBenef #hdfIdBenef').val(r.Id);
                $('#formBenef #CPF').val(r.CPF);
                $('#formBenef #Nome').val(r.Nome);

                $('#btnIncluirBenef').hide();
                $('#btnAlterarBenef').show();
            }
    });
}

function excluirBenef(idBenef) {
    var baseUrl = window.location.protocol + '//' + window.location.hostname + (window.location.port ? ':' + window.location.port : '');
    $.ajax({
        url: baseUrl + "/Beneficiario/Excluir/" + idBenef,
        method: "GET",
        error:
            function (r) {
                if (r.status == 400)
                    ModalDialog("Ocorreu um erro", r.responseJSON);
                else if (r.status == 500)
                    ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
            },
        success:
            function (r) {
                popularGridBenef();
            }
    });
}

function salvarAlteracaoBenef() {
    var baseUrl = window.location.protocol + '//' + window.location.hostname + (window.location.port ? ':' + window.location.port : '');
    $.ajax({
        url: baseUrl + "/Beneficiario/Alterar",
        method: "POST",
        data: {
            "ID": $('#formBenef #hdfIdBenef').val(),
            "CPF": $('#formBenef #CPF').val(),
            "NOME": $('#formBenef #Nome').val()
        },
        error:
            function (r) {
                if (r.status == 400)
                    ModalDialog("Ocorreu um erro", r.responseJSON);
                else if (r.status == 500)
                    ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
            },
        success:
            function (r) {
                ModalDialog("Sucesso!", r)
                $("#formBenef")[0].reset();
                popularGridBenef();
                $('#btnIncluirBenef').show();
                $('#btnAlterarBenef').hide();
            }
    });
}