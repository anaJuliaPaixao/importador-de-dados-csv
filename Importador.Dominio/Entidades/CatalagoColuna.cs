namespace Importador.Dominio.Entidades;

public class CatalagoColuna : EntidadeBase
{
    public int IdCatalago { get; set; }
    public Catalago Catalago { get; set; }
    public string Nome { get; set; }
    public ICollection<DadosCatalago> DadosCatalagos { get; set; }

}
