using Prova.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Prova.Infrastructure.Repositories.Data;

namespace Prova.Repositories
{
    public class MarcaRepository
    {
        private readonly AppDbContext _context;

        public MarcaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Marca>> GetAllAsync()
        {
            return await _context.Marcas.ToListAsync();
        }

        public async Task<Marca?> GetByIdAsync(int id)
        {
            return await _context.Marcas.FindAsync(id);
        }

        public async Task<Marca> AddAsync(Marca marca)
        {
            _context.Marcas.Add(marca);
            await _context.SaveChangesAsync();
            return marca;
        }

    }
}