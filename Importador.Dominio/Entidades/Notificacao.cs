using System;

namespace Importador.Dominio.Entidades;

public class Notificacao
{
    public string Chave { get; private set; }
    public string Mensagem { get; private set; }

    public Notificacao(string chave, string mensagem)
    {
        Chave = chave;
        Mensagem = mensagem;
    }
}
