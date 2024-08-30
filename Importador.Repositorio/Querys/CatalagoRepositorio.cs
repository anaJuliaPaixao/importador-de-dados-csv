using Importador.Dominio.Entidades;
using Importador.Dominio.Interfaces.Repositorios;
using Microsoft.EntityFrameworkCore;

namespace Importador.Infra.Repositorios
{
    public class CatalagoRepositorio : ICatalagoRepositorio
    {
        private readonly AplicacaoContexto _context;

        public CatalagoRepositorio(AplicacaoContexto context)
        {
            _context = context;
        }

        public async Task<List<Catalago>> ObterTodosAsync()
        {
            return await _context.Set<Catalago>().Include(c => c.Colunas).ToListAsync();
        }

        public async Task<Catalago> ObterPorIdAsync(int id)
        {
            return await _context.Set<Catalago>().Include(c => c.Colunas).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Catalago> AdicionarAsync(Catalago catalago)
        {
            await _context.Set<Catalago>().AddAsync(catalago);
            _context.SaveChanges();
            return await ObterPorNome(catalago.Nome);
        }

        public async Task AtualizarAsync(Catalago catalago)
        {
            _context.Set<Catalago>().Update(catalago);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverAsync(int id)
        {
            var catalago = await ObterPorIdAsync(id);
            if (catalago != null)
            {
                _context.Set<Catalago>().Remove(catalago);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Catalago?> ObterPorNome(string nome)
        {
            return _context.Catalago.FirstOrDefault(c => c.Nome == nome);
        }
    }
}