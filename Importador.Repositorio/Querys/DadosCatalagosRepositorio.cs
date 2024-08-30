using Importador.Dominio.Entidades;
using Importador.Dominio.Interfaces.Repositorios;
using Microsoft.EntityFrameworkCore;

namespace Importador.Repositorio.Querys;

public class DadosCatalagosRepositorio : IDadosCatalagoRepositorio
{
    private readonly AplicacaoContexto _context;

    public DadosCatalagosRepositorio(AplicacaoContexto context)
    {
        _context = context;
    }

    public async Task InserirDadosCatalago(DadosCatalago dadosCatalago)
    {
        _context.DadosCatalago.Add(dadosCatalago);
        _context.SaveChanges();
    }

    public Task<IEnumerable<DadosCatalago>> ObterPorColuna(int idCatalago, int idColuna)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<DadosCatalago>> ObterPorFiltro(int idCatalago, int? idColuna, string valor)
    {
        var query = _context.DadosCatalago.AsQueryable();

        query = query.Where(d => d.CatalagoColuna.IdCatalago == idCatalago);

        if (idColuna.HasValue && idColuna.Value > 0)
        {
            query = query.Where(d => d.IdCatalagoColuna == idColuna);
        }

        if (!string.IsNullOrEmpty(valor))
        {
            query = query.Where(d => d.Valor.Contains(valor));
        }

        return query.ToList();
    }

    public async Task<IEnumerable<DadosCatalago>> ObterPorLinha(List<int> linhas, int idCatalago)
    {
        return await _context.DadosCatalago
                    .Where(d => d.CatalagoColuna.IdCatalago == idCatalago &&
                    linhas.Contains(d.Linhas))
                    .Include(d => d.CatalagoColuna)
                    .ToListAsync();
    }
}
