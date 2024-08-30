using System.Linq;
using System.Net.Mail;
using Importador.Dominio.Entidades;
using Importador.Dominio.Interfaces.Repositorios;
using Microsoft.EntityFrameworkCore;

namespace Importador.Infra.Repositorios
{
    public class CatalagoColunaRepositorio : ICatalagoColunaRepositorio
    {
        private readonly AplicacaoContexto _context;

        public CatalagoColunaRepositorio(AplicacaoContexto context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CatalagoColuna>> ObterTodosAsync()
        {
            return await _context.Set<CatalagoColuna>().ToListAsync();
        }

        public async Task<CatalagoColuna> ObterPorIdAsync(int id)
        {
            return await _context.Set<CatalagoColuna>().FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<CatalagoColuna> ObterPornome(string nome)
        {
            return _context.CatalagoColuna.FirstOrDefault(c => c.Nome == nome);
        }
        public async Task<List<string>> ObterListaColunaPorNomeCatalago(string nomeCatalago)
        {
            return _context.CatalagoColuna.Where(c => c.Catalago.Nome == nomeCatalago).Select(x => x.Nome).ToList();
        }

        public async Task AdicionarColunaCatalago(CatalagoColuna catalagoColuna)
        {
            await _context.CatalagoColuna.AddAsync(catalagoColuna);
            _context.SaveChanges();
        }

        public async Task AtualizarAsync(CatalagoColuna catalagoColuna)
        {
            _context.Set<CatalagoColuna>().Update(catalagoColuna);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverAsync(int id)
        {
            var catalagoColuna = await ObterPorIdAsync(id);
            if (catalagoColuna != null)
            {
                _context.Set<CatalagoColuna>().Remove(catalagoColuna);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> ColunasEstaoValidas(List<string> colunas, int idCatalago)
        {
            var colunasCatalago = _context.CatalagoColuna
                .Where(x => colunas.Contains(x.Nome) && x.IdCatalago == idCatalago)
                .ToList();

            return colunasCatalago.Count == colunas.Count;
        }

        public async Task<CatalagoColuna> ObterPornomeECatalagoId(string nome, int catalagoId)
        {
            return _context.CatalagoColuna
                .FirstOrDefault(c => c.Nome == nome && c.IdCatalago == catalagoId);
        }
    }
}