using Importador.Dominio.Entidades;
using Importador.Dominio.Interfaces.Servicos;

public class NotificacaoServico : INotificacaoServico
{
    private readonly List<Notificacao> _notificacoes = new List<Notificacao>();

    public void AdicionarNotificacao(string chave, string mensagem)
    {
        _notificacoes.Add(new Notificacao(chave, mensagem));
    }

    public List<Notificacao> ObterNotificacoes()
    {
        return _notificacoes;
    }

    public bool TemNotificacoes()
    {
        return _notificacoes.Count > 0;
    }
}