namespace Importador.Dominio.Entidades;

public class DadosCatalago : EntidadeBase
{
    public int IdCatalagoColuna { get; set; }
    public CatalagoColuna CatalagoColuna { get; set; }
    public string Valor { get; set; }
    public int Linhas { get; set; }

}
