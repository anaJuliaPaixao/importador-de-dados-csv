using Microsoft.AspNetCore.Mvc;
using Importador.Dominio.Interfaces.Servicos;
using System.Data;

namespace Importador.Aplicacao.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImportacaoController : ControllerBase
    {
        private readonly INotificacaoServico _notificacaoServico;
        public ImportacaoController(INotificacaoServico notificacaoServico)
        {
            _notificacaoServico = notificacaoServico;
        }

        [HttpPost("ImportarArquivoNovo")]
        public async Task<IActionResult> ImportarArquivoNovo(IFormFile file, [FromServices] IImportadorServico importadorServico)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Arquivo CSV não enviado.");

            if (file.ContentType != "text/csv")
                return BadRequest("Arquivo enviado não é um CSV.");

            DataTable dataTable;
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                dataTable = ConvertCsvToDataTable(reader);

            }

            importadorServico.ImportarArquivo(dataTable, file.FileName);

            if (_notificacaoServico.TemNotificacoes())
            {
                return BadRequest(_notificacaoServico.ObterNotificacoes());
            }
            return Ok("Arquivo CSV processado com sucesso.");

        }

        [HttpPost("CatalagoExistente")]
        public async Task<IActionResult> InserirDadosCatalagoExistente(IFormFile file, [FromQuery] string nomeCatalago, [FromServices] IImportadorServico importadorServico)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Arquivo CSV não enviado.");

            if (file.ContentType != "text/csv")
                return BadRequest("Arquivo enviado não é um CSV.");

            DataTable dataTable;
            using (var reader = new StreamReader(file.OpenReadStream()))
                dataTable = ConvertCsvToDataTable(reader);

            var dadosFiltrados = await importadorServico.ImportarArquivoCatalagoExistente(dataTable, file.FileName, nomeCatalago);

            if (_notificacaoServico.TemNotificacoes())
                return BadRequest(_notificacaoServico.ObterNotificacoes());

            return Ok(dadosFiltrados);

        }

        private DataTable ConvertCsvToDataTable(StreamReader reader)
        {
            var dataTable = new DataTable();

            var LinhaCampo = reader.ReadLine();
            if (LinhaCampo != null)
            {
                var ValorCampos = LinhaCampo.Split(',');
                foreach (var AdicionarCampo in ValorCampos)
                {
                    dataTable.Columns.Add(AdicionarCampo.Trim());
                }

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line != null)
                    {
                        var values = line.Split(',');
                        dataTable.Rows.Add(values);
                    }
                }
            }
            return dataTable;
        }

        [HttpGet("FiltrarCatalago")]
        public async Task<IActionResult> FiltrarCatalago([FromQuery] string nomeCatalago, [FromQuery] string? nomeColuna,
            [FromQuery] string? valor,
            [FromServices] IImportadorServico importadorServico)
        {
            var dadosFiltrados = await importadorServico.FiltrarDadosCatalago(nomeCatalago, nomeColuna, valor);

            if (_notificacaoServico.TemNotificacoes())
            {
                return BadRequest(_notificacaoServico.ObterNotificacoes());
            }

            return Ok(dadosFiltrados);
        }

        [HttpGet("ObterColunaPorCatalago")]
        public async Task<IActionResult> ObterColunaPorCatalago([FromQuery] string nomeCatalago,
        [FromServices] IImportadorServico importadorServico)
        {
            var dadosFiltrados = await importadorServico.ObterColunaPorCatalago(nomeCatalago);

            if (_notificacaoServico.TemNotificacoes())
            {
                return BadRequest(_notificacaoServico.ObterNotificacoes());
            }

            return Ok(dadosFiltrados);
        }
    }
}
