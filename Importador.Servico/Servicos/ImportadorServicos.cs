using System.Data;
using Importador.Dominio.DTO;
using Importador.Dominio.Entidades;
using Importador.Dominio.Interfaces.Repositorios;
using Importador.Dominio.Interfaces.Servicos;

namespace Importador.Servico.Servicos;

public class ImportadorServicos : IImportadorServico
{
    private readonly ICatalagoRepositorio _catalagoRepositorio;
    private readonly ICatalagoColunaRepositorio _catalagoColunaRepositorio;
    private readonly IDadosCatalagoRepositorio _dadosCatalagoRepositorio;
    private readonly INotificacaoServico _notificacaoServico;

    public ImportadorServicos(ICatalagoRepositorio catalagoRepositorio,
        ICatalagoColunaRepositorio catalagoColunaRepositorio,
        INotificacaoServico notificacaoServico,
        IDadosCatalagoRepositorio dadosCatalagoRepositorio)
    {
        _catalagoRepositorio = catalagoRepositorio;
        _catalagoColunaRepositorio = catalagoColunaRepositorio;
        _notificacaoServico = notificacaoServico;
        _dadosCatalagoRepositorio = dadosCatalagoRepositorio;
    }

    public async void ImportarArquivo(DataTable conteudo, string nomeArquivo)
    {
        var nomeArquivoSemExtensao = Path.GetFileNameWithoutExtension(nomeArquivo);
        var catalago = await _catalagoRepositorio.ObterPorNome(nomeArquivoSemExtensao);

        if (catalago == null)
            await InserirNovoCatalago(conteudo, nomeArquivoSemExtensao);

        else
            await InserirDadosCatalagosExistentes(conteudo, catalago);
    }

    private async Task InserirNovoCatalago(DataTable conteudo, string nomeArquivo)
    {
        var novoCatalago = await _catalagoRepositorio.AdicionarAsync(new Catalago { Nome = nomeArquivo });

        var colunasDataTable = conteudo.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToList();

        foreach (var coluna in colunasDataTable)
            await _catalagoColunaRepositorio.AdicionarColunaCatalago(new CatalagoColuna { Nome = coluna, IdCatalago = novoCatalago.Id });

        await InserirDadosCatalago(conteudo, novoCatalago);
    }

    private async Task InserirDadosCatalagosExistentes(DataTable conteudo, Catalago catalago)
    {
        var colunasDataTable = conteudo.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToList();
        var colunasCatalago = await _catalagoColunaRepositorio.ColunasEstaoValidas(colunasDataTable, catalago.Id);

        if (!colunasCatalago)
        {
            _notificacaoServico.AdicionarNotificacao("Erro", "Colunas do arquivo não são válidas");
            return;
        }
        await InserirDadosCatalago(conteudo, catalago);
    }

    public async Task<RetornoFiltroDTO> ImportarArquivoCatalagoExistente(DataTable conteudo, string nomeArquivo, string nomeCatalago)
    {
        var nomeArquivoSemExtensao = Path.GetFileNameWithoutExtension(nomeArquivo);
        var colunasDataTable = await _catalagoColunaRepositorio.ObterListaColunaPorNomeCatalago(nomeCatalago);
        var catalago = await _catalagoRepositorio.ObterPorNome(nomeCatalago);
        var novoCatalago = await _catalagoRepositorio.AdicionarAsync(new Catalago { Nome = nomeArquivoSemExtensao });


        if (catalago == null || colunasDataTable.Count == 0)
        {
            _notificacaoServico.AdicionarNotificacao("Erro", "Catalago não encontrado");
            return default;
        }

        await AdicionarColunasCatalago(novoCatalago!, colunasDataTable);
        await InserirDadosCatalago(conteudo, novoCatalago!, catalago.Id);
        return await FiltrarDadosCatalago(nomeArquivoSemExtensao, null, null);
    }

    private async Task AdicionarColunasCatalago(Catalago novoCatalago, List<string> colunasCatalagoVinculado)
    {
        foreach (var coluna in colunasCatalagoVinculado)
            await _catalagoColunaRepositorio.AdicionarColunaCatalago(new CatalagoColuna { Nome = coluna, IdCatalago = novoCatalago.Id });
    }

