using System.Collections.Generic;
using Server.Models;

namespace Server.Services.Interfaces
{
    public interface IDbService
    {
        public Cup ConnectCup(string jsonStr);

        public void InsertTemperature(string jsonStr);
        
        public IEnumerable<Cup> GetCups();
        public void DisconnectCup(string jsonStr);
        public Cup GetCup(string id);
        public void UpdateCup(string id, CupFormData cup);
    }
}