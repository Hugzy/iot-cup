using Server.Models;
using Server.Services.Interfaces;

namespace Server.Services
{
    public class CupService : ICupService
    {
        private readonly IDbService _dbService;

        public CupService(IDbService dbService)
        {
            _dbService = dbService;
        }

        public void UpdateCup(string id, CupFormData cupUpdateData)
        {
            _dbService.UpdateCup(id, cupUpdateData);
            
        }
    }
}