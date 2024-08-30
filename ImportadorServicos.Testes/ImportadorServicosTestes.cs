using System.Data;
using System.IO.Pipes;
using Importador.Dominio.Entidades;
using Importador.Dominio.Interfaces.Repositorios;
using Importador.Dominio.Interfaces.Servicos;
using Importador.Servico.Servicos;
using Moq;

public class ImportadorServicosTestes
{
    private readonly Mock<ICatalagoRepositorio> _catalagoRepositorioMock;
    private readonly Mock<ICatalagoColunaRepositorio> _catalagoColunaRepositorioMock;
    private readonly Mock<IDadosCatalagoRepositorio> _dadosCatalagoRepositorioMock;
    private readonly Mock<INotificacaoServico> _notificacaoServicoMock;
    private readonly ImportadorServicos _importadorServicos;

    public ImportadorServicosTestes()
    {
        _catalagoRepositorioMock = new Mock<ICatalagoRepositorio>();
        _catalagoColunaRepositorioMock = new Mock<ICatalagoColunaRepositorio>();
        _dadosCatalagoRepositorioMock = new Mock<IDadosCatalagoRepositorio>();
        _notificacaoServicoMock = new Mock<INotificacaoServico>();

        _importadorServicos = new ImportadorServicos(
            _catalagoRepositorioMock.Object,
            _catalagoColunaRepositorioMock.Object,
            _notificacaoServicoMock.Object,
            _dadosCatalagoRepositorioMock.Object
        );
    }

    [Fact]
    public async Task ImportarArquivo_DeveInserirNovoCatalago_QuandoCatalagoNaoExiste()
    {
        var dataTable = new DataTable();
        var nomeArquivo = "teste.csv";
        _catalagoRepositorioMock.Setup(repo => repo.ObterPorNome(It.IsAny<string>())).ReturnsAsync((Catalago)null);

        _importadorServicos.ImportarArquivo(dataTable, nomeArquivo);

        _catalagoRepositorioMock.Verify(repo => repo.AdicionarAsync(It.IsAny<Catalago>()), Times.Once);
    }

    [Fact]
    public async Task ImportarArquivo_DeveInserirDadosEmCatalagoExistente_QuandoCatalagoExiste()
    {
        var dataTable = new DataTable();
        dataTable.Columns.Add("Column1", typeof(int));
        dataTable.Columns.Add("Column2", typeof(string));
        dataTable.Columns.Add("Column3", typeof(DateTime));

        dataTable.Rows.Add(1, "Value1", DateTime.Now);
        dataTable.Rows.Add(2, "Value2", DateTime.Now);
        dataTable.Rows.Add(3, "Value3", DateTime.Now);

        var colunasDataTable = dataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToList();


        var nomeArquivo = "teste.csv";
        var catalago = new Catalago { Id = 1, Nome = "teste" };



        _catalagoRepositorioMock.Setup(repo => repo.ObterPorNome(It.IsAny<string>())).ReturnsAsync(catalago);
        _catalagoColunaRepositorioMock.Setup(repo => repo.ObterListaColunaPorNomeCatalago(It.IsAny<string>())).ReturnsAsync(colunasDataTable);
        _catalagoColunaRepositorioMock.Setup(repo => repo.ObterPornomeECatalagoId(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(new CatalagoColuna());
        _catalagoRepositorioMock.Setup(repo => repo.ObterPorNome(It.IsAny<string>())).ReturnsAsync(catalago);
        _catalagoColunaRepositorioMock.Setup(repo => repo.ColunasEstaoValidas(It.IsAny<List<string>>(), It.IsAny<int>())).ReturnsAsync(true);

        _importadorServicos.ImportarArquivo(dataTable, nomeArquivo);

        _dadosCatalagoRepositorioMock.Verify(repo => repo.InserirDadosCatalago(It.IsAny<DadosCatalago>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task ImportarArquivoCatalagoExistente_DeveAdicionarNotificacaoErro_QuandoCatalagoNaoEncontrado()
    {
        var dataTable = new DataTable();
        var nomeArquivo = "teste.csv";
        var nomeCatalago = "inexistente";
        _catalagoRepositorioMock.Setup(repo => repo.ObterPorNome(It.IsAny<string>())).ReturnsAsync((Catalago)null);

        var result = await _importadorServicos.ImportarArquivoCatalagoExistente(dataTable, nomeArquivo, nomeCatalago);

        _notificacaoServicoMock.Verify(serv => serv.AdicionarNotificacao("Erro", "Catalago não encontrado"), Times.Once);
        Assert.Null(result);
    }

    [Fact]
    public async Task FiltrarDadosCatalago_DeveAdicionarNotificacaoErro_QuandoCatalagoNaoEncontrado()
    {
        var nomeCatalago = "inexistente";
        var nomeColuna = "coluna";
        var valor = "valor";
        _catalagoRepositorioMock.Setup(repo => repo.ObterPorNome(It.IsAny<string>())).ReturnsAsync((Catalago)null);

        var result = await _importadorServicos.FiltrarDadosCatalago(nomeCatalago, nomeColuna, valor);

        _notificacaoServicoMock.Verify(serv => serv.AdicionarNotificacao("Erro", "Catálogo não encontrado"), Times.Once);
        Assert.Null(result);
    }
}