using System;
using Importador.Dominio.Entidades;

namespace Importador.Dominio.Interfaces.Repositorios;

public interface ICatalagoRepositorio
{
    Task<List<Catalago>> ObterTodosAsync();
    Task<Catalago> ObterPorIdAsync(int id);
    Task<Catalago> ObterPorNome(string nome);
    Task<Catalago> AdicionarAsync(Catalago catalago);
    Task AtualizarAsync(Catalago catalago);
    Task RemoverAsync(int id);
}
