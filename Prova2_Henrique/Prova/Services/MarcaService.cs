using Prova.Repositories;
using Prova.Models;

namespace Prova.Services
{
    public class MarcaService
    {
        private readonly MarcaRepository _marcaRepository;

        public MarcaService(MarcaRepository marcaRepository)
        {
            _marcaRepository = marcaRepository;
        }

        public async Task<IEnumerable<Marca?>> GetAllAsync()
        {
            return await _marcaRepository.GetAllAsync();
        }

        public async Task<Marca?> GetByIdAsync(int id)
        {
            return await _marcaRepository.GetByIdAsync(id);
        }

        public async Task<Marca?> AddAsync(Marca marca)
        {
            return await _marcaRepository.AddAsync(marca);
        }
    }
}
