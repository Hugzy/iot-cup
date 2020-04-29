using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Models;

namespace Server.Services.Interfaces
{
    public interface IDbService
    {
        public Cup ConnectCup(string jsonStr);

        public void InsertTemperature(string jsonStr);

        public Task<IEnumerable<Temperature>> GetTemperature(string id, int limit);
        
        public IEnumerable<Cup> GetCups();
        public void DisconnectCup(string jsonStr);
        public Cup GetCup(string id);
        public void UpdateCup(string id, CupFormData cup);
    }
}