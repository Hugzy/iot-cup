using Server.Models;

namespace Server.Services.Interfaces
{
    public interface ICupService
    {
        public void UpdateCup(string id, CupFormData cupUpdateData);

    }
}