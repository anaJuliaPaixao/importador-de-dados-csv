using System.Collections.Generic;

namespace Importador.Dominio.DTO
{
    public class RetornoFiltroDTO
    {
        public string NomeArquivo { get; set; }
        public List<LinhaDTO> Dados { get; set; }
    }

    public class LinhaDTO
    {
        public List<ColunaDTO> Colunas { get; set; }
    }

    public class ColunaDTO
    {
        public string Nome { get; set; }
        public string Valor { get; set; }
    }
}