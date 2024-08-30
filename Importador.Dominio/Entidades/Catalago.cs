namespace Importador.Dominio.Entidades;

public class Catalago : EntidadeBase
{
    public string Nome { get; set; }
    public ICollection<CatalagoColuna> Colunas { get; set; }

}
