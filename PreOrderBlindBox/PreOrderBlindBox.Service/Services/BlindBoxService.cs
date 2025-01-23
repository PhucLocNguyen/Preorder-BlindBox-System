using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.IRepositories;
using PreOrderBlindBox.Services.IServices;

namespace PreOrderBlindBox.Services.Services
{
    public class BlindBoxService : IBlindBoxService
    {
        private readonly IBlindBoxRepository _blindBoxRepository;

        public BlindBoxService(IBlindBoxRepository blindBoxRepository)
        {
            _blindBoxRepository = blindBoxRepository;
        }

        public async Task<List<BlindBox>> GetBlindBoxesAsync()
        {
            return await _blindBoxRepository.GetAll();
        }
    }
}
