using System;
using Importador.Dominio.Entidades;

namespace Importador.Dominio.Interfaces.Repositorios;

public interface ICatalagoColunaRepositorio
{
    Task<IEnumerable<CatalagoColuna>> ObterTodosAsync();
    Task<CatalagoColuna> ObterPorIdAsync(int id);
    Task AdicionarColunaCatalago(CatalagoColuna catalagoColuna);
    Task<CatalagoColuna> ObterPornomeECatalagoId(string nome, int catalagoId);
    Task AtualizarAsync(CatalagoColuna catalagoColuna);
    Task<bool> ColunasEstaoValidas(List<string> colunas, int idCatalago);
    Task RemoverAsync(int id);
    Task<List<string>> ObterListaColunaPorNomeCatalago(string nomeCatalago);

}
