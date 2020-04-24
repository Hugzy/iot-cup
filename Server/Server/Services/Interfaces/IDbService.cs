using System.Collections.Generic;
using Server.Models;

namespace Server.Services.Interfaces
{
    public interface IDbService
    {
        public void ConnectCup(string jsonStr);

        public void Temperature(string jsonStr);
        
        public IEnumerable<Cup> GetCups();
        public void DisconnectCup(string jsonStr);
    }
}