using System;
using Importador.Dominio.Entidades;

namespace Importador.Dominio.Interfaces.Servicos;

public interface INotificacaoServico
{
    void AdicionarNotificacao(string chave, string mensagem);
    List<Notificacao> ObterNotificacoes();
    bool TemNotificacoes();

}
