using Prova.Repositories; // Certifique-se de que este namespace est� correto
using Prova.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            return await _marcaRepository.GetAllAsync(); // Assumindo que voc� tem esse m�todo no reposit�rio
        }

        public async Task<Marca?> GetByIdAsync(int id)
        {
            return await _marcaRepository.GetByIdAsync(id); // Assumindo que voc� tem esse m�todo no reposit�rio
        }

        public async Task<Marca?> AddAsync(Marca marca)
        {
            return await _marcaRepository.AddAsync(marca); // Assumindo que voc� tem esse m�todo no reposit�rio
        }
    }
}
