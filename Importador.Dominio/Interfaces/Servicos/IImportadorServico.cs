using System.Data;
using Importador.Dominio.DTO;

namespace Importador.Dominio.Interfaces.Servicos;

public interface IImportadorServico
{
    void ImportarArquivo(DataTable conteudo, string nomeArquivo);
    Task<RetornoFiltroDTO> FiltrarDadosCatalago(string nomeCatalago, string nomeColuna, string valor);
    Task<RetornoFiltroDTO> ImportarArquivoCatalagoExistente(DataTable conteudo, string nomeArquivo, string nomeCatalago);
    Task<List<string>> ObterColunaPorCatalago(string nomeCatalago);

}