    private async Task InserirDadosCatalago(DataTable conteudo, Catalago catalago, int idCatalagoVinculo = 0)
    {
        var colunasDataTable = await _catalagoColunaRepositorio.ObterListaColunaPorNomeCatalago(catalago.Nome);
        var numeroLinhas = await ObterQuantidadeLinhas(catalago);
        var campoFiltroIndex = 0;

        foreach (DataRow row in conteudo.Rows)
        {
            var campoFiltro = conteudo.Rows[campoFiltroIndex][0].ToString();

            foreach (var coluna in colunasDataTable)
            {
                var catalagoColuna = await _catalagoColunaRepositorio.ObterPornomeECatalagoId(coluna, catalago.Id);
                await _dadosCatalagoRepositorio.InserirDadosCatalago(new DadosCatalago
                {
                    IdCatalagoColuna = catalagoColuna.Id,
                    Valor = !row.Table.Columns.Contains(coluna) ? await bucarDadoCatalago(coluna, conteudo, idCatalagoVinculo, campoFiltro) : row[coluna].ToString(),
                    Linhas = row.Table.Rows.IndexOf(row) + numeroLinhas,
                });
            }
            campoFiltroIndex++;
        }
        return;
    }

    private async Task<string?> bucarDadoCatalago(string coluna, DataTable conteudo, int idCatalagoVinculo, string? campoFiltro)
    {
        var colunaFiltro = conteudo.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToList().First();

        var colunasDataTable = conteudo.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToList();
        var colunaVinculo = await _catalagoColunaRepositorio.ObterPornomeECatalagoId(colunaFiltro, idCatalagoVinculo);
        var dado = await _dadosCatalagoRepositorio.ObterPorFiltro(idCatalagoVinculo, colunaVinculo.Id, campoFiltro);
        var linhaDado = dado.Where(x => x.Valor.Equals(campoFiltro)).FirstOrDefault()!.Linhas;
        var colunaValor = await _catalagoColunaRepositorio.ObterPornomeECatalagoId(coluna, idCatalagoVinculo);

        var valor = await _dadosCatalagoRepositorio.ObterPorFiltro(idCatalagoVinculo, colunaValor.Id, null);

        return valor.Where(x => x.Linhas == linhaDado).FirstOrDefault()!.Valor;

    }

    public async Task<RetornoFiltroDTO> FiltrarDadosCatalago(string nomeCatalago, string nomeColuna, string valor)
    {
        var catalago = await _catalagoRepositorio.ObterPorNome(nomeCatalago);
        if (catalago == null)
        {
            _notificacaoServico.AdicionarNotificacao("Erro", "Catálogo não encontrado");
            return null;
        }

        var coluna = await _catalagoColunaRepositorio.ObterPornomeECatalagoId(nomeColuna, catalago.Id);

        var colunaId = coluna == null ? 0 : coluna.Id;

        var linhasDosDados = await _dadosCatalagoRepositorio.ObterPorFiltro(catalago.Id, colunaId, valor);

        List<int> linhasFiltradas = new List<int>();

        foreach (var linha in linhasDosDados)
        {
            linhasFiltradas.Add(linha.Linhas);
        }

        var dadosFiltrados = await _dadosCatalagoRepositorio.ObterPorLinha(linhasFiltradas, catalago.Id);

        var dadosAgrupadosPorLinha = dadosFiltrados
            .GroupBy(dc => dc.Linhas)
            .Select(grupo => new LinhaDTO
            {
                Colunas = grupo.Select(dc => new ColunaDTO
                {
                    Nome = dc.CatalagoColuna.Nome,
                    Valor = dc.Valor
                }).ToList()
            }).ToList();

        return new RetornoFiltroDTO
        {
            NomeArquivo = catalago.Nome,
            Dados = dadosAgrupadosPorLinha
        };
    }



    private async Task<int> ObterQuantidadeLinhas(Catalago catalago)
    {
        var linhas = await _dadosCatalagoRepositorio.ObterPorFiltro(catalago.Id, null, null);
        var ultimaLinha = linhas.LastOrDefault() == null ? 0 : linhas.Last().Linhas;
        return ultimaLinha + 1;
    }

    async Task<List<string>> IImportadorServico.ObterColunaPorCatalago(string nomeCatalago)
    {
        var colunas = await _catalagoColunaRepositorio.ObterListaColunaPorNomeCatalago(nomeCatalago);
        if (colunas == null || !colunas.Any())
        {
            _notificacaoServico.AdicionarNotificacao("Erro", "Catálogo não encontrado");
            return null;
        }

        return colunas;
    }
}