using Importador.Dominio.Entidades;

namespace Importador.Dominio.Interfaces.Repositorios;

public interface IDadosCatalagoRepositorio
{
    Task InserirDadosCatalago(DadosCatalago dadosCatalago);
    Task<IEnumerable<DadosCatalago>> ObterPorFiltro(int idCatalago, int? idColuna, string valor);
    Task<IEnumerable<DadosCatalago>> ObterPorLinha(List<int> linhas, int idCatalago);
    Task<IEnumerable<DadosCatalago>> ObterPorColuna(int idCatalago, int idColuna);



}
